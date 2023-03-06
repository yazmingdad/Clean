using Clean.Infrastructure.CleanDb.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Infrastructure.CleanDb.Seed
{
    public static class Loader
    {
        public static List<T> LoadFromJson<T>(string fileName)
        {
            var buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePath = buildDir + @$"\CleanDb\Seed\Data\Initial\{fileName}.json";

            using (StreamReader r = new StreamReader(filePath))
            {
                string json = r.ReadToEnd();
                List<T> items = JsonConvert.DeserializeObject<List<T>>(json);

                if(items == null || items.Count==0)
                {
                    throw new Exception("No items where loaded");
                }
                return items;
            }
        }
    }
}
