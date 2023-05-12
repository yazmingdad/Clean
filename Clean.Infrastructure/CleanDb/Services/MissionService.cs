using AutoMapper;
using Clean.Core.Models.Api;
using Clean.Core.Models.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreModel = Clean.Core.Models;
using DataModel = Clean.Infrastructure.CleanDb.Models;
namespace Clean.Infrastructure.CleanDb.Services
{
    public class MissionService : ServiceBase, IMissionService
    {
        public MissionService(IMapper mapper, DataModel.CleanContext cleanContext) : base(mapper, cleanContext)
        {
        }

        public List<Core.Models.Mission.Mission> GetAll(bool isActive = true)
        {
            var output = new List<CoreModel.Mission.Mission>();

            var cancelled = _cleanContext.Statuses.FirstOrDefault(s => s.Name == "Cancelled");

            if(cancelled == null)
            {
                throw new Exception("Mission Status Error");
            }

            output = (from mission in _cleanContext.Set<DataModel.Mission>()
                      where mission.StatusId != cancelled.Id
                      join status in _cleanContext.Set<DataModel.Status>()
                        on mission.StatusId equals status.Id
                      join department in _cleanContext.Set<DataModel.Department>()
                        on mission.DepartmentId equals department.Id
                      join city in _cleanContext.Set<DataModel.City>()
                        on mission.StartCityId equals city.Id
                      select new CoreModel.Mission.Mission
                      {
                          Id = mission.Id,
                          Department = Mapper.Map<Department>(department),
                          Status = Mapper.Map<CoreModel.Mission.Status>(status),
                          StartCity = Mapper.Map<City>(city),
                          Priority = mission.Priority,
                          Code = mission.Code,
                          Title = mission.Title,
                          Description = mission.Description,
                          Budget = mission.Budget,
                          Cost = mission.Cost,
                          StartDate = mission.StartDate,
                          EndDate = mission.EndDate,
                          CreatedDate = mission.CreatedDate,
                          Distance = mission.Distance,
                          IsInCountry = mission.IsInCountry,
                          Participants = Mapper.Map<List<Employee>>(
                              (from missionParticipant in _cleanContext.Set<DataModel.MissionParticipant>()
                               where missionParticipant.MissionId == mission.Id
                               join employee in _cleanContext.Set<DataModel.Employee>()
                                 on missionParticipant.EmployeeId equals employee.Id
                               select new DataModel.Employee
                               {
                                   Id= employee.Id,
                                   FirstName= employee.FirstName,
                                   LastName= employee.LastName,
                                   Avatar=employee.Avatar,
                               }
                               )),

                          Destinations = Mapper.Map<List<City>>(
                              (from missionDestination in _cleanContext.Set<DataModel.MissionDestination>()
                               where missionDestination.MissionId == mission.Id
                               join destination in _cleanContext.Set<DataModel.City>()
                                 on missionDestination.DestinationId equals destination.Id
                               select destination
                               ))

                      }).ToList();


            return output;


        }

        public Result Insert(CoreModel.Mission.MissionInsert mission)
        {
            using var transaction = _cleanContext.Database.BeginTransaction();
            try
            {
                var status = _cleanContext.Statuses.FirstOrDefault(s => s.Name == "Created");

                if(status == null)
                {
                    throw new Exception("Status Not Found");
                }

                mission.StatusId= status.Id;

                var existing = _cleanContext.Missions.FirstOrDefault(m => m.Code==mission.Code);

                if (existing!=null)
                {
                    throw new Exception("Code Already Exists");
                }

                var missionData = Mapper.Map<DataModel.Mission>(mission);

                _cleanContext.Missions.Add(missionData);
                _cleanContext.SaveChanges();

                if(missionData.Id == 0)
                {
                    throw new Exception("Mission Not Inserted");
                }

                int index = 1;

                foreach(var destinationId in mission.Destinations)
                {
                    DataModel.MissionDestination md = new DataModel.MissionDestination
                    {
                        MissionId = missionData.Id,
                        DestinationId = destinationId,
                        Order = index
                    };

                    _cleanContext.MissionDestinations.Add(md);
                    _cleanContext.SaveChanges();

                    if(md.Id == 0)
                    {
                        throw new Exception("Destination Not Inserted");
                    }

                    index++;
                }

                foreach(var participantId in mission.Participants)
                {
                    DataModel.MissionParticipant mp = new DataModel.MissionParticipant
                    {
                        MissionId = missionData.Id,
                        EmployeeId = participantId,
                    };

                    _cleanContext.MissionParticipants.Add(mp);
                    _cleanContext.SaveChanges();    

                    if(mp.Id == 0)
                    {
                        throw new Exception("Participant Not Inserted");
                    }
                }
                _cleanContext.SaveChanges();
                transaction.Commit();
            }
            catch(Exception ex)
            {
                transaction.Rollback();
                return new Result { IsFailure = true };
            }
            return new Result();
        }
    }
}
