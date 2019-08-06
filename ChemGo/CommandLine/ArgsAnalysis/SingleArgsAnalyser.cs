using System;
using System.Collections.Generic;
using System.Text;
using ChemGo.Data;

namespace ChemGo.CommandLine.ArgsAnalysis
{
    class SingleArgsAnalyser
    {
        /// <summary>
        /// 命令行参数
        /// </summary>
        private string arg;


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
        /// <param name="arg">命令行参数</param>
        public SingleArgsAnalyser(string arg)
        {
            this.arg = arg;
        }

        /// <summary>
        /// 原型单参数分析器
        /// </summary>
        public void Run()
        {
            try
            {
                JudgeIsAComputationalJob();
                if(isAComputionalJob==false)
                {
                    commandLineInformation.currentDirectory = null;
                    commandLineInformation.inputFilePath = null;
                    commandLineInformation.outputFilePath = null;
                    SingleArgsAnalyser_HelpOption singleArgsAnalyser_HelpOption = new SingleArgsAnalyser_HelpOption(arg);
                    singleArgsAnalyser_HelpOption.Run();
                    helpOptionType = singleArgsAnalyser_HelpOption.HelpOptionType;                    
                }
                else
                {
                    helpOptionType = HelpOptionType.noPrint;
                    SingleArgsAnalyser_FilePath singleArgsAnalyser_FilePath = new SingleArgsAnalyser_FilePath(arg);
                    singleArgsAnalyser_FilePath.Run();                    
                    commandLineInformation.currentDirectory = singleArgsAnalyser_FilePath.CurrentDirectory;
                    commandLineInformation.inputFilePath = singleArgsAnalyser_FilePath.InputFileFullPath;
                    commandLineInformation.outputFilePath = singleArgsAnalyser_FilePath.OutputFileFullPath;
                }
            }
            catch(CommandLineException e)
            {
                Console.Write(e.Message);
            }

        }

        /// <summary>
        /// 如果是帮助信息，isAComputionalJob为false；如果不是帮助信息，isAComputionalJob为true
        /// </summary>
        private void JudgeIsAComputationalJob()
        {
            if (arg.StartsWith("-"))
            {
                isAComputionalJob = false;
            }
            else
            {
                isAComputionalJob = true;
            }            
        }

        
    }
}
