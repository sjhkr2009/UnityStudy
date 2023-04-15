using System;

public class Timer {
    public DateTime StartTime { get; private set; }
    public TimeSpan GetElapsedTime => CurrentTime - StartTime;

    public float GetElapsedSeconds => (float)GetElapsedTime.TotalSeconds;
    
    private Timer() { }

    private static DateTime DefaultTimeGetter() => DateTime.Now;
    
    private Func<DateTime> getCurrentTimeFunc;

    private DateTime CurrentTime {
        get {
            getCurrentTimeFunc ??= DefaultTimeGetter;
            return getCurrentTimeFunc.Invoke();
        }
    }

    public static Timer StartNew() {
        var timer = new Timer();
        timer.Start();
        return timer;
    }

    public void Start() {
        StartTime = CurrentTime;
    }
}
