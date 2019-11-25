using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace eFiling
{
    /// <summary>
    /// 字串處理
    /// </summary>
    public static class StringFormatException
    {
        /// <summary>
        /// 模式
        /// </summary>
        public enum Mode { XML = 0, Exception = 1, Nbsp = 2, JavaScript = 3, CrossSiteScripting = 4, Sql = 5 };

        #region Replace()
        /// <summary>
        /// 字串取代
        /// </summary>
        /// <param name="Mode">Replace String 的模式</param>
        /// <param name="MyString">欲取代的字串</param>
        /// <returns></returns>
        public static string Replace(this string MyString, Mode Mode)
        {
            int mode = (int)Mode;

            if (mode == 0) return ReplaceXml(MyString);
            else if (mode == 1) return ReplaceException(MyString);
            else if (mode == 2) return ReplaceNbsp(MyString);
            else if (mode == 3) return ReplaceJavaScript(MyString);
            else if (mode == 4) return ReplaceCSS(MyString);
            else if (mode == 5) return ReplaceSql(MyString);
            return MyString;
        }
        #endregion

        #region Reduction()
        /// <summary>
        /// 字串還原
        /// </summary>
        /// <param name="Mode">Reduction String 的模式</param>
        /// <param name="MyString">欲還原的字串</param>
        /// <returns></returns>
        public static string Reduction(this string MyString, Mode Mode)
        {
            int mode = (int)Mode;

            if (mode == 0) return ReductionXml(MyString);

            return MyString;
        }
        #endregion

        #region ReplaceXml()
        /// <summary>
        /// XML
        /// </summary>
        /// <param name="MyString">欲取代的字串</param>
        /// <returns></returns>
        private static string ReplaceXml(this string MyString)
        {
            if (MyString.Trim().Length > 0)
            {
                MyString = MyString.Replace("&", "&amp;");
                MyString = MyString.Replace("\'", "&apos;");
                MyString = MyString.Replace("\"", "&quot;");
                MyString = MyString.Replace("<", "&lt;");
                MyString = MyString.Replace(">", "&gt;");
                MyString = MyString.Replace("\n", "&#x000A;");
                MyString = MyString.Replace("<br>", "&#x000A;");
                MyString = MyString.Replace("<br/>", "&#x000A;");
                MyString = MyString.Replace("<br />", "&#x000A;");
            }
            return MyString;
        }
        #endregion

        #region ReductionXml()
        /// <summary>
        /// XML
        /// </summary>
        /// <param name="MyString">欲還原的字串</param>
        /// <returns></returns>
        private static string ReductionXml(this string MyString)
        {
            if (MyString.Trim().Length > 0)
            {
                MyString = MyString.Replace("&amp;", "&");
                MyString = MyString.Replace("&apos;", "\'");
                MyString = MyString.Replace("&quot;", "\"");
                MyString = MyString.Replace("&lt;", "<");
                MyString = MyString.Replace("&gt;", ">");
                MyString = MyString.Replace("&#x000A;", "\n");
            }
            return MyString;
        }
        #endregion

        #region ReplaceException()
        /// <summary>
        /// Exception
        /// </summary>
        /// <param name="MyString">欲取代的字串</param>
        /// <returns></returns>
        private static string ReplaceException(this string MyString)
        {
            if (MyString.LastIndexOf(":line") > 0) return MyString.Replace("\r\n", ".") + MyString.Remove(0, MyString.LastIndexOf(":line"));
            else return MyString.Replace("\r\n", ".");
        }
        #endregion

        #region ReplaceNbsp()
        /// <summary>
        /// Nbsp
        /// </summary>
        /// <param name="MyString">欲取代的字串</param>
        /// <returns></returns>
        private static string ReplaceNbsp(this string MyString)
        {
            if (MyString.Trim() == "&nbsp;") return "";
            else return MyString.Trim();
        }
        #endregion

        #region ReplaceCSS()
        /// <summary>
        /// Cross Site Scripting
        /// </summary>
        /// <param name="MyString">欲取代的字串</param>
        /// <returns></returns>
        private static string ReplaceCSS(this string MyString)
        {
            if (MyString.Trim().Length > 0)
            {
                MyString = MyString.Replace("<", "");
                MyString = MyString.Replace(">", "");
                MyString = MyString.Replace("\'", "");
            }
            return MyString;
        }
        #endregion

        #region ReplaceJavaScript()
        /// <summary>
        /// JavaScript
        /// </summary>
        /// <param name="MyString">欲取代的字串</param>
        /// <returns></returns>
        private static string ReplaceJavaScript(this string MyString)
        {
            if (MyString.Trim().Length > 0)
            {
                MyString = MyString.Replace("\\", @"\\\");
                MyString = MyString.Replace("'", @"\\'");
            }
            return MyString;
        }
        #endregion

        #region ReplaceSql()
        /// <summary>
        /// SQL
        /// </summary>
        /// <param name="MyString">欲取代的字串</param>
        /// <returns></returns>
        private static string ReplaceSql(this string MyString)
        {
            if (MyString.Trim().Length > 0)
            {
                MyString = MyString.Replace("'", "\"");
                MyString = MyString.Replace("--", "");
            }
            return MyString;
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

        #region GetStringLength()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CheckStr"></param>
        /// <returns></returns>
        public static int GetStringLength(this string CheckStr)
        {
            int len = 0;

            float charLen = 0;

            char[] chs = CheckStr.ToCharArray();

            for (int i = 0; i < chs.Length; i++)
            {
                int charLength = System.Text.Encoding.UTF8.GetByteCount(chs[i].ToString());

                if (charLength == 3)
                {
                    len++;
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
                }
            }
            return len;
        }
        #endregion

        #region GetSubstring()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Str"></param>
        /// <param name="Length"></param>
        /// <returns></returns>
        public static string GetString(this string Str, int Length)
        {
            string newStr = string.Empty;

            int len = 0;

            float charLen = 0;

            char[] chs = Str.ToCharArray();

            for (int i = 0; i < chs.Length; i++)
            {
                int charLength = System.Text.Encoding.UTF8.GetByteCount(chs[i].ToString());

                if (charLength == 3)
                {
                    len++;
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
                }
                if (len < Length) newStr += chs[i].ToString();
            }
            return newStr;
        }
        #endregion

        #region RepublicToAD()
        /// <summary>
        /// 民國年轉西元年
        /// </summary>
        /// <param name="RepublicDate"></param>
        /// <returns></returns>
        public static string RepublicToAD(this string RepublicDate)
        {
            try
            {
                if (RepublicDate != null && RepublicDate.Length > 0)
                {
                    string[] temps = RepublicDate.Split('/');

                    if (temps.Length == 3)
                    {
                        RepublicDate = string.Format("{0}/{1}/{2}",
                            Convert.ToInt32(temps[0]) + 1911,
                            temps[1],
                            temps[2]);
                    }
                }
            }
            catch { }

            return RepublicDate;
        }
        #endregion

        #region ADToRepublic()
        /// <summary>
        /// 西元年轉民國年
        /// </summary>
        /// <param name="ADDate"></param>
        /// <returns></returns>
        public static string ADToRepublic(this string ADDate)
        {
            try
            {
                if (ADDate != null && ADDate.Length > 0)
                {
                    string[] temps = ADDate.Split('/');

                    if (temps.Length == 3)
                    {
                        ADDate = string.Format("{0}/{1}/{2}",
                            Convert.ToInt32(temps[0]) - 1911,
                            temps[1],
                            temps[2]);
                    }
                }
            }
            catch { }

            return ADDate;
        }
        #endregion

        #region Nbsp()
        /// <summary>
        /// 空值補空白
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static string Nbsp(this string Str)
        {
            return Str == null || Str.Length == 0 ? "&nbsp;" : Str;
        }
        #endregion

        static readonly byte[] IV = new byte[8] { 240, 3, 45, 29, 0, 76, 173, 59 };

        #region EncryptDES()
        /// <summary>
        /// 加密字串 
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string EncryptDES(this string Text)
        {
            return Text.EncryptDES(eFiling.Properties.Resource.EncryptKey.Trim());
        }
        /// <summary>
        /// 加密字串
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static string EncryptDES(this string Text, string Key)
        {
            TripleDESCryptoServiceProvider des = null;
            try
            {
                if (!String.IsNullOrEmpty(Text) && !String.IsNullOrEmpty(Key))
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(Text);

                    des = new TripleDESCryptoServiceProvider();

                    MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();

                    des.Key = MD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(Key));

                    des.IV = IV;

                    return Convert.ToBase64String(des.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length)).EncryptBase64();
                }
                else return string.Empty;
            }
            catch { throw new Exception("加密發生錯誤, 可能 Key 不正確"); }
            finally
            {
                if (des != null) des = null;
            }
        }
        #endregion

        #region DecryptDES()
        /// <summary>
        /// 解密字串 
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string DecryptDES(this string Text)
        {
            return Text.DecryptDES(eFiling.Properties.Resource.EncryptKey.Trim());
        }
        /// <summary>
        /// 解密字串
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static string DecryptDES(this string Text, string Key)
        {
            TripleDESCryptoServiceProvider des = null;
            try
            {
                if (!String.IsNullOrEmpty(Text))
                {
                    Text = Text.Replace(" ", "+");

                    Text = Text.DecryptBase64();

                    byte[] buffer = Convert.FromBase64String(Text);

                    des = new TripleDESCryptoServiceProvider();

                    MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();

                    des.Key = MD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(Key));

                    des.IV = IV;

                    return Encoding.UTF8.GetString(des.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length));
                }
                else return string.Empty;
            }
            catch { throw new Exception("解密發生錯誤, 可能 Key 不正確"); }
            finally
            {
                if (des != null) des = null;
            }
        }
        #endregion

        #region EncryptBase64()
        /// <summary>
        /// 編碼字串
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        public static string EncryptBase64(this string Message)
        {
            return String.IsNullOrEmpty(Message) ? string.Empty : Convert.ToBase64String(Encoding.UTF8.GetBytes(Message));
        }
        #endregion

        #region DecryptBase64()
        /// <summary>
        /// 解碼字串
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        public static string DecryptBase64(this string Message)
        {
            return String.IsNullOrEmpty(Message) ? string.Empty : Encoding.UTF8.GetString(Convert.FromBase64String(Message));
        }
        #endregion

        #region ConvertStringToHex()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AsciiString"></param>
        /// <returns></returns>
        public static string ConvertStringToHex(this string AsciiString)
        {
            StringBuilder hex = new StringBuilder();

            foreach (char c in AsciiString)
            {
                hex.AppendFormat("{0:x2}", (uint)c);
            }
            return hex.ToString();
        }
        #endregion

        #region ConvertHexToString()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="HexValue"></param>
        /// <returns></returns>
        public static string ConvertHexToString(this string HexValue)
        {
            StringBuilder strValue = new StringBuilder();

            while (HexValue.Length > 0)
            {
                strValue.Append(System.Convert.ToChar(System.Convert.ToUInt32(HexValue.Substring(0, 2), 16)).ToString());

                HexValue = HexValue.Substring(2, HexValue.Length - 2);
            }
            return strValue.ToString();
        }
        #endregion

        #region HtmlEncode()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="HexValue"></param>
        /// <returns></returns>
        public static string HtmlEncode(this string Text)
        {
            return HttpContext.Current.Server.HtmlEncode(Text);
        }
        #endregion

        #region HtmlDecode()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="HexValue"></param>
        /// <returns></returns>
        public static string HtmlDecode(this string Text)
        {
            return HttpContext.Current.Server.HtmlDecode(Text);
        }
        #endregion

        #region UrlEncode()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="HexValue"></param>
        /// <returns></returns>
        public static string UrlEncode(this string Text)
        {
            return HttpContext.Current.Server.UrlEncode(Text);
        }
        #endregion

        #region UrlDecode()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="HexValue"></param>
        /// <returns></returns>
        public static string UrlDecode(this string Text)
        {
            return HttpContext.Current.Server.UrlDecode(Text);
        }
        #endregion

        #region SHA256()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string SHA256(this string Text)
        {
            using (SHA256Managed sha1 = new SHA256Managed())
            {
                byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(Text));

                return Convert.ToBase64String(hash);
            }
        }
        #endregion
    }
}