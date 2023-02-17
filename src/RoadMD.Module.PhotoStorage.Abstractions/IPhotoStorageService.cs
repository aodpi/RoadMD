namespace RoadMD.Module.PhotoStorage.Abstractions
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
        Task<(string Url, Guid BlobName)> StorePhotoAsync(string filename, Stream content, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete a photo from blob
        /// </summary>
        /// <param name="blobNames"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> DeletePhotosAsync(IEnumerable<Guid> blobNames, CancellationToken cancellationToken = default);
    }
}
