namespace Mapshaper.Net;

/// <summary>
/// Options for a mapshaper command invocation that are commonly shared across import and output workflows.
/// </summary>
public sealed class MapshaperCommandOptions
{
    /// <summary>
    /// Gets or sets import options to apply while loading input datasets.
    /// </summary>
    public MapshaperImportOptions? Import { get; set; }

    /// <summary>
    /// Gets or sets output options to apply when writing output datasets.
    /// </summary>
    public MapshaperOutputOptions? Output { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether mapshaper should suppress console messages.
    /// </summary>
    public bool Quiet { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether mapshaper should print verbose console messages.
    /// </summary>
    public bool Verbose { get; set; }
}
