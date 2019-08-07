using System;
using ChemGo.Data;

namespace ChemGo.CommandLine
{
    class CommandLineApplication
    {
        /// <summary>
        /// 命令行参数
        /// </summary>        
        private readonly string[] args;

        /// <summary>
        /// 是否为一个计算工作
        /// </summary>
        private bool isAComputationalJob = false;
        // <summary>
        /// 是否为一个计算工作
        /// </summary>
        public bool IsAComputationalJob { get => isAComputationalJob; set => isAComputationalJob = value; }

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
        /// <param name="args">命令行参数</param>
        public CommandLineApplication(string[] args)
        {
            this.args = (string[])args.Clone();
        }
        
        /// <summary>
        /// 处理命令行参数
        /// </summary>
        public void Run()
        {
            ArgsAnalysis.MainAnalyser mainAnalyser = new ArgsAnalysis.MainAnalyser(args);
            mainAnalyser.Run();
            isAComputationalJob = mainAnalyser.IsAComputionalJob;
            helpOptionType = mainAnalyser.HelpOptionType;
            commandLineInformation = mainAnalyser.CommandLineInformation;
        }
    }    
}
