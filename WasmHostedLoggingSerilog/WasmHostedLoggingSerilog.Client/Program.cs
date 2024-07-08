using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Serilog;
using Serilog.Extensions.Logging;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

var http = new HttpClient()
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
};
http.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { NoCache = true };
builder.Services.AddScoped(_ => http);

// !: Serilog configuration
var levelSwitch = new Serilog.Core.LoggingLevelSwitch();
Log.Logger = new LoggerConfiguration()
    .Enrich.WithProperty("InstanceId", Guid.NewGuid().ToString("n"))
    .WriteTo.BrowserHttp(http)
    .CreateLogger();

builder.Logging.AddProvider(new SerilogLoggerProvider());

await builder.Build().RunAsync();