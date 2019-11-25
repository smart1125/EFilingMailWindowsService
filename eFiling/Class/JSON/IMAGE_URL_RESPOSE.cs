using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eFiling
{
    /// <summary>
    /// 
    /// </summary>
    public class IMAGE_URL_SYSTEM_CLASS
    {
        public IMAGE_URL_SYSTEM_INFO_CLASS SYSTEM { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class IMAGE_URL_SYSTEM_INFO_CLASS : SYSTEM_INFO_CLASS
    {
        public IMAGE_URL_CLASS CASE_INFO { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class IMAGE_URL_CLASS
    {
        public class IMAGE_URL_ITEM
        {
            public string FILE_ID { get; set; }

            public string URL { get; set; }
        }
        public IMAGE_URL_ITEM[] JPG_ITEM { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class IMAGE_URL_RESPOSE
    {
        public IMAGE_URL_SYSTEM_CLASS CHANGINGTEC { get; set; }
    }
}