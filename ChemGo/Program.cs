using System;
using ChemGo.CommandLine;
using ChemGo.Data;

namespace ChemGo
{
    class Program
    {
        /// <summary>
        /// 程序入口
        /// </summary>
        /// <param name="args">命令行参数</param>
        static void Main(string[] args)
        {
            CommandLineInformation commandLineInformation;
            try
            {
                CommandLineApplication app = new CommandLineApplication(args);
                app.Run();
                if (app.IsAComputationalJob == false)                                               //打印帮助信息
                {
                    CommandLine.ArgsDisposal.HelpOptionDisposer helpOptionDisposer = new CommandLine.ArgsDisposal.HelpOptionDisposer(app.HelpOptionType);
                    helpOptionDisposer.Run();
                    return;
                }
                else
                {
                    commandLineInformation = app.CommandLineInformation;                            //输出命令行信息
                }
            }
            catch (CommandLineException e)
            {
                Console.Write(e.Message);
                return;
            }

            //命令行信息驱动后续程序
            Drive.MainDrive mainDrive = new Drive.MainDrive(commandLineInformation);
            mainDrive.Run();

        }
    }
}

