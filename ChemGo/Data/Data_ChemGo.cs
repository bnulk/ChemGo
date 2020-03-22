using System;
using System.Collections.Generic;
using System.Text;

namespace ChemGo.Data
{
    public class Data_ChemGo
    {
        #region 命令行部分
        /// <summary>
        /// 命令行信息
        /// </summary>
        public CommandLineInformation commandLineInformation;
        /// <summary>
        /// 主输出文件
        /// </summary>
        public Output.WriteOutput mainOutput;
        /// <summary>
        /// 其它程序数据
        /// </summary>
        public OtherProgramData otherProgramData;
        #endregion 命令行部分结束

        /// <summary>
        /// 输入文件信息
        /// </summary>
        public InputFile inputFile;



        /// <summary>
        /// 单点信息
        /// </summary>
        public SinglePoint singlePoint;

        /// <summary>
        /// 极小势能面交叉点信息
        /// </summary>
        public Mecp mecp;

    }
}
