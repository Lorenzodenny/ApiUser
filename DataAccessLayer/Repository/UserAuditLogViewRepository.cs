using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Abstract;
using UserManagementAPI.Model;

namespace UserManagementAPI.DataAccessLayer.Repository
{
    public class UserAuditLogViewRepository : IUserAuditLogViewRepository
    {
        private readonly ApplicationDbContext _context;

        public UserAuditLogViewRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserAuditLogView>> GetAllAsync()
        {
            return await _context.UserAuditLogView.ToListAsync();
        }
    }
}
