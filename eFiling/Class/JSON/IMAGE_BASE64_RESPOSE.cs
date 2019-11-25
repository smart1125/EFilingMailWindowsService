using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eFiling
{
    /// <summary>
    /// 
    /// </summary>
    public class IMAGE_BASE64_SYSTEM_CLASS
    {
        public IMAGE_BASE64_SYSTEM_INFO_CLASS SYSTEM { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class IMAGE_BASE64_SYSTEM_INFO_CLASS : SYSTEM_INFO_CLASS
    {
        public IMAGE_BASE64_CLASS CASE_INFO { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class IMAGE_BASE64_CLASS
    {
        public class IMAGE_BASE64_ITEM
        {
            public string FILE_ID { get; set; }

            public string BODY { get; set; }
        }
        public IMAGE_BASE64_ITEM[] JPG_ITEM { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class IMAGE_BASE64_RESPOSE
    {
        public IMAGE_BASE64_SYSTEM_CLASS CHANGINGTEC { get; set; }
    }
}