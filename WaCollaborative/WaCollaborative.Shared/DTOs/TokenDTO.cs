#region Using

#endregion Using

namespace WaCollaborative.Shared.DTOs
{

    /// <summary>
    /// The class TokenDTO
    /// </summary>

    public class TokenDTO
    {

        #region Attributes

        public string Token { get; set; } = null!;

        public DateTime Expiration { get; set; }

        #endregion Attributes

    }
}