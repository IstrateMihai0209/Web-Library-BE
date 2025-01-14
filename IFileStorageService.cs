namespace OnlineLibrary
{
    public interface IFileStorageService
    {
        public Task UploadFile(IFile file);

        public Task OpenFile(IFile file);
    }
}
