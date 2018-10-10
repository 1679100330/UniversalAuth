using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UniversalAuth.Bll;
using UniversalAuth.Models;

namespace UniversalAuth.Controllers
{
    public class HomeController : Controller
    {
        private HomeBll bll = new HomeBll();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult DoLogin(User user)
        {
            MsgResult msg = new MsgResult();
            User u = bll.DoLogin(user);
            if(u==null){
                msg.Status = StatusEnum.FAILURE;
                msg.Obj = "用户名或密码错误";                
            }
            else
            {
                Session.Add("user", u);
                msg.Status = StatusEnum.SUCCESS;
                msg.Obj = "Index";
            }
            return Content(JsonConvert.SerializeObject(msg));
        }

        public ActionResult Logout() {
            Session.Remove("user");
            return RedirectToAction("Login");
        }

        public ActionResult Exce(string ex) 
        {
            MsgResult msg = new MsgResult();
            msg.Status = StatusEnum.FAILURE;
            msg.Obj = ex;
            return Content(JsonConvert.SerializeObject(msg));
        }

        public ActionResult NoAuth()
        {
            MsgResult msg = new MsgResult();
            msg.Status = StatusEnum.FAILURE;
            msg.Obj = "No Auth";
            return Content(JsonConvert.SerializeObject(msg));
        }
    }
}