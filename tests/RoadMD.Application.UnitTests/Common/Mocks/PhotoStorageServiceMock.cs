using RoadMD.Module.PhotoStorage.Abstractions;

namespace RoadMD.Application.UnitTests.Common.Mocks
{
    public class PhotoStorageServiceMock : Mock<IPhotoStorageService>
    {
        public PhotoStorageServiceMock MockStorePhoto(string filename, Stream content)
        {
            Setup(x => x.StorePhotoAsync(It.Is<string>(z => z == filename), It.Is<Stream>(s => s == content),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((new Faker().Internet.Url(), new Faker().Random.Guid()));
            return this;
        }

        public PhotoStorageServiceMock MockDeletePhotos(IEnumerable<Guid> blobNames)
        {
            Setup(x => x.DeletePhotosAsync(It.Is<IEnumerable<Guid>>(c => c == blobNames),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            return this;
        }
    }
}