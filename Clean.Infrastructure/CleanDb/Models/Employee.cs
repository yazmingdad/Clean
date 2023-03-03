
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Infrastructure.CleanDb.Models
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int RankId { get; set; }

        [ForeignKey("RankId")]
        public virtual Rank Rank { get; set; }

        [Required]
        public int? ActiveCardId { get; set; }
        [ForeignKey("ActiveCardId")]
        public virtual Card? ActiveCard { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }  

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public byte[] Avatar { get; set; }

        [Required]
        public string SSN { get; set; }
        public bool IsRetired { get; set; }
    }
}
