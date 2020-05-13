using System;
using System.Collections.Generic;

namespace ASP_MVC_Basic_14_04_2020.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Pseudo { get; set; }

        public static List<Author> GetAuthors()
        {
            var con = Models.Util.GetConnection();

            var cmd = new System.Data.SqlClient.SqlCommand(
                "SELECT Id, Name, Pseudo FROM Author", con);

            using (var reader = cmd.ExecuteReader())
            {
                List<Author> genres = new List<Author>();
                while (reader.Read())
                {
                    genres.Add(new Author()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Pseudo = reader.GetString(2)
                    });
                }
                return genres;
            }
        }
    }
}