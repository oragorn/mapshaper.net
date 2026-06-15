using System.ComponentModel;
using System.Diagnostics;

namespace Mapshaper.Net;

internal sealed class DefaultMapshaperProcessRunner : IMapshaperProcessRunner
{
    public async Task<MapshaperResult> RunAsync(
        MapshaperOptions options,
        IReadOnlyList<string> arguments,
        CancellationToken cancellationToken)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = options.ExecutablePath,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
        };

        if (!string.IsNullOrWhiteSpace(options.WorkingDirectory))
        {
            startInfo.WorkingDirectory = options.WorkingDirectory;
        }

        foreach (var argument in arguments)
        {
            startInfo.ArgumentList.Add(argument);
        }

        using var process = new Process { StartInfo = startInfo };

        try
        {
            if (!process.Start())
            {
                throw MapshaperException.ForStartFailure(
                    options.ExecutablePath,
                    arguments,
                    new InvalidOperationException("The process did not start."));
            }
        }
        catch (Win32Exception exception)
        {
            throw MapshaperException.ForStartFailure(options.ExecutablePath, arguments, exception);
        }
        catch (InvalidOperationException exception)
        {
            throw MapshaperException.ForStartFailure(options.ExecutablePath, arguments, exception);
        }

        var stdOutTask = process.StandardOutput.ReadToEndAsync(cancellationToken);
        var stdErrTask = process.StandardError.ReadToEndAsync(cancellationToken);

        try
        {
            await process.WaitForExitAsync(cancellationToken).ConfigureAwait(false);
            var stdOut = await stdOutTask.ConfigureAwait(false);
            var stdErr = await stdErrTask.ConfigureAwait(false);

            return new MapshaperResult(
                process.ExitCode,
                stdOut,
                stdErr,
                options.ExecutablePath,
                arguments);
        }
        catch (OperationCanceledException)
        {
            TryKill(process);
            throw;
        }
    }

    private static void TryKill(Process process)
    {
        try
        {
            if (!process.HasExited)
            {
                process.Kill(entireProcessTree: true);
            }
        }
        catch (InvalidOperationException)
        {
        }
        catch (Win32Exception)
        {
        }
    }
}
