namespace UserManagementAPI.Model
{
    public class UserAuditLogView
    {
        public int UserAuditLogId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Operation { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
