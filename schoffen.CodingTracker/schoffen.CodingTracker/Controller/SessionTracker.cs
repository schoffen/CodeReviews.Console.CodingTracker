using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace schoffen.CodingTracker.Controller;

public class SessionTracker
{
    private readonly Stopwatch _stopwatch = new();
    private bool _isRunning;
    public DateTime StartDateTime { get; private set; }
    public DateTime EndDateTime { get; private set; }

    public void Start()
    {
        StartDateTime = DateTime.Now;
        _stopwatch.Start();
        _isRunning = true;
    }

    public void Stop()
    {
        if (!_isRunning)
            return;
        
        _stopwatch.Stop();
        EndDateTime = DateTime.Now;
        _isRunning = false;
    }

    public TimeSpan GetElapsed()
    {
        return _stopwatch.Elapsed;
    }
}