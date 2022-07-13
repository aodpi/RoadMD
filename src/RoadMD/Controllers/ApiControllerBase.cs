using Microsoft.AspNetCore.Mvc;

namespace RoadMD.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public abstract class ApiControllerBase : ControllerBase
    {
    }
}
