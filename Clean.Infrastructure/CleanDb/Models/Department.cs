using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Infrastructure.CleanDb.Models
{
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int DepartmentTypeId { get; set; }
        [ForeignKey("DepartmentTypeId")]
        public virtual DepartmentType DepartmentType { get; set; }

        public int? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public virtual Department? Parent { get; set; }

        public int? ManagerId { get; set; }

        [ForeignKey("ManagerId")]
        public virtual Employee? Manager { get; set; }

        [Required]
        public int CityId { get; set; }
        [ForeignKey("CityId")]
        public virtual City City { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public bool IsDown { get; set; }

    }
}
