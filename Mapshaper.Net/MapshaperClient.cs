using System.Collections.ObjectModel;

namespace Mapshaper.Net;

/// <summary>
/// Thin client for running the external mapshaper CLI.
/// </summary>
public sealed class MapshaperClient
{
    private readonly MapshaperOptions _options;
    private readonly IMapshaperProcessRunner _runner;

    /// <summary>
    /// Initializes a new instance of the <see cref="MapshaperClient" /> class.
    /// </summary>
    public MapshaperClient()
        : this(new MapshaperOptions())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MapshaperClient" /> class.
    /// </summary>
    public MapshaperClient(MapshaperOptions options)
        : this(options, new DefaultMapshaperProcessRunner())
    {
    }

    internal MapshaperClient(MapshaperOptions options, IMapshaperProcessRunner runner)
    {
        _options = ValidateOptions(options);
        _runner = runner ?? throw new ArgumentNullException(nameof(runner));
    }

    /// <summary>
    /// Runs mapshaper with raw arguments.
    /// </summary>
    public Task<MapshaperResult> RunAsync(params string[] arguments)
    {
        return RunAsync(arguments.AsEnumerable(), CancellationToken.None);
    }

    /// <summary>
    /// Runs mapshaper with raw arguments.
    /// </summary>
    public Task<MapshaperResult> RunAsync(
        IEnumerable<string> arguments,
        CancellationToken cancellationToken = default)
    {
        return _runner.RunAsync(_options, NormalizeArguments(arguments), cancellationToken);
    }

    /// <summary>
    /// Runs mapshaper with raw arguments and throws when the process exits unsuccessfully.
    /// </summary>
    public async Task<MapshaperResult> RunOrThrowAsync(
        IEnumerable<string> arguments,
        CancellationToken cancellationToken = default)
    {
        var result = await RunAsync(arguments, cancellationToken).ConfigureAwait(false);
        return result.EnsureSuccess();
    }

    /// <summary>
    /// Converts an input dataset to the output path or format inferred by mapshaper.
    /// </summary>
    public Task<MapshaperResult> ConvertAsync(
        string inputPath,
        string outputPath,
        CancellationToken cancellationToken = default)
    {
        return RunAsync(BuildConvertArguments(inputPath, outputPath), cancellationToken);
    }

    /// <summary>
    /// Converts an input dataset to the output path or format inferred by mapshaper.
    /// </summary>
    public Task<MapshaperResult> ConvertAsync(
        FileInfo inputFile,
        FileInfo outputFile,
        CancellationToken cancellationToken = default)
    {
        return ConvertAsync(GetFullName(inputFile), GetFullName(outputFile), cancellationToken);
    }

    /// <summary>
    /// Simplifies an input dataset and writes the result to the output path.
    /// </summary>
    public Task<MapshaperResult> SimplifyAsync(
        string inputPath,
        string outputPath,
        string amount,
        CancellationToken cancellationToken = default)
    {
        ValidateValue(amount, nameof(amount));
        return RunAsync(BuildOutputCommand(inputPath, outputPath, "-simplify", amount), cancellationToken);
    }

    /// <summary>
    /// Simplifies an input dataset and writes the result to the output path.
    /// </summary>
    public Task<MapshaperResult> SimplifyAsync(
        FileInfo inputFile,
        FileInfo outputFile,
        string amount,
        CancellationToken cancellationToken = default)
    {
        return SimplifyAsync(GetFullName(inputFile), GetFullName(outputFile), amount, cancellationToken);
    }

    /// <summary>
    /// Clips an input dataset using another dataset and writes the result to the output path.
    /// </summary>
    public Task<MapshaperResult> ClipAsync(
        string inputPath,
        string clipPath,
        string outputPath,
        CancellationToken cancellationToken = default)
    {
        ValidatePath(clipPath, nameof(clipPath));
        return RunAsync(BuildOutputCommand(inputPath, outputPath, "-clip", clipPath), cancellationToken);
    }

    /// <summary>
    /// Clips an input dataset using another dataset and writes the result to the output path.
    /// </summary>
    public Task<MapshaperResult> ClipAsync(
        FileInfo inputFile,
        FileInfo clipFile,
        FileInfo outputFile,
        CancellationToken cancellationToken = default)
    {
        return ClipAsync(GetFullName(inputFile), GetFullName(clipFile), GetFullName(outputFile), cancellationToken);
    }

    /// <summary>
    /// Dissolves features using a field and writes the result to the output path.
    /// </summary>
    public Task<MapshaperResult> DissolveAsync(
        string inputPath,
        string field,
        string outputPath,
        CancellationToken cancellationToken = default)
    {
        ValidateValue(field, nameof(field));
        return RunAsync(BuildOutputCommand(inputPath, outputPath, "-dissolve", field), cancellationToken);
    }

