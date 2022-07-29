namespace RoadMD.Modules.Abstractions
{
    public interface IPhotoStorageService
    {
        /// <summary>
        /// Store a photo in azure blob storage
        /// </summary>
        /// <param name="filename">Original filename</param>
        /// <param name="content">Photo content</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<(string Url, Guid BlobName)> StorePhoto(string filename, Stream content,
            CancellationToken cancellationToken);

        /// <summary>
        /// Delete a photo from blob
        /// </summary>
        /// <param name="blobName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> DeletePhoto(Guid blobName, CancellationToken cancellationToken);
    }
}
