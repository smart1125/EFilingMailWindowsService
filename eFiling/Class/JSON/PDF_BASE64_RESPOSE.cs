using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eFiling
{
    /// <summary>
    /// 
    /// </summary>
    public class PDF_BASE64_SYSTEM_CLASS
    {
        public PDF_BASE64_SYSTEM_INFO_CLASS SYSTEM { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class PDF_BASE64_SYSTEM_INFO_CLASS : SYSTEM_INFO_CLASS
    {
        public PDF_BASE64_CLASS CASE_INFO { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class PDF_BASE64_CLASS
    {
        public class PDF_BASE64_ITEM
        {
            public string FILE_ID { get; set; }

            public string BODY { get; set; }
        }
        public PDF_BASE64_ITEM PDF_ITEM { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class PDF_BASE64_RESPOSE
    {
        public PDF_BASE64_SYSTEM_CLASS CHANGINGTEC { get; set; }
    }
}