using System;
using System.Collections.Generic;
using System.Text;
using ChemGo.Data;

namespace ChemGo.CommandLine.ArgsAnalysis
{
    class MultiArgsAnalyser
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

        public MultiArgsAnalyser(string[] args)
        {
            this.args = (string[])args.Clone();
        }

        /// <summary>
        /// 运行多参数分析器
        /// </summary>
        public void Run()
        {
            isAComputionalJob = false;
            helpOptionType = HelpOptionType.unknown;
            commandLineInformation.currentDirectory = null;
            commandLineInformation.inputFileFullPath = null;
            commandLineInformation.outputFileFullPath = null;
            throw new CommandLineException("ChemGo.CommandLine.ArgsAnalysis.MultiArgsAnalyser.Run() Error. Too many args.");
        }
    }
}
