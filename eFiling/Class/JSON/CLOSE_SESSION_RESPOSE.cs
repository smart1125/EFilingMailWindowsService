using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eFiling
{
    /// <summary>
    /// 
    /// </summary>
    public class CLOSE_SESSION_SYSTEM_CLASS
    {
        public CLOSE_SESSION_SYSTEM_INFO_CLASS SYSTEM { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class CLOSE_SESSION_SYSTEM_INFO_CLASS : SYSTEM_INFO_CLASS
    {
        public string SESSION_KEY { get; set; }

        public string YM { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class CLOSE_SESSION_RESPOSE
    {
        public CLOSE_SESSION_SYSTEM_CLASS CHANGINGTEC { get; set; }
    }
}