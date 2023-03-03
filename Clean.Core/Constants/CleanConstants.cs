using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Core.Constants
{
    public class CleanConstants
    {
        public static readonly string DefaultEmployee = "Cleaner";
        public static readonly string DefaultDepartment = "The Clean Corporation";
        public static readonly List<string> Countries = new List<string>() { "United States of America", "Morocco" };
        public static readonly List<string> DepartmentTypes = new() { "Central", "Regional", "Provincial"};
        public static readonly List<string> Ranks = new()
        {
            "President",
            "Vice President",
            "Chief Executive Officer",
            "Chief Operating Officer",
            "Chief Information Officer",
            "Chief Financial Officer",
            "Head of marketing",
            "Head of human resources",
            "Head of customer services",
            "Administrative officer",
            "Accountant",
            "Manager",
            "Supervisor",
            "Assistant",
            "Employee",
        };

        public static readonly string DefaultDepartmentType = "Company";

        
    }
}
