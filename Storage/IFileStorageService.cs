namespace OnlineLibrary.Storage
{
    public interface IFileStorageService
    {
        string GetUrl(IFile file);

        Task<MemoryStream> DownloadFileAsync(IFile file);

        Task UploadFileAsync(IFile file, bool overwrite);
    }
}
