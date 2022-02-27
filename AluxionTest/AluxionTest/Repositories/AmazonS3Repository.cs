using AluxionTest.Models.Internal;
using AluxionTest.Repositories.Interfaces;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AluxionTest.Repositories
{
    public class AmazonS3Repository : IAmazonS3Repository
    {
        private readonly IAmazonS3 _amazonS3;
        public AmazonS3Repository(IAmazonS3 amazonS3)
        {
            _amazonS3 = amazonS3;
        }
        public async Task<FileStreamResult> DownloadFile(string fileName, string bucketname)
        {
            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = bucketname,
                Key = fileName,
            };

            try
            {
                using GetObjectResponse result = await _amazonS3.GetObjectAsync(request);
                using Stream responseStream = result.ResponseStream;
                var stream = new MemoryStream();
                await responseStream.CopyToAsync(stream);
                stream.Position = 0;

                return new FileStreamResult(stream, result.Headers["Content-Type"])
                {
                    FileDownloadName = fileName
                };
            }

            catch
            {
                return null;
            }
        }

        public async Task<PutObjectResponse> UploadFile(IFormFile file)
        {
            var request = new PutObjectRequest()
            {
                BucketName = "bucket-prueba-pablo",
                Key = file.FileName,
                InputStream = file.OpenReadStream(),
                ContentType = file.ContentType,
            };
            try
            {
                var result = await _amazonS3.PutObjectAsync(request);
                return result;
            }
            catch
            {
                return null;
            }
        }

        public async Task<PutObjectResponse> UploadFileFromStream(StreamData streamData)
        {
            var request = new PutObjectRequest()
            {
                BucketName = "bucket-prueba-pablo",
                Key = streamData.Id + ".jpg",
                InputStream = streamData.StreamInfo,
                ContentType = "image/jpg",
            };
            try
            {
                var result = await _amazonS3.PutObjectAsync(request);
                return result;
            }
            catch
            {
                return null;
            }

        }

        public async Task<List<S3Object>> ListBucketObjects(string bucketName)
        {
            try
            {
                var request = new ListObjectsV2Request
                {
                    BucketName = "bucket-prueba-pablo",
                    MaxKeys = 100,
                };
                ListObjectsV2Response response;
                response = await _amazonS3.ListObjectsV2Async(request);

                return response.S3Objects;
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                Console.WriteLine("S3 error occurred. Exception: " + amazonS3Exception.ToString());
                Console.ReadKey();
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.ToString());
                Console.ReadKey();
                return null;
            }
        }

        public async Task<string> Get3ObjectURL(string objectName)
        {
            const double duration = 24;
            string urlString;

            //Revisar si el objeto existe en el bucket
            try
            {
                GetObjectRequest getRequest = new GetObjectRequest
                {
                    BucketName = "bucket-prueba-pablo",
                    Key = objectName,
                };
                await _amazonS3.GetObjectAsync(getRequest);
            }
            catch
            {
                return "error";
            }


            //Si existe, continuar.
            try
            {
                GetPreSignedUrlRequest request = new GetPreSignedUrlRequest
                {
                    BucketName = "bucket-prueba-pablo",
                    Key = objectName,
                    Expires = DateTime.UtcNow.AddHours(duration)
                };
                urlString = _amazonS3.GetPreSignedURL(request);
            }
            catch (AmazonS3Exception e)
            {
                return "Error";
            }
            catch (Exception e)
            {
                return "Error";
            }
            return urlString;
        }

        public async Task<CopyObjectResponse> CopyS3OBject(string objectName, string newObjectName)
        {
            try
            {
                CopyObjectRequest request = new CopyObjectRequest
                {
                    SourceBucket = "bucket-prueba-pablo",
                    SourceKey = objectName,
                    DestinationBucket = "bucket-prueba-pablo",
                    DestinationKey = newObjectName,
                };
                CopyObjectResponse response = await _amazonS3.CopyObjectAsync(request);
                return response;
            }
            catch (AmazonS3Exception e)
            {
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<DeleteObjectResponse> DeleteS3Object(string objectName)
        {
            try
            {
                var request = new DeleteObjectRequest
                {
                    BucketName = "bucket-prueba-pablo",
                    Key = objectName,
                };
                var response = await _amazonS3.DeleteObjectAsync(request);
                return response;
            }
            catch
            {
                return null;
            }

        }
    }
}