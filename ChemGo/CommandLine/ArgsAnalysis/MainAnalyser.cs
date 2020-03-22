using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ChemGo.Data;

namespace ChemGo.CommandLine.ArgsAnalysis
{
    class MainAnalyser
    {
        /// <summary>
        /// 命令行参数
        /// </summary>
        private string[] args;

        /// <summary>
        /// 是否为一个计算工作
        /// </summary>
        private bool isAComputionalJob;
        /// <summary>
        /// 是否为一个计算工作
        /// </summary>
        public bool IsAComputionalJob { get => isAComputionalJob; set => isAComputionalJob = value; }

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
        public MainAnalyser(string[] args)
        {
            this.args = (string[])args.Clone();
            IsAComputionalJob = false;
        }

        /// <summary>
        /// 运行参数分析器
        /// </summary>
        public void Run()
        {
            int numberOfArgsValue = args.Length;
            switch (numberOfArgsValue)
            {
                case 0:
                    ZeroArgsAnalysis();
                    break;
                case 1:
                    SingleArgsAnalysis(args[0]);
                    break;
                case 2:
                    DoubleArgsAnalysis(args);
                    break;
                default:
                    MultiArgsAnalysis(args);
                    break;
            }
            ObtainInputFileType(commandLineInformation.inputFileFullPath);
        }

        /// <summary>
        /// 零参数分析
        /// </summary>
        private void ZeroArgsAnalysis()
        {
            ZeroArgsAnalyser zeroValueAnalyser = new ZeroArgsAnalyser();
            zeroValueAnalyser.Run();
            IsAComputionalJob = zeroValueAnalyser.IsAComputionalJob;
            helpOptionType = zeroValueAnalyser.HelpOptionType;
        }

        /// <summary>
        /// 单参数分析
        /// </summary>
        private void SingleArgsAnalysis(string arg)
        {
            SingleArgsAnalyser singleArgsAnalyser = new SingleArgsAnalyser(arg);
            singleArgsAnalyser.Run();
            IsAComputionalJob = singleArgsAnalyser.IsAComputionalJob;
            if(IsAComputionalJob==true)
            {
                commandLineInformation = singleArgsAnalyser.CommandLineInformation;
            }
            else
            {
                helpOptionType = singleArgsAnalyser.HelpOptionType;
            }
        }

        /// <summary>
        /// 双参数分析
        /// </summary>
        /// <param name="args">参数</param>
        private void DoubleArgsAnalysis(string[] args)
        {
            DoubleArgsAnalyser doubleArgsAnalyser = new DoubleArgsAnalyser(args);
            doubleArgsAnalyser.Run();
            IsAComputionalJob = doubleArgsAnalyser.IsAComputionalJob;
            commandLineInformation = doubleArgsAnalyser.CommandLineInformation;
        }

        /// <summary>
        /// 多参数分析
        /// </summary>
        /// <param name="args">参数</param>
        private void MultiArgsAnalysis(string[] args)
        {
            MultiArgsAnalyser multiArgsAnalyser = new MultiArgsAnalyser(args);
            multiArgsAnalyser.Run();
            IsAComputionalJob = multiArgsAnalyser.IsAComputionalJob;
        }

        /// <summary>
        /// 根据扩展名，获取文件类型
        /// </summary>
        /// <returns>文件类型</returns>
        private void ObtainInputFileType(string inputFilePath)
        {
            string extension = Path.GetExtension(inputFilePath);
            switch (extension.ToLower())
            {
                case ".liu":
                    commandLineInformation.inputFileType = InputFileType.ChemGo;
                    break;
                case ".gjf":
                    commandLineInformation.inputFileType = InputFileType.Gaussian;
                    break;
                default:
                    commandLineInformation.inputFileType = InputFileType.unknown;
                    break;
            }
        }
    }
}
