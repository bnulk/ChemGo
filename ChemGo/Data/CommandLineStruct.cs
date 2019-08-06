using System;

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
    }
}