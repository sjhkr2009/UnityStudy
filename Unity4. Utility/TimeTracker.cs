using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEngine.Pool;

public class TimeTracker : IDisposable {
    protected struct TimeTrace {
        public long totalTime;
        public long elapsedTime;
        public string message;

        public TimeTrace(long totalTime, long elapsedTime, string message) {
            this.totalTime = totalTime;
            this.elapsedTime = elapsedTime;
            this.message = message;
        }
    }

    public TimeTracker(string name) {
        Name = name;
        TimeTraces = ListPool<TimeTrace>.Get();
        TimeTraces.Add(new TimeTrace(0, 0, "Start"));
        Stopwatch = Stopwatch.StartNew();
    }

    public string Name { get; }
    public int Count => TimeTraces.Count;
    
    protected Stopwatch Stopwatch { get; }
    protected List<TimeTrace> TimeTraces { get; }

    public void CheckPoint() => CheckPoint($"{Name}_{TimeTraces.Count}");
    public void CheckPoint(string msg) {
        var totalTime = Stopwatch.ElapsedMilliseconds;
        var elapsedTime = totalTime - TimeTraces.LastOrDefault().totalTime;
        TimeTraces.Add(new TimeTrace(totalTime, elapsedTime, msg));
    }

    public virtual string ExportLog() {
        StringBuilder log = new StringBuilder();

        log.AppendLine();
        log.AppendLine($"[TimeTracker] {Name}");
        for (int i = 0; i < Count; i++) {
            var msg = TimeTraces[i].message;
            var elapsedTime = TimeTraces[i].elapsedTime;
            log.AppendLine($"[{i}] {msg}: {elapsedTime}ms");
        }
        /* 로그 예시
         * [TimeTracker] Do Something
         * [0] Start: 0ms
         * [1] Check Network Status: 1ms
         * [2] Connect to Server: 267ms
         * [3] Initialize User Account: 95ms
         * [4] Find Profile Info: 41ms
         * ...
         */

        return log.ToString();
    }

    public void Stop() {
        Stopwatch.Stop();
    }

    public void Restart() {
        Stopwatch.Restart();
    }

    public virtual void Dispose() {
        Stopwatch?.Stop();
        ListPool<TimeTrace>.Release(TimeTraces);
        TimeTraces.Clear();
    }
}

public class TimeTrackerScope : TimeTracker {
    public TimeTrackerScope(string name) : base(name) { }

    public override void Dispose() {
        CheckPoint($"{Name} | End Process");
        UnityEngine.Debug.Log(ExportLog());

        base.Dispose();
    }
}
