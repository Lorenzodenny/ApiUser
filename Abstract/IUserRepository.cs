using System.Threading.Tasks;
using UserManagementAPI.DataAccessLayer;
using UserManagementAPI.Model;

namespace UserManagementAPI.Abstract
{
    public interface IUserRepository : IRepository<User>
    {
        Task<int> ExecuteAddUserWithLogAsync(User user, string operation, DateTime timestamp);
    }
}
