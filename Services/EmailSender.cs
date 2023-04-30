using DMed_Razor.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace API.Services
{

    public class EmailSender
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _config;

        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor,
                           ILogger<EmailSender> logger,
                           IConfiguration config)
        {
            _config = config;
            _logger = logger;
        }


        public async Task<bool> SendEmailAsync(string toEmail, string subject, string message)
        {

            bool success = await Execute(subject, message, toEmail);

            return success;
        }

        public async Task<bool> Execute(string subject, string message, string toEmail)
        {
            var apiKey = _config["SendGridKey"];
            var SendGridEmailAddress = _config["SendGridEmailAddress"];

            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(SendGridEmailAddress))
            {
                throw new Exception("Null SendGridKey or SendGridEmailAddress");
            }

            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(SendGridEmailAddress, "SkillGrid"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(toEmail));

            msg.SetClickTracking(false, false);
            var response = await client.SendEmailAsync(msg);
            _logger.LogInformation(response.IsSuccessStatusCode
                                   ? $"Email to {toEmail} queued successfully!"
                                   : $"Failure Email to {toEmail}");

            return response.IsSuccessStatusCode ? true : false;
        }
    }
}