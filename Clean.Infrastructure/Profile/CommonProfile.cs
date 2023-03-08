using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CoreModel = Clean.Core.Models;
using DataModel = Clean.Infrastructure.CleanDb.Models;

namespace Clean.Infrastructure.Profile
{
    public class CommonProfile : AutoMapper.Profile
    {
        public CommonProfile()
        {

            this.CreateMap<DataModel.Card, CoreModel.Company.Card>();
            this.CreateMap<CoreModel.Company.Card, DataModel.Card>();

            this.CreateMap<DataModel.City, CoreModel.Company.City>();
            this.CreateMap<CoreModel.Company.City, DataModel.City>();

            this.CreateMap<DataModel.Country, CoreModel.Company.Country>();
            this.CreateMap<CoreModel.Company.Country, DataModel.Country>();

            this.CreateMap<DataModel.Department, CoreModel.Company.Department>();
            this.CreateMap<CoreModel.Company.Department, DataModel.Department>();

            this.CreateMap<DataModel.DepartmentType, CoreModel.Company.DepartmentType>();
            this.CreateMap<CoreModel.Company.DepartmentType, DataModel.DepartmentType>();

            this.CreateMap<DataModel.Employee, CoreModel.Company.Employee>();
            this.CreateMap<CoreModel.Company.Employee, DataModel.Employee>();

            this.CreateMap<DataModel.Rank, CoreModel.Company.Rank>();
            this.CreateMap<CoreModel.Company.Rank, DataModel.Rank>();



        }

    }
}
