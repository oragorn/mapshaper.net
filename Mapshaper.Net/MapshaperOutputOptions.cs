namespace Mapshaper.Net;

/// <summary>
/// Common mapshaper output options.
/// </summary>
public sealed class MapshaperOutputOptions
{
    /// <summary>
    /// Gets or sets the output format, such as <c>geojson</c>, <c>topojson</c>, <c>shapefile</c>, or <c>json</c>.
    /// </summary>
    public string? Format { get; set; }

    /// <summary>
    /// Gets or sets the character encoding used to write output data.
    /// </summary>
    public string? Encoding { get; set; }

    /// <summary>
    /// Gets or sets the coordinate precision used when writing output data.
    /// </summary>
    public string? Precision { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether mapshaper may overwrite an existing output file.
    /// </summary>
    public bool Force { get; set; }

    /// <summary>
    /// Gets or sets the target layer or layers to write.
    /// </summary>
    public string? Target { get; set; }

    /// <summary>
    /// Gets or sets the source field to use for feature ids in supported output formats.
    /// </summary>
    public string? IdField { get; set; }
}
