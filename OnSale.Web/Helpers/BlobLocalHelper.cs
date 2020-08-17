using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace OnSale.Web.Helpers
{
    public class BlobLocalHelper : IBlobHelper
    {
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CloudBlobClient _blobClient;

        public BlobLocalHelper(
            IWebHostEnvironment env,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _env = env;
            _httpContextAccessor = httpContextAccessor;
            string connectionString = configuration["Blob:ConnectionString"];
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            _blobClient = storageAccount.CreateCloudBlobClient();
        }

        #region [ Blob Zulu ]
        public async Task<Guid> UploadBlobAsync(IFormFile file, string containerName)
        {
            var nameFile = Guid.NewGuid();
            string folder = Path.Combine(_env.WebRootPath, containerName);

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string path = Path.Combine(folder, $"{nameFile}.{file.ContentType}");
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return nameFile;
        }

        public async Task<Guid> UploadBlobAsync(byte[] file, string containerName)
        {
            MemoryStream stream = new MemoryStream(file);
            Guid name = Guid.NewGuid();
            CloudBlobContainer container = _blobClient.GetContainerReference(containerName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference($"{name}");
            await blockBlob.UploadFromStreamAsync(stream);
            return name;
        }

        public async Task<Guid> UploadBlobAsync(string image, string containerName)
        {
            Stream stream = File.OpenRead(image);
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

            var nameFile = $"{Guid.NewGuid()}{extension}";
            string folder = Path.Combine(_env.WebRootPath, container);

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string path = Path.Combine(folder, nameFile);
            await File.WriteAllBytesAsync(path, content);

            var urlDomine = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";

            var url = Path.Combine(urlDomine, container, nameFile).Replace("\\", "/");

            return url;
        }

        public async Task<string> SaveFile(byte[] file, string container)
        {
            var extension = ".jpg";

            var nameFile = $"{Guid.NewGuid()}{extension}";
            string folder = Path.Combine(_env.WebRootPath, container);

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string path = Path.Combine(folder, nameFile);

            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                await fs.WriteAsync(file, 0, file.Length);
            }

            var url = Path.Combine("", container, nameFile).Replace("\\", "/");

            return url;
        }

        public Task DeleteFile(string path, string container)
        {
            if (path != null)
            {
                var nameFile = Path.GetFileName(path);
                string folderFile = Path.Combine(_env.WebRootPath, container, nameFile);

                if (File.Exists(folderFile))
                    File.Delete(folderFile);
            }
            return Task.FromResult(0);
        }

        public async Task<string> EditFile(IFormFile file, string container, string path)
        {
            await DeleteFile(path, container);
            return await SaveFile(file, container);
        }

        public async Task<string> EditFile(byte[] file, string container, string path)
        {
            await DeleteFile(path, container);
            return await SaveFile(file, container);
        }

        #endregion [ Blob Gavilan ]
    }
}
