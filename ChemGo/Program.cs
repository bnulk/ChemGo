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
                if (app.IsAComputationalJob == false)
                {
                    return;
                }
                else
                {
                    commandLineInformation = app.CommandLineInformation;
                }
            }
            catch (CommandLineException e)
            {
                Console.Write(e.Message);
                return;
            }

            Drive.MainDrive mainDrive = new Drive.MainDrive(commandLineInformation);
            mainDrive.Run();

        }
    }
}

