using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using Log;

namespace FTP
{
    public class FtpWebClient
    {
        private ILog Log { get; set; }

        public enum FtpServerType
        {
            UNIX = 1, Windows = 2
        }
        public DirectoryCollection Directories = new DirectoryCollection();

        public FileCollection Files = new FileCollection();

        internal static string FtpUrl;
        internal static string FtpServerIP;
        internal static string FtpRemotePath;
        internal static string FtpUserID;
        internal static string FtpPassword;
        internal static string RemotePath;
        internal static int FtpPort;

        #region FtpType
        /// <summary>
        /// 
        /// </summary>
        internal static FtpServerType _FtpType = FtpServerType.Windows;
        /// <summary>
        /// 
        /// </summary>
        public FtpServerType FtpType
        {
            get { return _FtpType; }
            set { _FtpType = value; }
        }
        #endregion

        #region FtpWebClient()
        /// <summary>
        /// 連接FTP
        /// </summary>
        /// <param name="ServerIP">FTP連接位址</param>
        /// <param name="RemotePath">指定FTP連接成功後的當前目錄, 如果不指定即默認為根目錄</param>
        /// <param name="UserID">登入帳號</param>
        /// <param name="Password">登入密碼</param>
        public FtpWebClient(string ServerIP, string RemotePath, string UserID, string Password)
        {
            this.Log = Log;

            this.InitProc(ServerIP, RemotePath, UserID, Password, 21);
        }
        /// <summary>
        /// 連接FTP
        /// </summary>
        /// <param name="ServerIP">FTP連接位址</param>
        /// <param name="RemotePath">指定FTP連接成功後的當前目錄, 如果不指定即默認為根目錄</param>
        /// <param name="UserID">登入帳號</param>
        /// <param name="Password">登入密碼</param>
        /// <param name="Port">FTP連接埠</param>
        public FtpWebClient(string ServerIP, string RemotePath, string UserID, string Password, int Port)
        {
            this.Log = Log;

            this.InitProc(ServerIP, RemotePath, UserID, Password, Port);
        }
        #endregion

        #region InitProc()
        /// <summary>
        /// FTP 初始化
        /// </summary>
        /// <param name="ServerIP">FTP連接位址</param>
        /// <param name="RemotePath">指定FTP連接成功後的當前目錄, 如果不指定即默認為根目錄</param>
        /// <param name="UserID">登入帳號</param>
        /// <param name="Password">登入密碼</param>
        /// <param name="Port">FTP連接埠</param>
        private void InitProc(string ServerIP, string RemotePath, string UserID, string Password, int Port)
        {
            FtpWebClient.FtpRemotePath = RemotePath;
            FtpWebClient.FtpUserID = UserID;
            FtpWebClient.FtpPassword = Password;
            FtpWebClient.FtpUrl = string.Format("ftp://{0}:{1}/{2}", FtpWebClient.FtpServerIP = ServerIP, FtpWebClient.FtpPort = Port, String.IsNullOrEmpty(RemotePath) ? "" : string.Format("{0}/", FtpWebClient.RemotePath = RemotePath));
        }
        #endregion

        public class CollectionBase
        {
            internal string ResponseMessage = null;

            #region GetFilesDetailList()
            /// <summary>
            /// 取得當前目錄下明細(包含文件和文件夾)
            /// </summary>
            /// <returns></returns>
            public string[] GetFilesDetailList()
            {
                StringBuilder result = new StringBuilder();

                FtpWebRequest ftpReq = null;
                try
                {
                    ftpReq = (FtpWebRequest)FtpWebRequest.Create(new Uri(FtpWebClient.FtpUrl));
                    ftpReq.Credentials = new NetworkCredential(FtpWebClient.FtpUserID, FtpWebClient.FtpPassword);
                    ftpReq.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                    using (FtpWebResponse ftpRes = (FtpWebResponse)ftpReq.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(ftpRes.GetResponseStream(), Encoding.UTF8))
                        {
                            string line = reader.ReadLine();

                            while (line != null)
                            {
                                result.Append(line);
                                result.Append("\n");
                                line = reader.ReadLine();
                            }
                        }
                    }
                    return result.ToString().Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                }
                catch (System.Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
                finally
                {
                    if (ftpReq != null) ftpReq = null;
                }
            }
            #endregion

