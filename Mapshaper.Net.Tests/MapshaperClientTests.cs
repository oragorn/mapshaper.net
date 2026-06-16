using Mapshaper.Net;

namespace Mapshaper.Net.Tests;

public sealed class MapshaperClientTests
{
    [Fact]
    public async Task RunAsync_PassesRawArgumentsToRunner()
    {
        var runner = new FakeProcessRunner();
        var client = new MapshaperClient(new MapshaperOptions { ExecutablePath = "custom-mapshaper" }, runner);

        var result = await client.RunAsync(["input.geojson", "-o", "output.shp"]);

        Assert.True(result.IsSuccess);
        Assert.Equal("custom-mapshaper", runner.LastOptions?.ExecutablePath);
        Assert.Equal(["input.geojson", "-o", "output.shp"], runner.LastArguments);
    }

    [Fact]
    public async Task ConvertAsync_BuildsExpectedArguments()
    {
        var runner = new FakeProcessRunner();
        var client = new MapshaperClient(new MapshaperOptions(), runner);

        await client.ConvertAsync("input.geojson", "output.shp");

        Assert.Equal(["input.geojson", "-o", "output.shp"], runner.LastArguments);
    }

    [Fact]
    public async Task ConvertAsync_WithCommandOptions_BuildsExpectedArguments()
    {
        var runner = new FakeProcessRunner();
        var client = new MapshaperClient(new MapshaperOptions(), runner);

        await client.ConvertAsync(
            "input.geojson",
            "output.json",
            new MapshaperCommandOptions
            {
                Quiet = true,
                Import = new MapshaperImportOptions
                {
                    Encoding = "utf8",
                    IdField = "SOURCE_ID",
                },
                Output = new MapshaperOutputOptions
                {
                    Format = "geojson",
                    Encoding = "utf8",
                    Precision = "0.000001",
                    Force = true,
                    Target = "counties",
                    IdField = "FEATURE_ID",
                },
            });

        Assert.Equal(
            [
                "-i",
                "input.geojson",
                "encoding=utf8",
                "id-field=SOURCE_ID",
                "-quiet",
                "-o",
                "output.json",
                "format=geojson",
                "encoding=utf8",
                "precision=0.000001",
                "force",
                "target=counties",
                "id-field=FEATURE_ID",
            ],
            runner.LastArguments);
    }

    [Fact]
    public async Task ConvertAsync_WithMultipleInputsAndCombineFiles_BuildsExpectedArguments()
    {
        var runner = new FakeProcessRunner();
        var client = new MapshaperClient(new MapshaperOptions(), runner);

        await client.ConvertAsync(
            ["a.geojson", "b.geojson"],
            "combined.geojson",
            new MapshaperCommandOptions
            {
                Import = new MapshaperImportOptions { CombineFiles = true },
                Output = new MapshaperOutputOptions { Format = "geojson" },
            });

        Assert.Equal(
            [
                "-i",
                "a.geojson",
                "b.geojson",
                "combine-files",
                "-o",
                "combined.geojson",
                "format=geojson",
            ],
            runner.LastArguments);
    }

    [Fact]
    public async Task SimplifyAsync_BuildsExpectedArguments()
    {
        var runner = new FakeProcessRunner();
        var client = new MapshaperClient(new MapshaperOptions(), runner);

        await client.SimplifyAsync("input.geojson", "output.geojson", "10%");

        Assert.Equal(["input.geojson", "-simplify", "10%", "-o", "output.geojson"], runner.LastArguments);
    }

    [Fact]
    public async Task SimplifyAsync_WithCommandOptions_BuildsExpectedArguments()
    {
        var runner = new FakeProcessRunner();
        var client = new MapshaperClient(new MapshaperOptions(), runner);

        await client.SimplifyAsync(
            "input.geojson",
            "output.geojson",
            "10%",
            new MapshaperCommandOptions
            {
                Verbose = true,
                Output = new MapshaperOutputOptions
                {
                    Format = "geojson",
                    Force = true,
                },
            });

        Assert.Equal(
            ["input.geojson", "-verbose", "-simplify", "10%", "-o", "output.geojson", "format=geojson", "force"],
            runner.LastArguments);
    }

    [Fact]
    public async Task ClipAsync_BuildsExpectedArguments()
    {
        var runner = new FakeProcessRunner();
        var client = new MapshaperClient(new MapshaperOptions(), runner);

        await client.ClipAsync("input.geojson", "clip.geojson", "output.geojson");

        Assert.Equal(["input.geojson", "-clip", "clip.geojson", "-o", "output.geojson"], runner.LastArguments);
    }

