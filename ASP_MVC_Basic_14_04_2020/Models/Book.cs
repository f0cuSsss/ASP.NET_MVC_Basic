using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP_MVC_Basic_14_04_2020.Models
{
    public class Book
    {
        public int      Id           { get; set; }
        public int      Id_Author    { get; set; }
        public int      Id_Genre     { get; set; }
        public int      Id_Publisher { get; set; }
        public String   Title        { get; set; }
        public String   Annotation   { get; set; }
        public int      Year         { get; set; }
        public decimal  Price        { get; set; }
        public int      Pages        { get; set; }
        public String   Cover_file    { get; set; }


        public static List<Book> GetList()
        {
            var con = Models.Util.GetConnection();
            
            // TODO: Подставить поля в команду по рефлексии
            var cmd = new System.Data.SqlClient.SqlCommand(
                "SELECT Id, Id_Author, Id_Genre, Id_Publisher, Title, Annotation, Year, Price, Pages, Cover_file FROM Book", con);

            using (var reader = cmd.ExecuteReader())
            {
                List<Book> books = new List<Book>();
                while (reader.Read())
                {
                    books.Add(new Book()
                    {
                        Id = reader.GetInt32(0),
                        Id_Author = reader.GetInt32(1),
                        Id_Genre = reader.GetInt32(2),
                        Id_Publisher = reader.GetInt32(3),
                        Title = reader.GetValue(4).ToString(),
                        Annotation = reader.GetValue(5).ToString(),
                        Year = reader.GetInt32(6),
                        Price = reader.GetDecimal(7),
                        Pages = reader.GetInt32(8),
                        Cover_file = reader.GetValue(9).ToString()
                    });
                }
                return books;
            }
        }
    }
}