            #region GetFileList()
            /// <summary>
            /// 取得當前目錄下文件列表(僅文件)
            /// </summary>
            /// <param name="Mask"></param>
            /// <returns></returns>
            public string[] GetFileList(string Mask)
            {
                StringBuilder result = new StringBuilder();

                FtpWebRequest ftpReq = null;
                try
                {
                    ftpReq = (FtpWebRequest)FtpWebRequest.Create(new Uri(FtpWebClient.FtpUrl));
                    ftpReq.Credentials = new NetworkCredential(FtpWebClient.FtpUserID, FtpWebClient.FtpPassword);
                    ftpReq.Method = WebRequestMethods.Ftp.ListDirectory;

                    using (FtpWebResponse ftpRes = (FtpWebResponse)ftpReq.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(ftpRes.GetResponseStream(), Encoding.UTF8))
                        {
                            string line = reader.ReadLine();

                            while (line != null)
                            {
                                bool check = false;

                                if (!String.IsNullOrEmpty(Mask) && Mask.IndexOf('.') > 0 && Mask.Trim() != "*.*")
                                {
                                    int indexOf = Mask.IndexOf("*");

                                    if (indexOf == 0)
                                    {
                                        check = line.ToUpper().EndsWith(Mask.Split('.')[1].ToUpper());
                                    }
                                    else if (indexOf > 0)
                                    {
                                        check = line.ToUpper().EndsWith(Mask.Split('.')[0].ToUpper());
                                    }
                                    else check = line.ToUpper().Equals(Mask.ToUpper());
                                }
                                else check = true;

                                if (check)
                                {
                                    result.Append(line.Trim()); result.Append("\n");
                                }
                                line = reader.ReadLine();
                            }
                            reader.Close();
                        }
                        ftpRes.Close();
                    }
                    return result.ToString().Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                }
                catch (System.Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }
            #endregion

            #region GetFileListByFileExt()
            /// <summary>
            /// 取得當前目錄下文件列表(僅文件)
            /// </summary>
            /// <param name="FileExt">要查詢的副檔名，空值表示全部</param>
            /// <returns></returns>
            public string[] GetFileListByFileExt(string FileExt)
            {
                try
                {
                    string[] dirList = this.GetFilesDetailList();

                    string fileName = string.Empty;

                    string extension = string.Empty;

                    if (dirList == null) return null;

                    string result = string.Empty;

                    int subStrIndex = 0;

                    foreach (string str in dirList)
                    {
                        if (str.Contains("<DIR>") || "D".Equals(str.Trim().Substring(0, 1).ToUpper()))
                        {
                            continue;
                        }

                        switch (FtpWebClient._FtpType)
                        {
                            case FtpServerType.UNIX:
                                subStrIndex = 49;
                                break;
                            case FtpServerType.Windows:
                                subStrIndex = 39;
                                break;
                            default:
                                break;
                        }

                        if (FileExt == "")
                        {
                            result += str.Substring(subStrIndex).Trim() + "\n";
                        }
                        else
                        {
                            fileName = str.Substring(subStrIndex).Trim();
                            extension = fileName.Substring(fileName.LastIndexOf('.') + 1);
                            if (FileExt.Equals(extension))
                            {
                                result += fileName + "\n";
                            }
                        }
                    }
                    if (result.Length > 0)
                    {
                        result = result.Substring(0, result.Length - 1);
                    }
                    return result.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }
            #endregion

            #region GetDirectoryList()
            /// <summary>
            /// 取得當前目錄下所有的文件夾列表(僅文件夾)
            /// </summary>
            /// <returns></returns>
            public string[] GetDirectoryList()
            {
                try
                {
                    string[] dirList = this.GetFilesDetailList();

                    //取不到資料
                    if (dirList == null) return null;

                    string result = string.Empty;

                    foreach (string str in dirList)
                    {
                        //不同的 FTP Sever 擷取的檔名 Index不同
                        if (str.Contains("<DIR>"))
                        {
                            result += str.Substring(39).Trim() + "\n";
                        }
                        else if ("D".Equals(str.Trim().Substring(0, 1).ToUpper()))
                        {
                            result += str.Substring(49).Trim() + "\n";
                        }
                    }

                    if (result.Length > 0)
                    {
                        result = result.Substring(0, result.Length - 1);
                    }
                    return result.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                }
                catch (System.Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }
            #endregion

            #region GetResponse()
            /// <summary>
            /// 
            /// </summary>
            /// <param name="FtpReq"></param>
            /// <returns></returns>
            internal string GetResponse(FtpWebRequest FtpReq)
            {
                string result = string.Empty;

                using (FtpWebResponse ftpResponse = (FtpWebResponse)FtpReq.GetResponse())
                {
                    using (Stream stream = ftpResponse.GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(stream))
                        {
                            result = sr.ReadToEnd(); sr.Close();
                        }
                        stream.Close();
                    }
                    ftpResponse.Close();
                }
                return result;
            }
            #endregion
        }

