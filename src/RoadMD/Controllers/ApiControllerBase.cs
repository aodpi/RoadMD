using Microsoft.AspNetCore.Mvc;

namespace RoadMD.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    public abstract class ApiControllerBase : ControllerBase
    {
    }
}
