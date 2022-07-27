using Microsoft.Extensions.Logging;
using RoadMD.Modules.Abstractions;

namespace RoadMD.Modules.Email
{
    public class EmailSenderMailKit : IEmailSender
    {
        private readonly ILogger<EmailSenderMailKit> _logger;

        public EmailSenderMailKit(ILogger<EmailSenderMailKit> logger)
        {
            _logger = logger;
        }

        public void Send(string to, string email)
        {
            // Send email using mailkit;

            _logger.LogInformation("Sending e-mail using MailKit... (Currently not implemented)");
        }
    }
}
