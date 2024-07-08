# Set up Logging

### Server setup

Install these NuGet packages to the Server:

- Serilog.AspNetCore
- Serilog.AspNetCore.Ingestion (Prerelease)

Add `.UseSerilog()` before build:

```csharp
...
builder.Host.UseSerilog((hbc, lc) => lc.ReadFrom.Configuration(hbc.Configuration));

var app = builder.Build();
...
```

After configuring the HTTP request pipeline add these statements:

```csharp
app.UseSerilogIngestion();
app.UseSerilogRequestLogging();
```

Add the following Serilog section to the `appsettings.json` file:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Warning"
    },
    "WriteTo": [
      {
        "Name": "File",
        "Args": {
          "path": "./logs/log-.txt",
          "rollingInterval": "Day"
        }
      }
    ]
  }
}
```

### Client setup

Add the following NuGet packages:

- Microsoft.Extensions.Logging.Configuration
- Serilog.Extensions.Logging
- Serilog.Sinks.BrowserHttp (Prerelease)

Add this to the client `Program.cs`

```csharp
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
```