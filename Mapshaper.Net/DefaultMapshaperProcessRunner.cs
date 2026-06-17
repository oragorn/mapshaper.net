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

        MapshaperProcessArguments.AddTo(startInfo, arguments);

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

        var stdOutTask = process.StandardOutput.ReadToEndAsync();
        var stdErrTask = process.StandardError.ReadToEndAsync();

        try
        {
            await WaitForExitAsync(process, cancellationToken).ConfigureAwait(false);
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
                process.Kill();
            }
        }
        catch (InvalidOperationException)
        {
        }
        catch (Win32Exception)
        {
        }
    }

    private static Task WaitForExitAsync(Process process, CancellationToken cancellationToken)
    {
        if (process.HasExited)
        {
            return Task.CompletedTask;
        }

        var completion = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        void OnExited(object? sender, EventArgs args) => completion.TrySetResult(true);

        process.EnableRaisingEvents = true;
        process.Exited += OnExited;

        if (process.HasExited)
        {
            process.Exited -= OnExited;
            return Task.CompletedTask;
        }

        if (!cancellationToken.CanBeCanceled)
        {
            return completion.Task.ContinueWith(
                task =>
                {
                    process.Exited -= OnExited;
                    return task.GetAwaiter().GetResult();
                },
                CancellationToken.None,
                TaskContinuationOptions.ExecuteSynchronously,
                TaskScheduler.Default);
        }

        return WaitWithCancellationAsync(process, completion, OnExited, cancellationToken);
    }

    private static async Task WaitWithCancellationAsync(
        Process process,
        TaskCompletionSource<bool> completion,
        EventHandler onExited,
        CancellationToken cancellationToken)
    {
        using (cancellationToken.Register(() => completion.TrySetCanceled()))
        {
            try
            {
                await completion.Task.ConfigureAwait(false);
            }
            finally
            {
                process.Exited -= onExited;
            }
        }
    }

}

internal static class MapshaperProcessArguments
{
    public static void AddTo(ProcessStartInfo startInfo, IEnumerable<string> arguments)
    {
        if (TryAddWithArgumentList(startInfo, arguments))
        {
            return;
        }

        startInfo.Arguments = BuildArgumentString(arguments);
    }

    internal static string BuildArgumentString(IEnumerable<string> arguments)
    {
        return string.Join(" ", arguments.Select(QuoteArgument));
    }

    private static string QuoteArgument(string argument)
    {
        if (argument.Length == 0)
        {
            return "\"\"";
        }

        if (!argument.Any(character => char.IsWhiteSpace(character) || character == '"'))
        {
            return argument;
        }

        var quoted = new System.Text.StringBuilder();
        quoted.Append('"');

        var backslashCount = 0;
        foreach (var character in argument)
        {
            if (character == '\\')
            {
                backslashCount++;
                continue;
            }

            if (character == '"')
            {
                quoted.Append('\\', backslashCount * 2 + 1);
                quoted.Append('"');
                backslashCount = 0;
                continue;
            }

            quoted.Append('\\', backslashCount);
            backslashCount = 0;
            quoted.Append(character);
        }

        quoted.Append('\\', backslashCount * 2);
        quoted.Append('"');
        return quoted.ToString();
    }

    private static bool TryAddWithArgumentList(ProcessStartInfo startInfo, IEnumerable<string> arguments)
    {
        var argumentListProperty = typeof(ProcessStartInfo).GetProperty("ArgumentList");
        var argumentList = argumentListProperty?.GetValue(startInfo);
        var addMethod = argumentList?.GetType().GetMethod("Add", [typeof(string)]);
        if (addMethod is null)
        {
            return false;
        }

        foreach (var argument in arguments)
        {
            addMethod.Invoke(argumentList, [argument]);
        }

        return true;
    }
}
