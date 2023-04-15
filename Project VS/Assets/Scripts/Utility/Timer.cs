using System;

public class Timer {
    public DateTime StartTime { get; private set; }
    
    private DateTime lastUpdatedTime;
    public TimeSpan DeltaTime { get; private set; }
    public float DeltaTimeSeconds => (float)DeltaTime.TotalSeconds;
    
    public TimeSpan GetElapsedTime => lastUpdatedTime - StartTime;
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

    public void Update() {
        var currentTime = CurrentTime;
        DeltaTime = currentTime - lastUpdatedTime;
        lastUpdatedTime = CurrentTime;
    }

    public static Timer StartNew() {
        var timer = new Timer();
        timer.Start();
        return timer;
    }

    public void Start() {
        StartTime = CurrentTime;
        lastUpdatedTime = StartTime;
        DeltaTime = TimeSpan.Zero;
    }
}
