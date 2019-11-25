using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eFiling
{
    /// <summary>
    /// LogSecurityHandler 的摘要描述
    /// </summary>
    public class LogSecurityHandler : IHttpHandler
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