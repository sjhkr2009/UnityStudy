using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/** 어드레서블 다운로드와 관련해 유저와의 상호작용을 수행할 스크립트 */
public class DownloadPopup : MonoBehaviour {
    public enum State {
        None,
        CalculatingSize,
        NothingToDownload,
        AskingDownload,
        Downloading,
        DownloadFinished
    }

    [Serializable]
    public class Root {
        public State State;
        public GameObject root;
    }

    [SerializeField] private List<Root> roots;
    [SerializeField] private Text title;
    [SerializeField] private Text desc;
    [SerializeField] private Text downloadBarText;

    private AddressableDownloader Downloader;

    private DownloadProgress progressInfo;
    private DownloadUtility.SizeUnits sizeUnit;
    private long currentSize;
    private long totalSize;

    public State CurrentState { get; private set; } = State.None;

    private void Start() => StartAsync();

    private async Task StartAsync() {
        SetState(State.CalculatingSize, true);

        await Downloader.StartDownload(events => {
            events.SystemInitializedListener += OnInitialized;
            events.CatalogUpdatedListener += OnCatalogUpdated;
            events.SizeDownloadedListener += OnSizeDownloaded;
            events.BundleProgressListener += OnDownloadProgress;
            events.BundleDownloadCompleteListener += OnFinished;
        });
    }

    void SetState(State newState, bool updateUI) {
        var prevRoot = roots.Find(r => r.State == CurrentState);
        var nextRoot = roots.Find(r => r.State == newState);

        CurrentState = newState;

        prevRoot?.root.SetActive(false);
        nextRoot?.root.SetActive(true);

        if (updateUI) UpdateUI();
    }

    void UpdateUI() {
        title.text = CurrentState switch {
            State.CalculatingSize => "알림",
            State.NothingToDownload => "완료",
            State.AskingDownload => "확인",
            State.Downloading => "다운로드 중...",
            State.DownloadFinished => "완료",
            _ => string.Empty
        };
        
        desc.text = CurrentState switch {
            State.CalculatingSize => "다운로드 정보를 가져오고 있습니다.",
            State.NothingToDownload => "데이터가 최신 상태입니다.",
            State.AskingDownload => $"다운로드를 받으시겠습니까? (크기: {totalSize}{sizeUnit})",
            State.Downloading => $"다운로드 중... {progressInfo.progress * 100f:0.00}%",
            State.DownloadFinished => "다운로드가 완료되었습니다.",
            _ => string.Empty
        };
    }

    public void OnClickStartDownload() {
        SetState(State.Downloading, true);
    }
    
    public void OnClickCancel() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    
    public void OnClickEnterGame() {
        SceneManager.LoadScene(1);
    }
    
    private void OnInitialized() {
        Downloader.GoNext();
    }
    
    private void OnCatalogUpdated() {
        Downloader.GoNext();
    }
    
    private void OnSizeDownloaded(long size) {
        if (size == 0) {
            SetState(State.NothingToDownload, true);
        } else {
            sizeUnit = DownloadUtility.GetProperByteUnit(size);
            totalSize = DownloadUtility.ConvertByteByUnit(size, sizeUnit);
        }
    }

    private void OnDownloadProgress(DownloadProgress info) {
        bool changed = progressInfo.downloadedBytes < info.downloadedBytes;
        progressInfo = info;

        if (changed) {
            UpdateUI();
            currentSize = DownloadUtility.ConvertByteByUnit(info.downloadedBytes, sizeUnit);
        }
    }

    private void OnFinished(bool success) {
        SetState(State.DownloadFinished, true);
        Downloader.GoNext();
    }
}
