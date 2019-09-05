using System;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Net;
using System.Xml;
using System.Windows.Forms;
using System.Collections;
using System.Data.SqlClient;

namespace EFilingMailWindowsServiceTest
{
    public static class UtilityExtensions
    {
        #region DeleteDirectory()
        /// <summary>
        /// 刪除整個目錄
        /// </summary>
        /// <param name="DirectoryPath"></param>
        public static void DeleteDirectory(this string DirectoryPath)
        {
            try
            {
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
            if (!File.Exists(FilePath)) return true;

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

        #region DeleteTempDirectory()
        /// <summary>
        /// 清除暫存目錄
        /// </summary>
        /// <param name="DirectoryPath"></param>
        public static void DeleteTempDirectory(this string DirectoryPath)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(DirectoryPath);

                if (!dir.Exists) return;

                if (dir.GetDirectories().Length > 0)
                {
                    foreach (DirectoryInfo d in dir.GetDirectories())
                    {
                        if (DateTime.Compare(DateTime.Now, d.LastAccessTime.AddDays(1)) >= 0)
                        {
                            d.FullName.DeleteDirectory();
                        }
                    }
                }
            }
            catch { }
        }
        #endregion

        #region CreateAttribute()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MyXmlNode"></param>
        /// <param name="XmlDoc"></param>
        /// <param name="AttrName"></param>
        /// <param name="AttrValue"></param>
        /// <returns></returns>
        public static XmlNode CreateAttribute(this XmlNode MyXmlNode, XmlDocument XmlDoc, string AttrName, string AttrValue)
        {
            if (MyXmlNode.Attributes[AttrName] == null) ((XmlElement)MyXmlNode).SetAttributeNode(XmlDoc.CreateAttribute(AttrName));

            MyXmlNode.Attributes[AttrName].Value = AttrValue;

            return MyXmlNode;
        }
        #endregion

        #region CreateChildNode()
        /// <summary>
        /// 建立新節點
        /// </summary>
        /// <param name="XmlDoc">XmlDocument</param>
        /// <param name="ParentNode">父節點</param>
        /// <param name="XmlNamesPace">命名空間</param>
        /// <param name="NodeName">節點名稱</param>
        /// <param name="InnerText">內容</param>
        /// <returns></returns>
        public static XmlElement CreateChildNode(this XmlDocument XmlDoc, XmlElement ParentNode, string NodeName, string InnerText)
        {
            return XmlDoc.CreateChildNode(ParentNode, string.Empty, NodeName, InnerText);
        }
        /// <summary>
        /// 建立新節點
        /// </summary>
        /// <param name="XmlDoc">XmlDocument</param>
        /// <param name="ParentNode">父節點</param>
        /// <param name="XmlNamesPace">命名空間</param>
        /// <param name="NodeName">節點名稱</param>
        /// <param name="InnerText">內容</param>
        /// <returns></returns>
        public static XmlElement CreateChildNode(this XmlDocument XmlDoc, XmlElement ParentNode, string XmlNamesPace, string NodeName, string InnerText)
        {
            if (XmlDoc == null) return ParentNode;
            XmlElement el = XmlNamesPace.Length == 0 ? XmlDoc.CreateElement(NodeName) : XmlDoc.CreateElement(NodeName, XmlNamesPace);
            el.InnerText = InnerText;
            ParentNode.AppendChild(el);
            return el;
        }
        #endregion

        #region GetAttributesValue()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="XmlDoc"></param>
        /// <param name="xPath"></param>
        /// <param name="AttName"></param>
        /// <param name="Default"></param>
        /// <returns></returns>
        public static string GetAttributesValue(this XmlDocument XmlDoc, string xPath, string AttName, string Default)
        {
            XmlNode xmlNode = XmlDoc.SelectSingleNode(xPath);

            return xmlNode == null || xmlNode.Attributes[AttName] == null ? Default : xmlNode.Attributes[AttName].Value.Trim();
        }
        #endregion

        #region SetAttributesValue()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="XmlDoc"></param>
        /// <param name="xPath"></param>
        /// <param name="AttName"></param>
        /// <param name="AttValue"></param>
        public static void SetAttributesValue(this XmlDocument XmlDoc, string xPath, string AttName, string AttValue)
        {
            XmlNode xmlNode = XmlDoc.SelectSingleNode(xPath);

            xmlNode.Attributes[AttName].Value = AttValue;
        }
        #endregion

        #region GetInnerText()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="XmlDoc"></param>
        /// <param name="xPath"></param>
        /// <returns></returns>
        public static string GetInnerText(this XmlDocument XmlDoc, string xPath, string Default)
        {
            XmlNode xmlNode = XmlDoc.SelectSingleNode(xPath);

            return xmlNode == null ? !String.IsNullOrEmpty(Default) ? Default : string.Empty : xmlNode.InnerText.Trim();
        }
        #endregion

        #region SetInnerText()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="XmlDoc"></param>
        /// <param name="xPath"></param>
        /// <returns></returns>
        public static void SetInnerText(this XmlDocument XmlDoc, string xPath, string InnerText)
        {
            XmlNode xmlNode = XmlDoc.SelectSingleNode(xPath);

            xmlNode.InnerText = InnerText;
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

            if (FileRoot.EndsWith(@"\") || FileRoot.EndsWith("/")) FileRoot = FileRoot.Remove(FileRoot.Length - 1, 1);

            if (FilePath.StartsWith(@"\") || FilePath.StartsWith(@"/")) FilePath = FilePath.Remove(0, 1);

            if (FileRoot.StartsWith(@"\") || FileRoot.StartsWith(@"/")) file_path = Path.Combine(string.Format(@"{0}\", FileRoot), FilePath);
            else if (FileRoot.Length.Equals(1)) file_path = Path.Combine(string.Format(@"{0}:\", FileRoot), FilePath);
            else file_path = Path.Combine(FileRoot, FilePath);

            return file_path;
        }
        #endregion

        #region CopyFile()
        /// <summary>
        /// 複製檔案
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="SaveFilePath"></param>
        /// <returns></returns>
        public static bool CopyFile(this string FilePath, string SaveFilePath)
        {
            if (!File.Exists(FilePath)) return true;

            int retryCount = 0;

            while (true)
            {
                FileInfo fileInfo = new FileInfo(FilePath);
                try
                {
                    if (fileInfo.Exists)
                    {
                        fileInfo.CopyTo(SaveFilePath); return true;
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
    }
}
