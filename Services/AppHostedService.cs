using Microsoft.Extensions.Hosting;
using System.Text;

namespace TopLibraryManager.Services;

public class AppHostedService(IHostApplicationLifetime appLifetime) : BackgroundService
{
    private readonly IHostApplicationLifetime _appLifetime = appLifetime;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("Привет мир!");

        _appLifetime.StopApplication();
    }
}