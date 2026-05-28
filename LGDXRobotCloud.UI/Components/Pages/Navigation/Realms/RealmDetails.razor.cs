using System.Text;
using LGDXRobotCloud.UI.Client;
using LGDXRobotCloud.UI.Constants;
using LGDXRobotCloud.UI.Helpers;
using LGDXRobotCloud.UI.Services;
using LGDXRobotCloud.UI.ViewModels.Navigation;
using LGDXRobotCloud.UI.ViewModels.Shared;
using LGDXRobotCloud.Utilities.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Kiota.Abstractions;

namespace LGDXRobotCloud.UI.Components.Pages.Navigation.Realms;
public partial class RealmDetails : ComponentBase
{
  [Inject]
  public required NavigationManager NavigationManager { get; set; } = default!;

  [Inject]
  public required LgdxApiClient LgdxApiClient { get; set; }

  [Inject]
  public required ICachedRealmService CachedRealmService { get; set; }

  [Inject]
  public required ITokenService TokenService { get; set; }

  [Inject]
  public required AuthenticationStateProvider AuthenticationStateProvider { get; set; }

  [Parameter]
  public int? Id { get; set; }

  private int CurrentRealmId { get; set; } = 0;
  private RealmDetailsViewModel RealmDetailsViewModel { get; set; } = new();
  private DeleteEntryModalViewModel DeleteEntryModalViewModel { get; set; } = new();
  private EditContext _editContext = null!;
  private readonly CustomFieldClassProvider _customFieldClassProvider = new();

  private void LoadMap(InputFileChangeEventArgs e)
  {
    if (e.File != null)
    {
      RealmDetailsViewModel.SelectedMap = e.File;
    }
  }

  private void LoadSelectedKeepoutMask(InputFileChangeEventArgs e)
  {
    if (e.File != null)
    {
      RealmDetailsViewModel.SelectedKeepoutMask = e.File;
    }
  }

  private void LoadSelectedSpeedMask(InputFileChangeEventArgs e)
  {
    if (e.File != null)
    {
      RealmDetailsViewModel.SelectedSpeedMask = e.File;
    }
  }

  private static string ReadLine(BinaryReader reader)
  {
    var sb = new StringBuilder();
    char c;
    while ((c = reader.ReadChar()) != '\n')
      sb.Append(c);
    return sb.ToString();
  }

  private async Task<(string data, int width, int height)> ReadPgmFile(IBrowserFile file)
  {
    if (file.Size > LgdxApiConstants.ImageMaxSize)
    {
      throw new Exception("The file size is too large.");
    }

    // Save the file to a temporary location
    string path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.pgm");
    await using FileStream tempFs = new(path, FileMode.Create);
    await file.OpenReadStream(LgdxApiConstants.ImageMaxSize).CopyToAsync(tempFs);
    tempFs.Close();

    // Read the file
    await using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
    using var reader = new BinaryReader(fs);

    string magic = new(reader.ReadChars(2));
    if (magic != "P5")
        throw new Exception("Only P5 supported");

    // skip whitespace
    reader.ReadByte(); // '\n'

    // read width/height line
    string sizeLine = ReadLine(reader);
    var parts = sizeLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    int width = int.Parse(parts[0]);
    int height = int.Parse(parts[1]);

    // read maxval line
    _ = ReadLine(reader);

    // Find Start of Image Data
    byte[] data = new byte[width * height];
    await fs.ReadExactlyAsync(data);

    return (Convert.ToBase64String(data), width, height);
  }

