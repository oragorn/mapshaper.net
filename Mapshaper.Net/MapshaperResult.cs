namespace Mapshaper.Net;

/// <summary>
/// Represents the completed result of a mapshaper process invocation.
/// </summary>
public sealed record MapshaperResult(
    int ExitCode,
    string StdOut,
    string StdErr,
    string ExecutablePath,
    IReadOnlyList<string> Arguments)
{
    /// <summary>
    /// Gets a value indicating whether mapshaper exited successfully.
    /// </summary>
    public bool IsSuccess => ExitCode == 0;

    /// <summary>
    /// Throws a <see cref="MapshaperException" /> when the result is unsuccessful.
    /// </summary>
    public MapshaperResult EnsureSuccess()
    {
        if (!IsSuccess)
        {
            throw MapshaperException.ForFailedResult(this);
        }

        return this;
    }
}
