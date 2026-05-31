using LGDXRobotCloud.API.Repositories;
using LGDXRobotCloud.API.Services.Navigation;
using LGDXRobotCloud.Data.DbContexts;
using LGDXRobotCloud.Data.Entities;
using LGDXRobotCloud.Protos;
using LGDXRobotCloud.Utilities.Enums;
using Microsoft.EntityFrameworkCore;

namespace LGDXRobotCloud.API.Services.Automation;

public interface IAutoTaskPathPlannerService
{
  Task<List<RobotClientsDof>> GeneratePath(AutoTask autoTask);
}

public partial class AutoTaskPathPlannerService(
    IMapEditorService mapEditorService,
    IRobotDataRepository robotDataRepository,
    LgdxContext context
  ) : IAutoTaskPathPlannerService
{
  private readonly IMapEditorService _mapEditorService = mapEditorService ?? throw new ArgumentNullException(nameof(mapEditorService));
  private readonly IRobotDataRepository _robotDataRepository = robotDataRepository ?? throw new ArgumentNullException(nameof(robotDataRepository));
  private readonly LgdxContext _context = context ?? throw new ArgumentNullException(nameof(context));
  
  private static RobotClientsDof GenerateWaypoint(AutoTaskDetail taskDetail)
  {
    if (taskDetail.Waypoint != null)
    {
      var waypoint = new RobotClientsDof
      { X = taskDetail.Waypoint.X, Y = taskDetail.Waypoint.Y, Rotation = taskDetail.Waypoint.Rotation };
      if (taskDetail.CustomX != null)
        waypoint.X = (double)taskDetail.CustomX;
      if (taskDetail.CustomY != null)
        waypoint.X = (double)taskDetail.CustomY;
      if (taskDetail.CustomRotation != null)
        waypoint.X = (double)taskDetail.CustomRotation;
      return waypoint;
    }
    else
    {
      return new RobotClientsDof
      {
        X = taskDetail.CustomX != null ? (double)taskDetail.CustomX : 0,
        Y = taskDetail.CustomY != null ? (double)taskDetail.CustomY : 0,
        Rotation = taskDetail.CustomRotation != null ? (double)taskDetail.CustomRotation : 0
      };
    }
  }

  public async Task<List<RobotClientsDof>> GeneratePath(AutoTask autoTask)
  {
    var realmId = autoTask.RealmId;
    var hasRouteControl = _context.Realms.AsNoTracking()
      .Where(r => r.Id == realmId)
      .Select(r => r.HasRouteControl)
      .FirstOrDefault();

    List<AutoTaskDetail> taskDetails = [];
    if (autoTask.CurrentProgressId == (int)ProgressState.PreMoving)
    {
      var firstTaskDetail = await _context.AutoTasksDetail.AsNoTracking()
        .Where(t => t.AutoTaskId == autoTask.Id)
        .Include(t => t.Waypoint)
        .OrderBy(t => t.Order)
        .FirstOrDefaultAsync();
      if (firstTaskDetail != null)
        taskDetails.Add(firstTaskDetail);
    }
    else if (autoTask.CurrentProgressId == (int)ProgressState.Moving)
    {
      taskDetails = await _context.AutoTasksDetail.AsNoTracking()
        .Where(t => t.AutoTaskId == autoTask.Id)
        .Include(t => t.Waypoint)
        .OrderBy(t => t.Order)
        .ToListAsync();
    }
    if (taskDetails.Count == 0)
    {
      return [];
    }
    
    // Return the waypoints
    List<RobotClientsDof> paths = [];
    foreach (var t in taskDetails)
    {
      paths.Add(GenerateWaypoint(t));
    }
    return paths;
  }
}