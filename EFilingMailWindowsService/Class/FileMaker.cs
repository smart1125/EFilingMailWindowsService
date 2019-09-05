using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFilingMailWindowsService.Class
{
   public class FileMaker
    {
        public DateTime CREATE_DATETIME { get; set; }
        public string SESSION_KE { get; set; }
        public DateTime TXN_DATE { get; set; }
        public string TXN_ACCOUNT { get; set; }
        public string BRANCH_ID { get; set; }
        public string FILE_ID { get; set; }
        public DateTime FILE_CREATE_DATETIME { get; set; }
        public Int16 FILE_SEQ { get; set; }
        public string FILE_ROOT { get; set; }
        public string FILE_PATH { get; set; }
        public string PDF { get; set; }
        public Int16 FILE_TYPE { get; set; }
        /// <summary>
        /// CIF_ID
        /// </summary>
        public string T_MNEMONIC
        {
            get;
            set;
        }

        public string T_EMAIL_SIGN_1 { get; set; }
        public string T_EMAIL_1 { get; set; }
    }
}
