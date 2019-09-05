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

namespace eFilingWindowsServiceSetting
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

        #region SelectedComboBoxItemByValue()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ComBox"></param>
        /// <param name="SelVal"></param>
        public static void SelectedComboBoxItemByValue(this System.Windows.Forms.ComboBox ComBox, string SelVal)
        {
            if (ComBox.Items != null && ComBox.Items.Count > 0)
            {
                ComBox.ComboBoxItem comBoxItem = null;
                try
                {
                    for (int i = 0; i < ComBox.Items.Count; i++)
                    {
                        comBoxItem = (ComBox.ComboBoxItem)ComBox.Items[i];

                        if (comBoxItem.Value != null && comBoxItem.Value.ToString() == SelVal)
                        {
                            ComBox.SelectedIndex = i; break;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (comBoxItem != null) comBoxItem = null;
                }
            }
        }
        #endregion

        #region SelectedComboBoxItemByText()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ComBox"></param>
        /// <param name="SelText"></param>
        public static void SelectedComboBoxItemByText(this System.Windows.Forms.ComboBox ComBox, string SelText)
        {
            if (ComBox.Items != null && ComBox.Items.Count > 0)
            {
                ComBox.ComboBoxItem comBoxItem = null;
                try
                {
                    for (int i = 0; i < ComBox.Items.Count; i++)
                    {
                        comBoxItem = (ComBox.ComboBoxItem)ComBox.Items[i];

                        if (comBoxItem.Text != null && comBoxItem.Text.ToString() == SelText)
                        {
                            ComBox.SelectedIndex = i; break;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (comBoxItem != null) comBoxItem = null;
                }
            }
        }
        #endregion

        #region SelectedComboBoxItemByTag()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ComBox"></param>
        /// <param name="SelTag"></param>
        public static void SelectedComboBoxItemByTag(this System.Windows.Forms.ComboBox ComBox, object SelTag)
        {
            if (ComBox.Items != null && ComBox.Items.Count > 0)
            {
                ComBox.ComboBoxItem comBoxItem = null;
                try
                {
                    for (int i = 0; i < ComBox.Items.Count; i++)
                    {
                        comBoxItem = (ComBox.ComboBoxItem)ComBox.Items[i];

                        if (comBoxItem.Tag != null && comBoxItem.Tag.Equals(SelTag))
                        {
                            ComBox.SelectedIndex = i; break;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (comBoxItem != null) comBoxItem = null;
                }
            }
        }
        #endregion
    }
}
