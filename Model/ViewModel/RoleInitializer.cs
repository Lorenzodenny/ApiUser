using Microsoft.AspNetCore.Identity;

namespace UserManagementAPI.Model.ViewModel
{
    // Classe di inizializzazione dei ruoli
    public class RoleInitializer
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleInitializer(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task InitializeRolesAsync()
        {
            string[] roleNames = { "Admin", "User" };

            foreach (var roleName in roleNames)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
}
