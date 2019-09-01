using System;
using System.Collections.Generic;
using System.Text;

namespace ChemGo.Data
{
    /// <summary>
    /// 命令行信息
    /// </summary>
    public struct CommandLineInformation
    {
        public string currentDirectory;                         //当前目录
        public string inputFilePath;                            //输入文件路径
        public string outputFilePath;                           //输出文件路径
        public InputFileType inputFileType;                     //输入文件类型
    }

    /// <summary>
    /// 输入文件信息
    /// </summary>
    public struct InputFile
    {
        public Labels labels;
        public ChargeAndMultiplicity chargeAndMultiplicity;
        public InputCartesian inputCartesian;
        public InputZmatrix inputZmatrix;
    }
    
    public struct OtherProgramData
    {
        public DataGaussian.Data_Gaussian data_Gaussian;
    }
}
