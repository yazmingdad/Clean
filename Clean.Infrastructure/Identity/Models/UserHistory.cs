using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clean.Infrastructure.Identity.Models
{
    [Table("UsersHistory")]
    public class UserHistory
    {
        [Key]
        public int Id { get; set; }
        public string ByUserId { get; set; }
        [Required]
        [ForeignKey("ByUserId")]
        public virtual ApplicationUser ByUser { get; set; }

        public string UserId { get; set; }
        [Required]
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        public int ModificationTypeId { get; set; }
        [Required]
        [ForeignKey("ModificationTypeId")]
        public virtual ModificationType Modification { get; set; }

        public string RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual IdentityRole Role { get; set; }

        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Moment { get; set; }
    }
}
