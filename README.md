# Mapshaper.Net

A thin .NET wrapper around the external [mapshaper](https://github.com/mbloch/mapshaper) CLI.

This package does not bundle mapshaper. Install mapshaper separately and ensure `mapshaper` is available on `PATH`, or pass the executable path through `MapshaperOptions`.

## Install mapshaper

Mapshaper requires Node.js. After installing Node.js, install the mapshaper command line tools globally with npm:

```powershell
npm install -g mapshaper
```

Confirm the executable is available:

```powershell
mapshaper -v
```

If `mapshaper` is not on `PATH`, pass the full path to the installed executable or shim.

```csharp
var client = new MapshaperClient(new MapshaperOptions
{
    ExecutablePath = @"C:\Users\you\AppData\Roaming\npm\mapshaper.cmd"
});
```

## Usage

```csharp
using Mapshaper.Net;

var client = new MapshaperClient();

var result = await client.SimplifyAsync(
    "input.geojson",
    "output.geojson",
    "10%");

result.EnsureSuccess();
```

Use `RunAsync()` for raw mapshaper arguments:

```csharp
var result = await client.RunAsync("input.geojson", "-simplify", "10%", "-o", "output.geojson");
```

Configure the executable:

```csharp
var client = new MapshaperClient(new MapshaperOptions
{
    ExecutablePath = @"C:\tools\mapshaper.cmd"
});
```
