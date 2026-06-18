using Mapshaper.Net;

namespace Mapshaper.Net.Tests;

public sealed class MapshaperClientTests
{
    public static IEnumerable<object[]> PipelineCommandWrappers()
    {
        yield return ["-affine", (Action<MapshaperPipeline>)(pipeline => pipeline.Affine("arg=value"))];
        yield return ["-calc", (Action<MapshaperPipeline>)(pipeline => pipeline.Calc("arg=value"))];
        yield return ["-classify", (Action<MapshaperPipeline>)(pipeline => pipeline.Classify("arg=value"))];
        yield return ["-clean", (Action<MapshaperPipeline>)(pipeline => pipeline.Clean("arg=value"))];
        yield return ["-clip", (Action<MapshaperPipeline>)(pipeline => pipeline.Clip("arg=value"))];
        yield return ["-colorizer", (Action<MapshaperPipeline>)(pipeline => pipeline.Colorizer("arg=value"))];
        yield return ["-colors", (Action<MapshaperPipeline>)(pipeline => pipeline.Colors("arg=value"))];
        yield return ["-comment", (Action<MapshaperPipeline>)(pipeline => pipeline.Comment("arg=value"))];
        yield return ["-dashlines", (Action<MapshaperPipeline>)(pipeline => pipeline.Dashlines("arg=value"))];
        yield return ["-dissolve", (Action<MapshaperPipeline>)(pipeline => pipeline.Dissolve("arg=value"))];
        yield return ["-dissolve2", (Action<MapshaperPipeline>)(pipeline => pipeline.Dissolve2("arg=value"))];
        yield return ["-divide", (Action<MapshaperPipeline>)(pipeline => pipeline.Divide("arg=value"))];
        yield return ["-dots", (Action<MapshaperPipeline>)(pipeline => pipeline.Dots("arg=value"))];
        yield return ["-drop", (Action<MapshaperPipeline>)(pipeline => pipeline.Drop("arg=value"))];
        yield return ["-each", (Action<MapshaperPipeline>)(pipeline => pipeline.Each("arg=value"))];
        yield return ["-elif", (Action<MapshaperPipeline>)(pipeline => pipeline.Elif("arg=value"))];
        yield return ["-else", (Action<MapshaperPipeline>)(pipeline => pipeline.Else("arg=value"))];
        yield return ["-encodings", (Action<MapshaperPipeline>)(pipeline => pipeline.Encodings("arg=value"))];
        yield return ["-endif", (Action<MapshaperPipeline>)(pipeline => pipeline.Endif("arg=value"))];
        yield return ["-erase", (Action<MapshaperPipeline>)(pipeline => pipeline.Erase("arg=value"))];
        yield return ["-explode", (Action<MapshaperPipeline>)(pipeline => pipeline.Explode("arg=value"))];
        yield return ["-filter", (Action<MapshaperPipeline>)(pipeline => pipeline.Filter("arg=value"))];
        yield return ["-filter-fields", (Action<MapshaperPipeline>)(pipeline => pipeline.FilterFields("arg=value"))];
        yield return ["-filter-islands", (Action<MapshaperPipeline>)(pipeline => pipeline.FilterIslands("arg=value"))];
        yield return ["-filter-slivers", (Action<MapshaperPipeline>)(pipeline => pipeline.FilterSlivers("arg=value"))];
        yield return ["-frame", (Action<MapshaperPipeline>)(pipeline => pipeline.Frame("arg=value"))];
        yield return ["-graticule", (Action<MapshaperPipeline>)(pipeline => pipeline.Graticule("arg=value"))];
        yield return ["-grid", (Action<MapshaperPipeline>)(pipeline => pipeline.Grid("arg=value"))];
        yield return ["-help", (Action<MapshaperPipeline>)(pipeline => pipeline.Help("arg=value"))];
        yield return ["-if", (Action<MapshaperPipeline>)(pipeline => pipeline.If("arg=value"))];
        yield return ["-include", (Action<MapshaperPipeline>)(pipeline => pipeline.Include("arg=value"))];
        yield return ["-info", (Action<MapshaperPipeline>)(pipeline => pipeline.Info("arg=value"))];
        yield return ["-inlay", (Action<MapshaperPipeline>)(pipeline => pipeline.Inlay("arg=value"))];
        yield return ["-innerlines", (Action<MapshaperPipeline>)(pipeline => pipeline.Innerlines("arg=value"))];
        yield return ["-inspect", (Action<MapshaperPipeline>)(pipeline => pipeline.Inspect("arg=value"))];
        yield return ["-join", (Action<MapshaperPipeline>)(pipeline => pipeline.Join("arg=value"))];
        yield return ["-lines", (Action<MapshaperPipeline>)(pipeline => pipeline.Lines("arg=value"))];
        yield return ["-merge-layers", (Action<MapshaperPipeline>)(pipeline => pipeline.MergeLayers("arg=value"))];
        yield return ["-mosaic", (Action<MapshaperPipeline>)(pipeline => pipeline.Mosaic("arg=value"))];
        yield return ["-point-grid", (Action<MapshaperPipeline>)(pipeline => pipeline.PointGrid("arg=value"))];
        yield return ["-points", (Action<MapshaperPipeline>)(pipeline => pipeline.Points("arg=value"))];
        yield return ["-polygons", (Action<MapshaperPipeline>)(pipeline => pipeline.Polygons("arg=value"))];
        yield return ["-print", (Action<MapshaperPipeline>)(pipeline => pipeline.Print("arg=value"))];
        yield return ["-proj", (Action<MapshaperPipeline>)(pipeline => pipeline.Project("arg=value"))];
        yield return ["-projections", (Action<MapshaperPipeline>)(pipeline => pipeline.Projections("arg=value"))];
        yield return ["-rectangle", (Action<MapshaperPipeline>)(pipeline => pipeline.Rectangle("arg=value"))];
        yield return ["-rectangles", (Action<MapshaperPipeline>)(pipeline => pipeline.Rectangles("arg=value"))];
        yield return ["-rename-fields", (Action<MapshaperPipeline>)(pipeline => pipeline.RenameFields("arg=value"))];
        yield return ["-rename-layers", (Action<MapshaperPipeline>)(pipeline => pipeline.RenameLayers("arg=value"))];
        yield return ["-require", (Action<MapshaperPipeline>)(pipeline => pipeline.Require("arg=value"))];
        yield return ["-run", (Action<MapshaperPipeline>)(pipeline => pipeline.Run("arg=value"))];
        yield return ["-scalebar", (Action<MapshaperPipeline>)(pipeline => pipeline.Scalebar("arg=value"))];
        yield return ["-shape", (Action<MapshaperPipeline>)(pipeline => pipeline.Shape("arg=value"))];
        yield return ["-simplify", (Action<MapshaperPipeline>)(pipeline => pipeline.Simplify("arg=value"))];
        yield return ["-snap", (Action<MapshaperPipeline>)(pipeline => pipeline.Snap("arg=value"))];
        yield return ["-sort", (Action<MapshaperPipeline>)(pipeline => pipeline.Sort("arg=value"))];
        yield return ["-split", (Action<MapshaperPipeline>)(pipeline => pipeline.Split("arg=value"))];
        yield return ["-split-on-grid", (Action<MapshaperPipeline>)(pipeline => pipeline.SplitOnGrid("arg=value"))];
        yield return ["-stop", (Action<MapshaperPipeline>)(pipeline => pipeline.Stop("arg=value"))];
        yield return ["-style", (Action<MapshaperPipeline>)(pipeline => pipeline.Style("arg=value"))];
        yield return ["-subdivide", (Action<MapshaperPipeline>)(pipeline => pipeline.Subdivide("arg=value"))];
        yield return ["-symbols", (Action<MapshaperPipeline>)(pipeline => pipeline.Symbols("arg=value"))];
        yield return ["-target", (Action<MapshaperPipeline>)(pipeline => pipeline.Target("arg=value"))];
        yield return ["-union", (Action<MapshaperPipeline>)(pipeline => pipeline.Union("arg=value"))];
        yield return ["-uniq", (Action<MapshaperPipeline>)(pipeline => pipeline.Uniq("arg=value"))];
        yield return ["-version", (Action<MapshaperPipeline>)(pipeline => pipeline.Version("arg=value"))];
    }

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
    public async Task CleanAsync_BuildsExpectedArguments()
    {
        var runner = new FakeProcessRunner();
        var client = new MapshaperClient(new MapshaperOptions(), runner);

        await client.CleanAsync("input.geojson", "output.geojson");

        Assert.Equal(["input.geojson", "-clean", "-o", "output.geojson"], runner.LastArguments);
    }

