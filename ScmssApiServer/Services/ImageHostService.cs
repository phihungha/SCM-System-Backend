using Amazon.S3;
using Amazon.S3.Model;
using ScmssApiServer.Exceptions;
using ScmssApiServer.IServices;

namespace ScmssApiServer.Services
{
    public class ImageHostService : IImageHostService
    {
        private int _expiresInHours;
        private string _bucketName;

        private IAmazonS3 _s3Client;

        public ImageHostService(IConfiguration configuration, IAmazonS3 s3Client)
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

        public string GenerateUploadUrl(string key)
        {
            return GeneratePresignedUrl(key, HttpVerb.PUT);
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
