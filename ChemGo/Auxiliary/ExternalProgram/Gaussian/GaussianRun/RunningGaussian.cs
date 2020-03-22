using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace ChemGo.Auxiliary.ExternalProgram.Gaussian.GaussianRun
{
    class RunningGaussian
    {
        private string cmd;
        private string directoryBeforeCalculation;
        private string directoryOfCalculation;
        private string gjfFullPath;
        private string outFullPath;

        public RunningGaussian(string cmd, string gjfFullPath, string outFullPath)
        {
            this.cmd = cmd;
            this.gjfFullPath = gjfFullPath;
            this.outFullPath = outFullPath;
            directoryBeforeCalculation = Directory.GetCurrentDirectory();
            directoryOfCalculation = Path.GetDirectoryName(outFullPath);
        }

        public void Run()
        {
            //改变当前目录
            Directory.SetCurrentDirectory(directoryOfCalculation);

            //执行高斯计算
            Process RunGaussian = new Process();
            RunGaussian.StartInfo.FileName = cmd;
            RunGaussian.StartInfo.Arguments = gjfFullPath + " " + outFullPath;
            RunGaussian.EnableRaisingEvents = true;
            RunGaussian.Start();
            RunGaussian.WaitForExit();
            RunGaussian.Close();

            //回到原始目录
            Directory.SetCurrentDirectory(directoryBeforeCalculation);
        }
    }
}
