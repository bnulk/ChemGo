using System;
using System.Collections.Generic;
using System.Text;
using ChemGo.Data;

namespace ChemGo.CommandLine.ArgsAnalysis
{
    class ZeroArgsAnalyser
    {
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
        /// 运行零参数分析器。对于零个参数值，不做具体分析。
        /// </summary>
        public void Run()
        {
            isAComputionalJob = false;
            helpOptionType = HelpOptionType.unknown;
            commandLineInformation.currentDirectory = null;
            commandLineInformation.inputFileFullPath = null;
            commandLineInformation.outputFileFullPath = null;
        }
    }
}
