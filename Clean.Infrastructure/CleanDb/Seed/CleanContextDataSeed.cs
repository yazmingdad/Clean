using Clean.Core.Constants;
using Clean.Infrastructure.CleanDb.Models;
using Clean.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Clean.Infrastructure.CleanDb.Seed
{
    public class CleanContextDataSeed
    {
        //Insert the ranks
        private static Rank InsertRanks(CleanContext cleanContext)
        {      
                Rank rootRank = new Rank();

                var ranks = Loader.LoadFromJson<Rank>("ranks");

                foreach (var item in ranks)
                {

                    var rank = cleanContext.Ranks.FirstOrDefault(r => r.Name == item.Name);

                    if (rank == null)
                    {
                        rank = item;

                        cleanContext.Ranks.Add(rank);
                        cleanContext.SaveChanges();

                        if (rank.Id <= 0)
                        {
                            throw new Exception($"Could not Insert {item.Name} rank");
                        }
                    }

                    if (item.Name == "President")
                    {
                        rootRank.Id = rank.Id;
                    }

                }

                if (rootRank.Id <= 0)
                {
                    throw new Exception($"Could not find The root rank");
                }
                
                return rootRank;
        }

        // Insert the department types
        private static DepartmentType InsertDepartmentTypes(CleanContext cleanContext)
        {
            DepartmentType rootDepartmentType = new DepartmentType();

            var departmentTypes = Loader.LoadFromJson<DepartmentType>("departmentTypes");

            foreach (var item in departmentTypes)
            {
                var type = cleanContext.DepartmentTypes.FirstOrDefault(dt => dt.Name == item.Name);

                if (type == null)
                {
                    type = item;

                    cleanContext.DepartmentTypes.Add(type);
                    cleanContext.SaveChanges();

                    if (type.Id <= 0)
                    {
                        throw new Exception($"Could not Insert {item.Name} department type");
                    }
                }
                if (item.Name == "Central")
                {
                    rootDepartmentType.Id = type.Id;
                }
            }

            if (rootDepartmentType.Id <= 0)
            {
                throw new Exception($"Could not find The root department type");
            }

            return rootDepartmentType;
        }

        // Insert Countries
        private static Country InsertCountries(CleanContext cleanContext) 
        {
            // Insert Countries
            Country rootCountry = new Country();

            var countries = Loader.LoadFromJson<Country>("countries");

            foreach (var item in countries)
            {
                var country = cleanContext.Countries.FirstOrDefault(c => c.Name == item.Name);

                if (country == null)
                {
                    country = item;

                    cleanContext.Countries.Add(country);
                    cleanContext.SaveChanges();

                    if (country.Id <= 0)
                    {
                        throw new Exception($"Could not Insert {item} country");
                    }

                }

                if (item.Name == "Morocco")
                {
                    rootCountry.Id = country.Id;
                }
            }

            if (rootCountry.Id == 0)
            {
                throw new Exception($"Could not find The root country");
            }
            return rootCountry;
        }

        // Insert the root city

        private static City InsertCities(CleanContext cleanContext,Country rootCountry)
        {
            // Insert the root city

            City rootCity= new City();

            var cities = Loader.LoadFromJson<City>("cities");

            foreach (var item in cities)
            {
                var city = cleanContext.Cities.FirstOrDefault(c => c.Name == item.Name);

                if (city == null)
                {
                    city = item;
                    cleanContext.Cities.Add(city);
                    cleanContext.SaveChanges(); 

                    if(city.Id <= 0)
                    {
                        throw new Exception($"Couldn't insert {item.Name} city");
                    }
                }

                if(city.Name== "Rabat")
                {
                    rootCity = city;
                }
            }

            return rootCity;
        }

        // Insert the root Department
        private static Department InsertDepartments(CleanContext cleanContext,City rootCity,DepartmentType rootDepartmentType)
        {
            // Insert the root Department

            var department = Loader.LoadFromJson<Department>("departments").FirstOrDefault();

            if(department == null)
            {
                throw new Exception();
            }

            var rootDepartment = cleanContext.Departments.FirstOrDefault(d => d.Name == department.Name);

            if (rootDepartment == null)
            {
                rootDepartment = department;
                rootDepartment.DepartmentTypeId = rootDepartmentType.Id;
                rootDepartment.CityId = rootCity.Id;
                cleanContext.Departments.Add(rootDepartment);
                cleanContext.SaveChanges();
 
                
                if (rootDepartment.Id <= 0)
                {
                    throw new Exception("Could not Insert the root department");
                }


                rootDepartment.ParentId = rootDepartment.Id;
                cleanContext.Departments.Update(rootDepartment);
                
            }

            return rootDepartment;
        }

       
        private static  Employee InsertRootEmployee(
            CleanContext cleanContext, 
            Rank rootRank,
            Department rootDepartment,
            ApplicationUser user)
        {
            //Insert the root Employee

            var rootEmployee = cleanContext.Employees.FirstOrDefault(e => e.SSN == "ssn");

            if (rootEmployee == null)
            {
                var buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var filePath = buildDir + @"\CleanDb\Seed\Data\Initial\default.png";
                rootEmployee = new Employee
                {
                    RankId = rootRank.Id,
                    DepartmentId = rootDepartment.Id,
                    FirstName = user.UserName,
                    LastName = user.UserName,
                    SSN = "ssn",
                    Avatar = File.ReadAllBytes(filePath),
                };

                cleanContext.Employees.Add(rootEmployee);
                cleanContext.SaveChanges();

                if (rootEmployee.Id <= 0)
                {
                    throw new Exception("Could not insert the root Employee");
                }
            }

            if (rootDepartment.ManagerId == 0 || rootDepartment.ManagerId == null)
            {
                rootDepartment.ManagerId = rootEmployee.Id;
                cleanContext.Update(rootDepartment);
                cleanContext.SaveChanges();
            }

            return rootEmployee;

        }


        //Insert the User
        private static void InsertRootUser(CleanContext cleanContext, Employee rootEmployee, ApplicationUser user)
        {
            //Insert the User

            var rootUser = cleanContext.Users.FirstOrDefault(u => u.Id.Equals(new Guid(user.Id)));

            if (rootUser == null)
            {

                rootUser = new User
                {
                    Id = new Guid(user.Id),
                    EmployeeId = rootEmployee.Id,
                };

                cleanContext.Users.Add(rootUser);
                cleanContext.SaveChanges();

                rootUser = cleanContext.Users.FirstOrDefault(u => u.Id.Equals(new Guid(user.Id)));

                if (rootUser == null)
                {
                    throw new Exception("Could not create the root User in Clean database");
                }
            }
        }

        public static async Task SeedAsync(CleanContext cleanContext,UserManager<ApplicationUser> userManager)
        {

            using (cleanContext)
            {
                var exists = (cleanContext.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists();

                if (!exists)
                {
                    throw new Exception("Clean Database doesn't exist");

                }

                var rootRank = InsertRanks(cleanContext);
                var rootDepartmentType = InsertDepartmentTypes(cleanContext);
                var rootCountry = InsertCountries(cleanContext);
                var rootCity = InsertCities(cleanContext, rootCountry);
                var rootDepartment =InsertDepartments(cleanContext,rootCity,rootDepartmentType);

                var user = await userManager.FindByNameAsync(CleanConstants.DefaultEmployee);

                if (user == null)
                {
                    throw new Exception("Identity User does not exist");
                }

                var rootEmployee = InsertRootEmployee(cleanContext,rootRank, rootDepartment,user);            

                InsertRootUser(cleanContext, rootEmployee, user);

                var cards = rootEmployee.Cards;
                
               // var card = rootEmployee.ActiveCard;
                //var maneger = rootDepartment.Manager;
                //var parent = rootDepartment.Parent;

                cleanContext.SaveChanges();
            }
        }
    }


}
