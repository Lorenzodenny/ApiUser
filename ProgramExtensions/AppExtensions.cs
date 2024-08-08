using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using UserManagementAPI.Model.ViewModel;

namespace UserManagementAPI.Extensions
{
    public static class AppExtensions
    {
        public static WebApplication ConfigureApplication(this WebApplication app)
        {
            // Configura la pipeline di richiesta HTTP
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UserManagementAPI v1"));
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            
            app.UseRouting();

            app.UseAuthentication();  
            app.UseAuthorization();


            // Inizializza i ruoli
            using (var scope = app.Services.CreateScope())
            {
                var roleInitializer = scope.ServiceProvider.GetRequiredService<RoleInitializer>();
                roleInitializer.InitializeRolesAsync().Wait(); // Usa Wait() per chiamare il metodo asincrono in un contesto sincrono
            }

            app.MapControllers();

            return app;
        }
    }
}
