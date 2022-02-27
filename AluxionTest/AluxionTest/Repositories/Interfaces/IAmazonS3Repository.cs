using AluxionTest.Models.Internal;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AluxionTest.Repositories.Interfaces
{
    public interface IAmazonS3Repository
    {
        Task<PutObjectResponse> UploadFile(IFormFile file);
        Task<FileStreamResult> DownloadFile(string fileName, string bucketname);
        Task<CopyObjectResponse> CopyS3OBject(string objectName, string newObjectName);
        Task<DeleteObjectResponse> DeleteS3Object(string objectName);
        Task<PutObjectResponse> UploadFileFromStream(StreamData streamData);
        Task<List<S3Object>> ListBucketObjects(string bucketName);
        Task<string> Get3ObjectURL(string objectName);
    }
}
