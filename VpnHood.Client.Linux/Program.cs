using Microsoft.Extensions.Logging;
using VpnHood.Client.App;
using VpnHood.Client.App.Resources;
using VpnHood.Client.App.WebServer;
using VpnHood.Client.Linux.Device;
using VpnHood.Common.Logging;

namespace VpnHood.Client.Linux;

internal class Program
{
    private static async Task Main(string[] args)
    {
        _ = args;
        Console.WriteLine("Starting VpnHood Client for linux.");

        try
        {
            // check command line
            // initialize VpnHoodApp
            VpnHoodApp.Init(new LinuxDevice(), new AppOptions
            {
                Resource = DefaultAppResource.Resource,
                AccessKeys = [ClientOptions.SampleAccessKey]
            });

            // initialize SPA
            ArgumentNullException.ThrowIfNull(VpnHoodApp.Instance.Resource.SpaZipData);
            using var spaResource = new MemoryStream(VpnHoodApp.Instance.Resource.SpaZipData);
            var webServer = VpnHoodAppWebServer.Init(spaResource);
            Console.WriteLine($"Navigate {webServer.Url} to open VpnHood Client.");
            await Task.Delay(TimeSpan.FromMinutes(60));
        }
        catch (Exception ex)
        {
            VhLogger.Instance.LogError(ex, "Could not run the app.");
            throw;
        }
    }
}