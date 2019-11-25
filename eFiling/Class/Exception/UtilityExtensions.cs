using System;
using System.Xml;
using System.Text;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Web.UI;
using System.Xml.Serialization;
using System.Security.Cryptography;
using Log;
using System.Security;
using System.Net;
using System.Collections;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Web;
using OSerialize;
using System.Globalization;

namespace eFiling
{
    public static class UtilityExtensions
    {
        public static bool CheckValidationResult(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        #region FileSerialize()
        /// <summary>
        /// 檔案序列化
        /// </summary>
        /// <param name="FileFullPath"></param>
        /// <returns></returns>
        public static string FileSerialize(this string FileFullPath)
        {
            if (!File.Exists(FileFullPath)) return string.Empty;

            string message = string.Empty;

            Serialize serialize = new Serialize();

            Serialize.SerializeFileInfo info = null;
            try
            {
                info = serialize.File(FileFullPath);

                if (info.Message.Length > 0)
                {
                    throw new Exception(info.Message);
                }
                else return info.Body;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (serialize != null) serialize = null;

                if (info != null) info = null;

                GC.Collect(); GC.WaitForPendingFinalizers();
            }
        }
        #endregion

        #region ResponseXml()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MyPage"></param>
        /// <param name="XmlDoc"></param>
        public static void ResponseXml(this Page MyPage, XmlDocument XmlDoc)
        {
            MyPage.Response.ClearContent();
            MyPage.Response.Clear();
            MyPage.Response.ContentType = "text/xml; charset=utf-8";
            MyPage.Response.Write(XmlDoc.OuterXml);
            MyPage.Response.Flush();
            MyPage.Response.End();
        }
        #endregion

        #region ResponseImage()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MyPage"></param>
        /// <param name="ImageByte"></param>
        public static void ResponseImage(this Page MyPage, byte[] ImageByte)
        {
            MyPage.Response.ClearContent();
            MyPage.Response.ContentType = "image/png";
            MyPage.Response.BinaryWrite(ImageByte);
        }
        #endregion

        #region DeleteDirectory()
        /// <summary>
        /// 刪除整個目錄
        /// </summary>
        /// <param name="DirectoryPath"></param>
        public static void DeleteDirectory(this string DirectoryPath)
        {
            try
            {
                if (String.IsNullOrEmpty(DirectoryPath)) return;

                DirectoryInfo dir = new DirectoryInfo(DirectoryPath);

                if (!dir.Exists) return;

                if (dir.GetFiles().Length >= 0)
                {
                    foreach (FileInfo f in dir.GetFiles()) f.FullName.DeleteSigleFile();
                }

                if (dir.GetDirectories().Length > 0)
                {
                    foreach (DirectoryInfo d in dir.GetDirectories()) d.FullName.DeleteDirectory();
                }
                if (dir.GetFiles().Length.Equals(0)) dir.Delete();
            }
            catch { }
        }
        #endregion

        #region DeleteSigleFile()
        /// <summary>
        /// 刪除檔案
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static bool DeleteSigleFile(this string FilePath)
        {
            if (String.IsNullOrEmpty(FilePath) || !File.Exists(FilePath)) return true;

            int retryCount = 0;

            while (true)
            {
                FileInfo fileInfo = new FileInfo(FilePath);
                try
                {
                    if (fileInfo.Exists)
                    {
                        fileInfo.Delete(); return true;
                    }
                    else return false;
                }
                catch { retryCount++; System.Threading.Thread.Sleep(200); }
                finally
                {
                    if (fileInfo != null) fileInfo = null;
                }
                if (retryCount >= 3) break;
            }
            return false;
        }
        #endregion

        #region GetMaxString()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CheckStr"></param>
        /// <param name="MaxLength"></param>
        /// <returns></returns>
        public static string GetMaxString(this string CheckStr, int MaxLength)
        {
            int len = 0;

            float charLen = 0;

            char[] chs = CheckStr.ToCharArray();

            StringBuilder sb = new StringBuilder();

            int count = 0;

            for (int i = 0; i < chs.Length; i++)
            {
                int charLength = System.Text.Encoding.UTF8.GetByteCount(chs[i].ToString());

                sb.Append(chs[i].ToString());

                if (charLength == 3)
                {
                    len++;

                    count += 2;
                }
                else
                {
                    if (charLen == 0.5)
                    {
                        charLen = 0;
                    }
                    else
                    {
                        charLen = 0.5f; len++;
                    }
                    count += 1;
                }
                if (count >= MaxLength) return sb.ToString();
            }
            return sb.ToString();
        }
        #endregion

        #region GetContentType()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FileExtension"></param>
        /// <returns></returns>
        public static string GetContentType(this string FileExtension)
        {
            string contentType = string.Empty;

            switch (FileExtension.ToLower().Trim())
            {
                case "m3u8": contentType = "application/x-mpegURL"; break;
                case "ts": contentType = "video/MP2T"; break;

                case "gif": contentType = "image/gif"; break;

                case "pdf": contentType = "application/pdf"; break;
                case "swf": contentType = "application/x-shockwave-flash"; break;

                case "mp4": contentType = "video/mp4"; break;
                case "mp2":
                case "mpa":
                case "mpe":
                case "mpeg":
                case "mpg":
                case "mpv2": contentType = "video/mpeg"; break;
                case "qt":
                case "mov": contentType = "video/quicktime"; break;
                case "wmv":
                case "avi": contentType = "video/x-ms-wmv"; break;
                case "lsf":
                case "lsx": contentType = "video/x-la-asf"; break;
                case "asf":
                case "asx": contentType = "video/x-ms-asf"; break;
                case "movie": contentType = "video/x-sgi-movie"; break;

                case "au":
                case "snd": contentType = "audio/basic"; break;
                case "rmi": contentType = "audio/mid"; break;
                case "mp3": contentType = "audio/mpeg"; break;
                case "aif":
                case "aiff":
                case "aifc": contentType = "audio/x-aiff"; break;
                case "m3u": contentType = "audio/x-mpegurl"; break;
                case "ra": contentType = "audio/x-pn-realaudio"; break;
                case "wav": contentType = "file	audio/x-wav"; break;

                default: contentType = "application/octet-stream"; break;
            }
            return contentType;
        }
        #endregion

        #region WebRequest()
        /// <summary>
        /// 
        /// </summary>
        public enum RequestMode
        {
            GET, POST
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="Mode"></param>
        /// <param name="Parameter"></param>
        /// <returns></returns>
        public static string WebRequest(this string URL, RequestMode Mode, string Parameter)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(URL);

            request.Method = Mode.ToString();

            request.ContentType = "application/x-www-form-urlencoded";

            if (URL.ToLower().StartsWith("https")) ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);

            if (Mode.ToString().Equals("POST") && !String.IsNullOrEmpty(Parameter))
            {
                using (var sw = new StreamWriter(request.GetRequestStream()))
                {
                    sw.Write(Parameter);
                }
            }
            var resonse = "";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK) throw new Exception(string.Format("{0}-{1}", ((int)response.StatusCode).ToString(), "回應不成功"));

                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    resonse = sr.ReadToEnd();
                }
            }
            return resonse;
        }
        #endregion

        #region GetRequest()
        /// <summary>
        /// 
        /// </summary>
        public static string GetRequest(this HttpContext Context, string QueryName)
        {
            string result = Context.RequestQueryString(QueryName);

            if (String.IsNullOrEmpty(result)) result = Context.RequestFormString(QueryName);

            return result;
        }
        #endregion

        #region RequestQueryString()
        /// <summary>
        /// 取得參數
        /// </summary>
        public static string RequestQueryString(this HttpContext Context, string QueryName)
        {
            return Context.Request.QueryString[QueryName] != null ? Context.Request.QueryString[QueryName].Trim() : string.Empty;
        }
        #endregion

        #region RequestFormString()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="QueryName"></param>
        /// <returns></returns>
        public static string RequestFormString(this HttpContext Context, string QueryName)
        {
            return Context.Request.Form[QueryName] != null ? Context.Request.Form[QueryName].Trim() : string.Empty;
        }
        #endregion

        #region GetClientIPv4()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Context"></param>
        /// <returns></returns>
        public static string GetClientIPv4(this HttpContext Context)
        {
            try
            {
                string ipv4 = String.Empty;

                foreach (IPAddress ip in Dns.GetHostAddresses(GetClientIP()))
                {
                    if (ip.AddressFamily.ToString() == "InterNetwork")
                    {
                        ipv4 = ip.ToString(); break;
                    }
                }
                if (ipv4 != String.Empty) return ipv4;

                foreach (IPAddress ip in Dns.GetHostEntry(GetClientIP()).AddressList)
                {
                    if (ip.AddressFamily.ToString() == "InterNetwork")
                    {
                        ipv4 = ip.ToString(); break;
                    }

                }
                return ipv4;
            }
            catch
            {
                string ipAddressString = HttpContext.Current.Request.UserHostAddress;

                if (ipAddressString == null) return null;

                IPAddress ipAddress;

                IPAddress.TryParse(ipAddressString, out ipAddress);

                if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                {
                    List<IPAddress> addressList = new List<IPAddress>(System.Net.Dns.GetHostEntry(ipAddress).AddressList);

                    ipAddress = addressList.Find(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                }
                return ipAddress.ToString();
            }
        }
        #endregion

        #region GetClientIP()
        /// <summary>
        /// 取得客戶端主機位址
        /// </summary>
        private static string GetClientIP()
        {
            if (null == HttpContext.Current.Request.ServerVariables["HTTP_VIA"])
            {
                return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            else
            {
                return HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
        }
        #endregion

        #region GetClientMachineName()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Context"></param>
        /// <returns></returns>
        public static string GetClientMachineName(this HttpContext Context)
        {
            return Dns.GetHostEntry(Context.GetClientIPv4()).HostName;
        }
        #endregion

        #region GetMD5()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FS"></param>
        /// <returns></returns>
        public static string GetMD5(this byte[] FileByte)
        {
            if (FileByte == null || FileByte.Length.Equals(0)) return string.Empty;

            StringBuilder sb = new StringBuilder();

            var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

            byte[] hash_bytes = md5.ComputeHash(FileByte);

            for (int i = 0; i < hash_bytes.Length; i++) sb.Append(hash_bytes[i].ToString("x2").ToUpper());

            return sb.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FS"></param>
        /// <returns></returns>
        public static string GetMD5(this string FilePath)
        {
            if (!File.Exists(FilePath)) return string.Empty;

            string md5 = string.Empty;

            using (FileStream fs = new FileStream(FilePath, System.IO.FileMode.Open))
            {
                md5 = fs.GetMD5();

                fs.Close();
            }
            return md5;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FS"></param>
        /// <returns></returns>
        public static string GetMD5(this FileStream FS)
        {
            StringBuilder sb = new StringBuilder();

            var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

            byte[] hash_bytes = md5.ComputeHash(FS);

            for (int i = 0; i < hash_bytes.Length; i++) sb.Append(hash_bytes[i].ToString("x2").ToUpper());

            return sb.ToString();
        }
        #endregion

        #region ResponseSecurity()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="RCODE"></param>
        /// <returns></returns>
        public static void ResponseSecurity(this HttpContext Context)
        {
            Context.Response.Clear();
            Context.Response.ContentType = "text/plain";
            Context.Response.Write(string.Format("壞壞喔你～想幹麻阿？"));
            Context.Response.End();
        }
        #endregion

        #region SetValue()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="D"></param>
        /// <returns></returns>
        public static void SetValue(this Dictionary<string, string> D, string Key, string Value)
        {
            if (!D.ContainsKey(Key)) D.Add(Key, Value);
            else D[Key] = Value;
        }
        #endregion

        #region GetValue()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="D"></param>
        /// <returns></returns>
        public static string GetValue(this Dictionary<string, string> D, string Key)
        {
            return D.ContainsKey(Key) ? D[Key] : string.Empty;
        }
        #endregion

        #region ToLog()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="HT"></param>
        /// <returns></returns>
        public static string ToLog(this List<IDataParameter> Parameter)
        {
            if (Parameter == null || Parameter.Count.Equals(0)) return string.Empty;

            StringBuilder sb_SqlData = new StringBuilder();

            for (int i = 0; i < Parameter.Count; i++)
            {
                SqlParameter para = (SqlParameter)Parameter[i];

                sb_SqlData.AppendLine(string.Format("[{0}] {1}={2} ({3})", i.ToString(), para.ParameterName, para.Value != null ? para.Value.ToString() : string.Empty, para.Value != null ? para.Value.ToString().Length.ToString() : "0"));
            }
            return sb_SqlData.ToString();
        }
        #endregion

        #region RcodeIsPass()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="RCODE"></param>
        /// <returns></returns>
        public static bool RcodeIsPass(this string RCODE)
        {
            return RCODE.Equals("0");
        }
        #endregion

        #region SelectSingleNode()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Node"></param>
        /// <param name="XPath"></param>
        /// <param name="Code"></param>
        /// <returns></returns>
        public static string SelectSingleNode(this XmlNode Node, string XPath, ref SysCode Code)
        {
            return Node.SelectSingleNode(XPath, ref Code, true);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Node"></param>
        /// <param name="XPath"></param>
        /// <param name="NullException"></param>
        /// <param name="Code"></param>
        /// <returns></returns>
        public static string SelectSingleNode(this XmlNode Node, string XPath, ref SysCode Code, bool NullException)
        {
            try
            {
                XmlNode xmlNode = Node.SelectSingleNode(XPath);

                if (NullException && xmlNode == null) throw new Utility.ProcessException(string.Format("{0} is null", XPath), ref Code, SysCode.E003);

                return xmlNode != null ? xmlNode.InnerText.Trim() : string.Empty;
            }
            catch (Utility.ProcessException ex)
            {
                throw new Utility.ProcessException(ex.Message, ref Code, Code);
            }
            catch (System.Exception ex)
            {
                throw new Utility.ProcessException(ex.ToString(), ref Code, SysCode.E999);
            }
        }
        #endregion

        #region ParametersAdd()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Parameters"></param>
        /// <param name="ColumnName"></param>
        public static void ParametersAdd(this List<IDataParameter> Parameters, string ColumnName, string ParameterValue)
        {
            if (!String.IsNullOrEmpty(ParameterValue)) Parameters.Add(new SqlParameter(string.Format("@{0}", ColumnName), ParameterValue));
            else Parameters.Add(new SqlParameter(string.Format("@{0}", ColumnName), DBNull.Value));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Parameters"></param>
        /// <param name="Data"></param>
        /// <param name="ColumnName"></param>
        /// <param name="IsDate"></param>
        public static void ParametersAdd(this List<IDataParameter> Parameters, Hashtable Data, string ColumnName)
        {
            Parameters.ParametersAdd(Data, ColumnName, false, false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Parameters"></param>
        /// <param name="Data"></param>
        /// <param name="ColumnName"></param>
        /// <param name="IsDate"></param>
        public static void ParametersAdd(this List<IDataParameter> Parameters, Hashtable Data, string ColumnName, bool IsDate, bool IsTime)
        {
            if (Data.ContainsKey(ColumnName))
            {
                if (!IsDate && !IsTime)
                {
                    Parameters.Add(new SqlParameter(string.Format("@{0}", ColumnName), Data[ColumnName].ToString()));
                }
                else if (IsDate) Parameters.Add(new SqlParameter(string.Format("@{0}", ColumnName), !IsDate ? Data[ColumnName].ToString() : Data[ColumnName].ToString().ToDate()));
                else if (IsTime) Parameters.Add(new SqlParameter(string.Format("@{0}", ColumnName), !IsTime ? Data[ColumnName].ToString() : Data[ColumnName].ToString().ToTime()));
            }
            else Parameters.Add(new SqlParameter(string.Format("@{0}", ColumnName), DBNull.Value));
        }
        #endregion

        #region FilePathCombine()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FileRoot"></param>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static string FilePathCombine(this string FileRoot, string FilePath)
        {
            string file_path = string.Empty;

            if (FileRoot.StartsWith(@"\") || FileRoot.StartsWith(@"/")) file_path = Path.Combine(string.Format(@"{0}\", FileRoot), FilePath);
            else if (FileRoot.Length.Equals(1)) file_path = Path.Combine(string.Format(@"{0}:\", FileRoot), FilePath);
            else
            {
                if (FileRoot.EndsWith(@"\") || FileRoot.EndsWith(@"/")) FileRoot = FileRoot.Remove(FileRoot.Length - 1, 1);

                if (FilePath.StartsWith(@"\") || FilePath.StartsWith(@"/")) FilePath = FilePath.Remove(0, 1);

                file_path = Path.Combine(FileRoot, FilePath);
            }
            return file_path;
        }
        #endregion

        #region CreateSubDirectory()
        /// <summary>
        /// 產生子目錄
        /// </summary>
        /// <returns></returns>
        public static string CreateSubDirectory(this string DirectoryPath)
        {
            DateTime t = DateTime.Now;

            string subDirPath = System.IO.Path.Combine(t.ToString("yyyy", CultureInfo.InvariantCulture), t.ToString("MM", CultureInfo.InvariantCulture), t.ToString("dd", CultureInfo.InvariantCulture), t.ToString("HH", CultureInfo.InvariantCulture));

            string dirPath = System.IO.Path.Combine(DirectoryPath, subDirPath);

            if (!System.IO.Directory.Exists(dirPath)) System.IO.Directory.CreateDirectory(dirPath);

            return Path.Combine(DirectoryPath, subDirPath);
        }
        #endregion

        #region GetPreviousYM()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="YM"></param>
        /// <returns></returns>
        public static string GetPreviousYM(this string YM)
        {
            return DateTime.Parse(string.Format("{0}/01 00:00:00", YM.Insert(4, "/"))).AddMonths(-1).ToString("yyyyMM");
        }
        #endregion

        #region GetNextYM()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="YM"></param>
        /// <returns></returns>
        public static string GetNextYM(this string YM)
        {
            return DateTime.Parse(string.Format("{0}/01 00:00:00", YM.Insert(4, "/"))).AddMonths(1).ToString("yyyyMM");
        }
        #endregion

        #region GetValue()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="HT"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static string GetValue(this Hashtable HT, string Key)
        {
            return HT.ContainsKey(Key) ? HT[Key].ToString() : string.Empty;
        }
        #endregion

        #region ToDate()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="HT"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static string ToDate(this string Input)
        {
            string date = Input;

            DateTime t = DateTime.MinValue;

            if (!String.IsNullOrEmpty(Input) && Input.IndexOf("/") == -1)
            {
                date = date.Insert(4, "/");

                date = date.Insert(7, "/");
            }
            if (!DateTime.TryParse(date, out t)) return string.Empty;

            return date;
        }
        #endregion

        #region ToTime()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="HT"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static string ToTime(this string Input)
        {
            string date = Input;

            if (!String.IsNullOrEmpty(Input) && Input.IndexOf(":") == -1)
            {
                date = date.Insert(2, ":");

                date = date.Insert(5, ":");
            }
            return date;
        }
        #endregion

        #region GenerateSessionKey()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="G"></param>
        /// <returns></returns>
        public static string GenerateSessionKey(this Guid G, string YM)
        {
            return string.Format("{0}-{1}", YM, G.ToString());
        }
        #endregion
    }
}