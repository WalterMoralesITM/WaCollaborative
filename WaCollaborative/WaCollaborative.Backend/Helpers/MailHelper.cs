#region Using

using MailKit.Net.Smtp;
using MimeKit;
using WaCollaborative.Backend.Helpers.Interfaces;
using WaCollaborative.Shared.Responses;

#endregion Using

namespace WaCollaborative.Backend.Helpers
{

    /// <summary>
    /// The class MailHelper
    /// </summary>

    public class MailHelper : IMailHelper
    {

        #region Attributes

        private readonly IConfiguration _configuration;

        #endregion Attributes

        #region Constructor

        public MailHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion Constructor

        #region Methods

        public Response<string> SendMail(string toName, string toEmail, string subject, string body)
        {
            try
            {
                var from = _configuration["Mail:From"];
                var name = _configuration["Mail:Name"];
                var smtp = _configuration["Mail:Smtp"];
                var port = _configuration["Mail:Port"];
                var password = _configuration["Mail:Password"];

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(name, from));
                message.To.Add(new MailboxAddress(toName, toEmail));
                message.Subject = subject;
                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = body
                };
                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    client.Connect(smtp, int.Parse(port!), false);
                    client.Authenticate(from, password);
                    client.Send(message);
                    client.Disconnect(true);
                }

                return new Response<string> { WasSuccess = true };
            }
            catch (Exception ex)
            {
                return new Response<string>
                {
                    WasSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        #endregion Methods

    }
}