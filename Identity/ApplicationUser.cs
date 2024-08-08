using Microsoft.AspNetCore.Identity;

namespace UserManagementAPI.Identity
{
    public class ApplicationUser : IdentityUser
    {
        // Puoi aggiungere altre proprietà personalizzate qui se necessario
        public string PhoneNumber { get; set; }  // Nuovo campo
    }
}
