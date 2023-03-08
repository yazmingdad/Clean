using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Core.Models.Company
{
    public  class Employee
    {
        public int Id { get; set; }
        public int RankId { get; set; }
        public int? ActiveCardId { get; set; }
        public int DepartmentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[] Avatar { get; set; }
        public string SSN { get; set; }
        public bool IsRetired { get; set; }
        
        public string FullName 
        { 
            get 
            { 
                return $"{FirstName} {LastName}";
            } 
        }

        public Rank? Rank { get; set; }
        public Department? Department { get; set; }
        public Card? ActiveCard { get; set; }
        public List<Card>? Cards { get; set; }

    }
}
