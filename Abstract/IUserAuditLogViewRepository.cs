using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagementAPI.Model;

namespace UserManagementAPI.Abstract
{
    public interface IUserAuditLogViewRepository
    {
        Task<IEnumerable<UserAuditLogView>> GetAllAsync();
    }
}
