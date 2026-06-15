namespace Mapshaper.Net;

/// <summary>
/// Common mapshaper import options.
/// </summary>
public sealed class MapshaperImportOptions
{
    /// <summary>
    /// Gets or sets the character encoding used to read input data.
    /// </summary>
    public string? Encoding { get; set; }

    /// <summary>
    /// Gets or sets the source field to use for feature ids.
    /// </summary>
    public string? IdField { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether multiple input files should be imported into a single layer.
    /// </summary>
    public bool CombineFiles { get; set; }
}
