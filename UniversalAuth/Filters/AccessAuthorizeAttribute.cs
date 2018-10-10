using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using UniversalAuth.Models;

namespace UniversalAuth.Filters
{
    public class AccessAuthorizeAttribute : AuthorizeAttribute
    {        
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {        
            string url = httpContext.Request.RawUrl;
            User user = (User)httpContext.Session["user"];            
            string virtualPath = httpContext.Request.ApplicationPath;//当前请求的虚拟路径
            if (virtualPath == null || virtualPath == "/")
            {
                virtualPath = "";
            }            
            if (url.ToUpper().StartsWith((virtualPath + "/Home/Login").ToUpper())
                || url.ToUpper().StartsWith((virtualPath + "/Home/Index").ToUpper())
                || url.ToUpper().StartsWith((virtualPath + "/Home/DoLogin").ToUpper())
                || url.ToUpper().StartsWith((virtualPath + "/Home/Exce").ToUpper())
                || url.ToUpper().StartsWith((virtualPath + "/Home/NoAuth").ToUpper()))                                
            {
                return true;
            }
                        
            //判断用户是否登录
            if (user == null)
            {//没有登录
                return false;
            }

            if ("admin".Equals(user.Username))
            {//管理员权限或超级管理员                
                return true;
            }
            return CheckAccessPerm(url, user, virtualPath);                     
        }

        private bool CheckAccessPerm(string url, User user, string virtualPath)
        {
            IList<UniversalAuth.Models.Menu> menus = user.Menus;
            foreach (UniversalAuth.Models.Menu menu in menus)
            {
                if(!string.IsNullOrEmpty(menu.ControllerName)&&!string.IsNullOrEmpty(menu.ActionName)){
                    if (url.ToUpper().StartsWith((virtualPath + "/" + menu.ControllerName + "/" + menu.ActionName).ToUpper()))
                    {
                        return true;
                    }
                }
                ISet<Prem> prems = menu.Prems;
                foreach(Prem prem in prems){
                    string path = prem.Url;
                    if (url.ToUpper().StartsWith((virtualPath + path).ToUpper()))
                    {
                        return true;
                    }
                }                
            }
            IList<Role> roles = user.Roles;
            foreach (Role role in roles)
            {
                menus = role.Menus;
                foreach (UniversalAuth.Models.Menu menu in menus)
                {
                    ISet<Prem> prems = menu.Prems;
                    foreach (Prem prem in prems)
                    {
                        string path = prem.Url;
                        if (url.ToUpper().StartsWith((virtualPath + path).ToUpper()))
                        {
                            return true;
                        }
                    }
                }
            }
            IList<Group> groups = user.Groups;
            foreach(Group group in groups){
                roles = group.Roles;
                foreach (Role role in roles)
                {
                    menus = role.Menus;
                    foreach (UniversalAuth.Models.Menu menu in menus)
                    {
                        ISet<Prem> prems = menu.Prems;
                        foreach (Prem prem in prems)
                        {
                            string path = prem.Url;
                            if (url.ToUpper().StartsWith((virtualPath + path).ToUpper()))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            UrlHelper urlHelper = new UrlHelper(filterContext.RequestContext);
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new RedirectResult(
                    urlHelper.Action("NoAuth", "Home"));
            }
            else
            {
                filterContext.Result = new RedirectResult(
                    urlHelper.Action("Login", "Home",
                    new { target = filterContext.HttpContext.Request.Url.ToString() }));
            }
        }        
    }
}