using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Infrastructure.CleanDb.Models
{
    public class User
    {
        //[Key]
        public Guid Id { get; set; }

        //[Required]
        public int EmployeeId { get; set; }

        //[ForeignKey("EmployeeId")]
        //public virtual Employee Employee { get; set; }
       // public string Username { get; set; }
    }
}
