using System;

namespace TekRemittance.Repository.Entities
{
    public class GroupPermission
    {
        public Guid Id { get; set; }
        public Guid GroupId { get; set; }
        public Group Group { get; set; } = null!;
        public Guid PermissionId { get; set; }
        public Permission Permission { get; set; } = null!;
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
