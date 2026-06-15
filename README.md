# Mapshaper.Net

A thin .NET wrapper around the external [mapshaper](https://github.com/mbloch/mapshaper) CLI.

This package does not bundle mapshaper. Install mapshaper separately and ensure `mapshaper` is available on `PATH`, or pass the executable path through `MapshaperOptions`.

## Thin wrapper philosophy

Mapshaper.Net does not parse geospatial files or reimplement mapshaper behavior. It invokes the external mapshaper CLI safely, passes arguments through in a predictable way, and captures the process output so .NET code can inspect success, errors, stdout, and stderr.

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

Pass modeled options for common import and output flags:

```csharp
var result = await client.ConvertAsync(
    "input.geojson",
    "output.json",
    new MapshaperCommandOptions
    {
        Quiet = true,
        Import = new MapshaperImportOptions
        {
            Encoding = "utf8",
            IdField = "SOURCE_ID"
        },
        Output = new MapshaperOutputOptions
        {
            Format = "geojson",
            Precision = "0.000001",
            Force = true
        }
    });
```

Configure the executable:

```csharp
var client = new MapshaperClient(new MapshaperOptions
{
    ExecutablePath = @"C:\tools\mapshaper.cmd"
});
```
