using ScmssApiServer.DTOs;

namespace ScmssApiServer.IServices
{
    public interface IFileHostService
    {
        FileUploadInfoDto GenerateUploadUrl(string folderKey);
    }
}
