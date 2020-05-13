using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP_MVC_Basic_14_04_2020.Models
{
    public class AuthData
    {
        // Имена полей должны совпадать с именами input-ов
        public String UserLogin { get; set; }
        public String UserPassword { get; set; }
    }
}