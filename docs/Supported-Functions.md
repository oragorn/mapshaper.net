# Supported Functions

Mapshaper.Net exposes a small set of high-level convenience methods and a broader fluent pipeline for mapshaper commands. The package remains a thin wrapper: command behavior, accepted command arguments, and geospatial processing results come from the external `mapshaper` CLI.

## MapshaperClient

`MapshaperClient` is the main entry point for running mapshaper.

| Function | CLI command | Purpose |
| --- | --- | --- |
| `RunAsync(params string[] arguments)` | Any raw arguments | Runs mapshaper with the exact arguments supplied by the caller. |
| `RunAsync(IEnumerable<string> arguments, CancellationToken cancellationToken = default)` | Any raw arguments | Runs mapshaper with raw arguments and supports cancellation. |
| `RunOrThrowAsync(IEnumerable<string> arguments, CancellationToken cancellationToken = default)` | Any raw arguments | Runs mapshaper and throws `MapshaperException` when the process exits unsuccessfully. |
| `CreatePipeline()` | None initially | Creates an empty fluent command pipeline. |
| `CreatePipeline(params string[] inputPaths)` | Input paths | Creates a pipeline that starts with one or more input datasets. |
| `CreatePipeline(IEnumerable<string> inputPaths, MapshaperImportOptions? importOptions = null)` | `-i` when needed | Creates a pipeline with input datasets and optional import flags. |
| `ConvertAsync(...)` | `-o` | Converts one or more input datasets to an output path or mapshaper-inferred format. |
| `SimplifyAsync(...)` | `-simplify` | Simplifies an input dataset by an amount such as `10%` and writes an output dataset. |
| `ClipAsync(...)` | `-clip` | Clips an input dataset using another dataset and writes an output dataset. |
| `DissolveAsync(...)` | `-dissolve` | Dissolves features using a field and writes an output dataset. |
| `FilterFieldsAsync(...)` | `-filter-fields` | Keeps only selected fields and writes an output dataset. |
| `ProjectAsync(...)` | `-proj` | Projects an input dataset and writes an output dataset. |

The high-level file workflow methods support both string paths and `FileInfo` overloads where applicable. Methods that write output also accept `MapshaperCommandOptions` overloads for common import, output, quiet, and verbose flags.

## High-level examples

Convert a dataset:

```csharp
using Mapshaper.Net;

var client = new MapshaperClient();
var result = await client.ConvertAsync("input.geojson", "output.topojson");

result.EnsureSuccess();
```

Simplify a dataset and overwrite an existing output file:

```csharp
var result = await client.SimplifyAsync(
    "input.geojson",
    "simplified.geojson",
    "10%",
    new MapshaperCommandOptions
    {
        Quiet = true,
        Output = new MapshaperOutputOptions { Force = true }
    });
```

Run a mapshaper command that does not have a high-level helper:

```csharp
var result = await client.RunAsync(
    "input.geojson",
    "-clean",
    "-filter",
    "POP > 0",
    "-o",
    "output.geojson");
```

## MapshaperPipeline

`MapshaperPipeline` composes command sequences fluently and runs them through the same client.

| Function | CLI command | Purpose |
| --- | --- | --- |
| `Input(...)` | Input paths or `-i` | Imports one or more input datasets, with optional import flags. |
| `Output(...)` | `-o` | Writes the target layer or layers to an output path, with optional output flags. |
| `Quiet()` | `-quiet` | Suppresses mapshaper console messages. |
| `Verbose()` | `-verbose` | Prints verbose mapshaper console messages. |
| `Command(string command, params string[] arguments)` | Caller-supplied command | Appends any raw mapshaper command that starts with `-`. |
| `RunAsync(...)` | Accumulated pipeline | Runs the accumulated command sequence. |
| `RunOrThrowAsync(...)` | Accumulated pipeline | Runs the accumulated command sequence and throws on unsuccessful exit. |

Pipeline command wrappers append the matching mapshaper command token and pass all supplied arguments through unchanged.

