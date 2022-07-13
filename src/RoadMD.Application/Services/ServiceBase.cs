using MapsterMapper;
using RoadMD.EntityFrameworkCore;

namespace RoadMD.Application.Services
{
    /// <summary>
    /// Base class used for services containing dbcontext
    /// and object mapper
    /// </summary>
    public abstract class ServiceBase
    {
        public ServiceBase(ApplicationDbContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        /// <summary>
        /// Mapper from entity to Dto
        /// </summary>
        protected IMapper Mapper { get; init; }

        /// <summary>
        /// Database context
        /// </summary>
        protected ApplicationDbContext Context { get; init; }
    }
}
