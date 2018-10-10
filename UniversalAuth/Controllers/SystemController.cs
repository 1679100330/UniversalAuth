using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UniversalAuth.Bll;
using UniversalAuth.Models;

namespace UniversalAuth.Controllers
{
    public class SystemController : Controller
    {
        private SystemBll bll = new SystemBll();
        // GET: System
        public ActionResult User()
        {
            return View();
        }

        public ActionResult GetUser(string username) {
            return Content(bll.GetUser(username));
        }

        public ActionResult SaveUser(User user)
        {
            return Content(bll.SaveUser(user));
        }

        public ActionResult Menu() {
            return View();
        }

        public ActionResult SaveMenu(Menu menu) {
            return Content(bll.SaveMenu(menu));
        }

        public ActionResult GetMenus(string status) {
            return Content(bll.GetMenus(status));
        }

        public ActionResult Prem() {
            return View();
        }

        public ActionResult SavePrem(Menu menu)
        {
            return Content(bll.SavePrem(menu));
        }

        public ActionResult GetPrems() {
            return Content(bll.GetPrems());
        }

        public ActionResult SavePremItem(string menuId, Prem prem)
        {            
            return Content(bll.SavePremItem(menuId,prem));
        }

        public ActionResult GetPremItems(string menuId) {
            return Content(bll.GetPremItems(menuId));
        }

        public ActionResult Role() 
        {
            return View();
        }

        public ActionResult GetRoles()
        {
            return Content(bll.GetRoles());
        }

        public ActionResult SaveRole(Role role)
        {
            return Content(bll.SaveRole(role));
        }

        public ActionResult SavePremToRole(string roleId, string premIds)
        {
            return Content(bll.SavePremToRole(roleId, premIds));
        }

        public ActionResult GetPremsByRoleId(string roleId) {
            return Content(bll.GetPremsByRoleId(roleId));
        }

        public ActionResult SavePremToUser(string userId, string premIds)
        {
            return Content(bll.SavePremToUser(userId, premIds));
        }

        public ActionResult GetPremsByUserId(string userId) {
            return Content(bll.GetPremsByUserId(userId));
        }

        public ActionResult GetRolesByUserId(string userId) {
            return Content(bll.GetRolesByUserId(userId));
        }

        public ActionResult SaveRoleToUser(string userId, string roleIds)
        {
            return Content(bll.SaveRoleToUser(userId, roleIds));
        }

        public ActionResult Group() 
        {
            return View();
        }

        public ActionResult SaveGroup(Group group)
        {
            return Content(bll.SaveGroup(group));
        }

        public ActionResult GetGroups() {
            return Content(bll.GetGroups());
        }

        public ActionResult SaveRoleToGroup(string groupId, string roleIds) {
            return Content(bll.SaveRoleToGroup(groupId, roleIds));
        }

        public ActionResult GetRolesByGroupId(string groupId) {
            return Content(bll.GetRolesByGroupId(groupId));
        }

        public ActionResult GetGroupsByUserId(string userId) {
            return Content(bll.GetGroupsByUserId(userId));
        }

        public ActionResult SaveGroupToUser(string userId,string groupIds)
        {
            return Content(bll.SaveGroupToUser(userId, groupIds));
        }

        public ActionResult DeleteUser(string userId) {
            return Content(bll.DeleteUser(userId));
        }

        public ActionResult DeleteGroup(string groupId) {
            return Content(bll.DeleteGroup(groupId));
        }

        public ActionResult DeleteRole(string roleId)
        {
            return Content(bll.DeleteRole(roleId));
        }

        public ActionResult DeleteMenu(string menuId) {
            return Content(bll.DeleteMenu(menuId));
        }

        public ActionResult DeletePrem(string menuId) {
            return Content(bll.DeletePrem(menuId));
        }

        public ActionResult DeletePremItem(string premId) {
            return Content(bll.DeletePremItem(premId));
        }

        public ActionResult GetUsersByGroupId(string groupId) {
            return Content(bll.GetUsersByGroupId(groupId));
        }

        public ActionResult RemoveUserToGroup(string groupId, string userId) {
            return Content(bll.RemoveUserToGroup(groupId, userId));
        }

        public ActionResult GetUsersByRoleId(string roleId)
        {
            return Content(bll.GetUsersByRoleId(roleId));
        }

        public ActionResult RemoveUserToRole(string roleId, string userId) {
            return Content(bll.RemoveUserToRole(roleId, userId));
        }

        public ActionResult GetGroupsByRoleId(string roleId)
        {
            return Content(bll.GetGroupsByRoleId(roleId));
        }

        public ActionResult RemoveGroupToRole(string roleId, string groupId)
        {
            return Content(bll.RemoveGroupToRole(roleId, groupId));
        }
        
    }
}