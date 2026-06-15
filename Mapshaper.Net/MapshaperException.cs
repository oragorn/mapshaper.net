using System.Text;

namespace Mapshaper.Net;

/// <summary>
/// Represents an error raised while starting or running mapshaper.
/// </summary>
public sealed class MapshaperException : Exception
{
    internal MapshaperException(
        string message,
        string executablePath,
        IReadOnlyList<string> arguments,
        int? exitCode = null,
        string? stdErr = null,
        Exception? innerException = null)
        : base(message, innerException)
    {
        ExecutablePath = executablePath;
        Arguments = arguments;
        ExitCode = exitCode;
        StdErr = stdErr;
    }

    /// <summary>
    /// Gets the executable name or path used for the failed invocation.
    /// </summary>
    public string ExecutablePath { get; }

    /// <summary>
    /// Gets the arguments passed to mapshaper.
    /// </summary>
    public IReadOnlyList<string> Arguments { get; }

    /// <summary>
    /// Gets the process exit code when mapshaper started and exited.
    /// </summary>
    public int? ExitCode { get; }

    /// <summary>
    /// Gets standard error captured from mapshaper, when available.
    /// </summary>
    public string? StdErr { get; }

    internal static MapshaperException ForStartFailure(
        string executablePath,
        IReadOnlyList<string> arguments,
        Exception innerException)
    {
        return new MapshaperException(
            $"Failed to start mapshaper executable '{executablePath}' with arguments: {FormatArguments(arguments)}",
            executablePath,
            arguments,
            innerException: innerException);
    }

    internal static MapshaperException ForFailedResult(MapshaperResult result)
    {
        var stderrSummary = Summarize(result.StdErr);
        var message = new StringBuilder()
            .Append("Mapshaper exited with code ")
            .Append(result.ExitCode)
            .Append(" using executable '")
            .Append(result.ExecutablePath)
            .Append("' and arguments: ")
            .Append(FormatArguments(result.Arguments));

        if (!string.IsNullOrWhiteSpace(stderrSummary))
        {
            message.Append(". stderr: ").Append(stderrSummary);
        }

        return new MapshaperException(
            message.ToString(),
            result.ExecutablePath,
            result.Arguments,
            result.ExitCode,
            result.StdErr);
    }

    private static string Summarize(string value)
    {
        const int maxLength = 500;
        var trimmed = value.Trim();
        return trimmed.Length <= maxLength ? trimmed : trimmed[..maxLength] + "...";
    }

    private static string FormatArguments(IEnumerable<string> arguments)
    {
        return string.Join(" ", arguments.Select(argument => argument.Contains(' ') ? $"\"{argument}\"" : argument));
    }
}
