using LGDXRobotCloud.UI.Client;
using LGDXRobotCloud.UI.Client.Models;
using LGDXRobotCloud.UI.Helpers;
using LGDXRobotCloud.UI.Services;
using LGDXRobotCloud.UI.ViewModels.Navigation;
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
  private readonly CustomFieldClassProvider _customFieldClassProvider = new();
  private EditContext _editContext = null!;
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

  public void HandelResetMapEditor()
  {
    var user = AuthenticationStateProvider.GetAuthenticationStateAsync().Result.User;
    var settings = TokenService.GetSessionSettings(user);
    settings.MapEditorData = null;
    TokenService.UpdateSessionSettings(user, settings);
    NavigationManager.Refresh(true);
  }

  public async Task HandelSubmit()
  {
    MapEditorViewModel.ClearMessages();
    try
    {
      var realmId = Realm.Id ?? 0;
      await LgdxApiClient.Navigation.MapEditor[realmId].PostAsync(MapEditorViewModel.ToUpdateDto());
      MapEditorViewModel.IsSuccess = true;
    }
    catch (ApiException ex)
    {
      MapEditorViewModel.Errors = ApiHelper.GenerateErrorDictionary(ex);
    }
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
    _editContext = new EditContext(EditingWaypoint);
    _editContext.SetFieldCssClassProvider(_customFieldClassProvider);
    StateHasChanged();
  }

  [JSInvokable("HandleWaypointSelect")]
  public void HandleWaypointSelect(string waypointId)
  {
    IsEditingWaypoint = true;
    Guid id = Guid.Parse(waypointId);
    EditingWaypoint = MapEditorViewModel.Waypoints.FirstOrDefault(w => w.MapEditorObjectId == id)!;
    _editContext = new EditContext(EditingWaypoint);
    _editContext.SetFieldCssClassProvider(_customFieldClassProvider);
    StateHasChanged();
  }

  public async Task HandleWaypointSubmit()
  {
    if (EditingWaypoint.Id == null)
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
      MapEditorViewModel.Waypoints.RemoveAll(w => w.Id == EditingWaypoint.Id);
      MapEditorViewModel.Waypoints.Add(EditingWaypoint);
      await JSRuntime.InvokeVoidAsync("MapEditorMoveWaypoint", EditingWaypoint);
    }
    await JSRuntime.InvokeVoidAsync("HideSidebar");
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
      // Update View Model
      MapEditorViewModel.WaypointTraffics.Add(new WaypointTrafficViewModel
      {
        AlternativeWaypointFromId = SelectedFromWaypointId,
        AlternativeWaypointToId = SelectedToWaypointId,
      });
      if (isBothWaysTraffic)
      {
        MapEditorViewModel.WaypointTraffics.Add(new WaypointTrafficViewModel
        {
          AlternativeWaypointFromId = SelectedToWaypointId,
          AlternativeWaypointToId = SelectedFromWaypointId,
        });
      }
      // Update Map Editor
      var traffic = new WaypointTrafficDisplay
      {
        WaypointFromId = SelectedFromWaypointId,
        WaypointToId = SelectedToWaypointId,
        IsBothWaysTraffic = isBothWaysTraffic,
      };
      MapEditorViewModel.WaypointTrafficsDisplay.Add(traffic);
      List<WaypointTrafficDisplay> t1 = [traffic];
      await JSRuntime.InvokeVoidAsync("MapEditorAddTraffics", t1);
      SaveMapEditorViewModel();
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

  [JSInvokable("HandleDeleteTraffic")]
  public async Task HandleDeleteTraffic(string waypointIds)
  {
    var ids = waypointIds.Split('-');
    Guid fromWaypointId = Guid.Parse(ids[0]);
    Guid toWaypointId = Guid.Parse(ids[1]);

    // Delete model
    MapEditorViewModel.WaypointTraffics.RemoveAll(x => x.AlternativeWaypointFromId == fromWaypointId && x.AlternativeWaypointToId == toWaypointId);
    MapEditorViewModel.WaypointTraffics.RemoveAll(x => x.AlternativeWaypointFromId == toWaypointId && x.AlternativeWaypointToId == fromWaypointId);
    // Delete display
    MapEditorViewModel.WaypointTrafficsDisplay.RemoveAll(x => x.WaypointFromId == fromWaypointId && x.WaypointToId == toWaypointId);
    MapEditorViewModel.WaypointTrafficsDisplay.RemoveAll(x => x.WaypointFromId == toWaypointId && x.WaypointToId == fromWaypointId);

    await HandleMapEditorModeChange(MapEditorMode.Normal);
    SaveMapEditorViewModel();
    StateHasChanged();
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