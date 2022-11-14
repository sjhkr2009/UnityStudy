using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/** 다운로드 관련 클래스들을 조작하는 클래스로, 씬에 배치할 컴포넌트이다. */
public class AddressableDownloader : MonoBehaviour {
    [SerializeField] private string label;
    [SerializeField] private string url;
    
    public enum State {
        Idle,
        Initialize,
        UpdateCatalog,
        DownloadSize,
        DownloadDependencies,
        DownloadingBundle,
        Finished
    }

    private DownloadController Controller { get; set; }
    private Action<DownloadEvents> OnEventObtained { get; set; }
    
    public State CurrentState { get; private set; } = State.Idle;
    public State LastValidState { get; private set; } = State.Idle;

    public async Task StartDownload(Action<DownloadEvents> onEventObtained) {
        if (CurrentState != State.Idle) return;
        
        Controller = new DownloadController();
        OnEventObtained = onEventObtained;

        LastValidState = CurrentState = State.Initialize;
        while (CurrentState != State.Finished) {
            OnExecute();
            await Task.Yield();
        }
    }

    void OnExecute() {
        if (CurrentState == State.Idle) return;

        if (CurrentState == State.Initialize) {
            var events = Controller.InitializeSystem(label, url);
            OnEventObtained?.Invoke(events);
        } else if (CurrentState == State.UpdateCatalog) {
            Controller.UpdateCatalog();
            CurrentState = State.Idle;
        } else if (CurrentState == State.DownloadSize) {
            Controller.DownloadSize();
            CurrentState = State.Idle;
        } else if (CurrentState == State.DownloadDependencies) {
            Controller.StartDownload();
            CurrentState = State.DownloadingBundle;
        } else if (CurrentState == State.DownloadingBundle) {
            Controller.Update();
        }
    }

    public void GoNext() {
        CurrentState = LastValidState switch {
            State.Initialize => State.UpdateCatalog,
            State.UpdateCatalog => State.DownloadSize,
            State.DownloadSize => State.DownloadDependencies,
            State.DownloadDependencies => State.Finished,
            State.DownloadingBundle => State.Finished,
            _ => CurrentState
        };
        LastValidState = CurrentState;
    }
}
