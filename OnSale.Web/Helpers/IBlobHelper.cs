﻿using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace OnSale.Web.Helpers
{
    public interface IBlobHelper
    {
        #region [ Blob Zulu ]
        Task<Guid> UploadBlobAsync(IFormFile file, string containerName);

        Task<Guid> UploadBlobAsync(byte[] file, string containerName);

        Task<Guid> UploadBlobAsync(string image, string containerName);

        Task<Guid> UploadBlobAsync(Stream stream, string containerName);
        #endregion [ Blob Zulu ]

        #region [ Blob Gavilan ]
        public Task<string> SaveFile(IFormFile file, string container);
        
        public Task<string> SaveFile(byte[] file, string container);
        
        public Task DeleteFile(string path, string container);

        public Task<string> EditFile(IFormFile file, string container, string path);
        
        public Task<string> EditFile(byte[] file, string container, string path);
        #endregion [ Blob Gavilan ]
    }
}
