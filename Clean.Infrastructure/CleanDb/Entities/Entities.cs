using Clean.Infrastructure.Identity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Infrastructure.CleanDb.Entities
{
    public class Country
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
    }
    public class City
    {
        [Key] 
        public int Id { get; set; }
        public int CountryId { get; set; }
        [Required]
        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Latitude { get; set; } = string.Empty;
        public string Longitude { get; set; }= string.Empty;    
    }

    public class DepartmentType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }=string.Empty;  
    }
    public class Department
    {
        [Key] 
        public int Id { get; set; }
        [Required]
        public int DepartmentTypeId { get; set; }
        [Required]
        [ForeignKey("DepartmentTypeId")]
        public virtual DepartmentType DepartmentType { get; set; }
        public int ParentId { get; set; }
        [Required]
        [ForeignKey("ParentId")]
        public virtual Department Parent { get; set; }
        public int ManagerId { get; set; }
        [Required]
        [ForeignKey("ManagerId")]
        public virtual Employee Manager { get; set; }
        public int CityId { get; set; }
        [Required]
        [ForeignKey("CityId")]
        public virtual City City { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public bool IsDown { get; set; }
    }
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public int RankId { get; set; }
        [Required]
        [ForeignKey("RankId")]
        public virtual Rank Rank { get; set; }

        public int DepartmentId { get; set; }


        public int CardId { get; set; }
        [Required]
        [ForeignKey("CardId")]
        public virtual Card Card { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string SSN { get; set; } = string.Empty;
        public bool IsDown { get; set; }
    }

    public class Rank
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class Card
    {
        [Key]
        public int Id { get; set; }
        public int EmployeeId { get; set; }

        [Required]
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }

        public string Number { get; set; } = string.Empty;

    }
}