| Function | CLI command |
| --- | --- |
| `Affine(...)` | `-affine` |
| `Calc(...)` | `-calc` |
| `Classify(...)` | `-classify` |
| `Clean(...)` | `-clean` |
| `Clip(...)` | `-clip` |
| `Colorizer(...)` | `-colorizer` |
| `Colors(...)` | `-colors` |
| `Comment(...)` | `-comment` |
| `Dashlines(...)` | `-dashlines` |
| `Dissolve(...)` | `-dissolve` |
| `Dissolve2(...)` | `-dissolve2` |
| `Divide(...)` | `-divide` |
| `Dots(...)` | `-dots` |
| `Drop(...)` | `-drop` |
| `Each(...)` | `-each` |
| `Elif(...)` | `-elif` |
| `Else(...)` | `-else` |
| `Encodings(...)` | `-encodings` |
| `Endif(...)` | `-endif` |
| `Erase(...)` | `-erase` |
| `Explode(...)` | `-explode` |
| `Filter(...)` | `-filter` |
| `FilterFields(...)` | `-filter-fields` |
| `FilterIslands(...)` | `-filter-islands` |
| `FilterSlivers(...)` | `-filter-slivers` |
| `Frame(...)` | `-frame` |
| `Graticule(...)` | `-graticule` |
| `Grid(...)` | `-grid` |
| `Help(...)` | `-help` |
| `If(...)` | `-if` |
| `Include(...)` | `-include` |
| `Info(...)` | `-info` |
| `Inlay(...)` | `-inlay` |
| `Innerlines(...)` | `-innerlines` |
| `Inspect(...)` | `-inspect` |
| `Join(...)` | `-join` |
| `Lines(...)` | `-lines` |
| `MergeLayers(...)` | `-merge-layers` |
| `Mosaic(...)` | `-mosaic` |
| `PointGrid(...)` | `-point-grid` |
| `Points(...)` | `-points` |
| `Polygons(...)` | `-polygons` |
| `Print(...)` | `-print` |
| `Project(...)` | `-proj` |
| `Projections(...)` | `-projections` |
| `Rectangle(...)` | `-rectangle` |
| `Rectangles(...)` | `-rectangles` |
| `RenameFields(...)` | `-rename-fields` |
| `RenameLayers(...)` | `-rename-layers` |
| `Require(...)` | `-require` |
| `Run(...)` | `-run` |
| `Scalebar(...)` | `-scalebar` |
| `Shape(...)` | `-shape` |
| `Simplify(...)` | `-simplify` |
| `Snap(...)` | `-snap` |
| `Sort(...)` | `-sort` |
| `Split(...)` | `-split` |
| `SplitOnGrid(...)` | `-split-on-grid` |
| `Stop(...)` | `-stop` |
| `Style(...)` | `-style` |
| `Subdivide(...)` | `-subdivide` |
| `Symbols(...)` | `-symbols` |
| `Target(...)` | `-target` |
| `Union(...)` | `-union` |
| `Uniq(...)` | `-uniq` |
| `Version(...)` | `-version` |

Pipeline example:

```csharp
var result = await client
    .CreatePipeline("input.geojson")
    .Quiet()
    .Clean()
    .Filter("POP > 0")
    .RenameFields("NAME=label")
    .Output("output.geojson", new MapshaperOutputOptions
    {
        Format = "geojson",
        Force = true
    })
    .RunAsync();
```

## Options

`MapshaperOptions` controls how the external executable is started.

| Property | Purpose |
| --- | --- |
| `ExecutablePath` | Executable name or full path used to run mapshaper. Defaults to `mapshaper`. |
| `WorkingDirectory` | Working directory for mapshaper, or `null` to use the current process directory. |

`MapshaperCommandOptions` applies common flags to high-level client methods.

| Property | Purpose |
| --- | --- |
| `Import` | Import options used while loading input datasets. |
| `Output` | Output options used while writing output datasets. |
| `Quiet` | Adds `-quiet`. Cannot be combined with `Verbose`. |
| `Verbose` | Adds `-verbose`. Cannot be combined with `Quiet`. |

`MapshaperImportOptions` maps common import flags.

| Property | CLI flag | Purpose |
| --- | --- | --- |
| `Encoding` | `encoding=...` | Character encoding used to read input data. |
| `IdField` | `id-field=...` | Source field to use for feature ids. |
| `CombineFiles` | `combine-files` | Imports multiple input files into a single layer. |

`MapshaperOutputOptions` maps common output flags.

| Property | CLI flag | Purpose |
| --- | --- | --- |
| `Format` | `format=...` | Output format such as `geojson`, `topojson`, `shapefile`, or `json`. |
| `Encoding` | `encoding=...` | Character encoding used to write output data. |
| `Precision` | `precision=...` | Coordinate precision used when writing output data. |
| `Force` | `force` | Allows mapshaper to overwrite an existing output file. |
| `Target` | `target=...` | Target layer or layers to write. |
| `IdField` | `id-field=...` | Source field to use for feature ids in supported output formats. |

## Results and errors

`MapshaperResult` contains the completed process result.

| Property or method | Purpose |
| --- | --- |
| `ExitCode` | Process exit code returned by mapshaper. |
| `StdOut` | Captured standard output. |
| `StdErr` | Captured standard error. |
| `ExecutablePath` | Executable used for the invocation. |
| `Arguments` | Arguments passed to mapshaper. |
| `IsSuccess` | `true` when `ExitCode` is `0`. |
| `EnsureSuccess()` | Returns the result or throws `MapshaperException` on unsuccessful exit. |

`MapshaperException` is thrown when mapshaper cannot be started or when `RunOrThrowAsync()` or `EnsureSuccess()` observes an unsuccessful result. It exposes the executable path, arguments, exit code when available, and captured standard error.

## Validation notes

- Raw command names passed to `MapshaperPipeline.Command()` must start with `-`.
- Empty, null, or whitespace path and argument values are rejected.
- `Quiet` and `Verbose` cannot be enabled together.
- Pipeline command wrappers accept zero or more string arguments and pass them directly to mapshaper.
