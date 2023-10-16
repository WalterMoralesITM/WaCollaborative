#region Using

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using WaCollaborative.Backend.Helpers.Interfaces;

#endregion Using

namespace WaCollaborative.Backend.Helpers
{

    /// <summary>
    /// The class FileStorage
    /// </summary>

    public class FileStorage : IFileStorage
    {

        #region Attributes

        private readonly string _connectionString;

        #endregion Attributes

        #region Constructor

        public FileStorage(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("AzureStorage")!;
        }

        #endregion Constructor

        #region Methods

        public async Task RemoveFileAsync(string path, string containerName)
        {
            var client = new BlobContainerClient(_connectionString, containerName);
            await client.CreateIfNotExistsAsync();
            var fileName = Path.GetFileName(path);
            var blob = client.GetBlobClient(fileName);
            await blob.DeleteIfExistsAsync();
        }

        public async Task<string> SaveFileAsync(byte[] content, string extention, string containerName)
        {
            var client = new BlobContainerClient(_connectionString, containerName);
            await client.CreateIfNotExistsAsync();
            client.SetAccessPolicy(PublicAccessType.Blob);
            var fileName = $"{Guid.NewGuid()}{extention}";
            var blob = client.GetBlobClient(fileName);

            using (var ms = new MemoryStream(content))
            {
                await blob.UploadAsync(ms);
            }

            return blob.Uri.ToString();
        }

        #endregion Methods

    }
}