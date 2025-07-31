using Ardalis.Specification;
using InfrastructureDB.Data.Seed;
using Kurier.Common.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureDB.Data.Postgres
{
    public static class PostgresDataInitializer
    {
        public static void SeedDataAsync(this ModelBuilder modelBuilder)
        {
            try
            {
                SeedData data = GetSeedDataFromFile();

                modelBuilder.Entity<Order>().HasData(data.Orders);
                modelBuilder.Entity<User>().HasData(data.Users);
            }
            catch (Exception ex)
            {
                //TODO: добавить логгирование
                throw;
            }
        }

        private static SeedData GetSeedDataFromFile()
        {
            const string filename = "SeedData.json";
            const string path = "Data/Seed";

            string fileContent = File.ReadAllText($"{AppContext.BaseDirectory}/{path}/{filename}");

            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver
                {
                    DefaultMembersSearchFlags = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
                }
            };

            return JsonConvert.DeserializeObject<SeedData>(fileContent, settings) ?? new SeedData();
        }

    }
}
