using LGDXRobotCloud.UI.Client;
using LGDXRobotCloud.UI.Client.Models;
using LGDXRobotCloud.UI.Helpers;
using LGDXRobotCloud.UI.Services;
using LGDXRobotCloud.UI.ViewModels.Navigation;
using LGDXRobotCloud.Utilities.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Microsoft.Kiota.Abstractions;

namespace LGDXRobotCloud.UI.Components.Pages.Navigation.MapEditor;

public enum MapEditorMode
{
  Normal = 0,
  SingleWayTrafficFrom = 1,
  SingleWayTrafficTo = 2,
  BothWaysTrafficFrom = 3,
  BothWaysTrafficTo = 4,
  AddWaypoint = 5,
}

public enum MapEditorError
{
  None = 0,
  SameWaypoint = 1,
  HasTraffic = 2,
}

public partial class MapEditor : ComponentBase, IDisposable
{
  [Inject]
  public required LgdxApiClient LgdxApiClient { get; set; }

  [Inject]
  public required NavigationManager NavigationManager { get; set; } = default!;

  [Inject]
  public required ICachedRealmService CachedRealmService { get; set; }

  [Inject]
  public required IJSRuntime JSRuntime { get; set; }

  [Inject]
  public required ITokenService TokenService { get; set; }

  [Inject]
  public required AuthenticationStateProvider AuthenticationStateProvider { get; set; }

  private DotNetObjectReference<MapEditor> ObjectReference = null!;
  private string RealmName { get; set; } = string.Empty;
  private RealmDto Realm { get; set; } = null!;
  private MapEditorViewModel MapEditorViewModel { get; set; } = new()
  {
    IsSuccess = false,
  };
  bool HasRouteTrafficControl { get; set; } = false;

  // Map Editor
  private MapEditorMode MapEditorMode { get; set; } = MapEditorMode.Normal;
  private MapEditorError MapEditorError { get; set; } = MapEditorError.None;
  private Guid SelectedFromWaypointId { get; set; } = Guid.Empty;
  private Guid SelectedToWaypointId { get; set; } = Guid.Empty;
  
  // Edit Form
  bool IsEditingWaypoint { get; set; } = false;
  private WaypointDetailsViewModel EditingWaypoint { get; set; } = new();
  private WaypointTrafficViewModel EditingWaypointTraffic { get; set; } = new();
  private readonly CustomFieldClassProvider _customFieldClassProvider = new();
  private EditContext _editContextWaypoint = null!;
  private EditContext _editContextTraffic = null!;

  /*
  ** Map Editor
  */

  private async Task HandleMapEditorModeChange(MapEditorMode mode)
  {
    if (mode != MapEditorMode.Normal)
    {
      MapEditorError = MapEditorError.None;
    }

    if (mode == MapEditorMode.SingleWayTrafficFrom || mode == MapEditorMode.SingleWayTrafficTo)
    {
      // Avoid conflict with current editing
      await JSRuntime.InvokeVoidAsync("HideSidebar");
    }

    MapEditorMode = mode;
    await JSRuntime.InvokeVoidAsync("MapEditorSetMode", (int)mode);
  }

  private void SaveMapEditorViewModel()
  {
    var user = AuthenticationStateProvider.GetAuthenticationStateAsync().Result.User;
    var settings = TokenService.GetSessionSettings(user);
    settings.MapEditorData = MapEditorViewModel;
    TokenService.UpdateSessionSettings(user, settings);
  }

  public void HandelResetMapEditor(bool refresh = true)
  {
    var user = AuthenticationStateProvider.GetAuthenticationStateAsync().Result.User;
    var settings = TokenService.GetSessionSettings(user);
    settings.MapEditorData = null;
    TokenService.UpdateSessionSettings(user, settings);
    if (refresh)
    {
      NavigationManager.Refresh(true);
    }
  }

