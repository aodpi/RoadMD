using LanguageExt;
using LanguageExt.Common;
using RoadMD.Application.Dto.Infraction;
using RoadMD.Application.Dto.Infraction.Create;
using RoadMD.Application.Dto.Infraction.List;
using RoadMD.Application.Dto.Infraction.Update;

namespace RoadMD.Application.Services.Infractions
{
    public interface IInfractionService
    {
        Task<Result<InfractionDto>> GetAsync(Guid id, CancellationToken cancellationToken = default);

        Task<IEnumerable<InfractionListDto>> GetListAsync(
            CancellationToken cancellationToken = default);

        Task<Result<InfractionDto>> CreateAsync(CreateInfractionDto input,
            CancellationToken cancellationToken = default);

        Task<Result<InfractionDto>> UpdateAsync(UpdateInfractionDto input,
            CancellationToken cancellationToken = default);

        Task<Result<Unit>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}