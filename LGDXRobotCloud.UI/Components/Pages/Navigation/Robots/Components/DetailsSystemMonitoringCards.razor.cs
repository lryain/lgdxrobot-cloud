
using LGDXRobotCloud.Data.Models.Redis;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace LGDXRobotCloud.UI.Components.Pages.Navigation.Robots.Components;

public partial class DetailsSystemMonitoringCards : ComponentBase, IDisposable
{

  [Inject]
  public required IJSRuntime JSRuntime { get; set; }

  private DotNetObjectReference<DetailsSystemMonitoringCards> ObjectReference = null!;
  private RobotData? RobotData { get; set; }
  bool Initalised = false;

  private async Task UpdateCharts()
  {
    if (!Initalised || RobotData == null)
    {
      return;
    }
    await JSRuntime.InvokeVoidAsync("UpdateChartCpuUsage", Math.Round(RobotData!.SystemMonitoringInfo.CpuUsage, 1));
    float memoryUsage = (float)RobotData.SystemMonitoringInfo.MemoryUsed / RobotData.SystemMonitoringInfo.MemoryTotal * 100.0f;
    await JSRuntime.InvokeVoidAsync("UpdateChartMemoryUsage", Math.Round(memoryUsage, 1));
    float swapUsage = (float)RobotData.SystemMonitoringInfo.SwapUsed / RobotData.SystemMonitoringInfo.SwapTotal * 100.0f;
    await JSRuntime.InvokeVoidAsync("UpdateChartSwapUsage", Math.Round(swapUsage, 1));
    float diskUsage = (float)RobotData.SystemMonitoringInfo.DiskUsed / RobotData.SystemMonitoringInfo.DiskTotal * 100.0f;
    await JSRuntime.InvokeVoidAsync("UpdateChartDiskUsage", Math.Round(diskUsage, 1));
  }

  public async Task Refresh(RobotData? rd)
  {
    RobotData = rd;
    await UpdateCharts();
    await InvokeAsync(StateHasChanged);
  }

  static private string GetGiB(int value)
  {
    return $"{Math.Round(value / 1024.0, 1)} GB";
  }

  protected override async Task OnAfterRenderAsync(bool firstRender)
  {
    await base.OnAfterRenderAsync(firstRender);
    if (firstRender)
    {
      ObjectReference = DotNetObjectReference.Create(this);
      await JSRuntime.InvokeVoidAsync("InitDotNet", ObjectReference);
      await JSRuntime.InvokeVoidAsync("GenerateChartCpuUsage");
      await JSRuntime.InvokeVoidAsync("GenerateChartMemoryUsage");
      await JSRuntime.InvokeVoidAsync("GenerateChartSwapUsage");
      await JSRuntime.InvokeVoidAsync("GenerateChartDiskUsage");
      Initalised = true;
      await UpdateCharts();
    }
  }

  public void Dispose()
  {
    ObjectReference?.Dispose();
    GC.SuppressFinalize(this);
  }
}