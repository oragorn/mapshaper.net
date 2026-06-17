using System.Collections.ObjectModel;

namespace Mapshaper.Net;

/// <summary>
/// Fluent builder for composing a mapshaper command sequence.
/// </summary>
public sealed class MapshaperPipeline
{
    private readonly List<string> _arguments = [];
    private readonly MapshaperClient _client;
    private bool _quiet;
    private bool _verbose;

    internal MapshaperPipeline(MapshaperClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    /// <summary>
    /// Imports one or more input datasets.
    /// </summary>
    public MapshaperPipeline Input(
        IEnumerable<string> inputPaths,
        MapshaperImportOptions? importOptions = null)
    {
        var normalizedInputPaths = MapshaperClient.NormalizePaths(inputPaths, nameof(inputPaths));
        MapshaperClient.ValidateCommandOptions(new MapshaperCommandOptions { Import = importOptions });
        MapshaperClient.AppendInputArguments(_arguments, normalizedInputPaths, importOptions);
        return this;
    }

    /// <summary>
    /// Imports one or more input datasets.
    /// </summary>
    public MapshaperPipeline Input(params string[] inputPaths)
    {
        if (inputPaths is null)
        {
            throw new ArgumentNullException(nameof(inputPaths));
        }

        return Input(inputPaths.AsEnumerable(), importOptions: null);
    }

    /// <summary>
    /// Writes the target layer or layers to an output path.
    /// </summary>
    public MapshaperPipeline Output(string outputPath, MapshaperOutputOptions? options = null)
    {
        MapshaperClient.ValidatePath(outputPath, nameof(outputPath));
        MapshaperClient.ValidateCommandOptions(new MapshaperCommandOptions { Output = options });
        MapshaperClient.AppendOutputArguments(_arguments, outputPath, options);
        return this;
    }

    /// <summary>
    /// Suppresses mapshaper console messages.
    /// </summary>
    public MapshaperPipeline Quiet()
    {
        if (_verbose)
        {
            throw new InvalidOperationException("Quiet and verbose options cannot both be enabled.");
        }

        if (!_quiet)
        {
            _arguments.Add("-quiet");
            _quiet = true;
        }

        return this;
    }

    /// <summary>
    /// Prints verbose mapshaper console messages.
    /// </summary>
    public MapshaperPipeline Verbose()
    {
        if (_quiet)
        {
            throw new InvalidOperationException("Quiet and verbose options cannot both be enabled.");
        }

        if (!_verbose)
        {
            _arguments.Add("-verbose");
            _verbose = true;
        }

        return this;
    }

    /// <summary>
    /// Appends a raw mapshaper command and its arguments.
    /// </summary>
    public MapshaperPipeline Command(string command, params string[] arguments)
    {
        MapshaperClient.ValidateValue(command, nameof(command));

        if (!command.StartsWith("-", StringComparison.Ordinal))
        {
            throw new ArgumentException("Mapshaper command names must start with '-'.", nameof(command));
        }

        return AppendCommand(command, arguments);
    }

#pragma warning disable CS1591
    public MapshaperPipeline Affine(params string[] arguments) => AppendCommand("-affine", arguments);

    public MapshaperPipeline Calc(params string[] arguments) => AppendCommand("-calc", arguments);

    public MapshaperPipeline Classify(params string[] arguments) => AppendCommand("-classify", arguments);

    public MapshaperPipeline Clean(params string[] arguments) => AppendCommand("-clean", arguments);

    public MapshaperPipeline Clip(params string[] arguments) => AppendCommand("-clip", arguments);

    public MapshaperPipeline Colorizer(params string[] arguments) => AppendCommand("-colorizer", arguments);

    public MapshaperPipeline Colors(params string[] arguments) => AppendCommand("-colors", arguments);

    public MapshaperPipeline Comment(params string[] arguments) => AppendCommand("-comment", arguments);

    public MapshaperPipeline Dashlines(params string[] arguments) => AppendCommand("-dashlines", arguments);

    public MapshaperPipeline Dissolve(params string[] arguments) => AppendCommand("-dissolve", arguments);

    public MapshaperPipeline Dissolve2(params string[] arguments) => AppendCommand("-dissolve2", arguments);

    public MapshaperPipeline Divide(params string[] arguments) => AppendCommand("-divide", arguments);

    public MapshaperPipeline Dots(params string[] arguments) => AppendCommand("-dots", arguments);

    public MapshaperPipeline Drop(params string[] arguments) => AppendCommand("-drop", arguments);

    public MapshaperPipeline Each(params string[] arguments) => AppendCommand("-each", arguments);

    public MapshaperPipeline Elif(params string[] arguments) => AppendCommand("-elif", arguments);

    public MapshaperPipeline Else(params string[] arguments) => AppendCommand("-else", arguments);

    public MapshaperPipeline Encodings(params string[] arguments) => AppendCommand("-encodings", arguments);

    public MapshaperPipeline Endif(params string[] arguments) => AppendCommand("-endif", arguments);

    public MapshaperPipeline Erase(params string[] arguments) => AppendCommand("-erase", arguments);

    public MapshaperPipeline Explode(params string[] arguments) => AppendCommand("-explode", arguments);

    public MapshaperPipeline Filter(params string[] arguments) => AppendCommand("-filter", arguments);

    public MapshaperPipeline FilterFields(params string[] arguments) => AppendCommand("-filter-fields", arguments);

    public MapshaperPipeline FilterIslands(params string[] arguments) => AppendCommand("-filter-islands", arguments);

    public MapshaperPipeline FilterSlivers(params string[] arguments) => AppendCommand("-filter-slivers", arguments);

    public MapshaperPipeline Frame(params string[] arguments) => AppendCommand("-frame", arguments);

    public MapshaperPipeline Graticule(params string[] arguments) => AppendCommand("-graticule", arguments);

    public MapshaperPipeline Grid(params string[] arguments) => AppendCommand("-grid", arguments);

    public MapshaperPipeline Help(params string[] arguments) => AppendCommand("-help", arguments);

    public MapshaperPipeline If(params string[] arguments) => AppendCommand("-if", arguments);

    public MapshaperPipeline Include(params string[] arguments) => AppendCommand("-include", arguments);

    public MapshaperPipeline Info(params string[] arguments) => AppendCommand("-info", arguments);

    public MapshaperPipeline Inlay(params string[] arguments) => AppendCommand("-inlay", arguments);

    public MapshaperPipeline Innerlines(params string[] arguments) => AppendCommand("-innerlines", arguments);

    public MapshaperPipeline Inspect(params string[] arguments) => AppendCommand("-inspect", arguments);

    public MapshaperPipeline Join(params string[] arguments) => AppendCommand("-join", arguments);

    public MapshaperPipeline Lines(params string[] arguments) => AppendCommand("-lines", arguments);

    public MapshaperPipeline MergeLayers(params string[] arguments) => AppendCommand("-merge-layers", arguments);

    public MapshaperPipeline Mosaic(params string[] arguments) => AppendCommand("-mosaic", arguments);

    public MapshaperPipeline PointGrid(params string[] arguments) => AppendCommand("-point-grid", arguments);

    public MapshaperPipeline Points(params string[] arguments) => AppendCommand("-points", arguments);

    public MapshaperPipeline Polygons(params string[] arguments) => AppendCommand("-polygons", arguments);

    public MapshaperPipeline Print(params string[] arguments) => AppendCommand("-print", arguments);

    public MapshaperPipeline Project(params string[] arguments) => AppendCommand("-proj", arguments);

    public MapshaperPipeline Projections(params string[] arguments) => AppendCommand("-projections", arguments);

    public MapshaperPipeline Rectangle(params string[] arguments) => AppendCommand("-rectangle", arguments);

    public MapshaperPipeline Rectangles(params string[] arguments) => AppendCommand("-rectangles", arguments);

    public MapshaperPipeline RenameFields(params string[] arguments) => AppendCommand("-rename-fields", arguments);

    public MapshaperPipeline RenameLayers(params string[] arguments) => AppendCommand("-rename-layers", arguments);

    public MapshaperPipeline Require(params string[] arguments) => AppendCommand("-require", arguments);

    public MapshaperPipeline Run(params string[] arguments) => AppendCommand("-run", arguments);

    public MapshaperPipeline Scalebar(params string[] arguments) => AppendCommand("-scalebar", arguments);

    public MapshaperPipeline Shape(params string[] arguments) => AppendCommand("-shape", arguments);

    public MapshaperPipeline Simplify(params string[] arguments) => AppendCommand("-simplify", arguments);

    public MapshaperPipeline Snap(params string[] arguments) => AppendCommand("-snap", arguments);

    public MapshaperPipeline Sort(params string[] arguments) => AppendCommand("-sort", arguments);

    public MapshaperPipeline Split(params string[] arguments) => AppendCommand("-split", arguments);

    public MapshaperPipeline SplitOnGrid(params string[] arguments) => AppendCommand("-split-on-grid", arguments);

    public MapshaperPipeline Stop(params string[] arguments) => AppendCommand("-stop", arguments);

    public MapshaperPipeline Style(params string[] arguments) => AppendCommand("-style", arguments);

    public MapshaperPipeline Subdivide(params string[] arguments) => AppendCommand("-subdivide", arguments);

    public MapshaperPipeline Symbols(params string[] arguments) => AppendCommand("-symbols", arguments);

    public MapshaperPipeline Target(params string[] arguments) => AppendCommand("-target", arguments);

    public MapshaperPipeline Union(params string[] arguments) => AppendCommand("-union", arguments);

    public MapshaperPipeline Uniq(params string[] arguments) => AppendCommand("-uniq", arguments);

    public MapshaperPipeline Version(params string[] arguments) => AppendCommand("-version", arguments);
#pragma warning restore CS1591

    /// <summary>
    /// Runs the accumulated mapshaper command sequence.
    /// </summary>
    public Task<MapshaperResult> RunAsync(CancellationToken cancellationToken = default)
    {
        return _client.RunAsync(new ReadOnlyCollection<string>(_arguments.ToArray()), cancellationToken);
    }

    /// <summary>
    /// Runs the accumulated mapshaper command sequence and throws when the process exits unsuccessfully.
    /// </summary>
    public async Task<MapshaperResult> RunOrThrowAsync(CancellationToken cancellationToken = default)
    {
        var result = await RunAsync(cancellationToken).ConfigureAwait(false);
        return result.EnsureSuccess();
    }

    private MapshaperPipeline AppendCommand(string command, string[] arguments)
    {
        if (arguments is null)
        {
            throw new ArgumentNullException(nameof(arguments));
        }

        _arguments.Add(command);

        foreach (var argument in arguments)
        {
            MapshaperClient.ValidateValue(argument, nameof(arguments));
            _arguments.Add(argument);
        }

        return this;
    }
}
