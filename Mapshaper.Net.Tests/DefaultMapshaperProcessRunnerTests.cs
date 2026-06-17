using System.Text.Json;
using Mapshaper.Net;

namespace Mapshaper.Net.Tests;

public sealed class DefaultMapshaperProcessRunnerTests
{
    public static IEnumerable<object[]> FallbackArgumentStrings()
    {
        yield return [new[] { "" }, "\"\""];
        yield return [new[] { "input.geojson" }, "input.geojson"];
        yield return [new[] { "folder with spaces/input file.geojson" }, "\"folder with spaces/input file.geojson\""];
        yield return [new[] { "NAME == \"A B\"" }, "\"NAME == \\\"A B\\\"\""];
        yield return [new[] { "field=\"quoted\"" }, "\"field=\\\"quoted\\\"\""];
        yield return [new[] { @"C:\folder with spaces\" }, @"""C:\folder with spaces\\"""];
        yield return [
            new[] { @"C:\input folder\data.geojson", "-filter", "NAME == \"A B\"", "-o", @"C:\output folder\" },
            @"""C:\input folder\data.geojson"" -filter ""NAME == \""A B\"""" -o ""C:\output folder\\"""
        ];
    }

    public static IEnumerable<object[]> ProcessArguments()
    {
        yield return [new[] { @"C:\input folder\data.geojson" }];
        yield return [new[] { "field=\"quoted value\"" }];
        yield return [new[] { @"C:\output folder\" }];
        yield return [new[] { "NAME == \"A B\"" }];
        yield return [
            new[]
            {
                @"C:\input folder\data.geojson",
                "-filter",
                "NAME == \"A B\"",
                "-o",
                @"C:\output folder\",
            }
        ];
    }

    [Theory]
    [MemberData(nameof(FallbackArgumentStrings))]
    public void BuildArgumentString_EscapesArgumentsForProcessStartInfoArguments(
        string[] arguments,
        string expected)
    {
        var argumentString = MapshaperProcessArguments.BuildArgumentString(arguments);

        Assert.Equal(expected, argumentString);
    }

    [Theory]
    [MemberData(nameof(ProcessArguments))]
    public async Task RunAsync_PreservesProcessArguments(string[] capturedArguments)
    {
        var powershellPath = FindPowerShell();
        if (powershellPath is null)
        {
            return;
        }

        using var tempDirectory = TemporaryDirectory.Create();
        var scriptPath = Path.Combine(tempDirectory.Path, "capture args.ps1");
        await File.WriteAllTextAsync(
            scriptPath,
            """
            [Console]::OutputEncoding = [System.Text.Encoding]::UTF8
            @($args) | ConvertTo-Json -Compress
            """);

        var runner = new DefaultMapshaperProcessRunner();
        var result = await runner.RunAsync(
            new MapshaperOptions { ExecutablePath = powershellPath },
            [
                "-NoProfile",
                "-ExecutionPolicy",
                "Bypass",
                "-File",
                scriptPath,
                .. capturedArguments,
            ],
            CancellationToken.None);

        Assert.True(result.IsSuccess, result.StdErr);

        var actualArguments = DeserializePowerShellJsonArray(result.StdOut);
        Assert.Equal(capturedArguments, actualArguments);
    }

    private static string[] DeserializePowerShellJsonArray(string json)
    {
        using var document = JsonDocument.Parse(json);
        if (document.RootElement.ValueKind == JsonValueKind.String)
        {
            return [document.RootElement.GetString()!];
        }

        return document.RootElement.EnumerateArray()
            .Select(element => element.GetString()!)
            .ToArray();
    }

    private static string? FindPowerShell()
    {
        foreach (var candidate in GetPowerShellCandidates())
        {
            try
            {
                using var process = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = candidate,
                    Arguments = "-NoProfile -Command \"$PSVersionTable.PSVersion.ToString()\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                });

                if (process is null)
                {
                    continue;
                }

                process.WaitForExit(5000);
                if (process.HasExited && process.ExitCode == 0)
                {
                    return candidate;
                }

                if (!process.HasExited)
                {
                    process.Kill();
                }
            }
            catch (Exception exception) when (exception is InvalidOperationException or System.ComponentModel.Win32Exception)
            {
            }
        }

        return null;
    }

    private static IEnumerable<string> GetPowerShellCandidates()
    {
        if (OperatingSystem.IsWindows())
        {
            yield return "powershell.exe";
        }

        yield return "pwsh";
        yield return "powershell";
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
                $"mapshaper-net-runner-tests-{Guid.NewGuid():N}");
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
