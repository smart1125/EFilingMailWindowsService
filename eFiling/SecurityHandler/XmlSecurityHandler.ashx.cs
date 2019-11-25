using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eFiling
{
    /// <summary>
    /// XmlSecurityHandler 的摘要描述
    /// </summary>
    public class XmlSecurityHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.ResponseSecurity();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}