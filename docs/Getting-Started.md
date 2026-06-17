# Getting Started

## Requirements

- .NET SDK for development and testing.
- Node.js and the external `mapshaper` CLI at runtime.

Mapshaper.Net does not bundle mapshaper. Install mapshaper separately and make sure `mapshaper` is available on `PATH`, or configure the executable path with `MapshaperOptions`.

## Install mapshaper

Install the mapshaper command line tools globally with npm:

```powershell
npm install -g mapshaper
```

Confirm the executable is available:

```powershell
mapshaper -v
```

If `mapshaper` is not on `PATH`, pass the full executable path:

```csharp
using Mapshaper.Net;

var client = new MapshaperClient(new MapshaperOptions
{
    ExecutablePath = @"C:\Users\you\AppData\Roaming\npm\mapshaper.cmd"
});
```

## Install Mapshaper.Net

Install the package from NuGet:

```powershell
dotnet add package Mapshaper.Net
```

## First use

Simplify a GeoJSON file:

```csharp
using Mapshaper.Net;

var client = new MapshaperClient();

var result = await client.SimplifyAsync(
    "input.geojson",
    "output.geojson",
    "10%");

result.EnsureSuccess();
```

Run raw mapshaper arguments:

```csharp
var result = await client.RunAsync(
    "input.geojson",
    "-simplify",
    "10%",
    "-o",
    "output.geojson");
```

Compose a fluent command pipeline:

```csharp
var result = await client
    .CreatePipeline("input.geojson")
    .Clean()
    .Filter("POP > 0")
    .RenameFields("NAME=label")
    .Output("output.geojson", new MapshaperOutputOptions { Force = true })
    .RunAsync();
```

## Handling failures

`RunAsync()` returns a `MapshaperResult` so callers can inspect the process result themselves:

```csharp
var result = await client.RunAsync("input.geojson", "-info");

if (!result.IsSuccess)
{
    Console.Error.WriteLine(result.StdErr);
}
```

Use `RunOrThrowAsync()` or `EnsureSuccess()` when unsuccessful mapshaper exits should throw a `MapshaperException`.

## Local development

Restore, build, and test the repository with the .NET SDK:

```powershell
dotnet restore
dotnet build
dotnet test
```

Integration tests that execute mapshaper require the `mapshaper` command to be installed and available on `PATH`.
