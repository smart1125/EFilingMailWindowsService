using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EFilingConvertPDFTool.Class
{
   public class PDFFILE
    {
        private string filePath;

        private string fileBody;
        public string FileBody
        {
            get
            {

                return fileBody;
            }

            set
            {
                fileBody = value.Trim().Replace(" ", "+");
            }
        }

        public string FilePath
        {
            get
            {
                return filePath;
            }

            set
            {
                filePath = value;
                this.GetFileBody();
            }
        }

        public PDFFILE(string filePath)
        {
            this.FilePath = filePath;
        }

        public void ConvertToFile(string savePath)
        {
            try
            {
                savePath.DeleteSigleFile();

                byte[] fileByte = Convert.FromBase64String(FileBody.Trim().Replace(" ", "+"));

                using (FileStream fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write))
                {
                    fileStream.Write(fileByte, 0, fileByte.Length);
                    fileStream.Close();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GetFileBody()
        {
            if (File.Exists(filePath))
            {
                using (StreamReader str = new StreamReader(filePath))
                { FileBody = str.ReadToEnd(); }
            }
            else { MessageBox.Show("檔案不存在！"); }
        }
    }
}
