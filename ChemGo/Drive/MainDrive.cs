using System;
using System.Collections.Generic;
using System.Text;
using ChemGo.Data;

namespace ChemGo.Drive
{
    class MainDrive
    {
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
            data_ChemGo.mainOutput = new Output.WriteOutput(data_ChemGo.commandLineInformation.outputFileFullPath);

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
            Drive_0000_WriteCommandLineInfoAndWriteTitle drive_0000 = new Drive_0000_WriteCommandLineInfoAndWriteTitle(data_ChemGo.commandLineInformation, data_ChemGo.mainOutput);
            drive_0000.Run();

            //读输入文件
            Drive_0001_ReadInputFile drive_0001 = new Drive_0001_ReadInputFile(data_ChemGo.commandLineInformation, data_ChemGo, data_ChemGo.mainOutput);
            drive_0001.Run();
            drive_0001.UpdateChemGoData(ref data_ChemGo);
            drive_0001.ShowChemGoData(data_ChemGo);

            //处理输入文件坐标
            Drive_0002_DisposeInputCoordinate drive_0002 = new Drive_0002_DisposeInputCoordinate(data_ChemGo.inputFile, data_ChemGo, data_ChemGo.mainOutput);
            drive_0002.Run();
            drive_0002.UpdateChemGoData(ref data_ChemGo);
            drive_0002.ShowChemGoData(data_ChemGo);

            //根据计算任务，驱动计算流程。
            switch(data_ChemGo.inputFile.labels.control.task)
            {
                case Task.mecp:                //极小势能面交叉点任务
                    Drive_0101_Mecp drive_0101 = new Drive_0101_Mecp(data_ChemGo, data_ChemGo.mainOutput);
                    drive_0101.Run();
                    drive_0101.UpdateMecpData(ref data_ChemGo.mecp, ref data_ChemGo.singlePoint);
                    drive_0101.ShowChemGoData(data_ChemGo.mecp);
                    break;
                default:
                    throw new DriveException("Unknown computational tasks.\n  ChemGo.Drive.MainDrive.Run() Error \n");
            }
        }

    }
}
