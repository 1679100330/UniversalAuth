using Newtonsoft.Json;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UniversalAuth.Models;
using UniversalAuth.Utils;

namespace UniversalAuth.Dao
{
    public class HomeDao
    {

        public User DoLogin(User user)
        {
            MsgResult msg = new MsgResult();
            using (ISession session = DbUtils.GetSession())
            {
                user = session
                    .CreateCriteria(typeof(User))
                    .Add(Restrictions.Eq("Username", user.Username))
                    .UniqueResult<User>();
                if("admin".Equals(user.Username)){
                    user.Menus = session
                        .CreateCriteria(typeof(Menu))
                        .List<Menu>(); ;
                }
                IList<Menu> menus = new List<Menu>();
                if(user!=null){
                    //加載懶加載的數據
                    NHibernateUtil.Initialize(user.Menus);
                    foreach(Menu menu in user.Menus){
                        NHibernateUtil.Initialize(menu.Prems);
                        menus.Add(menu);
                    }
                    NHibernateUtil.Initialize(user.Roles);
                    foreach (Role role in user.Roles) {
                        NHibernateUtil.Initialize(role.Menus);
                        foreach (Menu menu in role.Menus)
                        {
                            NHibernateUtil.Initialize(menu.Prems);
                            bool flag = true;
                            foreach (Menu m in menus)
                            {
                                if (menu.Id==m.Id)
                                {
                                    flag = false;
                                }
                            }
                            if(flag){
                                menus.Add(menu);
                            }
                        }
                    }
                    NHibernateUtil.Initialize(user.Groups);
                    foreach (Group group in user.Groups)
                    {
                        NHibernateUtil.Initialize(group.Roles);
                        foreach (Role role in group.Roles)
                        {
                            NHibernateUtil.Initialize(role.Menus);
                            foreach (Menu menu in role.Menus)
                            {
                                NHibernateUtil.Initialize(menu.Prems);
                                bool flag = true;
                                foreach (Menu m in menus)
                                {
                                    if (menu.Id == m.Id)
                                    {
                                        flag = false;
                                    }
                                }
                                if (flag)
                                {
                                    menus.Add(menu);
                                }
                            }
                        }
                    }
                    user.Menus = menus;
                }                
                return user;
            }
        }
    }
}