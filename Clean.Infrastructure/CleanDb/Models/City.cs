using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Infrastructure.CleanDb.Models
{
    public class City
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //[Required]  
        public int CountryId { get; set; }

        //[ForeignKey("CountryId")]
       // public virtual Country Country { get; set; }

        //[Required]
        //[Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        //[Required]
        //[Column(TypeName = "nvarchar(25)")]
        public string Latitude { get; set; }

        //[Required]
        //[Column(TypeName = "nvarchar(25)")]
        public string Longitude { get; set; }
    }
}