    [Fact]
    public async Task DissolveAsync_BuildsExpectedArguments()
    {
        var runner = new FakeProcessRunner();
        var client = new MapshaperClient(new MapshaperOptions(), runner);

        await client.DissolveAsync("input.geojson", "REGION", "output.geojson");

        Assert.Equal(["input.geojson", "-dissolve", "REGION", "-o", "output.geojson"], runner.LastArguments);
    }

    [Fact]
    public async Task FilterFieldsAsync_BuildsExpectedArguments()
    {
        var runner = new FakeProcessRunner();
        var client = new MapshaperClient(new MapshaperOptions(), runner);

        await client.FilterFieldsAsync("input.geojson", ["NAME", "CODE"], "output.geojson");

        Assert.Equal(["input.geojson", "-filter-fields", "NAME,CODE", "-o", "output.geojson"], runner.LastArguments);
    }

    [Fact]
    public async Task ProjectAsync_BuildsExpectedArguments()
    {
        var runner = new FakeProcessRunner();
        var client = new MapshaperClient(new MapshaperOptions(), runner);

        await client.ProjectAsync("input.geojson", "wgs84", "output.geojson");

        Assert.Equal(["input.geojson", "-proj", "wgs84", "-o", "output.geojson"], runner.LastArguments);
    }

    [Fact]
    public async Task FileInfoOverload_UsesFullNamesAndPreservesSpaces()
    {
        var runner = new FakeProcessRunner();
        var client = new MapshaperClient(new MapshaperOptions(), runner);
        var input = new FileInfo(Path.Combine(Path.GetTempPath(), "folder with spaces", "input file.geojson"));
        var output = new FileInfo(Path.Combine(Path.GetTempPath(), "folder with spaces", "output file.geojson"));

        await client.ConvertAsync(input, output);

        Assert.Equal([input.FullName, "-o", output.FullName], runner.LastArguments);
    }

    [Fact]
    public async Task RunOrThrowAsync_ThrowsMapshaperExceptionOnNonZeroExit()
    {
        var runner = new FakeProcessRunner
        {
            Result = new MapshaperResult(2, "", "bad things happened", "mapshaper", []),
        };
        var client = new MapshaperClient(new MapshaperOptions(), runner);

        var exception = await Assert.ThrowsAsync<MapshaperException>(
            () => client.RunOrThrowAsync(["input.geojson", "-o", "output.geojson"]));

        Assert.Equal(2, exception.ExitCode);
        Assert.Equal("bad things happened", exception.StdErr);
        Assert.Contains("bad things happened", exception.Message);
    }

    [Fact]
    public async Task RunAsync_ObservesCancellation()
    {
        var runner = new FakeProcessRunner();
        var client = new MapshaperClient(new MapshaperOptions(), runner);
        using var cancellation = new CancellationTokenSource();
        await cancellation.CancelAsync();

        await Assert.ThrowsAsync<OperationCanceledException>(
            () => client.RunAsync(["input.geojson"], cancellation.Token));
    }

    [Fact]
    public async Task RunAsync_RejectsEmptyArguments()
    {
        var client = new MapshaperClient(new MapshaperOptions(), new FakeProcessRunner());

        await Assert.ThrowsAsync<ArgumentException>(() => client.RunAsync(Array.Empty<string>()));
    }

    [Fact]
    public async Task ConvertAsync_RejectsQuietAndVerboseTogether()
    {
        var client = new MapshaperClient(new MapshaperOptions(), new FakeProcessRunner());

        await Assert.ThrowsAsync<ArgumentException>(
            () => client.ConvertAsync(
                "input.geojson",
                "output.geojson",
                new MapshaperCommandOptions { Quiet = true, Verbose = true }));
    }

    [Fact]
    public async Task ConvertAsync_RejectsEmptyOptionValues()
    {
        var client = new MapshaperClient(new MapshaperOptions(), new FakeProcessRunner());

        await Assert.ThrowsAsync<ArgumentException>(
            () => client.ConvertAsync(
                "input.geojson",
                "output.geojson",
                new MapshaperCommandOptions { Output = new MapshaperOutputOptions { Format = " " } }));
    }

    [Fact]
    public void Constructor_RejectsMissingExecutablePath()
    {
        Assert.Throws<ArgumentException>(
            () => new MapshaperClient(new MapshaperOptions { ExecutablePath = " " }, new FakeProcessRunner()));
    }

    [Fact]
    public async Task RunAsync_WithMissingExecutable_ThrowsMapshaperException()
    {
        var executableName = $"missing-mapshaper-{Guid.NewGuid():N}";
        var client = new MapshaperClient(new MapshaperOptions { ExecutablePath = executableName });

        var exception = await Assert.ThrowsAsync<MapshaperException>(
            () => client.RunAsync("input.geojson"));

        Assert.Equal(executableName, exception.ExecutablePath);
        Assert.Equal(["input.geojson"], exception.Arguments);
        Assert.Null(exception.ExitCode);
    }
}
