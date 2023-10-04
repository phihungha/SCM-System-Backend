using SCM_System_Api_Server.Models;

namespace SCM_System_Api_Server.DomainServices
{
    public interface IUsersService
    {
        Task<List<User>> GetUsers();
    }
}