using System;

/** 어드레서블 다운로드와 관련한 이벤트를 외부에서 등록할 수 있게 제공한다. */
public class DownloadEvents {
    // 1. 어드레서블 시스템 초기화
    public event Action SystemInitializedListener;
    public void OnInitialized() => SystemInitializedListener?.Invoke();

    // 2. Catalog 업데이트
    public event Action CatalogUpdatedListener;
    public void OnCatalogUpdated() => CatalogUpdatedListener?.Invoke();

    // 3. Size 다운로드
    public event Action<long> SizeDownloadedListener;
    public void OnSizeDownloaded(long size) => SizeDownloadedListener?.Invoke(size);

    // 4. Bundle 다운로드 진행 중일 때
    public event Action<DownloadProgress> BundleProgressListener;
    public void OnBundleProgressUpdated(DownloadProgress progress) => BundleProgressListener?.Invoke(progress);
    
    // 5. 번들 다운로드 완료
    public event Action<bool> BundleDownloadCompleteListener;
    public void OnBundleDownloadComplete(bool success) => BundleDownloadCompleteListener?.Invoke(success);
}