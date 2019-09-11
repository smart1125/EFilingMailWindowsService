using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFilingConvertPDFTool.Class
{
   public static class UtilityExtensions
    {
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
    }
}
