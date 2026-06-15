using Mapshaper.Net;

namespace Mapshaper.Net.Tests;

internal sealed class FakeProcessRunner : IMapshaperProcessRunner
{
    public MapshaperOptions? LastOptions { get; private set; }

    public IReadOnlyList<string>? LastArguments { get; private set; }

    public MapshaperResult Result { get; set; } = new(0, "", "", "mapshaper", []);

    public Task<MapshaperResult> RunAsync(
        MapshaperOptions options,
        IReadOnlyList<string> arguments,
        CancellationToken cancellationToken)
    {
        LastOptions = options;
        LastArguments = arguments;

        cancellationToken.ThrowIfCancellationRequested();

        return Task.FromResult(Result with
        {
            ExecutablePath = options.ExecutablePath,
            Arguments = arguments,
        });
    }
}
