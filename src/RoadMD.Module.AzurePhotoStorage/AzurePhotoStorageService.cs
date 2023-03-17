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

        /// <inheritdoc />
        public async Task<(string Url, Guid BlobName)> StorePhotoAsync(string filename, Stream content, CancellationToken cancellationToken = default)
        {
            var containerName = _configuration.GetValue(PhotoContainerKey, string.Empty);

            if (string.IsNullOrEmpty(containerName))
            {
                throw new InvalidOperationException("Photo container name is missing in the configuration.");
            }

            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            if (!await containerClient.ExistsAsync(cancellationToken))
            {
                await containerClient.CreateAsync(cancellationToken: cancellationToken);
            }

            var extension = Path.GetExtension(filename);
            var blobName = Guid.NewGuid();
            var newPath = $"{blobName}{extension}";

            await containerClient.UploadBlobAsync(newPath, content, cancellationToken);

            var rawUrl = Path.Combine(containerClient.Uri.ToString(), newPath);
            var url = new Uri(rawUrl, UriKind.Absolute).ToString();

            return (url, blobName);
        }

        /// <inheritdoc />
        public async Task<bool> DeletePhotosAsync(IEnumerable<Guid> blobNames, CancellationToken cancellationToken = default)
        {
            var containerName = _configuration.GetValue<string>(PhotoContainerKey, string.Empty);

            if (string.IsNullOrEmpty(containerName))
            {
                throw new InvalidOperationException("Photo container name is missing in the configuration.");
            }

            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            var tasks = blobNames.Select(blobName =>
                containerClient.DeleteBlobIfExistsAsync(blobName.ToString(), cancellationToken: cancellationToken));

            var results = await Task.WhenAll(tasks);

            return results.All(r => r);
        }
    }
}