using VpnHood.Client.Device;

namespace VpnHood.Client.Linux.Device;

public class LinuxDevice : IDevice
{
#pragma warning disable 0067
    public event EventHandler? OnStartAsService;
#pragma warning restore 0067

    public string OsInfo => Environment.OSVersion + ", " + (Environment.Is64BitOperatingSystem ? "64-bit" : "32-bit");
    public bool IsAlwaysOnSupported => false;
    public bool IsExcludeAppsSupported => false;
    public bool IsLogToConsoleSupported => true;
    public bool IsIncludeAppsSupported => false;

    public DeviceAppInfo[] InstalledApps => throw new NotSupportedException();
    public event EventHandler? StartedAsService;

    public Task<IPacketCapture> CreatePacketCapture(IUiContext? uiContext)
    {
        var res = (IPacketCapture)new LinuxPacketCapture();
        return Task.FromResult(res);
    }

    public void Dispose()
    {
    }
}