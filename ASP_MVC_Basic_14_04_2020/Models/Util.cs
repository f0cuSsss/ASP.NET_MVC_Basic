using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ASP_MVC_Basic_14_04_2020.Models
{
    public class Util
    {
        private static SqlConnection con;

        public static SqlConnection GetConnection()
        {
            if(con == null)
            {
                con = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["db1"].ConnectionString);
                con.Open();
            }
            return con;
        }

        public static String GetSHA_256(String str)
        {
            using (var hasher = System.Security.Cryptography.SHA256.Create())
            {
                byte[] strBytes = System.Text.Encoding.ASCII.GetBytes(str);

                byte[] hashBytes = hasher.ComputeHash(strBytes);

                var sb = new System.Text.StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }

                return sb.ToString();
            }
        }
        

        public static bool IsValueInDBField(
            String TableName,
            String FieldName,
            String Value)
        {

            var cmd = new SqlCommand($"SELECT COUNT(*) FROM {TableName} " +
                        $"WHERE LOWER([{FieldName}]) LIKE LOWER(N'{Value}')", GetConnection());

            return Convert.ToInt32(cmd.ExecuteScalar()) > 0 ? true : false;
        }

        public static void InsertInTable(String TableName, object Data)
        {
            //String values;
            String query = $"INSERT INTO {TableName} ( ";
            String values = "VALUES ( ";
            bool notFirst = false;
            foreach (var prop in Data.GetType().GetProperties())
            {
                if (prop.Name.ToUpper().Equals("ID")) continue;

                if (notFirst)
                {
                    query += ", ";
                    values += ", ";
                }
                else
                {
                    notFirst = true;
                }
                query += prop.Name;
                
                // Homework : 
                // 1. Учесть регистр написания ++
                // 2. Учесть типы, которые также нужно брать в кавычки ++
                // 3. Проверить локализацию - запятые в дробных числах заменить на точки ++

                if (prop.PropertyType.Name.ToLower().Equals("string"))
                {
                    object val = prop.GetValue(Data);
                    if (val == null)
                        values += "null";
                    else
                        values += $"N'{prop.GetValue(Data)}'";
                }
                else if (prop.PropertyType.Name.ToLower().Equals("datetime"))
                {
                    values += $"'{prop.GetValue(Data)}'";
                }
                else if (prop.PropertyType.Name.ToLower().Equals("double" )
                      || prop.PropertyType.Name.ToLower().Equals("float"  )
                      || prop.PropertyType.Name.ToLower().Equals("decimal"))
                {
                    values += prop.GetValue(Data).ToString().Replace(',', '.');
                }
                else
                {
                    values += prop.GetValue(Data);
                }
            }
            query += " ) ";
            values += " ) ";
            
            using (var cmd = new SqlCommand(query + values, con))
            {
                cmd.ExecuteNonQuery();
            }
            
        }

        public static void DeleteFromTable(String TableName, int Id)
        {
            using (var cmd = new SqlCommand(
                $"DELETE FROM {TableName} WHERE Id = {Id}", GetConnection()))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public static void UpdateInTable(String TableName, object Data)
        {
            int id = 0;
            String query = $"UPDATE {TableName} SET ";
            bool notFirst = false;
            foreach (var prop in Data.GetType().GetProperties())
            {
                if (prop.Name.ToUpper().Equals("ID"))
                {
                    id = Convert.ToInt32(prop.GetValue(Data));
                    continue;
                }
                

                if (notFirst)
                {
                    query += ", ";
                }
                else
                {
                    notFirst = true;
                }
                query += prop.Name;

                // Homework : 
                // 1. Учесть регистр написания ++
                // 2. Учесть типы, которые также нужно брать в кавычки ++
                // 3. Проверить локализацию - запятые в дробных числах заменить на точки ++

                if (prop.PropertyType.Name.ToLower().Equals("string"))
                {
                    query += $" = N'{prop.GetValue(Data)}'";
                }
                else if (prop.PropertyType.Name.ToLower().Equals("datetime"))
                {
                    query += $" = '{prop.GetValue(Data)}'";
                }
                else if (prop.PropertyType.Name.ToLower().Equals("double" )
                      || prop.PropertyType.Name.ToLower().Equals("float"  )
                      || prop.PropertyType.Name.ToLower().Equals("decimal"))
                {
                    query += prop.GetValue(Data).ToString().Replace(',', '.');
                }
                else
                {
                    query += " = " + prop.GetValue(Data);
                }
            }
            query += $" WHERE Id = {id}";

            using (var cmd = new SqlCommand(query, con))
            {
                cmd.ExecuteNonQuery();
            }
        }

    }
}