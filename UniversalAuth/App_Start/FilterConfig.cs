using System.Web;
using System.Web.Mvc;
using UniversalAuth.Filters;

namespace UniversalAuth
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new GlobalHandleErrorAttribute());
            filters.Add(new AccessAuthorizeAttribute());
        }
    }
}
