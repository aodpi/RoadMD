using Microsoft.AspNetCore.Mvc;
using RoadMD.Modules.Abstractions;

namespace RoadMD.Controllers
{
    /// <summary>
    /// Incindent reports
    /// </summary>
    public class ReportsController : ApiControllerBase
    {
        private readonly IEmailSender _emailSender;

        public ReportsController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        /// <summary>
        /// List reports based on type
        /// </summary>
        /// <remarks>
        /// ***This is a test description***
        /// </remarks>
        /// <param name="typeOfGet">The type you want to get</param>
        /// <returns>An example message</returns>
        /// <response code="200" href="https://google.com">Everything is fine</response>
        /// <response code="400">you didn't specify the type</response>
        /// <response code="500">server error</response>
        [HttpGet]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public string Get(string typeOfGet)
        {
            _emailSender.Send("admin@admin.com", "someone tried to access reports service");
            return "Hello World!!";
        }
    }
}
