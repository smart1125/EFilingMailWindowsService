using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eFiling
{
    /// <summary>
    /// 
    /// </summary>
    public class T24UPDATE_STATE_RESPOSE_SYSTEM_CLASS
    {
        public SYSTEM_INFO_CLASS SYSTEM { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class T24UPDATE_STATE_RESPOSE_RESPOSE
    {
        public T24UPDATE_STATE_RESPOSE_SYSTEM_CLASS CHANGINGTEC { get; set; }
    }
}