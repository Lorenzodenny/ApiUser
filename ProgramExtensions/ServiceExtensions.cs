using Microsoft.EntityFrameworkCore;
using UserManagementAPI.BusinessLayer.Service;
using UserManagementAPI.BusinessLayer;
using UserManagementAPI.DataAccessLayer;
using UserManagementAPI.DataAccessLayer.Repository;
using UserManagementAPI.Abstract;
using UserManagementAPI.DataAccessLayer.UnitOfWork;
using UserManagementAPI.Model;
using UserManagementAPI.BusinessLayer.Service.UserManagementAPI.BusinessLayer.Service;

namespace UserManagementAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Connessione al DB
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Aggiunge il supporto per i controller
            services.AddControllers();

            // Configura Swagger
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // Register repository and service interfaces
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));


            services.AddScoped<IUserRepository, UserRepository>()
        .AddScoped<IUserService, UserService>()
        .AddLogging();

            // registro l'interfaccia e la classe UnitOfWork, per le transizioni
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Qui puoi aggiungere altri servizi come la configurazione di Identity, servizi di log, ecc.

            return services;
        }
    }
}
