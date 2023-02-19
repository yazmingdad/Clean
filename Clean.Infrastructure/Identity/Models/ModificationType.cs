using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clean.Infrastructure.Identity.Models
{
    [Table("ModificationTypes")]
    public class ModificationType
    {
        [Key]
        public int Id { get; set; }
        public string ModificationName { get; set; }
    }
}
