using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cloudscribe.QueryTool.Models
{
    public class SavedQuery
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Name { get; set; } = default!;

        public string Statement { get; set; } = default!;

        public bool EnableAsApi { get; set; } = false;

        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

        public Guid CreatedBy { get; set; } = Guid.Empty;

        public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;

        public Guid LastModifiedBy { get; set; } = Guid.Empty;

        public DateTime LastRunUtc { get; set; } = DateTime.UtcNow;

        public Guid LastRunBy { get; set; } = Guid.Empty;
    }
}
