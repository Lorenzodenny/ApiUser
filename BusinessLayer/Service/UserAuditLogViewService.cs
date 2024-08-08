using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagementAPI.Abstract;
using UserManagementAPI.Model;

namespace UserManagementAPI.BusinessLayer.Service
{
    public class UserAuditLogViewService : IUserAuditLogViewService
    {
        private readonly IUserAuditLogViewRepository _repository;

        public UserAuditLogViewService(IUserAuditLogViewRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<UserAuditLogView>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }
    }
}
