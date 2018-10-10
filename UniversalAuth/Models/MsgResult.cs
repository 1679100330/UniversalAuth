using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UniversalAuth.Models
{
    public class MsgResult
    {
        public StatusEnum Status { set; get; }
        
        public object Obj { set; get; }
    }
}