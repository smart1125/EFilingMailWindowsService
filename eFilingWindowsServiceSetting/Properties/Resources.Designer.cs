﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本:4.0.30319.42000
//
//     對這個檔案所做的變更可能會造成錯誤的行為，而且如果重新產生程式碼，
//     變更將會遺失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace eFilingMailServiceSetting.Properties {
    using System;
    
    
    /// <summary>
    ///   用於查詢當地語系化字串等的強類型資源類別。
    /// </summary>
    // 這個類別是自動產生的，是利用 StronglyTypedResourceBuilder
    // 類別透過 ResGen 或 Visual Studio 這類工具。
    // 若要加入或移除成員，請編輯您的 .ResX 檔，然後重新執行 ResGen
    // (利用 /str 選項)，或重建您的 VS 專案。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   傳回這個類別使用的快取的 ResourceManager 執行個體。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("eFilingMailServiceSetting.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   覆寫目前執行緒的 CurrentUICulture 屬性，對象是所有
        ///   使用這個強類型資源類別的資源查閱。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   查詢類似 &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;
        ///&lt;Changingtec&gt;
        ///  &lt;FTP&gt;
        ///    &lt;FTP_IP&gt;192.168.8.134&lt;/FTP_IP&gt;
        ///    &lt;FTP_Port&gt;21&lt;/FTP_Port&gt;
        ///    &lt;FTP_User&gt;CG&lt;/FTP_User&gt;
        ///    &lt;FTP_PWD&gt;123123&lt;/FTP_PWD&gt;
        ///    &lt;FTP_Remote_Path&gt;/Juster&lt;/FTP_Remote_Path&gt;
        ///  &lt;/FTP&gt;
        ///  &lt;TimerGroup&gt;
        ///    &lt;Timer id=&quot;1&quot;&gt;
        ///      &lt;name&gt;Batch&lt;/name&gt;
        ///      &lt;switch&gt;Y&lt;/switch&gt;
        ///      &lt;interval&gt;200&lt;/interval&gt;
        ///      &lt;!--24小時制--&gt;
        ///      &lt;start&gt;1607&lt;/start&gt;
        ///      &lt;lastWorkingTime&gt;2019/09/04 14:38:56&lt;/lastWorkingTime&gt;
        ///    &lt;/Timer&gt;
        ///    &lt;Timer id=&quot;2&quot;&gt;
        ///   [字串的其餘部分已遭截斷]&quot;; 的當地語系化字串。
        /// </summary>
        internal static string Setting {
            get {
                return ResourceManager.GetString("Setting", resourceCulture);
            }
        }
    }
}
