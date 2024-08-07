using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Model;

namespace UserManagementAPI.DataAccessLayer
{
    public class ApplicationDbContext : DbContext
    {
        

        public DbSet<User> Users { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurazione della relazione uno-a-molti tra User e AuditLog
            modelBuilder.Entity<User>()
                .HasMany(u => u.AuditLogs)
                .WithOne()
                .HasForeignKey(al => al.UserId)
                .OnDelete(DeleteBehavior.SetNull);  // Configura il comportamento di eliminazione
        }
    }
}
