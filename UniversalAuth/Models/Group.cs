using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UniversalAuth.Models
{
    public class Group
    {
        public virtual string Id { get; set; }
        public virtual string Name { set; get; }
        public virtual DateTime CreateDate { set; get; }
        [JsonIgnore]
        public virtual IList<Role> Roles { get; set; }
        [JsonIgnore]
        public virtual IList<User> Users { get; set; }
        /** 版本控制 */
        public virtual int Version { get; set; }
    }
}