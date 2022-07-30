using LanguageExt;
using LanguageExt.Common;
using RoadMD.Application.Dto.Infractions;
using RoadMD.Application.Dto.Infractions.Create;
using RoadMD.Application.Dto.Infractions.List;
using RoadMD.Application.Dto.Infractions.Update;

namespace RoadMD.Application.Services.Infractions
{
    public interface IInfractionService
    {
        Task<Result<InfractionDto>> GetAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<InfractionListDto>> GetListAsync(CancellationToken cancellationToken = default);
        Task<Result<InfractionDto>> CreateAsync(CreateInfractionDto input, CancellationToken cancellationToken = default);
        Task<Result<InfractionDto>> UpdateAsync(UpdateInfractionDto input, CancellationToken cancellationToken = default);
        Task<Result<Unit>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result<Unit>> DeletePhotoAsync(Guid id, Guid photoId, CancellationToken cancellationToken = default);
    }
}