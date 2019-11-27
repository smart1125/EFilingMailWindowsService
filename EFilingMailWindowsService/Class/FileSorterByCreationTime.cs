using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace EFilingMailWindowsService
{
    public class FileSorterByLastWriteTime : IComparer
    {
        public int Compare(object x, object y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            FileInfo xInfo = (FileInfo)x, yInfo = (FileInfo)y;
            return xInfo.LastWriteTime.CompareTo(yInfo.LastWriteTime);
        }
    }
}
