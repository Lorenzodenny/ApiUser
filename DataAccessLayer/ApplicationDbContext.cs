using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Identity;
using UserManagementAPI.Model;

namespace UserManagementAPI.DataAccessLayer
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        

        public DbSet<User> Users { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<UserAuditLogView> UserAuditLogView { get; set; }  // Tabella per la Vista

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAuditLogView>().HasNoKey(); // setta la vista al fine di non avere una chiave primaria

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
