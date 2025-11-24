using System;

namespace TekRemittance.Repository.Entities
{
    public class UserGroup
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public Guid GroupId { get; set; }
        public Group Group { get; set; } = null!;
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
