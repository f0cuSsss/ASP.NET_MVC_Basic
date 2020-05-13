using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP_MVC_Basic_14_04_2020.Models
{
    public class Genre
    {
        // ORM
        public int Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }

        // Data Source:

        public static List<Genre> GetGenres()
        {
            var con = Models.Util.GetConnection();
            
            var cmd = new System.Data.SqlClient.SqlCommand(
                "SELECT Id, Name, Description FROM Genre", con);

            using (var reader = cmd.ExecuteReader())
            {
                //if (!reader.HasRows) return new List<Genre>();
                List<Genre> genres = new List<Genre>();
                while (reader.Read())
                {
                    genres.Add(new Genre()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = reader.GetString(2)
                    });
                }
                return genres;
            }
        }
    }
}