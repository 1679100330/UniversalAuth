using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UniversalAuth.Dao;
using UniversalAuth.Models;

namespace UniversalAuth.Bll
{
    public class SystemBll
    {
        private SystemDao dao = new SystemDao();
        public string SaveUser(User user)
        {
            return JsonConvert.SerializeObject(dao.SaveUser(user));
        }

        public string GetUser(string username)
        {
            return JsonConvert.SerializeObject(dao.GetUser(username));
        }

        public string SaveMenu(Menu menu)
        {
            return JsonConvert.SerializeObject(dao.SaveMenu(menu));
        }

        public string GetMenus(string status)
        {
            return JsonConvert.SerializeObject(dao.GetMenus(status));
        }

        public string SavePrem(Menu menu)
        {
            return JsonConvert.SerializeObject(dao.SavePrem(menu));
        }

        public string GetPrems()
        {
            return JsonConvert.SerializeObject(dao.GetPrems());
        }

        public string SavePremItem(string menuId, Prem prem)
        {
            return JsonConvert.SerializeObject(dao.SavePremItem(menuId,prem));
        }

        public string GetPremItems(string menuId)
        {
            return JsonConvert.SerializeObject(dao.GetPremItems(menuId));
        }

        public string GetRoles()
        {
            return JsonConvert.SerializeObject(dao.GetRoles());
        }

        public string SaveRole(Role role)
        {
            return JsonConvert.SerializeObject(dao.SaveRole(role));
        }

        public string SavePremToRole(string roleId, string premIds)
        {
            return JsonConvert.SerializeObject(dao.SavePremToRole(roleId, premIds));
        }

        public string GetPremsByRoleId(string roleId)
        {
            return JsonConvert.SerializeObject(dao.GetPremsByRoleId(roleId));
        }

        public string SavePremToUser(string userId, string premIds)
        {
            return JsonConvert.SerializeObject(dao.SavePremToUser(userId, premIds));
        }

        public string GetPremsByUserId(string userId)
        {
            return JsonConvert.SerializeObject(dao.GetPremsByUserId(userId));
        }

        public string GetRolesByUserId(string userId)
        {
            return JsonConvert.SerializeObject(dao.GetRolesByUserId(userId));
        }

        public string SaveRoleToUser(string userId, string roleIds)
        {
            return JsonConvert.SerializeObject(dao.SaveRoleToUser(userId, roleIds));
        }

        public string SaveGroup(Group group)
        {
            return JsonConvert.SerializeObject(dao.SaveGroup(group));
        }

        public string GetGroups()
        {
            return JsonConvert.SerializeObject(dao.GetGroups());
        }

        public string SaveRoleToGroup(string groupId, string roleIds)
        {
            return JsonConvert.SerializeObject(dao.SaveRoleToGroup(groupId, roleIds));
        }

        public string GetRolesByGroupId(string groupId)
        {
            return JsonConvert.SerializeObject(dao.GetRolesByGroupId(groupId));
        }

        public string GetGroupsByUserId(string userId)
        {
            return JsonConvert.SerializeObject(dao.GetGroupsByUserId(userId));
        }

        public string SaveGroupToUser(string userId, string groupIds)
        {
            return JsonConvert.SerializeObject(dao.SaveGroupToUser(userId, groupIds));
        }

        public string DeleteUser(string userId)
        {
            return JsonConvert.SerializeObject(dao.DeleteUser(userId));
        }

        public string DeleteGroup(string groupId)
        {
            return JsonConvert.SerializeObject(dao.DeleteGroup(groupId));
        }

        public string DeleteRole(string roleId)
        {
            return JsonConvert.SerializeObject(dao.DeleteRole(roleId));
        }

        public string DeleteMenu(string menuId)
        {
            return JsonConvert.SerializeObject(dao.DeleteMenu(menuId));
        }

        public string DeletePrem(string menuId)
        {
            return JsonConvert.SerializeObject(dao.DeletePrem(menuId));
        }

        public string DeletePremItem(string premId)
        {
            return JsonConvert.SerializeObject(dao.DeletePremItem(premId));
        }

        public string GetUsersByGroupId(string groupId)
        {
            return JsonConvert.SerializeObject(dao.GetUsersByGroupId(groupId));
        }

        public string RemoveUserToGroup(string groupId, string userId)
        {
            return JsonConvert.SerializeObject(dao.RemoveUserToGroup(groupId, userId));
        }

        public string GetUsersByRoleId(string roleId)
        {
            return JsonConvert.SerializeObject(dao.GetUsersByRoleId(roleId));
        }

        public string RemoveUserToRole(string roleId, string userId)
        {
            return JsonConvert.SerializeObject(dao.RemoveUserToRole(roleId, userId));
        }

        public string GetGroupsByRoleId(string roleId)
        {
            return JsonConvert.SerializeObject(dao.GetGroupsByRoleId(roleId));
        }

        public string RemoveGroupToRole(string roleId, string groupId)
        {
            return JsonConvert.SerializeObject(dao.RemoveGroupToRole(roleId, groupId));
        }        
    }
}