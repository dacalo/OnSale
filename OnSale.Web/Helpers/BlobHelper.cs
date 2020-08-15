using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace OnSale.Web.Helpers
{
    public class BlobHelper : IBlobHelper
    {
        private readonly CloudBlobClient _blobClient;

        public BlobHelper(IConfiguration configuration)
        {
            string connectionString = configuration["Blob:ConnectionString"];
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            _blobClient = storageAccount.CreateCloudBlobClient();
        }

        #region [ Blob Zulu ]
        public async Task<Guid> UploadBlobAsync(byte[] file, string containerName)
        {
            MemoryStream stream = new MemoryStream(file);
            return await UploadStreamAsync(stream, containerName);
        }

        public async Task<Guid> UploadBlobAsync(IFormFile file, string containerName)
        {
            Stream stream = file.OpenReadStream();
            return await UploadStreamAsync(stream, containerName);
        }

        public async Task<Guid> UploadBlobAsync(string image, string containerName)
        {
            Stream stream = File.OpenRead(image);
            return await UploadStreamAsync(stream, containerName);
        }

        private async Task<Guid> UploadStreamAsync(Stream stream, string containerName)
        {
            Guid name = Guid.NewGuid();
            CloudBlobContainer container = _blobClient.GetContainerReference(containerName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference($"{name}");
            await blockBlob.UploadFromStreamAsync(stream);
            return name;
        }
        #endregion [ Blob Zulu ]

        #region [ Blob Gavilan ]
        public async Task<string> SaveFile(IFormFile file, string container)
        {
            MemoryStream memoryStream = new MemoryStream();

            await file.CopyToAsync(memoryStream);
            var content = memoryStream.ToArray();
            var extension = Path.GetExtension(file.FileName);

            var containerReference = _blobClient.GetContainerReference(container);

            await containerReference.CreateIfNotExistsAsync();
            await containerReference.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });

            var nameFile = $"{Guid.NewGuid()}{extension}";
            var blob = containerReference.GetBlockBlobReference(nameFile);
            await blob.UploadFromByteArrayAsync(content, 0, content.Length);
            blob.Properties.ContentType = file.ContentType;
            await blob.SetPropertiesAsync();
            return blob.Uri.ToString();
        }

        public async Task DeleteFile(string path, string container)
        {
            if (path != null)
            {
                var containerReference = _blobClient.GetContainerReference(container);
                var nameBlob = Path.GetFileName(path);
                var blob = containerReference.GetBlobReference(nameBlob);
                await blob.DeleteIfExistsAsync();
            }
        }

        public async Task<string> EditFile(IFormFile file, string container, string path)
        {
            await DeleteFile(path, container);
            return await SaveFile(file, container);
        }
        #endregion [ Blob Gavilan ]
    }

}
