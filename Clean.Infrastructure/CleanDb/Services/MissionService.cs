using AutoMapper;
using Clean.Core.Models.Api;
using Clean.Core.Models.Company;
using Clean.Core.Models.Mission;
using Clean.Infrastructure.CleanDb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Infrastructure.CleanDb.Services
{
    public class MissionService : ServiceBase, IMissionService
    {
        public MissionService(IMapper mapper, CleanContext cleanContext) : base(mapper, cleanContext)
        {
        }
        public Result Insert(MissionInsert mission)
        {
            using var transaction = _cleanContext.Database.BeginTransaction();
            try
            {
                var existing = _cleanContext.Missions.FirstOrDefault(m => m.Code==mission.Code);

                if (existing!=null)
                {
                    throw new Exception("Code Already Exists");
                }

                var missionData = Mapper.Map<Mission>(mission);

                _cleanContext.Missions.Add(missionData);
                _cleanContext.SaveChanges();

                if(missionData.Id == 0)
                {
                    throw new Exception("Mission Not Inserted");
                }

                int index = 1;

                foreach(var destinationId in mission.Destinations)
                {
                    MissionDestination md = new MissionDestination
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
                    MissionParticipant mp = new MissionParticipant
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
            }
            return new Result();
        }
    }
}
