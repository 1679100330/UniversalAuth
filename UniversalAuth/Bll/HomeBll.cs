using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UniversalAuth.Dao;
using UniversalAuth.Models;

namespace UniversalAuth.Bll
{
    public class HomeBll
    {
        private HomeDao dao = new HomeDao();
        public User DoLogin(User user)
        {
            return dao.DoLogin(user);
        }
    }
}