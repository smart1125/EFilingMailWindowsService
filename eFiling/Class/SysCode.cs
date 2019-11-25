using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eFiling
{
    public enum SysCode
    {
        /// <summary>
        /// 交易成功
        /// </summary>
        A000,
        /// <summary>
        /// 資料處理中
        /// </summary>
        A001,
        /// <summary>
        /// 等待拆檔
        /// </summary>
        A002,
        /// <summary>
        /// 拆檔中
        /// </summary>
        A003,
        /// <summary>
        /// 拆檔完成
        /// </summary>
        A009,
        /// <summary>
        /// 取檔完成
        /// </summary>
        B000,
        /// <summary>
        /// 取檔作業處理中
        /// </summary>
        B001,
        /// <summary>
        /// 檔案或目錄相關發生錯誤
        /// </summary>
        E001,
        /// <summary>
        /// 資料庫發生錯誤
        /// </summary>
        E002,
        /// <summary>
        /// 資料格式錯誤
        /// </summary>
        E003,
        /// <summary>
        /// 參數錯誤
        /// </summary>
        E004,
        /// <summary>
        /// 拆檔發生錯誤
        /// </summary>
        E005,
        /// <summary>
        /// 檔案不存在
        /// </summary>
        E006,
        /// <summary>
        /// 查無資料
        /// </summary>
        E007,
        /// <summary>
        /// 資料重覆
        /// </summary>
        E008,
        /// <summary>
        /// 案件已刪除
        /// </summary>
        E010,
        /// <summary>
        /// 檔案已刪除
        /// </summary>
        E011,
        /// <summary>
        /// TRANS_STATE 不符合規範
        /// </summary>
        E012,
        /// <summary>
        /// 發生不明錯誤
        /// </summary>
        E999
    }
}