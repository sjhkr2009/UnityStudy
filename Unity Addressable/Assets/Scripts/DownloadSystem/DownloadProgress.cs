public struct DownloadProgress {
    public static DownloadProgress Create(long downloadedByte, long totalByte) {
        return new DownloadProgress() {
            downloadedBytes = downloadedByte,
            remainBytes = totalByte - downloadedByte,
            totalBytes = totalByte,
            progress = (float)((double)downloadedByte / totalByte)
        };
    }
    public long downloadedBytes;
    public long totalBytes;
    public long remainBytes;
    public float progress;
}