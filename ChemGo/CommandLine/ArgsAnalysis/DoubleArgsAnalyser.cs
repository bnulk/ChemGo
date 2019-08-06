using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ChemGo.Data;

namespace ChemGo.CommandLine.ArgsAnalysis
{
    class DoubleArgsAnalyser
    {
        /// <summary>
        /// 第一个参数
        /// </summary>
        private string arg0;
        /// <summary>
        /// 第二个参数
        /// </summary>
        private string arg1;

        /// <summary>
        /// 是否为一个计算工作
        /// </summary>
        private bool isAComputionalJob;
        /// <summary>
        /// 是否为一个计算工作
        /// </summary>
        public bool IsAComputionalJob { get => isAComputionalJob; }

        /// <summary>
        /// 帮助信息的类型
        /// </summary>
        private HelpOptionType helpOptionType;
        /// <summary>
        /// 帮助信息的类型
        /// </summary>
        public HelpOptionType HelpOptionType { get => helpOptionType; set => helpOptionType = value; }

        /// <summary>
        /// 命令行信息
        /// </summary>
        private CommandLineInformation commandLineInformation;
        /// <summary>
        /// 命令行信息
        /// </summary>
        public CommandLineInformation CommandLineInformation { get => commandLineInformation; set => commandLineInformation = value; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="args"></param>
        public DoubleArgsAnalyser(string[] args)
        {
            arg0 = args[0];
            arg1 = args[1];
            isAComputionalJob = true;
        }

        /// <summary>
        /// 运行双参数分析器
        /// </summary>
        public void Run()
        {
            isAComputionalJob = true;
            helpOptionType = HelpOptionType.noPrint;
            ObtainCurrentDirectory();
            ObtainInputFileFullName();
            ObtainOutputFileFullName();
        }

        /// <summary>
        /// 获取当前目录
        /// </summary>
        private void ObtainCurrentDirectory()
        {
            commandLineInformation.currentDirectory = Directory.GetCurrentDirectory();
            return;
        }

        /// <summary>
        /// 获取输入文件的绝对路径
        /// </summary>
        private void ObtainInputFileFullName()
        {
            if (File.Exists(arg0))
            {
                if (Path.IsPathRooted(arg0))
                {
                    commandLineInformation.inputFilePath = arg0;
                }
                else
                {
                    commandLineInformation.inputFilePath = Path.Combine(commandLineInformation.currentDirectory, arg0);
                }
            }
            else
            {
                throw new CommandLineException("ChemGo.CommandLine.Analyser.DoubleArgsAnalyser.ObtainInputFileFullPath() Error, Input file does not exist.");
            }
        }

        /// <summary>
        /// 获取输出文件的绝对路径
        /// </summary>
        private void ObtainOutputFileFullName()
        {
            if(IsValidFilePath(arg1))
            {
                if(Path.GetExtension(arg1).ToLower()!="kun")
                {
                    throw new CommandLineException("ChemGo.CommandLine.Analyser.DoubleArgsAnalyser.ObtainOutputFileFullName() Error, The extension of the output file must be \" kun \"");
                }
            }
            else
            {
                throw new CommandLineException("ChemGo.CommandLine.Analyser.DoubleArgsAnalyser.ObtainOutputFileFullName() Error, Invalid Output File Path.");
            }

            if (Path.IsPathRooted(arg1))
            {
                commandLineInformation.outputFilePath = arg1;
            }
            else
            {
                commandLineInformation.outputFilePath = Path.Combine(commandLineInformation.currentDirectory, arg1);
            }
        }

        /// <summary>
        /// 文件路径是否合法
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private bool IsValidFilePath(string filePath)
        {
            bool isValidFileName = false;
            if (filePath.IndexOfAny(System.IO.Path.GetInvalidPathChars()) >= 0)
            {
                isValidFileName = false;
            }
            else
            {
                isValidFileName = true;
            }
            return isValidFileName;
        }

    }
}
