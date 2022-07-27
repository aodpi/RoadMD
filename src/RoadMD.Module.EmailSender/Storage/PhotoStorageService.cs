using Azure.Storage.Blobs;
using RoadMD.Modules.Abstractions;

namespace RoadMD.Modules.Storage
{
    public class PhotoStorageService : IPhotoStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public PhotoStorageService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<(string Url, Guid BlobName)> StorePhoto(string filename, Stream content, CancellationToken cancellationToken = default)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("photos");

            var extension = Path.GetExtension(filename);
            var blobName = Guid.NewGuid();
            var newPath = $"{blobName}{extension}";

            await containerClient.UploadBlobAsync(newPath, content, cancellationToken);

            string rawUrl = Path.Combine(containerClient.Uri.ToString(), newPath);
            var url = new Uri(rawUrl, UriKind.Absolute).ToString();
            
            return (url, blobName);
        }

        public async Task<bool> DeletePhoto(Guid blobName, CancellationToken cancellationToken = default)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("photos");

            var response = await containerClient.DeleteBlobIfExistsAsync(blobName.ToString(), cancellationToken: cancellationToken);

            return response.Value;
        }
    }
}
