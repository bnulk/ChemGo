using System;
using System.Collections.Generic;
using System.Text;
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
        /// 命令行类型
        /// </summary>
        private CommandLineType commandLineType;

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
        public MainAnalyser(string[] args)
        {
            this.args = (string[])args.Clone();
            isAComputionalJob = false;
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
                    commandLineType = CommandLineType.zeroArgs;
                    ZeroArgsAnalysis();
                    break;
                case 1:
                    commandLineType = CommandLineType.singleArgs;
                    SingleArgsAnalysis(args[0]);
                    break;
                case 2:
                    commandLineType = CommandLineType.doubleArgs;
                    DoubleArgsAnalysis(args);
                    break;
                default:
                    commandLineType = CommandLineType.multiArgs;
                    MultiArgsAnalysis(args);
                    break;
            }
        }

        /// <summary>
        /// 零参数分析
        /// </summary>
        private void ZeroArgsAnalysis()
        {
            ZeroArgsAnalyser zeroValueAnalyser = new ZeroArgsAnalyser();
            zeroValueAnalyser.Run();
            isAComputionalJob = zeroValueAnalyser.IsAComputionalJob;
            helpOptionType = zeroValueAnalyser.HelpOptionType;
        }

        /// <summary>
        /// 单参数分析
        /// </summary>
        private void SingleArgsAnalysis(string arg)
        {
            SingleArgsAnalyser singleArgsAnalyser = new SingleArgsAnalyser(arg);
            singleArgsAnalyser.Run();
            isAComputionalJob = singleArgsAnalyser.IsAComputionalJob;
            if(isAComputionalJob==true)
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
            isAComputionalJob = doubleArgsAnalyser.IsAComputionalJob;
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
            isAComputionalJob = multiArgsAnalyser.IsAComputionalJob;
        }
    }
}