    /// <summary>
    /// Dissolves features using a field and writes the result to the output path.
    /// </summary>
    public Task<MapshaperResult> DissolveAsync(
        FileInfo inputFile,
        string field,
        FileInfo outputFile,
        CancellationToken cancellationToken = default)
    {
        return DissolveAsync(GetFullName(inputFile), field, GetFullName(outputFile), cancellationToken);
    }

    /// <summary>
    /// Keeps a selected set of fields and writes the result to the output path.
    /// </summary>
    public Task<MapshaperResult> FilterFieldsAsync(
        string inputPath,
        IEnumerable<string> fields,
        string outputPath,
        CancellationToken cancellationToken = default)
    {
        var fieldList = NormalizeFields(fields);
        return RunAsync(BuildOutputCommand(inputPath, outputPath, "-filter-fields", string.Join(",", fieldList)), cancellationToken);
    }

    /// <summary>
    /// Keeps a selected set of fields and writes the result to the output path.
    /// </summary>
    public Task<MapshaperResult> FilterFieldsAsync(
        FileInfo inputFile,
        IEnumerable<string> fields,
        FileInfo outputFile,
        CancellationToken cancellationToken = default)
    {
        return FilterFieldsAsync(GetFullName(inputFile), fields, GetFullName(outputFile), cancellationToken);
    }

    /// <summary>
    /// Projects an input dataset and writes the result to the output path.
    /// </summary>
    public Task<MapshaperResult> ProjectAsync(
        string inputPath,
        string projection,
        string outputPath,
        CancellationToken cancellationToken = default)
    {
        ValidateValue(projection, nameof(projection));
        return RunAsync(BuildOutputCommand(inputPath, outputPath, "-proj", projection), cancellationToken);
    }

    /// <summary>
    /// Projects an input dataset and writes the result to the output path.
    /// </summary>
    public Task<MapshaperResult> ProjectAsync(
        FileInfo inputFile,
        string projection,
        FileInfo outputFile,
        CancellationToken cancellationToken = default)
    {
        return ProjectAsync(GetFullName(inputFile), projection, GetFullName(outputFile), cancellationToken);
    }

    private static IReadOnlyList<string> BuildConvertArguments(string inputPath, string outputPath)
    {
        ValidatePath(inputPath, nameof(inputPath));
        ValidatePath(outputPath, nameof(outputPath));

        return new ReadOnlyCollection<string>([inputPath, "-o", outputPath]);
    }

    private static IReadOnlyList<string> BuildOutputCommand(
        string inputPath,
        string outputPath,
        string command,
        string commandValue)
    {
        ValidatePath(inputPath, nameof(inputPath));
        ValidatePath(outputPath, nameof(outputPath));

        return new ReadOnlyCollection<string>([inputPath, command, commandValue, "-o", outputPath]);
    }

    private static string GetFullName(FileInfo file)
    {
        ArgumentNullException.ThrowIfNull(file);
        return file.FullName;
    }

    private static IReadOnlyList<string> NormalizeArguments(IEnumerable<string> arguments)
    {
        ArgumentNullException.ThrowIfNull(arguments);

        var normalized = arguments.ToArray();
        if (normalized.Length == 0)
        {
            throw new ArgumentException("At least one mapshaper argument is required.", nameof(arguments));
        }

        foreach (var argument in normalized)
        {
            ValidateValue(argument, nameof(arguments));
        }

        return new ReadOnlyCollection<string>(normalized);
    }

    private static IReadOnlyList<string> NormalizeFields(IEnumerable<string> fields)
    {
        ArgumentNullException.ThrowIfNull(fields);

        var normalized = fields.ToArray();
        if (normalized.Length == 0)
        {
            throw new ArgumentException("At least one field is required.", nameof(fields));
        }

        foreach (var field in normalized)
        {
            ValidateValue(field, nameof(fields));
        }

        return new ReadOnlyCollection<string>(normalized);
    }

    private static MapshaperOptions ValidateOptions(MapshaperOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        ValidateValue(options.ExecutablePath, nameof(options.ExecutablePath));

        if (options.WorkingDirectory is not null)
        {
            ValidateValue(options.WorkingDirectory, nameof(options.WorkingDirectory));
        }

        return options;
    }

    private static void ValidatePath(string path, string parameterName)
    {
        ValidateValue(path, parameterName);
    }

    private static void ValidateValue(string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value cannot be null, empty, or whitespace.", parameterName);
        }
    }
}
