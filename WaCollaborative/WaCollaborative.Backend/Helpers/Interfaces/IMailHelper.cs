namespace WaCollaborative.Backend.Helpers.Interfaces
{

    #region Import

    using WaCollaborative.Shared.Responses;

    #endregion Import

    /// <summary>
    /// The interface IMailHelper
    /// </summary>

    public interface IMailHelper
    {

        #region Methods

        public Response<string> SendMail(string toName, string toEmail, string subject, string body);

        Task<Response<string>> SendMailWithAttachmentAsync(string toName, string toEmail, string subject, string body, Stream attachmentStream, string attachmentFileName);
        #endregion Methods

    }
}