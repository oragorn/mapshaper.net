# Mapshaper.Net

[![CI](https://github.com/oragorn/mapshaper.net/actions/workflows/ci.yml/badge.svg)](https://github.com/oragorn/mapshaper.net/actions/workflows/ci.yml)

A thin .NET wrapper around the external [mapshaper](https://github.com/mbloch/mapshaper) CLI.

This package does not bundle mapshaper. Install mapshaper separately and ensure `mapshaper` is available on `PATH`, or pass the executable path through `MapshaperOptions`.

## Features

- Runs the external mapshaper CLI from .NET code.
- Captures exit code, standard output, standard error, and the exact argument list used.
- Provides convenience methods for common workflows like convert, simplify, clean, erase, join, rename fields, and inspect info.
- Includes a fluent pipeline builder for composing mapshaper command sequences.
- Supports raw mapshaper arguments for commands that are not modeled directly.
- Targets `netstandard2.0` for broad .NET compatibility.

## Requirements

- A .NET runtime compatible with `netstandard2.0`. The library is intended to run on major .NET-supported operating systems, including Windows, Linux, and macOS.
- .NET SDK for development and testing.
- Node.js and the external `mapshaper` CLI at runtime.

## Thin wrapper philosophy

Mapshaper.Net does not parse geospatial files or reimplement mapshaper behavior. It invokes the external mapshaper CLI safely, passes arguments through in a predictable way, and captures the process output so .NET code can inspect success, errors, stdout, and stderr.

## Install mapshaper

Mapshaper requires Node.js. After installing Node.js, install the mapshaper command line tools globally with npm:

```shell
npm install -g mapshaper
```

Confirm the executable is available:

```shell
mapshaper -v
```

If `mapshaper` is not on `PATH`, pass the executable name or path through configuration. The exact path depends on the operating system and installation method.

```csharp
var client = new MapshaperClient(new MapshaperOptions
{
    ExecutablePath = Environment.GetEnvironmentVariable("MAPSHAPER_PATH") ?? "mapshaper"
});
```

## Install Mapshaper.Net

Install the package from NuGet:

```shell
dotnet add package Mapshaper.Net
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

Use high-level helpers for common file workflows:

```csharp
await client.CleanAsync("input.geojson", "clean.geojson");
await client.RenameFieldsAsync("clean.geojson", ["NAME=label"], "renamed.geojson");

var info = await client.InfoAsync("renamed.geojson");
```

Use `CreatePipeline()` to compose thin wrappers for mapshaper commands:

```csharp
var result = await client
    .CreatePipeline("input.geojson")
    .Clean()
    .Filter("POP > 0")
    .RenameFields("NAME=label")
    .Output("output.geojson", new MapshaperOutputOptions { Force = true })
    .RunAsync();
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
    ExecutablePath = Environment.GetEnvironmentVariable("MAPSHAPER_PATH") ?? "mapshaper"
});
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

```csharp
await client
    .CreatePipeline("input.geojson")
    .Simplify("10%")
    .Output("output.geojson")
    .RunOrThrowAsync();
```

## Development

Restore, build, and test the repository with the .NET SDK:

```shell
dotnet restore
dotnet build
dotnet test
```

Integration tests that execute mapshaper require the `mapshaper` command to be installed and available on `PATH`.

## Documentation

- [Documentation index](docs/README.md)
- [Getting Started](docs/Getting-Started.md)
- [Supported Functions](docs/Supported-Functions.md)

## Releasing

Package versions are derived from Git tags. See [RELEASE.md](RELEASE.md) for the release process and [CHANGELOG.md](CHANGELOG.md) for version history.

## Contributing

Contributions are welcome. Please read [CONTRIBUTING.md](CONTRIBUTING.md) before opening an issue or pull request.

## License

This project is licensed under the MIT License. See [LICENSE](LICENSE) for details.
