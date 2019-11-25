using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eFiling
{
    /// <summary>
    /// 
    /// </summary>
    public class PDF_URL_SYSTEM_CLASS
    {
        public PDF_URL_SYSTEM_INFO_CLASS SYSTEM { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class PDF_URL_SYSTEM_INFO_CLASS : SYSTEM_INFO_CLASS
    {
        public PDF_URL_CLASS CASE_INFO { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class PDF_URL_CLASS
    {
        public class PDF_URL_ITEM
        {
            public string FILE_ID { get; set; }

            public string URL { get; set; }
        }
        public PDF_URL_ITEM PDF_ITEM { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class PDF_URL_RESPOSE
    {
        public PDF_URL_SYSTEM_CLASS CHANGINGTEC { get; set; }
    }
}