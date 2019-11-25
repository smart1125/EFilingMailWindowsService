using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eFiling
{
    /// <summary>
    /// 
    /// </summary>
    public class UPLOAD_FILE_SYSTEM_CLASS
    {
        public UPLOAD_FILE_SYSTEM_INFO_CLASS SYSTEM { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class UPLOAD_FILE_SYSTEM_INFO_CLASS : SYSTEM_INFO_CLASS
    {
        public Dictionary<string, string> PROCESS_INFO { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class UPLOAD_FILE_RESPOSE
    {
        public UPLOAD_FILE_SYSTEM_CLASS CHANGINGTEC { get; set; }
    }
}