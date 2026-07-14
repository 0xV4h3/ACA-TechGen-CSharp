using System.Diagnostics;

namespace CopyTool;

public class ConsoleProgressBar : IProgressReporter
{
    private readonly ulong _totalTicks;
    private readonly int _barLength;
    private readonly Stopwatch _stopwatch;
    private readonly ulong _updateIntervalTicks;
    private readonly Func<double, string> _speedFormatter;
    private int _lastLineLength;
    private bool _disposed;
    private bool _finished;
    
    public ConsoleProgressBar(
        ulong totalTicks,
        int barLength = 30,
        ulong updateIntervalTicks = 1,
        Func<double, string>? speedFormatter = null)
    {
        _totalTicks = totalTicks;
        _barLength = barLength;
        _updateIntervalTicks = updateIntervalTicks;
        
        _speedFormatter = speedFormatter ?? (speed => speed switch
        {
            >= 1_000_000_000 => $"{speed / 1_000_000_000:F1} G it/s",
            >= 1_000_000 => $"{speed / 1_000_000:F1} M it/s",
            >= 1_000 => $"{speed / 1_000:F1} K it/s",
            _ => $"{speed:N0} it/s"
        });
            
        _stopwatch = new Stopwatch();
        Console.CursorVisible = false;
    }

    public void Report(ulong currentTick, string customMessage = "")
    {
        if (!_stopwatch.IsRunning && !_finished)
            _stopwatch.Start();

        if (_finished || (currentTick % _updateIntervalTicks != 0 && currentTick != _totalTicks))
            return;

        double progress = _totalTicks == 0 ? 1.0 : (double)currentTick / _totalTicks;
        int progressChars = (int)Math.Round(progress * _barLength);
        progressChars = Math.Clamp(progressChars, 0, _barLength);

        double elapsedSeconds = _stopwatch.Elapsed.TotalSeconds;
        double speed = elapsedSeconds > 0 ? currentTick / elapsedSeconds : 0;

        string etaStr = "Calculating...";
        if (progress > 0 && currentTick < _totalTicks)
        {
            double remainingSeconds = (elapsedSeconds / progress) - elapsedSeconds;
            TimeSpan remainingTime = TimeSpan.FromSeconds(remainingSeconds);

            etaStr = remainingTime.TotalHours >= 1
                ? remainingTime.ToString(@"hh\:mm\:ss")
                : remainingTime.ToString(@"mm\:ss");
        }
        else if (currentTick == _totalTicks)
        {
            etaStr = "00:00 (Done)";
        }

        string bar = new string('█', progressChars) + new string('░', _barLength - progressChars);

        string speedStr = _speedFormatter(speed);
        
        string output = $"\r[{bar}] {progress:P1} | Speed: {speedStr} | Remaining: {etaStr} | {customMessage}";
        
        int overlap = _lastLineLength - output.Length;
        if (overlap > 0)
            Console.Write(output + new string(' ', overlap));
        else
            Console.Write(output);
        
        _lastLineLength = output.Length;
        
        if (currentTick == _totalTicks)
            FinishBar();
    }

    private void FinishBar()
    {
        if (!_finished)
        {
            _finished = true;
            Console.WriteLine();
            Console.CursorVisible = true;
        }
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _disposed = true;
            FinishBar();
        }
    }
}
