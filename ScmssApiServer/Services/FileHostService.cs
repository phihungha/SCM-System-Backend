using Amazon.S3;
using Amazon.S3.Model;
using ScmssApiServer.Exceptions;
using ScmssApiServer.IServices;

namespace ScmssApiServer.Services
{
    public class FileHostService : IFileHostService
    {
        public const string BaseUrl = "https://scmss.s3.ap-southeast-1.amazonaws.com/public";

        private string _bucketName;
        private int _expiresInHours;
        private IAmazonS3 _s3Client;

        public FileHostService(IConfiguration configuration, IAmazonS3 s3Client)
        {
            _s3Client = s3Client;

            IConfigurationSection section = configuration.GetRequiredSection("ImageHost");
            var expiresInHours = section.GetValue<int>("ExpiresInHours");
            var bucketName = section.GetValue<string>("BucketName");
            if (bucketName != null)
            {
                _expiresInHours = expiresInHours;
                _bucketName = bucketName;
            }
            else
            {
                throw new AppConfigException("Image hosting is not properly configured.");
            }
        }

        public static Uri GetUri(string folderKey, string key) => new Uri($"{BaseUrl}/{folderKey}/{key}");

        public static Uri GetUri(string folderKey, int key) => new Uri($"{BaseUrl}/{folderKey}/{key}");

        public string GenerateUploadUrl(string folderKey, string key)
        {
            return GeneratePresignedUrl($"public/{folderKey}/{key}", HttpVerb.PUT);
        }

        public string GenerateUploadUrl(string folderKey, int key)
        {
            return GeneratePresignedUrl($"public/{folderKey}/{key}", HttpVerb.PUT);
        }

        private string GeneratePresignedUrl(string key, HttpVerb verb)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = key,
                Verb = verb,
                Expires = DateTime.UtcNow.AddHours(_expiresInHours)
            };
            return _s3Client.GetPreSignedURL(request);
        }
    }
}
