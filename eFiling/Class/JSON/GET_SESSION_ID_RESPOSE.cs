using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eFiling
{
    /// <summary>
    /// 
    /// </summary>
    public class GET_SESSION_ID_SYSTEM_CLASS
    {
        public GET_SESSION_ID_SYSTEM_INFO_CLASS SYSTEM { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class GET_SESSION_ID_SYSTEM_INFO_CLASS : SYSTEM_INFO_CLASS
    {
        public string SESSION_KEY { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class GET_SESSION_ID_RESPOSE
    {
        public GET_SESSION_ID_SYSTEM_CLASS CHANGINGTEC { get; set; }
    }
}