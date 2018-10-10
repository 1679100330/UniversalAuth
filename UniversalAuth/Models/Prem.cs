using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UniversalAuth.Models
{
    public class Prem
    {
        public virtual string Id { get; set; }
        public virtual string Url { get; set; }
        public virtual DateTime CreateDate { set; get; }
        [JsonIgnore]
        public virtual Menu Menu { get; set; }
        /** 版本控制 */
        public virtual int Version { get; set; }
    }
}