  public async Task HandelSubmit()
  {
    MapEditorViewModel.ClearMessages();

    // Validation
    HashSet<int> featureIds = [];
    foreach (var waypoint in MapEditorViewModel.Waypoints)
    {
      if (waypoint.FeatureId != null)
      {
        if (featureIds.Contains((int)waypoint.FeatureId))
        {
          MapEditorViewModel.Errors = [];
          MapEditorViewModel.Errors.Add(nameof(MapEditorViewModel.Waypoints), "Feature ID cannot duplicate.");
          return;
        }
        featureIds.Add((int)waypoint.FeatureId);
      }
      else if (HasRouteTrafficControl)
      {
        MapEditorViewModel.Errors = [];
        MapEditorViewModel.Errors.Add(nameof(MapEditorViewModel.Waypoints), "Feature ID is required when Route Control is enabled.");
        return;
      }
    }
    foreach (var traffic in MapEditorViewModel.WaypointTraffics)
    {
      if (featureIds.Contains((int)traffic.FeatureId!))
      {
        MapEditorViewModel.Errors = [];
        MapEditorViewModel.Errors.Add(nameof(MapEditorViewModel.WaypointTraffics), "Feature ID cannot duplicate.");
        return;
      }
      featureIds.Add((int)traffic.FeatureId);
    }

    Dictionary<Guid, int> objectIdToId = [];
    foreach (var waypoint in MapEditorViewModel.Waypoints)
    {
      if (waypoint.Id != null)
      {
        objectIdToId.Add((Guid)waypoint.MapEditorObjectId!, (int)waypoint.Id!);
      }
    }
    foreach (var traffic in MapEditorViewModel.WaypointTraffics)
    {
      var fromWaypoint = traffic.AlternativeWaypointFromId;
      if (objectIdToId.TryGetValue((Guid)fromWaypoint!, out var fromWaypointId))
      {
        traffic.WaypointFromId = fromWaypointId;
      }
      var toWaypoint = traffic.AlternativeWaypointToId;
      if (objectIdToId.TryGetValue((Guid)toWaypoint!, out var toWaypointId))
      {
        traffic.WaypointToId = toWaypointId;
      }
    }

    try
    {
      var realmId = Realm.Id ?? 0;
      await LgdxApiClient.Navigation.MapEditor[realmId].PostAsync(MapEditorViewModel.ToUpdateDto());
      MapEditorViewModel.IsSuccess = true;
      HandelResetMapEditor(false);
    }
    catch (ApiException ex)
    {
      MapEditorViewModel.Errors = ApiHelper.GenerateErrorDictionary(ex);
    }
  }

  public bool IsDuplicatingFeatureId(int? featureId)
  {
    if (featureId == null)
    {
      return true;
    }
    return MapEditorViewModel.Waypoints.Any(w => w.FeatureId == featureId)
      || MapEditorViewModel.WaypointTraffics.Any(w => w.FeatureId == featureId);
  }

  /*
  ** Waypoints
  */

  [JSInvokable("HandleWaypointCreate")]
  public async Task HandleWaypointCreate(double x, double y)
  {
    IsEditingWaypoint = true;
    await HandleMapEditorModeChange(MapEditorMode.Normal);
    EditingWaypoint = new WaypointDetailsViewModel {
      Id = null,
      RealmId = Realm.Id,
      RealmName = Realm.Name,
      X = x,
      Y = y,
    };
    _editContextWaypoint = new EditContext(EditingWaypoint);
    _editContextWaypoint.SetFieldCssClassProvider(_customFieldClassProvider);
    StateHasChanged();
  }

  [JSInvokable("HandleWaypointSelect")]
  public void HandleWaypointSelect(string waypointId)
  {
    IsEditingWaypoint = true;
    Guid id = Guid.Parse(waypointId);
    var waypoint = MapEditorViewModel.Waypoints.FirstOrDefault(w => w.MapEditorObjectId == id)!;
    EditingWaypoint = LgdxHelper.DeepCopy(waypoint);
    _editContextWaypoint = new EditContext(EditingWaypoint);
    _editContextWaypoint.SetFieldCssClassProvider(_customFieldClassProvider);
    StateHasChanged();
  }

  public async Task HandleWaypointSubmit()
  {
    if (HasRouteTrafficControl && EditingWaypoint.FeatureId == null)
    {
      EditingWaypoint.Errors = [];
      EditingWaypoint.Errors.Add(nameof(EditingWaypoint.FeatureId), "Feature ID is required when Route Control is enabled.");
      return;
    }
    if (IsDuplicatingFeatureId(EditingWaypoint.FeatureId))
    {
      EditingWaypoint.Errors = [];
      EditingWaypoint.Errors.Add(nameof(EditingWaypoint.FeatureId), "Feature ID is duplicated.");
      return;
    }

    if (EditingWaypoint.MapEditorObjectId == null)
    {
      // Create
      EditingWaypoint.MapEditorObjectId = Guid.NewGuid();
      MapEditorViewModel.Waypoints.Add(EditingWaypoint);
      List<WaypointDetailsViewModel> waypoints = [EditingWaypoint];
      await JSRuntime.InvokeVoidAsync("MapEditorAddWaypoints", waypoints);
    }
    else
    {
      // Update
      int index = MapEditorViewModel.Waypoints.FindIndex(w => w.MapEditorObjectId == EditingWaypoint.MapEditorObjectId);
      MapEditorViewModel.Waypoints[index] = EditingWaypoint;
      await JSRuntime.InvokeVoidAsync("MapEditorMoveWaypoint", EditingWaypoint);
    }
    await JSRuntime.InvokeVoidAsync("HideSidebar");
    SaveMapEditorViewModel();
  }

