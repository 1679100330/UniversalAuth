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
    public class SystemDao
    {
        public MsgResult SaveUser(User user)
        {            
            using (ISession session = DbUtils.GetSession())
            {
                using (ITransaction tx = session.BeginTransaction())
                {
                    try
                    {
                        MsgResult msg = new MsgResult();
                        msg.Status = StatusEnum.SUCCESS;
                        if (string.IsNullOrEmpty(user.Id))
                        {
                            User oldUser = session
                                .CreateCriteria(typeof(User))
                                .Add(Restrictions.Eq("Username", user.Username))
                                .UniqueResult<User>();
                            if (oldUser!=null) {
                                msg.Status = StatusEnum.FAILURE;
                                msg.Obj = "该用户名已存在";
                                tx.Rollback();
                                return msg;
                            }
                            user.Id = Guid.NewGuid().ToString();
                            user.CreateDate = DateTime.Now;
                            session.Save(user);
                            msg.Obj = "添加成功";
                        }
                        else
                        {                            
                            User oldUser = session.Get<User>(user.Id);
                            oldUser.Name = user.Name;
                            session.Update(oldUser);
                            msg.Obj = "修改成功";
                        }
                        tx.Commit();
                        return msg;
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        throw ex;
                    }                    
                }                
            }
        }        

        public MsgResult GetUser(string username)
        {
            using (ISession session = DbUtils.GetSession())
            {
                
                ICriteria ic = session
                    .CreateCriteria(typeof(User));
                if(username!=""){
                    ic = ic.Add(Restrictions.Eq("Username", username));
                }
                IList<User> users = ic.List<User>();
                MsgResult msg = new MsgResult();
                msg.Status = StatusEnum.SUCCESS;
                msg.Obj = users;
                return msg;
            }
        }

        public MsgResult SaveMenu(Menu menu)
        {
            using (ISession session = DbUtils.GetSession())
            {
                using (ITransaction tx = session.BeginTransaction())
                {
                    try
                    {
                        MsgResult msg = new MsgResult();
                        msg.Status = StatusEnum.SUCCESS;                        
                        if (string.IsNullOrEmpty(menu.Id))
                        {
                            menu.Id = Guid.NewGuid().ToString();
                            menu.Status = "0";
                            menu.CreateDate = DateTime.Now;
                            session.Save(menu);
                            msg.Obj = "添加成功";
                        }
                        else
                        {
                            if (menu.Id==menu.Pid) {
                                msg.Status = StatusEnum.FAILURE;
                                msg.Obj = "上級菜單不能是自己";
                                tx.Rollback();
                                return msg;
                            }
                            Menu oldMenu = session.Get<Menu>(menu.Id);
                            oldMenu.Pid = menu.Pid;
                            oldMenu.Name = menu.Name;
                            oldMenu.ControllerName = menu.ControllerName;
                            oldMenu.ActionName = menu.ActionName;
                            session.Update(oldMenu);
                            msg.Obj = "修改成功";
                        }
                        tx.Commit();
                        return msg;
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public MsgResult GetMenus(string status)
        {
            using (ISession session = DbUtils.GetSession())
            {
                IList<Menu> menus = session.CreateQuery("from Menu where Status=:status")
                    .SetString("status", status)
                    .List<Menu>();
                MsgResult msg = new MsgResult();
                msg.Status = StatusEnum.SUCCESS;
                msg.Obj = menus;
                return msg;
            }            
        }

        public MsgResult SavePrem(Menu menu)
        {
            using (ISession session = DbUtils.GetSession())
            {
                using (ITransaction tx = session.BeginTransaction())
                {
                    try
                    {
                        MsgResult msg = new MsgResult();
                        msg.Status = StatusEnum.SUCCESS;
                        if (string.IsNullOrEmpty(menu.Id))
                        {
                            menu.Id = Guid.NewGuid().ToString();
                            menu.Status = "1";
                            menu.CreateDate = DateTime.Now;
                            session.Save(menu);
                            msg.Obj = "添加成功";
                        }
                        else
                        {                            
                            Menu oldMenu = session.Get<Menu>(menu.Id);
                            oldMenu.Pid = menu.Pid;
                            oldMenu.Name = menu.Name;
                            session.Update(oldMenu);
                            msg.Obj = "修改成功";
                        }
                        tx.Commit();
                        return msg;
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public MsgResult GetPrems()
        {
            using (ISession session = DbUtils.GetSession())
            {
                IList<Menu> menus = session.CreateQuery("from Menu")
                    .List<Menu>();
                MsgResult msg = new MsgResult();
                msg.Status = StatusEnum.SUCCESS;
                msg.Obj = menus;
                return msg;
            }
        }

        public MsgResult SavePremItem(string menuId, Prem prem)
        {
            using (ISession session = DbUtils.GetSession())
            {
                using (ITransaction tx = session.BeginTransaction())
                {
                    try
                    {
                        MsgResult msg = new MsgResult();                        
                        Menu menu = session.Get<Menu>(menuId);                                                
                        prem.Id = Guid.NewGuid().ToString();
                        prem.CreateDate = DateTime.Now;
                        prem.Menu = menu;
                        session.Save(prem);
                        msg.Status = StatusEnum.SUCCESS;
                        msg.Obj = "添加成功";
                        tx.Commit();
                        return msg;
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public MsgResult GetPremItems(string menuId)
        {
            MsgResult msg = new MsgResult();
            using (ISession session = DbUtils.GetSession())
            {
                Menu menu = session.Get<Menu>(menuId);
                if (menu == null)
                {
                    msg.Status = StatusEnum.FAILURE;
                    msg.Obj = "该权限不存在";
                    return msg;
                }
                //加載懶加載的數據
                NHibernateUtil.Initialize(menu.Prems);
                msg.Status = StatusEnum.SUCCESS;
                msg.Obj = menu.Prems;
                return msg;
            }            
        }


        public MsgResult GetRoles()
        {
            MsgResult msg = new MsgResult();
            using (ISession session = DbUtils.GetSession())
            {
                IList<Role> roles = session.CreateQuery("from Role")
                    .List<Role>();
                msg.Status = StatusEnum.SUCCESS;
                msg.Obj = roles;
                return msg;
            }
        }



        public MsgResult SaveRole(Role role)
        {
            using (ISession session = DbUtils.GetSession())
            {
                using (ITransaction tx = session.BeginTransaction())
                {
                    try
                    {
                        MsgResult msg = new MsgResult();
                        msg.Status = StatusEnum.SUCCESS;
                        if (string.IsNullOrEmpty(role.Id))
                        {
                            role.Id = Guid.NewGuid().ToString();
                            role.CreateDate = DateTime.Now;
                            session.Save(role);
                            msg.Obj = "添加成功";
                        }
                        else
                        {
                            Role oldRole = session.Get<Role>(role.Id);
                            oldRole.Name = role.Name;
                            session.Update(oldRole);
                            msg.Obj = "修改成功";
                        }
                        tx.Commit();
                        return msg;
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        throw ex;
                    }
                }
            }
        }


        public MsgResult SavePremToRole(string roleId, string premIds)
        {
            MsgResult msg = new MsgResult();
            using (ISession session = DbUtils.GetSession())
            {
                using (ITransaction tx = session.BeginTransaction())
                {
                    try
                    {
                        Role role = session.Get<Role>(roleId);
                        role.Menus.Clear();
                        foreach (string menuId in premIds.Split(','))
                        {
                            Menu menu = session.Get<Menu>(menuId);
                            role.Menus.Add(menu);
                        }
                        session.Save(role);                                                
                        tx.Commit();
                        msg.Status = StatusEnum.SUCCESS;
                        msg.Obj = "修改成功";
                        return msg;
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public MsgResult GetPremsByRoleId(string roleId)
        {
            MsgResult msg = new MsgResult();
            using (ISession session = DbUtils.GetSession())
            {
                Role role = session.Get<Role>(roleId);                
                //加載懶加載的數據
                NHibernateUtil.Initialize(role.Menus);
                msg.Status = StatusEnum.SUCCESS;
                msg.Obj = role.Menus;
                return msg;
            } 
        }

        public MsgResult SavePremToUser(string userId, string premIds)
        {
            MsgResult msg = new MsgResult();
            using (ISession session = DbUtils.GetSession())
            {
                using (ITransaction tx = session.BeginTransaction())
                {
                    try
                    {
                        User user = session.Get<User>(userId);
                        user.Menus.Clear();
                        foreach (string menuId in premIds.Split(','))
                        {
                            Menu menu = session.Get<Menu>(menuId);
                            user.Menus.Add(menu);
                        }
                        session.Save(user);
                        tx.Commit();
                        msg.Status = StatusEnum.SUCCESS;
                        msg.Obj = "修改成功";
                        return msg;
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public MsgResult GetPremsByUserId(string userId)
        {
            MsgResult msg = new MsgResult();
            using (ISession session = DbUtils.GetSession())
            {
                User user = session.Get<User>(userId);
                //加載懶加載的數據
                NHibernateUtil.Initialize(user.Menus);
                msg.Status = StatusEnum.SUCCESS;
                msg.Obj = user.Menus;
                return msg;
            } 
        }

        public MsgResult GetRolesByUserId(string userId)
        {
            MsgResult msg = new MsgResult();
            using (ISession session = DbUtils.GetSession())
            {
                User user = session.Get<User>(userId);
                //加載懶加載的數據
                NHibernateUtil.Initialize(user.Roles);
                msg.Status = StatusEnum.SUCCESS;
                msg.Obj = user.Roles;
                return msg;
            } 
        }


        public MsgResult SaveRoleToUser(string userId, string roleIds)
        {
            MsgResult msg = new MsgResult();
            using (ISession session = DbUtils.GetSession())
            {
                using (ITransaction tx = session.BeginTransaction())
                {
                    try
                    {
                        User user = session.Get<User>(userId);
                        user.Roles.Clear();
                        foreach (string roleId in roleIds.Split(','))
                        {
                            Role role = session.Get<Role>(roleId);
                            user.Roles.Add(role);
                        }
                        session.Save(user);
                        tx.Commit();
                        msg.Status = StatusEnum.SUCCESS;
                        msg.Obj = "修改成功";
                        return msg;
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public MsgResult SaveGroup(Group group)
        {
            using (ISession session = DbUtils.GetSession())
            {
                using (ITransaction tx = session.BeginTransaction())
                {
                    try
                    {
                        MsgResult msg = new MsgResult();
                        msg.Status = StatusEnum.SUCCESS;
                        if (string.IsNullOrEmpty(group.Id))
                        {
                            group.Id = Guid.NewGuid().ToString();
                            group.CreateDate = DateTime.Now;
                            session.Save(group);
                            msg.Obj = "添加成功";
                        }
                        else
                        {
                            Group oldGroup = session.Get<Group>(group.Id);
                            oldGroup.Name = group.Name;
                            session.Update(oldGroup);
                            msg.Obj = "修改成功";
                        }
                        tx.Commit();
                        return msg;
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public MsgResult GetGroups()
        {
            MsgResult msg = new MsgResult();
            using (ISession session = DbUtils.GetSession())
            {
                IList<Group> groups = session.CreateQuery("from Group")
                    .List<Group>();
                msg.Status = StatusEnum.SUCCESS;
                msg.Obj = groups;
                return msg;
            }
        }

        public MsgResult SaveRoleToGroup(string groupId, string roleIds)
        {
            MsgResult msg = new MsgResult();
            using (ISession session = DbUtils.GetSession())
            {
                using (ITransaction tx = session.BeginTransaction())
                {
                    try
                    {
                        Group group = session.Get<Group>(groupId);
                        group.Roles.Clear();
                        foreach (string roleId in roleIds.Split(','))
                        {
                            Role role = session.Get<Role>(roleId);
                            group.Roles.Add(role);
                        }
                        session.Save(group);
                        tx.Commit();
                        msg.Status = StatusEnum.SUCCESS;
                        msg.Obj = "修改成功";
                        return msg;
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public MsgResult GetRolesByGroupId(string groupId)
        {
            MsgResult msg = new MsgResult();
            using (ISession session = DbUtils.GetSession())
            {
                Group group = session.Get<Group>(groupId);
                //加載懶加載的數據
                NHibernateUtil.Initialize(group.Roles);
                msg.Status = StatusEnum.SUCCESS;
                msg.Obj = group.Roles;
                return msg;
            } 
        }


        public MsgResult GetGroupsByUserId(string userId)
        {
            MsgResult msg = new MsgResult();
            using (ISession session = DbUtils.GetSession())
            {
                User user = session.Get<User>(userId);
                //加載懶加載的數據
                NHibernateUtil.Initialize(user.Groups);
                msg.Status = StatusEnum.SUCCESS;
                msg.Obj = user.Groups;
                return msg;
            } 
        }

        public MsgResult SaveGroupToUser(string userId, string groupIds)
        {
            MsgResult msg = new MsgResult();
            using (ISession session = DbUtils.GetSession())
            {
                using (ITransaction tx = session.BeginTransaction())
                {
                    try
                    {
                        User user = session.Get<User>(userId);
                        user.Groups.Clear();
                        foreach (string groupId in groupIds.Split(','))
                        {
                            Group group = session.Get<Group>(groupId);
                            user.Groups.Add(group);
                        }
                        session.Save(user);
                        tx.Commit();
                        msg.Status = StatusEnum.SUCCESS;
                        msg.Obj = "修改成功";
                        return msg;
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public MsgResult DeleteUser(string userId)
        {
            MsgResult msg = new MsgResult();
            using (ISession session = DbUtils.GetSession())
            {
                using (ITransaction tx = session.BeginTransaction())
                {
                    try
                    {
                        User user = session.Get<User>(userId);
                        session.Delete(user);
                        tx.Commit();
                        msg.Status = StatusEnum.SUCCESS;
                        msg.Obj = "删除成功";
                        return msg;
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public MsgResult DeleteGroup(string groupId)
        {
            MsgResult msg = new MsgResult();
            using (ISession session = DbUtils.GetSession())
            {
                using (ITransaction tx = session.BeginTransaction())
                {
                    try
                    {
                        Group group = session.Get<Group>(groupId);
                        if (group.Users.Count > 0)
                        {
                            msg.Status = StatusEnum.FAILURE;
                            msg.Obj = "有关联的用户，不能删除。";
                            return msg;
                        }
                        session.Delete(group);
                        tx.Commit();
                        msg.Status = StatusEnum.SUCCESS;
                        msg.Obj = "删除成功";
                        return msg;
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public MsgResult DeleteRole(string roleId)
        {
            MsgResult msg = new MsgResult();
            using (ISession session = DbUtils.GetSession())
            {
                using (ITransaction tx = session.BeginTransaction())
                {
                    try
                    {
                        Role role = session.Get<Role>(roleId);
                        if (role.Users.Count > 0)
                        {
                            msg.Status = StatusEnum.FAILURE;
                            msg.Obj = "有关联的用户，不能删除。";
                            return msg;
                        }
                        if (role.Groups.Count > 0)
                        {
                            msg.Status = StatusEnum.FAILURE;
                            msg.Obj = "有关联的群组，不能删除。";
                            return msg;
                        }
                        session.Delete(role);
                        tx.Commit();
                        msg.Status = StatusEnum.SUCCESS;
                        msg.Obj = "删除成功";
                        return msg;
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public MsgResult DeleteMenu(string menuId)
        {
            MsgResult msg = new MsgResult();
            using (ISession session = DbUtils.GetSession())
            {
                using (ITransaction tx = session.BeginTransaction())
                {
                    try
                    {
                        Menu menu = session.Get<Menu>(menuId);
                        if (menu.Users.Count > 0)
                        {
                            msg.Status = StatusEnum.FAILURE;
                            msg.Obj = "有关联的用户，不能删除。";
                            return msg;
                        }
                        if (menu.Roles.Count > 0)
                        {
                            msg.Status = StatusEnum.FAILURE;
                            msg.Obj = "有关联的角色，不能删除。";
                            return msg;
                        }
                        IList<Menu> menus = session
                            .CreateCriteria(typeof(Menu))
                            .Add(Restrictions.Eq("Pid", menuId))
                            .List<Menu>();
                        if (menus.Count>0) {
                            msg.Status = StatusEnum.FAILURE;
                            msg.Obj = "有子菜单或权限，不能删除。";
                            return msg;
                        }
                        session.Delete(menu);
                        tx.Commit();
                        msg.Status = StatusEnum.SUCCESS;
                        msg.Obj = "删除成功";
                        return msg;
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public object DeletePrem(string menuId)
        {
            MsgResult msg = new MsgResult();
            using (ISession session = DbUtils.GetSession())
            {
                using (ITransaction tx = session.BeginTransaction())
                {
                    try
                    {
                        Menu menu = session.Get<Menu>(menuId);
                        if (menu.Users.Count > 0)
                        {
                            msg.Status = StatusEnum.FAILURE;
                            msg.Obj = "有关联的用户，不能删除。";
                            return msg;
                        }
                        if (menu.Roles.Count > 0)
                        {
                            msg.Status = StatusEnum.FAILURE;
                            msg.Obj = "有关联的角色，不能删除。";
                            return msg;
                        }
                        IList<Menu> menus = session
                            .CreateCriteria(typeof(Menu))
                            .Add(Restrictions.Eq("Pid", menuId))
                            .List<Menu>();
                        if (menus.Count > 0)
                        {
                            msg.Status = StatusEnum.FAILURE;
                            msg.Obj = "有子菜单或权限，不能删除。";
                            return msg;
                        }
                        foreach (Prem prem in menu.Prems) {
                            session.Delete(prem);
                        }
                        session.Delete(menu);
                        tx.Commit();
                        msg.Status = StatusEnum.SUCCESS;
                        msg.Obj = "删除成功";
                        return msg;
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public object DeletePremItem(string premId)
        {
            MsgResult msg = new MsgResult();
            using (ISession session = DbUtils.GetSession())
            {
                using (ITransaction tx = session.BeginTransaction())
                {
                    try
                    {
                        Prem prem = session.Get<Prem>(premId);
                        session.Delete(prem);
                        tx.Commit();
                        msg.Status = StatusEnum.SUCCESS;
                        msg.Obj = "删除成功";
                        return msg;
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public MsgResult GetUsersByGroupId(string groupId)
        {
            MsgResult msg = new MsgResult();
            using (ISession session = DbUtils.GetSession())
            {
                Group group = session.Get<Group>(groupId);
                //加載懶加載的數據
                NHibernateUtil.Initialize(group.Users);
                msg.Status = StatusEnum.SUCCESS;
                msg.Obj = group.Users;
                return msg;
            }
        }

        public MsgResult RemoveUserToGroup(string groupId, string userId)
        {
            MsgResult msg = new MsgResult();
            using (ISession session = DbUtils.GetSession())
            {
                using (ITransaction tx = session.BeginTransaction())
                {
                    try
                    {
                        Group group = session.Get<Group>(groupId);                        
                        for (int i = 0; i < group.Users.Count; i++)
                        {
                            if (group.Users[i].Id == userId)
                            {
                                group.Users.Remove(group.Users[i]);                                
                            }
                        } 
                        session.Save(group);
                        tx.Commit();
                        msg.Status = StatusEnum.SUCCESS;
                        msg.Obj = "移除成功";
                        return msg;
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public MsgResult GetUsersByRoleId(string roleId)
        {
            MsgResult msg = new MsgResult();
            using (ISession session = DbUtils.GetSession())
            {
                Role role = session.Get<Role>(roleId);
                //加載懶加載的數據
                NHibernateUtil.Initialize(role.Users);
                msg.Status = StatusEnum.SUCCESS;
                msg.Obj = role.Users;
                return msg;
            }
        }

        public MsgResult RemoveUserToRole(string roleId, string userId)
        {
            MsgResult msg = new MsgResult();
            using (ISession session = DbUtils.GetSession())
            {
                using (ITransaction tx = session.BeginTransaction())
                {
                    try
                    {
                        Role role = session.Get<Role>(roleId);
                        for (int i = 0; i < role.Users.Count; i++)
                        {
                            if (role.Users[i].Id == userId)
                            {
                                role.Users.Remove(role.Users[i]);
                            }
                        }
                        session.Save(role);
                        tx.Commit();
                        msg.Status = StatusEnum.SUCCESS;
                        msg.Obj = "移除成功";
                        return msg;
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public MsgResult GetGroupsByRoleId(string roleId)
        {
            MsgResult msg = new MsgResult();
            using (ISession session = DbUtils.GetSession())
            {
                Role role = session.Get<Role>(roleId);
                //加載懶加載的數據
                NHibernateUtil.Initialize(role.Groups);
                msg.Status = StatusEnum.SUCCESS;
                msg.Obj = role.Groups;
                return msg;
            }
        }

        public MsgResult RemoveGroupToRole(string roleId, string groupId)
        {
            MsgResult msg = new MsgResult();
            using (ISession session = DbUtils.GetSession())
            {
                using (ITransaction tx = session.BeginTransaction())
                {
                    try
                    {
                        Role role = session.Get<Role>(roleId);
                        for (int i = 0; i < role.Groups.Count; i++)
                        {
                            if (role.Groups[i].Id == groupId)
                            {
                                role.Groups.Remove(role.Groups[i]);
                            }
                        }
                        session.Save(role);
                        tx.Commit();
                        msg.Status = StatusEnum.SUCCESS;
                        msg.Obj = "移除成功";
                        return msg;
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        throw ex;
                    }
                }
            }
        }        
    }
}