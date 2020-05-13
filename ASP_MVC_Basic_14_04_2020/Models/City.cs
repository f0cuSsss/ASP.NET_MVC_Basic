using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP_MVC_Basic_14_04_2020.Models
{
    public class City
    {
        // ORM
        public int Id { get; set; }
        public string Name { get; set; }

        // Data Source

        public static List<City> GetCities()
        {
            var con = Models.Util.GetConnection();

            var cmd = new System.Data.SqlClient.SqlCommand(
                "SELECT Id, Name FROM City", con);

            using (var reader = cmd.ExecuteReader())
            {
                List<City> cities = new List<City>();
                if (!reader.HasRows) return cities;

                while (reader.Read())
                {
                    cities.Add(new City()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1)
                    });
                }
                return cities;
            }
        }
    }
}