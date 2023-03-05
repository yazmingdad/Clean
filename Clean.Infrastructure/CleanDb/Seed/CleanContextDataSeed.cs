﻿using Clean.Core.Constants;
using Clean.Infrastructure.CleanDb.Models;
using Clean.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Infrastructure.CleanDb.Seed
{
    public class CleanContextDataSeed
    {
        //Insert the ranks
        private static Rank InsertRanks(CleanContext cleanContext)
        {
                Rank rootRank = new Rank();

                foreach (var item in CleanConstants.Ranks)
                {

                    var rank = cleanContext.Ranks.FirstOrDefault(d => d.Name == item);

                    if (rank == null)
                    {
                        rank = new Rank
                        {
                            Name = item,
                        };

                        cleanContext.Ranks.Add(rank);
                        cleanContext.SaveChanges();

                        if (rank.Id <= 0)
                        {
                            throw new Exception($"Could not Insert {item} rank");
                        }

                        if (item == "President")
                        {
                            rootRank.Id = rank.Id;
                        }
                    }
                    else
                    {
                        if (item == "President")
                        {
                            rootRank.Id = rank.Id;
                        }
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

            foreach (var item in CleanConstants.DepartmentTypes)
            {
                var type = cleanContext.DepartmentTypes.FirstOrDefault(d => d.Name == item);

                if (type == null)
                {
                    type = new DepartmentType
                    {
                        Name = item
                    };

                    cleanContext.DepartmentTypes.Add(type);
                    cleanContext.SaveChanges();

                    if (type.Id <= 0)
                    {
                        throw new Exception($"Could not Insert {item} department type");
                    }

                    if (item == "Central")
                    {
                        rootDepartmentType.Id = type.Id;
                    }
                }
                else
                {
                    if (item == "Central")
                    {
                        rootDepartmentType.Id = type.Id;
                    }
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

            foreach (var item in CleanConstants.Countries)
            {
                var country = cleanContext.Countries.FirstOrDefault(d => d.Name == item);

                if (country == null)
                {
                    country = new Country
                    {
                        Name = item
                    };

                    cleanContext.Countries.Add(country);
                    cleanContext.SaveChanges();

                    if (country.Id <= 0)
                    {
                        throw new Exception($"Could not Insert {item} country");
                    }

                }

                if (item == "Morocco")
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
            var cities = LoadCities();

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

            //var rootCity = cleanContext.Cities.FirstOrDefault(c => c.Name == "Rabat");

            //if (rootCity == null)
            //{
            //    rootCity = new City()
            //    {
            //        CountryId = rootCountry.Id,
            //        Name = "Rabat",
            //        Latitude = "34.02199",
            //        Longitude = "-6.83762"
            //    };

            //    cleanContext.Cities.Add(rootCity);
            //    cleanContext.SaveChanges();

            //    if (rootCity.Id <= 0)
            //    {
            //        throw new Exception("Cannot add the Rabat City");
            //    }
            //}

            //return rootCity;
        }

        // Insert the root Department
        private static Department InsertDepartments(CleanContext cleanContext,City rootCity,DepartmentType rootDepartmentType)
        {
            // Insert the root Department

            var rootDepartment = cleanContext.Departments.FirstOrDefault(d => d.Name == CleanConstants.DefaultDepartment);

            if (rootDepartment == null)
            {
                rootDepartment = new Department
                {
                    Name = CleanConstants.DefaultDepartment,
                    ShortName = "Clean",
                    DepartmentTypeId = rootDepartmentType.Id,
                    CityId = rootCity.Id
                };
                cleanContext.Departments.Add(rootDepartment);

                try
                {
                    cleanContext.SaveChanges();
                }
                catch(Exception ex)
                {

                }
                

                if (rootDepartment.Id <= 0)
                {
                    throw new Exception("Could not Insert the root department");
                }

                if (rootDepartment.ParentId == 0)
                {
                    rootDepartment.ParentId = rootDepartment.Id;
                    cleanContext.Departments.Update(rootDepartment);
                }
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

            try
            {


            var rootEmployee = cleanContext.Employees.FirstOrDefault(e => e.SSN == "ssn");

            if (rootEmployee == null)
            {
                var buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var filePath = buildDir + @"\CleanDb\Seed\Data\default.png";
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

            if (rootDepartment.ManagerId == 0)
            {
                rootDepartment.ManagerId = rootEmployee.Id;
            }

            return rootEmployee;
            }
            catch (Exception ex)
            {

            }
            return new Employee();
        }


        //Insert the User
        private static void InsertRootUser(CleanContext cleanContext, Employee rootEmployee, ApplicationUser user)
        {
            //Insert the User

            var rootUser = cleanContext.Users.FirstOrDefault(u => u.Id.Equals(user.Id));

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
                cleanContext.Database.EnsureCreated();

                //Insert the ranks

                //Rank rootRank = new Rank();

                //foreach (var item in CleanConstants.Ranks)
                //{

                //    var rank = cleanContext.Ranks.FirstOrDefault(d => d.Name == item);

                //    if (rank == null)
                //    {
                //        rank = new Rank
                //        {
                //            Name = item,
                //        };

                //        cleanContext.Ranks.Add(rank);

                //        if(rank.Id <= 0)
                //        {
                //            throw new Exception($"Could not Insert {item} rank");
                //        }

                //        if (item == "President")
                //        {
                //            rootRank.Id= rank.Id;
                //        }
                //    }
                //    else
                //    {
                //        rootRank.Id = rank.Id;
                //    }
                //}

                //if (rootRank.Id <= 0)
                //{
                //    throw new Exception($"Could not find The root rank");
                //}

                //// Insert the department types
                //DepartmentType rootDepartmentType = new DepartmentType();

                //foreach (var item in CleanConstants.DepartmentTypes)
                //{
                //    var type = cleanContext.DepartmentTypes.FirstOrDefault(d => d.Name == item);

                //    if (type == null)
                //    {
                //        type = new DepartmentType
                //        {
                //          Name = item
                //        };

                //        cleanContext.DepartmentTypes.Add(type);

                //        if(type.Id <= 0)
                //        {
                //            throw new Exception($"Could not Insert {item} department type");
                //        }

                //        if (item == "Central")
                //        {
                //            rootDepartmentType.Id = type.Id;
                //        }
                //    }
                //    else
                //    {
                //        rootDepartmentType.Id = type.Id;
                //    }
                //}

                //if(rootDepartmentType.Id <= 0) 
                //{
                //    throw new Exception($"Could not find The root department type");
                //}

                //// Insert Countries
                //Country rootCountry = new Country();

                //foreach (var item in CleanConstants.Countries)
                //{
                //    var country = cleanContext.Countries.FirstOrDefault(d => d.Name == item);

                //    if (country == null)
                //    {
                //        country = new Country
                //        {
                //            Name = item
                //        };

                //        cleanContext.Countries.Add(country);

                //        if(country.Id <= 0)
                //        {
                //            throw new Exception($"Could not Insert {item} country");
                //        }

                //        if (item == "Morocco")
                //        {
                //            rootCountry.Id = country.Id;
                //        }

                //    }
                //    else
                //    {
                //        rootCountry.Id = country.Id;
                //    }
                //}

                //if(rootCountry.Id == 0)
                //{
                //    throw new Exception($"Could not find The root country");
                //}


                // Insert the root city

                //var rootCity = cleanContext.Cities.FirstOrDefault(c => c.Name == "Rabat");

                //if (rootCity == null)
                //{
                //    rootCity = new City()
                //    {
                //        CountryId = rootCountry.Id,
                //        Name = "Rabat",
                //        Latitude = "34.02199",
                //        Longitude = "-6.83762"
                //    };

                //    cleanContext.Cities.Add(rootCity);

                //    if (rootCity.Id <= 0)
                //    {
                //        throw new Exception("Cannot add the Rabat City");
                //    }
                //}

                // Insert the root Department

                //var rootDepartment = cleanContext.Departments.FirstOrDefault(d => d.Name == CleanConstants.DefaultDepartment);

                //if (rootDepartment == null)
                //{
                //    rootDepartment = new Department
                //    {
                //        Name = CleanConstants.DefaultDepartment,
                //        ShortName = "Clean",
                //        DepartmentTypeId = rootDepartmentType.Id,
                //        CityId = rootCity.Id
                //    };
                //    cleanContext.Departments.Add(rootDepartment);

                //    if (rootDepartment.Id <= 0)
                //    {
                //        throw new Exception("Could not Insert the root department");
                //    }

                //    if (rootDepartment.ParentId == 0)
                //    {
                //        rootDepartment.ParentId = rootDepartment.Id;
                //        cleanContext.Departments.Update(rootDepartment);
                //    }
                //}

                ////Insert the root Employee

                //var user = await userManager.FindByNameAsync(CleanConstants.DefaultEmployee);

                //if (user == null)
                //{
                //    throw new Exception("Identity User does not exist");
                //}


                //var rootEmployee = cleanContext.Employees.FirstOrDefault(e => e.SSN == "ssn");

                //if (rootEmployee == null)
                //{
                //    rootEmployee = new Employee
                //    {
                //        RankId = rootRank.Id,
                //        DepartmentId = rootDepartment.Id,
                //        FirstName = user.UserName,
                //        LastName = user.UserName,
                //        SSN = "ssn",
                //        Avatar = new byte[32],
                //    };

                //    cleanContext.Employees.Add(rootEmployee);

                //    if (rootEmployee.Id <= 0)
                //    {
                //        throw new Exception("Could not insert the root Employee");
                //    }
                //}

                //if (rootDepartment.ManagerId == 0)
                //{
                //    rootDepartment.ManagerId = rootEmployee.Id;
                //}

                //Insert the User

                //var rootUser = cleanContext.Users.FirstOrDefault(u => u.Id.Equals(user.Id));

                //if (rootUser == null)
                //{

                //    rootUser = new User
                //    {
                //        Id = new Guid(user.Id),
                //        EmployeeId = rootEmployee.Id,
                //    };

                //    cleanContext.Users.Add(rootUser);

                //    rootUser = cleanContext.Users.FirstOrDefault(u => u.Id.Equals(user.Id));

                //    if (rootUser == null)
                //    {
                //        throw new Exception("Could not create the root User in Clean database");
                //    }
                //}

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

                cleanContext.SaveChanges();
            }
        }

        public static List<City> LoadCities()
        {
            var buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePath = buildDir + @"\CleanDb\Seed\Data\cities.json";

            using (StreamReader r = new StreamReader(filePath))
            {
                string json = r.ReadToEnd();
                List<City> items = JsonConvert.DeserializeObject<List<City>>(json);
                return items?? new List<City>();
            }
        }
    }


}