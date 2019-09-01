using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ChemGo.Data;
using ChemGo.Data.DataGaussian;

namespace ChemGo.Input.ReadInputFile
{
    class ReadInputFile
    {
        private CommandLineInformation commandLineInformation;
        private List<string> inputList;


        private Labels labels;
        private InterfaceBetweenGaussianAndChemGo interfaceBetweenGaussianAndChemGo;
        private GaussianInputSegment gaussianInputSegment;

        /// <summary>
        /// 标签集合
        /// </summary>
        public Labels Labels { get => labels; set => labels = value; }
        /// <summary>
        /// ChemGo和Gaussian交互信息
        /// </summary>
        public InterfaceBetweenGaussianAndChemGo InterfaceBetweenGaussianAndChemGo { get => interfaceBetweenGaussianAndChemGo; set => interfaceBetweenGaussianAndChemGo = value; }
        /// <summary>
        /// Gaussian输入的碎片格式
        /// </summary>
        public GaussianInputSegment GaussianInputSegment { get => gaussianInputSegment; set => gaussianInputSegment = value; }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="commandLineInformation"></param>
        public ReadInputFile(CommandLineInformation commandLineInformation)
        {
            this.commandLineInformation = commandLineInformation;
        }


        public void Run()
        {
            //获取文件列
            ObtainInputList();

            //获取大括号内的内容：ChemGo标签；ChemGo和其它程序的交互部分。
            ReadLabels.ReadLabelsApplication readLabelsApplication = new ReadLabels.ReadLabelsApplication(commandLineInformation, inputList);
            readLabelsApplication.Run();
            labels = readLabelsApplication.Labels;
            interfaceBetweenGaussianAndChemGo = readLabelsApplication.InterfaceBetweenGaussianAndChemGo;            


            //根据文件类型，获取内容为：其它程序输入文件的结构对象。
            switch (commandLineInformation.inputFileType)
            {
                case InputFileType.Gaussian:
                    ReadOtherProgramsInputSection.ReadGaussian readGaussian = new ReadOtherProgramsInputSection.ReadGaussian(inputList);
                    readGaussian.Run();
                    GaussianInputSegment = readGaussian.GaussianInputSegment;
                    break;
                default:
                    break;
            }
        }
        

        /// <summary>
        /// 获取输入列表
        /// </summary>
        private void ObtainInputList()
        {
            //打开输入文件，即打开控制文件
            StreamReader inputFile = File.OpenText(commandLineInformation.inputFilePath);
            string str = "";                                //临时用字符串，读一行文本
            inputList = new List<string>();
            while (inputFile.Peek() > -1)
            {
                str = inputFile.ReadLine();
                str = str.Trim();
                inputList.Add(str);
            }
            inputFile.Dispose();
        }
    }
}
