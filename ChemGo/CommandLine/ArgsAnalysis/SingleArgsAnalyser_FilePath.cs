using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ChemGo.CommandLine.ArgsAnalysis
{
    class SingleArgsAnalyser_FilePath
    {
        /// <summary>
        /// 命令行参数
        /// </summary>
        private string arg;

        private string currentDirectory;
        private string inputFileDirectory;
        private string inputFileFullPath;
        private string outputFileFullPath;

        /// <summary>
        /// 当前目录
        /// </summary>
        public string CurrentDirectory { get => currentDirectory; set => currentDirectory = value; }
        /// <summary>
        /// 输入文件绝对路径
        /// </summary>
        public string InputFileFullPath { get => inputFileFullPath; set => inputFileFullPath = value; }
        /// <summary>
        /// 输出文件绝对路径
        /// </summary>
        public string OutputFileFullPath { get => outputFileFullPath; set => outputFileFullPath = value; }
        //输入文件所在目录
        public string InputFileDirectory { get => inputFileDirectory; set => inputFileDirectory = value; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="arg"></param>
        public SingleArgsAnalyser_FilePath(string arg)
        {
            this.arg = arg;
        }

        public void Run()
        {
            ObtainCurrentDirectory();
            ObtainInputFileFullPath();
            ObtainOutputFileFullPath();
        }

        /// <summary>
        /// 获取当前目录
        /// </summary>
        private void ObtainCurrentDirectory()
        {
            currentDirectory = Directory.GetCurrentDirectory();
            return;
        }

        /// <summary>
        /// 获取输入文件的绝对路径
        /// </summary>
        private void ObtainInputFileFullPath()
        {
            if(File.Exists(arg))
            {
                if (Path.IsPathRooted(arg))
                {
                    inputFileFullPath = arg;
                }
                else
                {
                    inputFileFullPath = Path.Combine(currentDirectory, arg);
                }
                //获取输入文件所在的目录
                InputFileDirectory = Path.GetDirectoryName(inputFileFullPath);
            }
            else
            {
                throw new CommandLineException("ChemGo.CommandLine.Analyser.SingleArgsAnalyser_FilePath.ObtainInputFileFullPath() Error, Input file does not exist.");
            }            
            return;
        }

        /// <summary>
        /// 获取输出文件的绝对路径
        /// </summary>
        private void ObtainOutputFileFullPath()
        {
            outputFileFullPath = Path.ChangeExtension(inputFileFullPath, "kun");
        }
    }
}
