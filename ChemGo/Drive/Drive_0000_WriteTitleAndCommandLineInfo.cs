using System;
using System.Collections.Generic;
using System.Text;

namespace ChemGo.Drive
{
    static class Drive_0000_WriteTitleAndCommandLineInfo
    {
        public static void WriteTitle(Output.WriteOutput kunOutput)
        {
            StringBuilder m_Result = new StringBuilder();
            //程序来源
            m_Result.Append("PROGRAM ChemGo, Version " + Data.ProgramData.VERSION.ToString() + "\n" + "Liu Kun  2019-08-20" + "\n" + "\n");
            m_Result.Append(DateTime.Now.ToString() + "\n");
            m_Result.Append("*********************************************" + "\n");
            m_Result.Append("Author Information: Liu Kun, College of Chemistry, Tianjin Normal University, Tianjin 300387, China" + "\n");
            m_Result.Append("Email: bnulk@foxmail.com" + "\n");
            m_Result.Append("You can obtain the newest version of the program by contacting the author." + "\n");
            m_Result.Append("Program code can be downloaded from https://github.com/bnulk" + "\n");
            m_Result.Append("*********************************************" + "\n");

            m_Result.Append("" + "\n");
            m_Result.Append("" + "\n");
            m_Result.Append("" + "\n");
            m_Result.Append("" + "\n");
            m_Result.Append("　　　　　　　　　1s　　　　　　　　 " + "\n");
            m_Result.Append("　　　　　　　5,　1s　:r　　　　　　 " + "\n");
            m_Result.Append("　　　　　　　 9i 1s i9　　　　　　　" + "\n");
            m_Result.Append("　　　　　　　　9h1s59　　　　　　　 " + "\n");
            m_Result.Append("　　　　　　　　 9999　　　　　　　　" + "\n");
            m_Result.Append("　　　　　　　　　99　　　　　　　　 " + "\n");
            m_Result.Append("　　　　　　　　　1s　　　　　　　　 " + "\n");
            m_Result.Append("　　　　　　　　　h1　　　　　　　　 " + "\n");
            m_Result.Append("　　　　;99999　 9993　 99999,　　　 " + "\n");
            m_Result.Append("　　　　　 9　　391s93　 rS　　　　　" + "\n");
            m_Result.Append("　　　　　 9　 99 1s 99　rS　　　　　" + "\n");
            m_Result.Append("　　　　　 9　9S　1s　S9 rS　　　　　" + "\n");
            m_Result.Append("　　　　　 9　　　1s　　 rS　　　　　" + "\n");
            m_Result.Append("　　　　　 99;　　　　　i9S　　　　　" + "\n");
            m_Result.Append("　　　　　 9 99,　　　i99sS　　　　　" + "\n");
            m_Result.Append("　　　　　 9　 99　 i99　S1　　　　　" + "\n");
            m_Result.Append("　　　　　 5S　　9999　　9　　　　　 " + "\n");
            m_Result.Append("　　　　9　 9　 r9999.　i9　 9　　　 " + "\n");
            m_Result.Append("　　　 9i　 r9i99　　99 9　　;9　　　" + "\n");
            m_Result.Append("　　　h5　　 59　　　 s9,　　 55　　 " + "\n");
            m_Result.Append("　　　9:　99: ,995::999　.99　.9　　 " + "\n");
            m_Result.Append("　　　9s99:　　　h99;　　　:99i9　　 " + "\n");
            m_Result.Append("　　　.93　　　　　　　　　　S9,　　 " + "\n");
            m_Result.Append("　　　999: r9　　　　　　9r ,999　　 " + "\n");
            m_Result.Append("　　 35　999　　　　　　　399　59　　" + "\n");
            m_Result.Append("　　;9　　　　　　　　　　　　　9;　 " + "\n");
            m_Result.Append("　　9.　　　　　　　　　　　　　.9　 " + "\n");
            m_Result.Append("　　9　　　　　　　　　　　　　　9　 " + "\n");
            m_Result.Append("　 .S　　　　　　　　　　　　　　S.　" + "\n");
            m_Result.Append("　 s1　　　　　　　　　　　　　　11　" + "\n");
            m_Result.Append("" + "\n");
            m_Result.Append("" + "\n");
            m_Result.Append("" + "\n");
            m_Result.Append("" + "\n");

            kunOutput.m_Result = m_Result;
            kunOutput.WriteOutput_continue();
        }

        /// <summary>
        /// 文本输出“命令行参数”
        /// </summary>
        /// <param name="cmdData">命令行参数</param>
        public static void WriteCmdLine(Output.WriteOutput kunOutput, Data.CommandLineInformation commandLineInformation)
        {
            StringBuilder m_Result = new StringBuilder();
            //输入的命令行标志
            m_Result.Append("\n");
            m_Result.Append("bnulk@foxmail.com-CommandLineInformation" + "\n");
            m_Result.Append("*********************************************" + "\n\n");
            //输入的命令行
            m_Result.Append("currentOS: " + Environment.OSVersion.ToString() + "\n");
            m_Result.Append("currentDirectory: " + commandLineInformation.currentDirectory.ToString() + "\n");
            m_Result.Append("inputName: " + commandLineInformation.inputFilePath.ToString() + "\n");
            m_Result.Append("outputName: " + commandLineInformation.outputFilePath.ToString() + "\n");

            kunOutput.m_Result = m_Result;
            kunOutput.WriteOutput_continue();
        }
    }
}
