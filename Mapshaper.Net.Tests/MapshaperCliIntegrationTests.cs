using System.Diagnostics;
using System.Text.Json;
using Mapshaper.Net;

namespace Mapshaper.Net.Tests;

public sealed class MapshaperCliIntegrationTests
{
    [MapshaperCliFact]
    public async Task ConvertAsync_WithRealCli_WritesTopoJson()
    {
        using var tempDirectory = TemporaryDirectory.Create();
        var inputPath = GetFixturePath();
        var outputPath = Path.Combine(tempDirectory.Path, "converted.topojson");
        var client = CreateClient();

        var result = await client.ConvertAsync(inputPath, outputPath);

        Assert.True(result.IsSuccess, result.StdErr);
        Assert.True(File.Exists(outputPath));
        Assert.NotEqual(0, new FileInfo(outputPath).Length);

        using var document = JsonDocument.Parse(await File.ReadAllTextAsync(outputPath));
        Assert.Equal("Topology", document.RootElement.GetProperty("type").GetString());
    }

    [MapshaperCliFact]
    public async Task SimplifyAsync_WithRealCli_WritesGeoJson()
    {
        using var tempDirectory = TemporaryDirectory.Create();
        var inputPath = GetFixturePath();
        var outputPath = Path.Combine(tempDirectory.Path, "simplified.geojson");
        var client = CreateClient();

        var result = await client.SimplifyAsync(inputPath, outputPath, "50%");

        Assert.True(result.IsSuccess, result.StdErr);
        Assert.True(File.Exists(outputPath));

        using var document = JsonDocument.Parse(await File.ReadAllTextAsync(outputPath));
        Assert.Equal("FeatureCollection", document.RootElement.GetProperty("type").GetString());
    }

    [MapshaperCliFact]
    public async Task FilterFieldsAsync_WithRealCli_KeepsOnlyRequestedFields()
    {
        using var tempDirectory = TemporaryDirectory.Create();
        var inputPath = GetFixturePath();
        var outputPath = Path.Combine(tempDirectory.Path, "filtered.geojson");
        var client = CreateClient();

        var result = await client.FilterFieldsAsync(inputPath, ["NAME", "CODE"], outputPath);

        Assert.True(result.IsSuccess, result.StdErr);
        Assert.True(File.Exists(outputPath));

        using var document = JsonDocument.Parse(await File.ReadAllTextAsync(outputPath));
        var features = document.RootElement.GetProperty("features");
        Assert.NotEmpty(features.EnumerateArray());

        foreach (var feature in features.EnumerateArray())
        {
            var properties = feature.GetProperty("properties");
            Assert.True(properties.TryGetProperty("NAME", out _));
            Assert.True(properties.TryGetProperty("CODE", out _));
            Assert.False(properties.TryGetProperty("DROP", out _));
        }
    }

    private static string GetFixturePath()
    {
        return Path.Combine(AppContext.BaseDirectory, "Fixtures", "two-polygons.geojson");
    }

    private static MapshaperClient CreateClient()
    {
        return new MapshaperClient(new MapshaperOptions { ExecutablePath = MapshaperCli.ExecutablePath! });
    }

    private sealed class TemporaryDirectory : IDisposable
    {
        private TemporaryDirectory(string path)
        {
            Path = path;
        }

        public string Path { get; }

        public static TemporaryDirectory Create()
        {
            var path = System.IO.Path.Combine(
                System.IO.Path.GetTempPath(),
                $"mapshaper-net-tests-{Guid.NewGuid():N}");
            Directory.CreateDirectory(path);
            return new TemporaryDirectory(path);
        }

        public void Dispose()
        {
            if (Directory.Exists(Path))
            {
                Directory.Delete(Path, recursive: true);
            }
        }
    }
}

public sealed class MapshaperCliFactAttribute : FactAttribute
{
    public MapshaperCliFactAttribute()
    {
        Skip = MapshaperCli.SkipReason;
    }
}

internal static class MapshaperCli
{
    private static readonly Lazy<(string? ExecutablePath, string? SkipReason)> Availability = new(CheckAvailability);

    public static string? ExecutablePath => Availability.Value.ExecutablePath;

    public static string? SkipReason => Availability.Value.SkipReason;

    private static (string? ExecutablePath, string? SkipReason) CheckAvailability()
    {
        foreach (var executablePath in GetExecutableCandidates())
        {
            if (CanRunVersionCheck(executablePath, out var skipReason))
            {
                return (executablePath, null);
            }

            if (skipReason == "mapshaper CLI version check timed out.")
            {
                return (null, skipReason);
            }
        }

        return (null, "mapshaper CLI is not available.");
    }

    private static IEnumerable<string> GetExecutableCandidates()
    {
        yield return "mapshaper";

        if (OperatingSystem.IsWindows())
        {
            yield return "mapshaper.exe";
            yield return "mapshaper.cmd";
            yield return "mapshaper.bat";
        }
    }

    private static bool CanRunVersionCheck(string executablePath, out string? skipReason)
    {
        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = executablePath,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
            };
            startInfo.ArgumentList.Add("-v");

            using var process = Process.Start(startInfo);
            if (process is null)
            {
                skipReason = "mapshaper CLI is not available.";
                return false;
            }

            process.WaitForExit(5000);
            if (!process.HasExited)
            {
                process.Kill(entireProcessTree: true);
                skipReason = "mapshaper CLI version check timed out.";
                return false;
            }

            skipReason = process.ExitCode == 0 ? null : "mapshaper CLI is not available.";
            return process.ExitCode == 0;
        }
        catch (Exception exception) when (exception is InvalidOperationException or System.ComponentModel.Win32Exception)
        {
            skipReason = "mapshaper CLI is not available.";
            return false;
        }
    }
}
