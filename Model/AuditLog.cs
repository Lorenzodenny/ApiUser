using System;

namespace UserManagementAPI.Model
{
    public class AuditLog
    {
        public int AuditLogId { get; set; }  // Chiave primaria
        public int? UserId { get; set; }  // Foreign Key riferimento a Users
        public string Operation { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
