using Clean.Core.Models.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Infrastructure.CleanDb.Services
{
    public interface IRankService
    {
        List<Rank> getAll();
    }
}
