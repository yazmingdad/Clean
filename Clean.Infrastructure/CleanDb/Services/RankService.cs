using AutoMapper;
using Clean.Infrastructure.CleanDb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreModel = Clean.Core.Models.Company;

namespace Clean.Infrastructure.CleanDb.Services
{
    public class RankService : ServiceBase,IRankService
    {
        public RankService(IMapper mapper, CleanContext cleanContext) : base(mapper, cleanContext)
        {
        }
        public List<CoreModel.Rank> getAll()
        {
            //using (_cleanContext)
            //{
                return (from rank in _cleanContext.Set<Rank>() select Mapper.Map<CoreModel.Rank>(rank)).ToList();
            //}
        }
    }
}
