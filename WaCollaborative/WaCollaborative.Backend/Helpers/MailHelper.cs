#region Using

using MailKit.Net.Smtp;
using MailKit.Security;
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
                    client.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;//TODO: verificar si esta línea es la solución óptima para el error de conexión
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

        public async Task<Response<string>> SendMailWithAttachmentAsync(string toName, string toEmail, string subject, string body, Stream attachmentStream, string attachmentFileName)
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

                // Adjuntar el archivo
                var attachment = new MimePart("application", "vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    Content = new MimeContent(attachmentStream, ContentEncoding.Default),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = attachmentFileName
                };

                bodyBuilder.Attachments.Add(attachment);
                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

                    await client.ConnectAsync(smtp, int.Parse(port!), SecureSocketOptions.StartTls);

                    await client.AuthenticateAsync(from, password);

                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
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