  public async Task HandleWaypointDelete()
  {
    MapEditorViewModel.Waypoints.RemoveAll(w => w.MapEditorObjectId == EditingWaypoint.MapEditorObjectId);
    await JSRuntime.InvokeVoidAsync("MapEditorDeleteWaypoint", EditingWaypoint.MapEditorObjectId);
    await JSRuntime.InvokeVoidAsync("HideSidebar");
  }

  /*
  ** Traffic
  */

  public async Task CheckAndAddTraffic(bool isBothWaysTraffic)
  {
    bool isValid = true;
    // The two waypoint must not be the same
    if (SelectedFromWaypointId == SelectedToWaypointId)
    {
      isValid = false;
      MapEditorError = MapEditorError.SameWaypoint;
    }
    // The two waypoint must not have any traffic
    if (MapEditorViewModel.WaypointTraffics.Any(x => x.AlternativeWaypointFromId == SelectedFromWaypointId && x.AlternativeWaypointToId == SelectedToWaypointId)
      || MapEditorViewModel.WaypointTraffics.Any(x => x.AlternativeWaypointFromId == SelectedToWaypointId && x.AlternativeWaypointToId == SelectedFromWaypointId))
    {
      isValid = false;
      MapEditorError = MapEditorError.HasTraffic;
    }

    if (isValid)
    {
      IsEditingWaypoint = false;
      var fromWaypoint = MapEditorViewModel.Waypoints.FirstOrDefault(x => x.MapEditorObjectId == SelectedFromWaypointId);
      var toWaypoint = MapEditorViewModel.Waypoints.FirstOrDefault(x => x.MapEditorObjectId == SelectedToWaypointId);
      EditingWaypointTraffic = new WaypointTrafficViewModel
      {
        IsBothWaysTraffic = isBothWaysTraffic,
        WaypointFromName = fromWaypoint?.Name,
        WaypointToName = toWaypoint?.Name,
        WaypointFromFeatureId = fromWaypoint?.FeatureId,
        WaypointToFeatureId = toWaypoint?.FeatureId,
      };
      _editContextTraffic = new EditContext(EditingWaypointTraffic);
      _editContextTraffic.SetFieldCssClassProvider(_customFieldClassProvider);
      await JSRuntime.InvokeVoidAsync("ShowSidebar");
      StateHasChanged();
    }
    await HandleMapEditorModeChange(MapEditorMode.Normal);
  }

  [JSInvokable("HandleAddTraffic")]
  public async Task HandleAddTraffic(string waypointId)
  {
    Guid id = Guid.Parse(waypointId);
    switch (MapEditorMode)
    {
      case MapEditorMode.SingleWayTrafficFrom:
        // Save WaypointFromId and ask WaypointToId
        SelectedFromWaypointId = id;
        await HandleMapEditorModeChange(MapEditorMode.SingleWayTrafficTo);
        break;
      case MapEditorMode.SingleWayTrafficTo:
        // Save WaypointToId and update map
        SelectedToWaypointId = id;
        await CheckAndAddTraffic(false);
        break;
      case MapEditorMode.BothWaysTrafficFrom:
        // Save WaypointFromId and ask WaypointToId
        SelectedFromWaypointId = id;
        await HandleMapEditorModeChange(MapEditorMode.BothWaysTrafficTo);
        break;
      case MapEditorMode.BothWaysTrafficTo:
        // Save WaypointToId and update map
        SelectedToWaypointId = id;
        await CheckAndAddTraffic(true);
        break;
    }
    StateHasChanged();
  }

