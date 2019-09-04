using System;
using System.Collections.Generic;
using System.Text;
using ChemGo.Data;

namespace ChemGo.Drive
{
    class MainDrive
    {
        /// <summary>
        /// 主要的输出文件
        /// </summary>
        private Output.WriteOutput mainOutput;
        /// <summary>
        /// ChemGo数据
        /// </summary>
        private Data_ChemGo data_ChemGo = new Data_ChemGo();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="commandLineInformation">命令行信息</param>
        public MainDrive(CommandLineInformation commandLineInformation)
        {
            //填充命令行信息到data_ChemGo中
            data_ChemGo.commandLineInformation = commandLineInformation;

            //定义输出文件
            mainOutput = new Output.WriteOutput(data_ChemGo.commandLineInformation.outputFilePath);

            //初始化关联程序数据
            switch(commandLineInformation.inputFileType)
            {
                case InputFileType.ChemGo:
                    break;
                case InputFileType.Gaussian:
                    data_ChemGo.otherProgramData.data_Gaussian = new Data.DataGaussian.Data_Gaussian();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 运行驱动
        /// </summary>
        public void Run()
        {            
            //输出标题和命令行信息
            Drive_0000_WriteTitleAndCommandLineInfo.WriteTitle(mainOutput);
            Drive_0000_WriteTitleAndCommandLineInfo.WriteCmdLine(mainOutput, data_ChemGo.commandLineInformation);

            //读输入文件
            Drive_0001_ReadInputFile drive_0001 = new Drive_0001_ReadInputFile(data_ChemGo.commandLineInformation, mainOutput);
            drive_0001.UpdateInputData(ref data_ChemGo);
        }

    }
}
