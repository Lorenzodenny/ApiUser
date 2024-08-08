using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagementAPI.Model;

namespace UserManagementAPI.BusinessLayer
{
    public interface IUserAuditLogViewService
    {
        Task<IEnumerable<UserAuditLogView>> GetAllAsync();
    }
}
