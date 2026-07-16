using System.Diagnostics;

public class Timer
{
    private Stopwatch _sw;

    public double ResultDuration;
    public string Tag;
    public bool Stopped;

    public Timer(string tag)
    {
        Tag = tag;
        _sw = new Stopwatch();
        Start();
    }

    public void Start()
    {
        _sw.Start();
    }

    public void Stop()
    {
        _sw.Stop();
        ResultDuration = _sw.Elapsed.TotalMilliseconds;
        Stopped = true;
    }

    public override string ToString()
    {
        return Tag + " : " + ResultDuration + " ms";
    }
}