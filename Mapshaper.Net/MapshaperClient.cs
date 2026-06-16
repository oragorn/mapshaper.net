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
        return RunAsync(BuildConvertArguments(inputPath, outputPath, options: null), cancellationToken);
    }

    /// <summary>
    /// Converts an input dataset to the output path or format inferred by mapshaper.
    /// </summary>
    public Task<MapshaperResult> ConvertAsync(
        string inputPath,
        string outputPath,
        MapshaperCommandOptions options,
        CancellationToken cancellationToken = default)
    {
        return RunAsync(BuildConvertArguments(inputPath, outputPath, options), cancellationToken);
    }

    /// <summary>
    /// Converts input datasets to the output path or format inferred by mapshaper.
    /// </summary>
    public Task<MapshaperResult> ConvertAsync(
        IEnumerable<string> inputPaths,
        string outputPath,
        MapshaperCommandOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        return RunAsync(BuildConvertArguments(inputPaths, outputPath, options), cancellationToken);
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
    /// Converts an input dataset to the output path or format inferred by mapshaper.
    /// </summary>
    public Task<MapshaperResult> ConvertAsync(
        FileInfo inputFile,
        FileInfo outputFile,
        MapshaperCommandOptions options,
        CancellationToken cancellationToken = default)
    {
        return ConvertAsync(GetFullName(inputFile), GetFullName(outputFile), options, cancellationToken);
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
        return RunAsync(BuildOutputCommand(inputPath, outputPath, "-simplify", amount, options: null), cancellationToken);
    }

    /// <summary>
    /// Simplifies an input dataset and writes the result to the output path.
    /// </summary>
    public Task<MapshaperResult> SimplifyAsync(
        string inputPath,
        string outputPath,
        string amount,
        MapshaperCommandOptions options,
        CancellationToken cancellationToken = default)
    {
        ValidateValue(amount, nameof(amount));
        return RunAsync(BuildOutputCommand(inputPath, outputPath, "-simplify", amount, options), cancellationToken);
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
    /// Simplifies an input dataset and writes the result to the output path.
    /// </summary>
    public Task<MapshaperResult> SimplifyAsync(
        FileInfo inputFile,
        FileInfo outputFile,
        string amount,
        MapshaperCommandOptions options,
        CancellationToken cancellationToken = default)
    {
        return SimplifyAsync(GetFullName(inputFile), GetFullName(outputFile), amount, options, cancellationToken);
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
        return RunAsync(BuildOutputCommand(inputPath, outputPath, "-clip", clipPath, options: null), cancellationToken);
    }

    /// <summary>
    /// Clips an input dataset using another dataset and writes the result to the output path.
    /// </summary>
    public Task<MapshaperResult> ClipAsync(
        string inputPath,
        string clipPath,
        string outputPath,
        MapshaperCommandOptions options,
        CancellationToken cancellationToken = default)
    {
        ValidatePath(clipPath, nameof(clipPath));
        return RunAsync(BuildOutputCommand(inputPath, outputPath, "-clip", clipPath, options), cancellationToken);
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
    /// Clips an input dataset using another dataset and writes the result to the output path.
    /// </summary>
    public Task<MapshaperResult> ClipAsync(
        FileInfo inputFile,
        FileInfo clipFile,
        FileInfo outputFile,
        MapshaperCommandOptions options,
        CancellationToken cancellationToken = default)
    {
        return ClipAsync(GetFullName(inputFile), GetFullName(clipFile), GetFullName(outputFile), options, cancellationToken);
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
        return RunAsync(BuildOutputCommand(inputPath, outputPath, "-dissolve", field, options: null), cancellationToken);
    }

    /// <summary>
    /// Dissolves features using a field and writes the result to the output path.
    /// </summary>
    public Task<MapshaperResult> DissolveAsync(
        string inputPath,
        string field,
        string outputPath,
        MapshaperCommandOptions options,
        CancellationToken cancellationToken = default)
    {
        ValidateValue(field, nameof(field));
        return RunAsync(BuildOutputCommand(inputPath, outputPath, "-dissolve", field, options), cancellationToken);
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
    /// Dissolves features using a field and writes the result to the output path.
    /// </summary>
    public Task<MapshaperResult> DissolveAsync(
        FileInfo inputFile,
        string field,
        FileInfo outputFile,
        MapshaperCommandOptions options,
        CancellationToken cancellationToken = default)
    {
        return DissolveAsync(GetFullName(inputFile), field, GetFullName(outputFile), options, cancellationToken);
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
        return RunAsync(BuildOutputCommand(inputPath, outputPath, "-filter-fields", string.Join(",", fieldList), options: null), cancellationToken);
    }

    /// <summary>
    /// Keeps a selected set of fields and writes the result to the output path.
    /// </summary>
    public Task<MapshaperResult> FilterFieldsAsync(
        string inputPath,
        IEnumerable<string> fields,
        string outputPath,
        MapshaperCommandOptions options,
        CancellationToken cancellationToken = default)
    {
        var fieldList = NormalizeFields(fields);
        return RunAsync(BuildOutputCommand(inputPath, outputPath, "-filter-fields", string.Join(",", fieldList), options), cancellationToken);
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
    /// Keeps a selected set of fields and writes the result to the output path.
    /// </summary>
    public Task<MapshaperResult> FilterFieldsAsync(
        FileInfo inputFile,
        IEnumerable<string> fields,
        FileInfo outputFile,
        MapshaperCommandOptions options,
        CancellationToken cancellationToken = default)
    {
        return FilterFieldsAsync(GetFullName(inputFile), fields, GetFullName(outputFile), options, cancellationToken);
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
        return RunAsync(BuildOutputCommand(inputPath, outputPath, "-proj", projection, options: null), cancellationToken);
    }

    /// <summary>
    /// Projects an input dataset and writes the result to the output path.
    /// </summary>
    public Task<MapshaperResult> ProjectAsync(
        string inputPath,
        string projection,
        string outputPath,
        MapshaperCommandOptions options,
        CancellationToken cancellationToken = default)
    {
        ValidateValue(projection, nameof(projection));
        return RunAsync(BuildOutputCommand(inputPath, outputPath, "-proj", projection, options), cancellationToken);
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

    /// <summary>
    /// Projects an input dataset and writes the result to the output path.
    /// </summary>
    public Task<MapshaperResult> ProjectAsync(
        FileInfo inputFile,
        string projection,
        FileInfo outputFile,
        MapshaperCommandOptions options,
        CancellationToken cancellationToken = default)
    {
        return ProjectAsync(GetFullName(inputFile), projection, GetFullName(outputFile), options, cancellationToken);
    }

    private static IReadOnlyList<string> BuildConvertArguments(
        string inputPath,
        string outputPath,
        MapshaperCommandOptions? options)
    {
        return BuildConvertArguments([inputPath], outputPath, options);
    }

    private static IReadOnlyList<string> BuildConvertArguments(
        IEnumerable<string> inputPaths,
        string outputPath,
        MapshaperCommandOptions? options)
    {
        var normalizedInputPaths = NormalizePaths(inputPaths, nameof(inputPaths));
        ValidatePath(outputPath, nameof(outputPath));
        ValidateCommandOptions(options);

        var arguments = new List<string>();
        AppendInputArguments(arguments, normalizedInputPaths, options?.Import);
        AppendMessageOptions(arguments, options);
        AppendOutputArguments(arguments, outputPath, options?.Output);

        return new ReadOnlyCollection<string>(arguments);
    }

    private static IReadOnlyList<string> BuildOutputCommand(
        string inputPath,
        string outputPath,
        string command,
        string commandValue,
        MapshaperCommandOptions? options)
    {
        var normalizedInputPaths = NormalizePaths([inputPath], nameof(inputPath));
        ValidatePath(outputPath, nameof(outputPath));
        ValidateCommandOptions(options);

        var arguments = new List<string>();
        AppendInputArguments(arguments, normalizedInputPaths, options?.Import);
        AppendMessageOptions(arguments, options);
        arguments.Add(command);
        arguments.Add(commandValue);
        AppendOutputArguments(arguments, outputPath, options?.Output);

        return new ReadOnlyCollection<string>(arguments);
    }

    private static void AppendMessageOptions(List<string> arguments, MapshaperCommandOptions? options)
    {
        if (options?.Quiet == true)
        {
            arguments.Add("-quiet");
        }

        if (options?.Verbose == true)
        {
            arguments.Add("-verbose");
        }
    }

    private static void AppendInputArguments(
        List<string> arguments,
        IReadOnlyList<string> inputPaths,
        MapshaperImportOptions? options)
    {
        if (inputPaths.Count == 1 && options is null)
        {
            arguments.Add(inputPaths[0]);
            return;
        }

        arguments.Add("-i");
        arguments.AddRange(inputPaths);

        if (options?.Encoding is not null)
        {
            arguments.Add($"encoding={options.Encoding}");
        }

        if (options?.IdField is not null)
        {
            arguments.Add($"id-field={options.IdField}");
        }

        if (options?.CombineFiles == true)
        {
            arguments.Add("combine-files");
        }
    }

    private static void AppendOutputArguments(
        List<string> arguments,
        string outputPath,
        MapshaperOutputOptions? options)
    {
        arguments.Add("-o");
        arguments.Add(outputPath);

        if (options?.Format is not null)
        {
            arguments.Add($"format={options.Format}");
        }

        if (options?.Encoding is not null)
        {
            arguments.Add($"encoding={options.Encoding}");
        }

        if (options?.Precision is not null)
        {
            arguments.Add($"precision={options.Precision}");
        }

        if (options?.Force == true)
        {
            arguments.Add("force");
        }

        if (options?.Target is not null)
        {
            arguments.Add($"target={options.Target}");
        }

        if (options?.IdField is not null)
        {
            arguments.Add($"id-field={options.IdField}");
        }
    }

    private static string GetFullName(FileInfo file)
    {
        if (file is null)
        {
            throw new ArgumentNullException(nameof(file));
        }

        return file.FullName;
    }

    private static IReadOnlyList<string> NormalizeArguments(IEnumerable<string> arguments)
    {
        if (arguments is null)
        {
            throw new ArgumentNullException(nameof(arguments));
        }

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

    private static IReadOnlyList<string> NormalizePaths(IEnumerable<string> paths, string parameterName)
    {
        if (paths is null)
        {
            throw new ArgumentNullException(nameof(paths));
        }

        var normalized = paths.ToArray();
        if (normalized.Length == 0)
        {
            throw new ArgumentException("At least one path is required.", parameterName);
        }

        foreach (var path in normalized)
        {
            ValidatePath(path, parameterName);
        }

        return new ReadOnlyCollection<string>(normalized);
    }

    private static IReadOnlyList<string> NormalizeFields(IEnumerable<string> fields)
    {
        if (fields is null)
        {
            throw new ArgumentNullException(nameof(fields));
        }

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
        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        ValidateValue(options.ExecutablePath, nameof(options.ExecutablePath));

        if (options.WorkingDirectory is not null)
        {
            ValidateValue(options.WorkingDirectory, nameof(options.WorkingDirectory));
        }

        return options;
    }

    private static void ValidateCommandOptions(MapshaperCommandOptions? options)
    {
        if (options is null)
        {
            return;
        }

        if (options.Quiet && options.Verbose)
        {
            throw new ArgumentException("Quiet and verbose options cannot both be enabled.", nameof(options));
        }

        if (options.Import is not null)
        {
            ValidateOptionalValue(options.Import.Encoding, nameof(options.Import.Encoding));
            ValidateOptionalValue(options.Import.IdField, nameof(options.Import.IdField));
        }

        if (options.Output is not null)
        {
            ValidateOptionalValue(options.Output.Format, nameof(options.Output.Format));
            ValidateOptionalValue(options.Output.Encoding, nameof(options.Output.Encoding));
            ValidateOptionalValue(options.Output.Precision, nameof(options.Output.Precision));
            ValidateOptionalValue(options.Output.Target, nameof(options.Output.Target));
            ValidateOptionalValue(options.Output.IdField, nameof(options.Output.IdField));
        }
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

    private static void ValidateOptionalValue(string? value, string parameterName)
    {
        if (value is not null)
        {
            ValidateValue(value, parameterName);
        }
    }
}
