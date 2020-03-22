using System;
using System.Collections.Generic;
using System.Text;

namespace ChemGo.Auxiliary.OperatingSystem
{
    class OS_BasisFunction
    {
        //全局变量
        public static string OS_Name = "linux";        //操作系统类别

        public OS_BasisFunction()
        {
            OS_Name = ObtianOSName();
        }

        /// <summary>
        /// 获取操作系统信息。OS_Name是"linux",或者"windows".
        /// </summary>
        public static string ObtianOSName()
        {
            string tmpOsInfo = "linux";
            string str;
            int indexMark;
            str = Environment.OSVersion.VersionString.ToLower();
            indexMark = str.IndexOf("win");
            if (indexMark != -1)
            {
                tmpOsInfo = "windows";
            }
            return tmpOsInfo;
        }
    }
}
