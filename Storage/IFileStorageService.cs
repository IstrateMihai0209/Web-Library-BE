namespace OnlineLibrary.Storage
{
    public interface IFileStorageService
    {
        string GetUrl(IFile file);

        Task<MemoryStream> DownloadFileAsync(IFormFile file);

        Task UploadFileAsync(IFormFile file, bool overwrite);
    }
}
