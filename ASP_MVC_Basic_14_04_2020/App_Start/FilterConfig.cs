using System.Web;
using System.Web.Mvc;

namespace ASP_MVC_Basic_14_04_2020
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
