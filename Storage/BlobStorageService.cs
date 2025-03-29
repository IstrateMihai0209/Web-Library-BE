using Azure;
using Azure.Storage.Blobs;
using Extensions;

namespace OnlineLibrary.Storage
{
    public class BlobStorageService : IFileStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly BlobContainerClient _blobContainerClient;
        private readonly string _blobContainerName;
        
        public BlobStorageService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("AzureStorageConnectionString");
            _blobContainerName = configuration["AzureStorage:lib-files"];
            _blobServiceClient = new BlobServiceClient(connectionString);
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(_blobContainerName);
        }

        public string GetUrl(IFile file)
        {
            try
            {
                var blobClient = _blobContainerClient.GetBlobClient(file.Name);
                return blobClient.Uri.AbsoluteUri;
            }
            catch (RequestFailedException ex) when (ex.ErrorCode == "ContainerNotFound")
            {
                throw new InvalidOperationException("The specified container does not exist!", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the blob url!", ex);
            }
        }

        public async Task<MemoryStream> DownloadFileAsync(IFormFile file)
        {
            try
            {
                var blobClient = _blobContainerClient.GetBlobClient(file.FileName);

                if (!await blobClient.ExistsAsync())
                {
                    throw new FileNotFoundException("The blob was not found!", file.FileName);
                }

                var memoryStream = new MemoryStream();
                await blobClient.DownloadToAsync(memoryStream);
                memoryStream.Position = 0;
                return memoryStream;
            }
            catch (RequestFailedException ex) when (ex.ErrorCode == "BlobNotFound")
            {
                throw new FileNotFoundException("The specified blob does not exist!", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while downloading the file!", ex);
            }
        }

        public async Task UploadFileAsync(IFormFile file, bool overwrite = true)
        {
            try
            {
                var blobClient = _blobContainerClient.GetBlobClient($"{file.FileName}{file.GetFileExtension()}");
                
                using var uploadFileStream = file.OpenReadStream();
                await blobClient.UploadAsync(uploadFileStream, overwrite);
                uploadFileStream.Close();
            }
            catch (RequestFailedException ex) when (ex.ErrorCode == "ContainerNotFound")
            {
                throw new InvalidOperationException("The specified container does not exist!", ex);
            }
            catch (RequestFailedException ex) when (ex.ErrorCode == "LeaseIdMissing")
            {
                throw new InvalidOperationException("A lease ID is missing for this operation!", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while uploading the file!", ex);
            }
        }

        public async Task DeleteFileAsync(string blobName)
        {
            try
            {
                var blobClient = _blobContainerClient.GetBlobClient(blobName);
                await blobClient.DeleteIfExistsAsync();
            }
            catch (RequestFailedException ex) when (ex.ErrorCode == "ContainerNotFound")
            {
                throw new InvalidOperationException("The specified container does not exist!", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the file!", ex);
            }
        }
    }
}