  [JSInvokable("HandleSelectTraffic")]
  public void HandleSelectTraffic(string waypointIds)
  {
    IsEditingWaypoint = false;
    var ids = waypointIds.Split('&');
    Guid fromWaypointId = Guid.Parse(ids[0]);
    Guid toWaypointId = Guid.Parse(ids[1]);

    var fromWaypoint = MapEditorViewModel.Waypoints.FirstOrDefault(x => x.MapEditorObjectId == fromWaypointId);
    var toWaypoint = MapEditorViewModel.Waypoints.FirstOrDefault(x => x.MapEditorObjectId == toWaypointId);

    var traffic = MapEditorViewModel.WaypointTraffics
      .FirstOrDefault(x => x.AlternativeWaypointFromId == fromWaypointId && x.AlternativeWaypointToId == toWaypointId)!;
    EditingWaypointTraffic = LgdxHelper.DeepCopy(traffic);
    EditingWaypointTraffic.WaypointFromName = fromWaypoint?.Name;
    EditingWaypointTraffic.WaypointToName = toWaypoint?.Name;
    EditingWaypointTraffic.WaypointFromFeatureId = fromWaypoint?.FeatureId;
    EditingWaypointTraffic.WaypointToFeatureId = toWaypoint?.FeatureId;

    var reverseWaypoint = MapEditorViewModel.WaypointTraffics
      .FirstOrDefault(x => x.AlternativeWaypointFromId == toWaypointId && x.AlternativeWaypointToId == fromWaypointId);
    if (reverseWaypoint != null)
    {
      EditingWaypointTraffic.IsBothWaysTraffic = true;
      EditingWaypointTraffic.ReverseFeatureId = reverseWaypoint?.FeatureId;
    }
    else
    {
      EditingWaypointTraffic.IsBothWaysTraffic = false;
    }
    
    _editContextTraffic = new EditContext(EditingWaypointTraffic);
    _editContextTraffic.SetFieldCssClassProvider(_customFieldClassProvider);
    StateHasChanged();
  }

  [JSInvokable("HandleDeleteTraffic")]
  public async Task HandleDeleteTraffic()
  {
    // Delete model
    MapEditorViewModel.WaypointTraffics.RemoveAll(x => x.AlternativeWaypointFromId == EditingWaypointTraffic.AlternativeWaypointFromId && x.AlternativeWaypointToId == EditingWaypointTraffic.AlternativeWaypointToId);
    MapEditorViewModel.WaypointTraffics.RemoveAll(x => x.AlternativeWaypointFromId == EditingWaypointTraffic.AlternativeWaypointToId && x.AlternativeWaypointToId == EditingWaypointTraffic.AlternativeWaypointFromId);
    // Delete display
    MapEditorViewModel.WaypointTrafficsDisplay.RemoveAll(x => x.WaypointFromId == EditingWaypointTraffic.AlternativeWaypointFromId && x.WaypointToId == EditingWaypointTraffic.AlternativeWaypointToId);
    MapEditorViewModel.WaypointTrafficsDisplay.RemoveAll(x => x.WaypointFromId == EditingWaypointTraffic.AlternativeWaypointToId && x.WaypointToId == EditingWaypointTraffic.AlternativeWaypointFromId);
    await JSRuntime.InvokeVoidAsync("HandleDeleteTraffic", EditingWaypointTraffic.AlternativeWaypointFromId, EditingWaypointTraffic.AlternativeWaypointToId);
    await JSRuntime.InvokeVoidAsync("HideSidebar");
    SaveMapEditorViewModel();
    StateHasChanged();
  }

