
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
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //[Required]
        public int RankId { get; set; }

        //[ForeignKey("RankId")]
        //public virtual Rank Rank { get; set; }

        //[Required]
        public int? ActiveCardId { get; set; }
        //[ForeignKey("ActiveCardId")]
        //public virtual Card? ActiveCard { get; set; }

        //[Required]
        public int DepartmentId { get; set; }

        //[ForeignKey("DepartmentId")]
        //public virtual Department Department { get; set; }

        //[Required]
        //[Column(TypeName = "nvarchar(35)")]
        public string FirstName { get; set; }

        //[Required]
        //[Column(TypeName = "nvarchar(35)")]
        public string LastName { get; set; }

        //[Required]
        public byte[] Avatar { get; set; }

        //[Required]
        //[Column(TypeName = "nvarchar(35)")]
        public string SSN { get; set; }
        public bool IsRetired { get; set; }
        //public virtual ICollection<Card> Cards { get; set; }
        //public Rank? Rank { get; set; }
        //public Department? Department { get; set; }
        //public Card? ActiveCard { get; set; }
        //public List<Card>? Cards { get; set; }
    }
}
