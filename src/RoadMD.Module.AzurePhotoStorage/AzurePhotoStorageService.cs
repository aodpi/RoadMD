using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using RoadMD.Module.PhotoStorage.Abstractions;

namespace RoadMD.Module.AzurePhotoStorage
{
    public class AzurePhotoStorageService : IPhotoStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IConfiguration _configuration;

        private const string PhotoContainerKey = "BlobStorage:PhotoContainerName";

        public AzurePhotoStorageService(BlobServiceClient blobServiceClient, IConfiguration configuration)
        {
            _blobServiceClient = blobServiceClient;
            _configuration = configuration;
        }

        public async Task<(string Url, Guid BlobName)> StorePhoto(string filename, Stream content, CancellationToken cancellationToken = default)
        {
            var containerName = _configuration.GetValue(PhotoContainerKey, string.Empty);


            if (!await _blobServiceClient.GetBlobContainerClient(containerName).ExistsAsync(cancellationToken))
            {
                await _blobServiceClient.CreateBlobContainerAsync(containerName, cancellationToken: cancellationToken);
            }


            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            var extension = Path.GetExtension(filename);
            var blobName = Guid.NewGuid();
            var newPath = $"{blobName}{extension}";

            await containerClient.UploadBlobAsync(newPath, content, cancellationToken);

            string rawUrl = Path.Combine(containerClient.Uri.ToString(), newPath);
            var url = new Uri(rawUrl, UriKind.Absolute).ToString();

            return (url, blobName);
        }

        public async Task<bool> DeletePhotos(IEnumerable<Guid> blobNames, CancellationToken cancellationToken = default)
        {
            var containerName = _configuration.GetValue(PhotoContainerKey, string.Empty);

            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            bool result = false;

            foreach (var blobName in blobNames)
            {
                result &= await containerClient.DeleteBlobIfExistsAsync(blobName.ToString(), cancellationToken: cancellationToken);
            }

            return result;
        }
    }
}