  public async Task HandleValidSubmit()
  {
    // Read Map File
    try
    {
      var file = RealmDetailsViewModel.SelectedMap;
      if (file != null)
      {
        var (data, width, height) = await ReadPgmFile(file);
        RealmDetailsViewModel.Map = data;
        RealmDetailsViewModel.MapWidth = width;
        RealmDetailsViewModel.MapHeight = height;
      }
    }
    catch (Exception ex)
    {
      RealmDetailsViewModel.Errors = [];
      RealmDetailsViewModel.Errors.Add(nameof(RealmDetailsViewModel.SelectedMap), ex.Message);
      return;
    }

    // Read Keepout Mask File
    try {
      var file = RealmDetailsViewModel.SelectedKeepoutMask;
      if (file != null)
      {
        var (data, width, height) = await ReadPgmFile(file);
        RealmDetailsViewModel.KeepoutMask = data;
        if (width != RealmDetailsViewModel.MapWidth || height != RealmDetailsViewModel.MapHeight)
        {
          throw new Exception("Keepout mask size does not match map size.");
        }
      }
    }
    catch (Exception ex)
    {
      RealmDetailsViewModel.Errors = [];
      RealmDetailsViewModel.Errors.Add(nameof(RealmDetailsViewModel.SelectedKeepoutMask), ex.Message);
      return;
    }

    // Read Speed Mask File
    try {
      var file = RealmDetailsViewModel.SelectedSpeedMask;
      if (file != null)
      {
        var (data, width, height) = await ReadPgmFile(file);
        RealmDetailsViewModel.SpeedMask = data;
        if (width != RealmDetailsViewModel.MapWidth || height != RealmDetailsViewModel.MapHeight)
        {
          throw new Exception("Keepout mask size does not match map size.");
        }
      }
    }
    catch (Exception ex)
    {
      RealmDetailsViewModel.Errors = [];
      RealmDetailsViewModel.Errors.Add(nameof(RealmDetailsViewModel.SelectedSpeedMask), ex.Message);
      return;
    }

    // Save
    try
    {
      if (Id != null)
      {
        // Update
        await LgdxApiClient.Navigation.Realms[(int)Id].PutAsync(RealmDetailsViewModel.ToUpdateDto());
        CachedRealmService.ClearCache((int)Id);
      }
      else
      {
        // Create
        await LgdxApiClient.Navigation.Realms.PostAsync(RealmDetailsViewModel.ToCreateDto());
      }
      NavigationManager.NavigateTo(AppRoutes.Navigation.Realms.Index);
    }
    catch (ApiException ex)
    {
      RealmDetailsViewModel.Errors = ApiHelper.GenerateErrorDictionary(ex);
    }
  }

  public async Task HandleTestDelete()
  {
    DeleteEntryModalViewModel.Errors = null;
    try
    {
      await LgdxApiClient.Navigation.Realms[(int)Id!].TestDelete.PostAsync();
      DeleteEntryModalViewModel.IsReady = true;
    }
    catch (ApiException ex)
    {
      DeleteEntryModalViewModel.Errors = ApiHelper.GenerateErrorDictionary(ex);
    }
  }

  public async Task HandleDelete()
  {
    try
    {
      await LgdxApiClient.Navigation.Realms[(int)Id!].DeleteAsync();
      NavigationManager.NavigateTo(AppRoutes.Navigation.Realms.Index);
    }
    catch (ApiException ex)
    {
      RealmDetailsViewModel.Errors = ApiHelper.GenerateErrorDictionary(ex);
    }
  }

  protected override void OnInitialized()
  {
    var user = AuthenticationStateProvider.GetAuthenticationStateAsync().Result.User;
    var settings = TokenService.GetSessionSettings(user);
    CurrentRealmId = settings.CurrentRealmId;
    base.OnInitializedAsync();
  }

  public override async Task SetParametersAsync(ParameterView parameters)
  {
    parameters.SetParameterProperties(this);
    if (parameters.TryGetValue<int?>(nameof(Id), out var _id) && _id != null)
    {
      var response = await LgdxApiClient.Navigation.Realms[(int)_id].GetAsync();
      RealmDetailsViewModel.FromDto(response!);
      _editContext = new EditContext(RealmDetailsViewModel);
      _editContext.SetFieldCssClassProvider(_customFieldClassProvider);
    }
    else
    {
      _editContext = new EditContext(RealmDetailsViewModel);
      _editContext.SetFieldCssClassProvider(_customFieldClassProvider);
    }
    await base.SetParametersAsync(ParameterView.Empty);
  }
}