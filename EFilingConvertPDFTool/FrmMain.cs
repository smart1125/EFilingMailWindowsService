using EFilingConvertPDFTool.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EFilingConvertPDFTool
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDlg = new OpenFileDialog();

                openFileDlg.InitialDirectory = Application.StartupPath;
                openFileDlg.Filter = "TXT|*.txt";

                if (openFileDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    PDFFILE pdfFIle = new PDFFILE(openFileDlg.FileName);

                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.Filter = "PDF|*.pdf";

                    saveFileDialog1.Title = "Save File";
                    if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK && saveFileDialog1.FileName.Length > 0)
                    {
                        pdfFIle.ConvertToFile(saveFileDialog1.FileName);
                    }

                    pdfFIle = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {

            }
        }
    }
}
