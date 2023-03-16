using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Core.Models.Api
{
    public class Result
    {
        public bool IsFailure { get; set; }
        public string Reason { get; set; } = "Some thing wrong happened";
    }
}
