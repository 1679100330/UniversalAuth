using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UniversalAuth.Filters
{
    public class GlobalHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {            
            string msgInfo = filterContext.Exception.Message;            
            msgInfo = HttpUtility.UrlEncode(msgInfo);
            filterContext.HttpContext.Response.Redirect("~/Home/Exce?ex=" + msgInfo);            
        }
    }
}