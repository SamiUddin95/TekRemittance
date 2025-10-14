using System;

namespace TekRemittance.Repository.Entities
{
    public class RevokedToken
    {
        public Guid Id { get; set; }
        public string Jti { get; set; } = string.Empty;
        public DateTime RevokedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