        public class FileCollection : CollectionBase
        {
            #region Upload()
            /// <summary>
            /// 上傳
            /// </summary>
            /// <param name="FilePath">來源檔案路徑</param>
            public void Upload(string FilePath, string UploadPath)
            {
                this.ResponseMessage = null;

                FileInfo fi = null;

                FtpWebRequest ftpReq = null;
                try
                {
                    fi = new FileInfo(FilePath);

                    ftpReq = (FtpWebRequest)FtpWebRequest.Create(new Uri(FtpWebClient.FtpUrl + UploadPath));
                    ftpReq.Credentials = new NetworkCredential(FtpWebClient.FtpUserID, FtpWebClient.FtpPassword);
                    ftpReq.KeepAlive = false;
                    ftpReq.UseBinary = true;
                    //ftpReq.UsePassive = false;
                    ftpReq.Method = WebRequestMethods.Ftp.UploadFile;

                    int buffLen = 2048;
                    byte[] buff = new byte[buffLen];
                    int contentLen;
                    try
                    {
                        using (FileStream fileStream = fi.OpenRead())
                        {
                            using (Stream stream = ftpReq.GetRequestStream())
                            {
                                while (true)
                                {
                                    contentLen = fileStream.Read(buff, 0, buffLen);
                                    if (contentLen == 0) break;
                                    stream.Write(buff, 0, contentLen);
                                }
                                stream.Close();
                            }
                            fileStream.Close();
                        }
                        fi = null;

                        this.ResponseMessage = this.GetResponse(ftpReq);
                    }
                    catch (System.Exception ex)
                    {
                        throw new Exception(string.Format("File.Upload.FileStream.Exception::{0}", ex.ToString()));
                    }
                }
                catch (System.Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
                finally
                {
                    if (fi != null) fi = null;

                    if (ftpReq != null) ftpReq = null;
                }
            }
            #endregion

            #region Download()
            /// <summary>
            /// 下載
            /// </summary>
            /// <param name="FileName"></param>
            /// <param name="LocalFilePath"></param>
            public void Download(string FileName, string LocalFilePath)
            {
                this.ResponseMessage = null;

                FtpWebRequest ftpReq = null;
                try
                {
                    ftpReq = (FtpWebRequest)FtpWebRequest.Create(new Uri(FtpWebClient.FtpUrl + FileName));
                    ftpReq.Credentials = new NetworkCredential(FtpWebClient.FtpUserID, FtpWebClient.FtpPassword);
                    ftpReq.UseBinary = true;
                    ftpReq.Method = WebRequestMethods.Ftp.DownloadFile;

                    using (FtpWebResponse ftpResponse = (FtpWebResponse)ftpReq.GetResponse())
                    {
                        using (Stream stream = ftpResponse.GetResponseStream())
                        {
                            using (FileStream fileStream = new FileStream(LocalFilePath, FileMode.Create))
                            {
                                int buffLen = 2048;

                                int readCount;

                                byte[] buff = new byte[buffLen];

                                readCount = stream.Read(buff, 0, buffLen);

                                while (readCount > 0)
                                {
                                    fileStream.Write(buff, 0, readCount);

                                    readCount = stream.Read(buff, 0, buffLen);
                                }
                                fileStream.Close();
                            }
                            stream.Close();
                        }
                        ftpResponse.Close();
                    }
                }
                catch (System.Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
                finally
                {
                    if (ftpReq != null) ftpReq = null;
                }
            }
            #endregion

            #region Delete()
            /// <summary>
            /// 刪除文件
            /// </summary>
            /// <param name="FileName"></param>
            public void Delete(string FileName)
            {
                this.ResponseMessage = null;

                FtpWebRequest ftpReq = null;
                try
                {
                    ftpReq = (FtpWebRequest)FtpWebRequest.Create(new Uri(FtpWebClient.FtpUrl + FileName));
                    ftpReq.Credentials = new NetworkCredential(FtpWebClient.FtpUserID, FtpWebClient.FtpPassword);
                    ftpReq.KeepAlive = false;
                    ftpReq.Method = WebRequestMethods.Ftp.DeleteFile;

                    this.ResponseMessage = this.GetResponse(ftpReq);
                }
                catch (System.Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
                finally
                {
                    if (ftpReq != null) ftpReq = null;
                }
            }
            #endregion

            #region Exist()
            /// <summary>
            /// 判斷當前目錄下指定的文件是否存在
            /// </summary>
            /// <param name="RemoteFileName">遠程文件名</param>
            public bool Exist(string RemoteFileName)
            {
                this.ResponseMessage = null;

                string[] fileList = this.GetFileList("*.*");

                bool check = false;
                
                foreach (string s in fileList)
                {
                    if ((check = s.Trim().Equals(RemoteFileName.Trim()))) break;
                }
                return check;
            }
            #endregion

            #region Size()
            /// <summary>
            /// 獲取指定文件大小
            /// </summary>
            /// <param name="FileName"></param>
            /// <returns></returns>
            public long Size(string FileName)
            {
                FtpWebRequest ftpReq = null;

                long fileSize = -1;
                try
                {
                    ftpReq = (FtpWebRequest)FtpWebRequest.Create(new Uri(FtpWebClient.FtpUrl + FileName));
                    ftpReq.Method = WebRequestMethods.Ftp.GetFileSize;
                    ftpReq.UseBinary = true;
                    ftpReq.Credentials = new NetworkCredential(FtpWebClient.FtpUserID, FtpWebClient.FtpPassword);
                    using (FtpWebResponse ftpResponse = (FtpWebResponse)ftpReq.GetResponse())
                    {
                        using (Stream ftpStream = ftpResponse.GetResponseStream())
                        {
                            fileSize = ftpResponse.ContentLength; ftpStream.Close();
                        }
                        ftpResponse.Close();
                    }
                }
                catch (System.Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
                finally
                {
                    if (ftpReq != null) ftpReq = null;
                }
                return fileSize;
            }
            #endregion

            #region Rename()
            /// <summary>
            /// 改名
            /// </summary>
            /// <param name="CurrentFilename"></param>
            /// <param name="NewFilename"></param>
            public void Rename(string CurrentFilename, string NewFilename)
            {
                this.ResponseMessage = null;

                FtpWebRequest ftpReq = null;
                try
                {
                    ftpReq = (FtpWebRequest)FtpWebRequest.Create(new Uri(FtpWebClient.FtpUrl + CurrentFilename));
                    ftpReq.Method = WebRequestMethods.Ftp.Rename;
                    ftpReq.RenameTo = NewFilename;
                    ftpReq.UseBinary = true;
                    ftpReq.Credentials = new NetworkCredential(FtpWebClient.FtpUserID, FtpWebClient.FtpPassword);

                    this.ResponseMessage = this.GetResponse(ftpReq);
                }
                catch (System.Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
                finally
                {
                    if (ftpReq != null) ftpReq = null;
                }
            }
            #endregion
        }

        public class DirectoryCollection : CollectionBase
        {
            #region Goto()
            /// <summary>
            /// 切換當前目錄
            /// </summary>
            /// <param name="DirectoryName"></param>
            /// <param name="IsRoot">true 絕對路徑   false 相對路徑</param> 
            public void Goto(string DirectoryName, bool IsRoot)
            {
                if (IsRoot)
                {
                    FtpWebClient.FtpRemotePath = DirectoryName;
                }
                else
                {
                    FtpWebClient.FtpRemotePath += DirectoryName + "/";
                }
                FtpWebClient.FtpUrl = string.Format("ftp://{0}:{1}/{2}", FtpWebClient.FtpServerIP, FtpWebClient.FtpPort, FtpWebClient.RemotePath);
            }
            #endregion

            #region Make()
            /// <summary>
            /// 創建文件夾
            /// </summary>
            /// <param name="DirName"></param>
            public void Make(string DirName)
            {
                this.ResponseMessage = null;

                FtpWebRequest ftpReq = null;
                try
                {
                    ftpReq = (FtpWebRequest)FtpWebRequest.Create(new Uri(FtpWebClient.FtpUrl + DirName));
                    ftpReq.Method = WebRequestMethods.Ftp.MakeDirectory;
                    ftpReq.UseBinary = true;
                    ftpReq.Credentials = new NetworkCredential(FtpWebClient.FtpUserID, FtpWebClient.FtpPassword);

                    this.ResponseMessage = this.GetResponse(ftpReq);
                }
                catch (System.Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
                finally
                {
                    if (ftpReq != null) ftpReq = null;
                }
            }
            #endregion

            #region Exist()
            /// <summary>
            /// 判斷當前目錄下指定的子目錄是否存在
            /// </summary>
            /// <param name="RemoteDirectoryName">指定的目錄名</param>
            public bool Exist(string RemoteDirectoryName)
            {
                string[] dirList = this.GetDirectoryList();

                bool check = false;

                foreach (string s in dirList)
                {
                    if ((check = s.Trim().Equals(RemoteDirectoryName.Trim()))) break;
                }
                return check;
            }
            #endregion
        }
    }
}
