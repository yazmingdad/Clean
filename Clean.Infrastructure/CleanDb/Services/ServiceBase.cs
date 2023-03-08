using AutoMapper;
using Clean.Infrastructure.CleanDb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Infrastructure.CleanDb.Services
{
    public  abstract class ServiceBase
    {

        public IMapper Mapper { get; set; }

        protected readonly CleanContext _cleanContext;

        public ServiceBase(IMapper mapper, CleanContext cleanContext)
        {
            
            _cleanContext = cleanContext;
            Mapper = mapper;
        }
    }
}
