namespace Mapshaper.Net;

internal interface IMapshaperProcessRunner
{
    Task<MapshaperResult> RunAsync(
        MapshaperOptions options,
        IReadOnlyList<string> arguments,
        CancellationToken cancellationToken);
}
