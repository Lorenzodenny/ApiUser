using UserManagementAPI.Abstract;

namespace UserManagementAPI.Model
{
    public class User : ISoftDeletable
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsDeleted { get; set; }  // Implementazione della proprietà IsDeleted 

        // Proprietà di navigazione
        public List<AuditLog> AuditLogs { get; set; }

        public User()
        {
            AuditLogs = new List<AuditLog>();  // Inizializza la lista per evitare problemi di null reference
        }
    }

}