  public async Task HandleTrafficSubmit()
  {
    if (HasRouteTrafficControl)
    {
      if (EditingWaypointTraffic.IsBothWaysTraffic && EditingWaypointTraffic.ReverseFeatureId == null)
      {
        EditingWaypointTraffic.Errors = [];
        EditingWaypointTraffic.Errors.Add(nameof(EditingWaypointTraffic.ReverseFeatureId), "Reverse Feature ID is required when Route Control is enabled.");
        return;
      }
    }
    if (IsDuplicatingFeatureId(EditingWaypointTraffic.FeatureId))
    {
      EditingWaypointTraffic.Errors = [];
      EditingWaypointTraffic.Errors.Add(nameof(EditingWaypointTraffic.FeatureId), "Feature ID is duplicated.");
      return;
    }

    if (EditingWaypointTraffic.MapEditorObjectId == null)
    {
      // Create
      EditingWaypointTraffic.MapEditorObjectId = Guid.NewGuid();
      EditingWaypointTraffic.AlternativeWaypointFromId = SelectedFromWaypointId;
      EditingWaypointTraffic.AlternativeWaypointToId = SelectedToWaypointId;
      MapEditorViewModel.WaypointTraffics.Add(EditingWaypointTraffic);
      if (EditingWaypointTraffic.IsBothWaysTraffic)
      {
        var second = LgdxHelper.DeepCopy(EditingWaypointTraffic);
        second.MapEditorObjectId = Guid.NewGuid();
        second.FeatureId = (int)EditingWaypointTraffic.ReverseFeatureId!;
        second.AlternativeWaypointFromId = SelectedToWaypointId;
        second.AlternativeWaypointToId = SelectedFromWaypointId;
        MapEditorViewModel.WaypointTraffics.Add(second);
      }
      // Update Map Editor
      var traffic = new WaypointTrafficDisplay
      {
        WaypointFromId = SelectedFromWaypointId,
        WaypointToId = SelectedToWaypointId,
        IsBothWaysTraffic = EditingWaypointTraffic.IsBothWaysTraffic,
      };
      MapEditorViewModel.WaypointTrafficsDisplay.Add(traffic);
      List<WaypointTrafficDisplay> t1 = [traffic];
      await JSRuntime.InvokeVoidAsync("MapEditorAddTraffics", t1);
      SaveMapEditorViewModel();
    }
    else
    {
      int index = MapEditorViewModel.WaypointTraffics
        .FindIndex(x => x.AlternativeWaypointFromId == EditingWaypointTraffic.AlternativeWaypointFromId && x.AlternativeWaypointToId == EditingWaypointTraffic.AlternativeWaypointToId);
      MapEditorViewModel.WaypointTraffics[index] = EditingWaypointTraffic;

      if (EditingWaypointTraffic.IsBothWaysTraffic)
      {
        var second = LgdxHelper.DeepCopy(EditingWaypointTraffic);
        index = MapEditorViewModel.WaypointTraffics
          .FindIndex(x => x.AlternativeWaypointFromId == EditingWaypointTraffic.AlternativeWaypointToId && x.AlternativeWaypointToId == EditingWaypointTraffic.AlternativeWaypointFromId);
        second.AlternativeWaypointFromId = EditingWaypointTraffic.AlternativeWaypointToId;
        second.AlternativeWaypointToId = EditingWaypointTraffic.AlternativeWaypointFromId;
        second.FeatureId = (int)EditingWaypointTraffic.ReverseFeatureId!;
        MapEditorViewModel.WaypointTraffics[index] = second;
      }
    }

    await JSRuntime.InvokeVoidAsync("HideSidebar");
    SaveMapEditorViewModel();
  }

  protected override async Task OnInitializedAsync()
  {
    // Get Realm
    var user = AuthenticationStateProvider.GetAuthenticationStateAsync().Result.User;
    var settings = TokenService.GetSessionSettings(user);
    Realm = await CachedRealmService.GetCurrrentRealmAsync(settings.CurrentRealmId);
    RealmName = Realm.Name ?? string.Empty;
    HasRouteTrafficControl = await CachedRealmService.GetHasRouteTrafficControlAsync(settings.CurrentRealmId);
    _editContextWaypoint = new EditContext(EditingWaypoint);
    _editContextWaypoint.SetFieldCssClassProvider(_customFieldClassProvider);
    _editContextTraffic = new EditContext(EditingWaypointTraffic);
    _editContextTraffic.SetFieldCssClassProvider(_customFieldClassProvider);
    await base.OnInitializedAsync();
  }

  protected override async Task OnAfterRenderAsync(bool firstRender)
  {
    if (firstRender)
    {
      ObjectReference = DotNetObjectReference.Create(this);
      await JSRuntime.InvokeVoidAsync("InitNavigationMap", ObjectReference);
      if (!string.IsNullOrWhiteSpace(Realm.Map))
      {
        var tempMap = Convert.FromBase64String(Realm.Map);
        await JSRuntime.InvokeVoidAsync("UpdateMap", Realm.MapWidth, Realm.MapHeight, tempMap, ObjectReference);
      }

      // Get Map Editor data
      var user = AuthenticationStateProvider.GetAuthenticationStateAsync().Result.User;
      var settings = TokenService.GetSessionSettings(user);
      var mapEditor = settings.MapEditorData;
      if (mapEditor != null)
      {
        MapEditorViewModel = mapEditor;
      }
      else
      {
        var realmId = Realm.Id ?? 0;
        var me = await LgdxApiClient.Navigation.MapEditor[realmId].GetAsync();
        if (me != null)
          MapEditorViewModel.FromDto(me);
      }
      // Update Map Editor
      if (MapEditorViewModel.Waypoints.Count > 0)
      {
        await JSRuntime.InvokeVoidAsync("MapEditorAddWaypoints", MapEditorViewModel.Waypoints);
      }
      if (MapEditorViewModel.WaypointTrafficsDisplay.Count > 0)
      {
        await JSRuntime.InvokeVoidAsync("MapEditorAddTraffics", MapEditorViewModel.WaypointTrafficsDisplay);
      }
    }
    await base.OnAfterRenderAsync(firstRender);
  }

  public void Dispose()
  {
    ObjectReference?.Dispose();
    GC.SuppressFinalize(this);
  }
}