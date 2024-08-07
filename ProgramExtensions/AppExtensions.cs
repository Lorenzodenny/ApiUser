using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace UserManagementAPI.Extensions
{
    public static class AppExtensions
    {
        public static WebApplication ConfigureApplication(this WebApplication app)
        {
            // Configura la pipeline di richiesta HTTP
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            return app;
        }
    }
}
