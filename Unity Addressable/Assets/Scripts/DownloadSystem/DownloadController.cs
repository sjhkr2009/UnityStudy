using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.Exceptions;

/** 다운로드의 핵심 로직들을 실행하는 클래스 */
public class DownloadController {
    public static string DownloadURL { get; private set; }
    private DownloadEvents Events { get; set; }
    private string LabelToDownload { get; set; }
    private long TotalSize { get; set; }
    public AsyncOperationHandle DownloadHandle { get; private set; }

    public DownloadEvents InitializeSystem(string label, string url) {
        Events = new DownloadEvents();

        Addressables.InitializeAsync().Completed += _ => Events.OnInitialized();
        LabelToDownload = label;
        DownloadURL = url;
        
        // ResourceManager.ExceptionHandler를 통해 유니티 리소스 다운로드 중 에러 발생 시 이벤트를 추가할 수 있다.
        ResourceManager.ExceptionHandler += OnException;

        return Events;
    }
    
    public void Update() {
        // AsyncOperationHandle은 nullable 타입이 아니므로 IsValid로 핸들이 할당되었는지 확인
        if (!DownloadHandle.IsValid()) return;
        // 완료되었거나 실패했으면 리턴
        if (DownloadHandle.IsDone && DownloadHandle.Status == AsyncOperationStatus.Failed) return;

        var status = DownloadHandle.GetDownloadStatus();
        DownloadProgress progress = DownloadProgress.Create(status.DownloadedBytes, TotalSize);
        Events.OnBundleProgressUpdated(progress);
    }

    public void UpdateCatalog() {
        Addressables.CheckForCatalogUpdates().Completed += result => {
            var needUpdateList = result.Result;
            if (needUpdateList.Count > 0) {
                Addressables.UpdateCatalogs(needUpdateList).Completed += OnCatalogUpdate;
            } else {
                Events.OnCatalogUpdated();
            }
        };
    }

    public void DownloadSize() {
        Addressables.GetDownloadSizeAsync(LabelToDownload).Completed += OnSizeDownloaded;
    }

    public void StartDownload() {
        // DownloadDependenciesAsync로 해당 라벨이 붙은 에셋번들과 의존성이 있는 모든 에셋을 다운로드한다.
        DownloadHandle = Addressables.DownloadDependenciesAsync(LabelToDownload);
        DownloadHandle.Completed += OnDependencyDownloaded;
    }

    private void OnCatalogUpdate(AsyncOperationHandle<List<IResourceLocator>> obj) {
        Events.OnCatalogUpdated();
    }
    
    private void OnSizeDownloaded(AsyncOperationHandle<long> result) {
        // GetDownloadSizeAsync의 반환값 result.Result에 확인한 사이즈가 반환된다.
        TotalSize = result.Result;
        Events.OnSizeDownloaded(TotalSize);
    }
    
    private void OnDependencyDownloaded(AsyncOperationHandle result) {
        Events.OnBundleDownloadComplete(result.Status == AsyncOperationStatus.Succeeded);
    }
    
    private void OnException(AsyncOperationHandle handle, Exception exception) {
        Debug.LogError($"Exception Caught: {exception.GetType().Name} - {exception.Message}");

        if (exception is RemoteProviderException) {
            Debug.LogError($"Exception on Addressable Download!!");
        }
    }
}