    [Fact]
    public async Task CleanAsync_WithCommandArguments_BuildsExpectedArguments()
    {
        var runner = new FakeProcessRunner();
        var client = new MapshaperClient(new MapshaperOptions(), runner);

        await client.CleanAsync(
            "input.geojson",
            "output.geojson",
            ["gap-fill-area=100"],
            new MapshaperCommandOptions { Quiet = true });

        Assert.Equal(
            ["input.geojson", "-quiet", "-clean", "gap-fill-area=100", "-o", "output.geojson"],
            runner.LastArguments);
    }

    [Fact]
    public async Task EraseAsync_BuildsExpectedArguments()
    {
        var runner = new FakeProcessRunner();
        var client = new MapshaperClient(new MapshaperOptions(), runner);

        await client.EraseAsync("input.geojson", "erase.geojson", "output.geojson");

        Assert.Equal(["input.geojson", "-erase", "erase.geojson", "-o", "output.geojson"], runner.LastArguments);
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
    public async Task JoinAsync_WithCommandArguments_BuildsExpectedArguments()
    {
        var runner = new FakeProcessRunner();
        var client = new MapshaperClient(new MapshaperOptions(), runner);

        await client.JoinAsync("input.geojson", "table.csv", "output.geojson", ["keys=GEOID", "fields=NAME"]);

        Assert.Equal(["input.geojson", "-join", "table.csv", "keys=GEOID", "fields=NAME", "-o", "output.geojson"], runner.LastArguments);
    }

    [Fact]
    public async Task RenameFieldsAsync_BuildsExpectedArguments()
    {
        var runner = new FakeProcessRunner();
        var client = new MapshaperClient(new MapshaperOptions(), runner);

        await client.RenameFieldsAsync("input.geojson", ["NAME=label", "CODE=id"], "output.geojson");

        Assert.Equal(["input.geojson", "-rename-fields", "NAME=label,CODE=id", "-o", "output.geojson"], runner.LastArguments);
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
    public async Task InfoAsync_BuildsExpectedArguments()
    {
        var runner = new FakeProcessRunner();
        var client = new MapshaperClient(new MapshaperOptions(), runner);

        await client.InfoAsync(
            "input.geojson",
            new MapshaperCommandOptions
            {
                Verbose = true,
                Import = new MapshaperImportOptions { Encoding = "utf8" },
            });

        Assert.Equal(["-i", "input.geojson", "encoding=utf8", "-verbose", "-info"], runner.LastArguments);
    }

    [Fact]
    public async Task InfoAsync_RejectsOutputOptions()
    {
        var client = new MapshaperClient(new MapshaperOptions(), new FakeProcessRunner());

        await Assert.ThrowsAsync<ArgumentException>(
            () => client.InfoAsync(
                "input.geojson",
                new MapshaperCommandOptions { Output = new MapshaperOutputOptions { Format = "geojson" } }));
    }

    [Fact]
    public async Task Pipeline_BuildsExpectedArguments()
    {
        var runner = new FakeProcessRunner();
        var client = new MapshaperClient(new MapshaperOptions(), runner);

        await client
            .CreatePipeline("input.geojson")
            .Quiet()
            .Clean()
            .Filter("POP > 0")
            .RenameFields("NAME=label")
            .Output("output.geojson", new MapshaperOutputOptions { Format = "geojson", Force = true })
            .RunAsync();

        Assert.Equal(
            [
                "input.geojson",
                "-quiet",
                "-clean",
                "-filter",
                "POP > 0",
                "-rename-fields",
                "NAME=label",
                "-o",
                "output.geojson",
                "format=geojson",
                "force",
            ],
            runner.LastArguments);
    }

    [Fact]
    public async Task Pipeline_WithMultipleInputsAndOptions_BuildsExpectedArguments()
    {
        var runner = new FakeProcessRunner();
        var client = new MapshaperClient(new MapshaperOptions(), runner);

        await client
            .CreatePipeline(
                ["a.geojson", "b.geojson"],
                new MapshaperImportOptions { Encoding = "utf8", IdField = "ID", CombineFiles = true })
            .Verbose()
            .MergeLayers()
            .Output("output.geojson", new MapshaperOutputOptions { Target = "*", Precision = "0.001" })
            .RunAsync();

        Assert.Equal(
            [
                "-i",
                "a.geojson",
                "b.geojson",
                "encoding=utf8",
                "id-field=ID",
                "combine-files",
                "-verbose",
                "-merge-layers",
                "-o",
                "output.geojson",
                "precision=0.001",
                "target=*",
            ],
            runner.LastArguments);
    }

    [Theory]
    [MemberData(nameof(PipelineCommandWrappers))]
    public async Task Pipeline_CommandWrappers_AppendExpectedCommandToken(
        string expectedCommand,
        Action<MapshaperPipeline> appendCommand)
    {
        var runner = new FakeProcessRunner();
        var client = new MapshaperClient(new MapshaperOptions(), runner);

        var pipeline = client.CreatePipeline("input.geojson");
        appendCommand(pipeline);
        await pipeline.RunAsync();

        Assert.Equal(["input.geojson", expectedCommand, "arg=value"], runner.LastArguments);
    }

    [Fact]
    public async Task Pipeline_Command_AllowsRawCommand()
    {
        var runner = new FakeProcessRunner();
        var client = new MapshaperClient(new MapshaperOptions(), runner);

        await client
            .CreatePipeline("input.geojson")
            .Command("-custom", "arg=value")
            .RunAsync();

        Assert.Equal(["input.geojson", "-custom", "arg=value"], runner.LastArguments);
    }

    [Fact]
    public async Task Pipeline_RunOrThrowAsync_ThrowsMapshaperExceptionOnNonZeroExit()
    {
        var runner = new FakeProcessRunner
        {
            Result = new MapshaperResult(2, "", "pipeline failed", "mapshaper", []),
        };
        var client = new MapshaperClient(new MapshaperOptions(), runner);

        var exception = await Assert.ThrowsAsync<MapshaperException>(
            () => client.CreatePipeline("input.geojson").Info().RunOrThrowAsync());

        Assert.Equal(2, exception.ExitCode);
        Assert.Equal("pipeline failed", exception.StdErr);
        Assert.Equal(["input.geojson", "-info"], exception.Arguments);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Pipeline_CommandWrapper_RejectsInvalidArgument(string? argument)
    {
        var client = new MapshaperClient(new MapshaperOptions(), new FakeProcessRunner());

        Assert.Throws<ArgumentException>(() => client.CreatePipeline().Filter(argument!));
    }

    [Fact]
    public void Pipeline_CommandWrapper_AllowsZeroArguments()
    {
        var client = new MapshaperClient(new MapshaperOptions(), new FakeProcessRunner());

        var pipeline = client.CreatePipeline().Clean();

        Assert.NotNull(pipeline);
    }

    [Fact]
    public void Pipeline_RejectsQuietAndVerboseTogether()
    {
        var client = new MapshaperClient(new MapshaperOptions(), new FakeProcessRunner());

        Assert.Throws<InvalidOperationException>(() => client.CreatePipeline().Quiet().Verbose());
        Assert.Throws<InvalidOperationException>(() => client.CreatePipeline().Verbose().Quiet());
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
