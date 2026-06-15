namespace Mapshaper.Net;

/// <summary>
/// Options that control how the external mapshaper executable is invoked.
/// </summary>
public sealed class MapshaperOptions
{
    /// <summary>
    /// Gets or sets the executable name or full path used to run mapshaper.
    /// </summary>
    public string ExecutablePath { get; set; } = "mapshaper";

    /// <summary>
    /// Gets or sets the working directory for mapshaper, or <c>null</c> to use the current process directory.
    /// </summary>
    public string? WorkingDirectory { get; set; }
}
