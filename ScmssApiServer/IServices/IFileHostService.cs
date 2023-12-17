namespace ScmssApiServer.IServices
{
    public interface IFileHostService
    {
        string GenerateUploadUrl(string folderKey, string key);

        string GenerateUploadUrl(string folderKey, int key);
    }
}
