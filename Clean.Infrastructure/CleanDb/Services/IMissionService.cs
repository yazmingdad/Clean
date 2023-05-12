using Clean.Core.Models.Api;
using Clean.Core.Models.Company;
using Clean.Core.Models.Mission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Infrastructure.CleanDb.Services
{
    public interface IMissionService
    {
        Result Insert(MissionInsert mission);
        List<Mission> GetAll(bool isActive = true);
    }
}
