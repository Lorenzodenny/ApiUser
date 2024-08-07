using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Model; // Aggiungi questo using per il modello User
using UserManagementAPI.Abstract; // Aggiungi questo using per l'interfaccia IUserRepository

namespace UserManagementAPI.DataAccessLayer.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(ApplicationDbContext context, ILogger<UserRepository> logger) : base(context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<int> ExecuteAddUserWithLogAsync(User user, string operation, DateTime timestamp)
        {
            var storedProcedure = @"
                EXEC AddUserWithLog
                @FirstName = {0},
                @LastName = {1},
                @Email = {2},
                @RegistrationDate = {3},
                @Operation = {4},
                @Timestamp = {5}";

            var result = await _context.Database.ExecuteSqlRawAsync(storedProcedure,
            user.FirstName, user.LastName, user.Email, user.RegistrationDate, operation, timestamp);

            _logger.LogInformation("Stored procedure eseguita con successo per l'utente: {FirstName} {LastName}", user.FirstName, user.LastName);

            return result;
        }
    }
}
