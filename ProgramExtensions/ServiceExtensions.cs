using Microsoft.EntityFrameworkCore;
using UserManagementAPI.BusinessLayer.Service;
using UserManagementAPI.BusinessLayer;
using UserManagementAPI.DataAccessLayer;
using UserManagementAPI.DataAccessLayer.Repository;
using UserManagementAPI.Abstract;
using UserManagementAPI.DataAccessLayer.UnitOfWork;
using UserManagementAPI.Model;
using UserManagementAPI.BusinessLayer.Service.UserManagementAPI.BusinessLayer.Service;
using Microsoft.AspNetCore.Identity;
using UserManagementAPI.Identity;
using FluentValidation.AspNetCore;
using UserManagementAPI.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "UserManagementAPI", Version = "v1" });

                // Configura l'autenticazione JWT per Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
            });


            // REgistro il servizio Token
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];

            


            // Register repository and service interfaces
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

            // Registra repository e Service per le Dependency Injection
            services.AddScoped<IUserRepository, UserRepository>()
                    .AddScoped<IUserService, UserService>()
                    .AddLogging();

            // Registra repository e service per le Dependency Injenction della vista User + AuditoLog
            services.AddScoped<IUserAuditLogViewRepository, UserAuditLogViewRepository>();
            services.AddScoped<IUserAuditLogViewService, UserAuditLogViewService>();

            // registro l'interfaccia e la classe UnitOfWork, per le transizioni
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Configura FluentValidation per registrare tutti i validator presenti nell'assembly
            services.AddFluentValidationAutoValidation()
                    .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Registro il servizio di Identity, per l'autenticazione
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .AddRoles<IdentityRole>(); // Aggiungi questa linea per supportare i ruoli

            // Autenticazione 
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });

            // Autorizzazione
            services.AddAuthorization();

            return services;
        }
    }
}
