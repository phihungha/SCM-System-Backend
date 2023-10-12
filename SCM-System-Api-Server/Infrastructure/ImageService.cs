using Amazon.S3;
using Amazon.S3.Model;

namespace SCM_System_Api_Server.Infrastructure
{
    public class ImageService : IImageService
    {
        private int EXPIRE_IN_HOUR = 1;
        private string BUCKET_NAME = "scm-system";

        private IAmazonS3 _s3Client;

        public ImageService(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
        }

        public string GetImageUploadUrl(string key)
        {
            return GeneratePresignedUrl(key, HttpVerb.PUT);
        }

        private string GeneratePresignedUrl(string key, HttpVerb verb)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = BUCKET_NAME,
                Key = key,
                Verb = verb,
                Expires = DateTime.UtcNow.AddHours(EXPIRE_IN_HOUR)
            };
            return _s3Client.GetPreSignedURL(request);
        }
    }
}