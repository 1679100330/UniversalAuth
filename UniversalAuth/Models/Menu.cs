using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UniversalAuth.Models
{
    public class Menu
    {
        public virtual string Id { get; set; }
        public virtual string Pid { get; set; }
        public virtual string Name { set; get; }
        public virtual string ControllerName { set; get; }
        public virtual string ActionName { set; get; }
        public virtual string Status { set; get; }
        public virtual DateTime CreateDate { set; get; }
        [JsonIgnore]
        public virtual ISet<Prem> Prems { get; set; }
        [JsonIgnore]
        public virtual IList<Role> Roles { get; set; }
        [JsonIgnore]
        public virtual IList<User> Users { get; set; }
        /** 版本控制 */
        public virtual int Version { get; set; }        
    }
}