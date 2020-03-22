using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ChemGo.Data;

namespace ChemGo.Input.InputFileReadings
{
    class InputFileReadingApplication
    {
        private CommandLineInformation commandLineInformation;
        private List<string> inputList;

        #region ChemGo型输入文件
        private InputFile inputFile; 
        /// <summary>
        /// ChemGo标准格式之输入文件
        /// </summary>
        public InputFile InputFile { get => inputFile; set => inputFile = value; }
        #endregion

        #region Gaussian型输入文件
        private Data.DataGaussian.InterfaceBetweenGaussianAndChemGo interfaceBetweenGaussianAndChemGo;
        private Data.DataGaussian.GaussianInputSegment gaussianInputSegment;
        /// <summary>
        /// ChemGo和Gaussian交互信息
        /// </summary>
        public Data.DataGaussian.InterfaceBetweenGaussianAndChemGo InterfaceBetweenGaussianAndChemGo { get => interfaceBetweenGaussianAndChemGo; set => interfaceBetweenGaussianAndChemGo = value; }
        /// <summary>
        /// Gaussian输入的碎片格式
        /// </summary>
        public Data.DataGaussian.GaussianInputSegment GaussianInputSegment { get => gaussianInputSegment; set => gaussianInputSegment = value; }
        #endregion


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="commandLineInformation"></param>
        public InputFileReadingApplication(CommandLineInformation commandLineInformation)
        {
            this.commandLineInformation = commandLineInformation;
        }


        public void Run()
        {
            //获取文件列
            ObtainInputList();

            //把输入文件读入程序
            switch(commandLineInformation.inputFileType)
            {
                case InputFileType.ChemGo:
                    ReadChemGoTypeInputFile();
                    break;
                case InputFileType.Gaussian:
                    ReadGaussianTypeInputFile();
                    break;
                default:
                    throw new ReadInputFileException("InputFileType Error. \n ChemGo.Input.ReadInputFile.ReadInputFile.Run()");
            }
            
        }


        /// <summary>
        /// 获取输入列表
        /// </summary>
        private void ObtainInputList()
        {
            //打开输入文件，即打开控制文件
            StreamReader inputFile = File.OpenText(commandLineInformation.inputFileFullPath);
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

        /// <summary>
        /// 读Gaussian型的输入文件
        /// </summary>
        private void ReadGaussianTypeInputFile()
        {
            GaussianInputFileReadings.MainGaussianInputFileReader mainGaussianInputFileReader = new GaussianInputFileReadings.MainGaussianInputFileReader(inputList);
            mainGaussianInputFileReader.Run();
            this.inputFile = mainGaussianInputFileReader.InputFile;
            this.interfaceBetweenGaussianAndChemGo = mainGaussianInputFileReader.InterfaceBetweenGaussianAndChemGo;
            this.gaussianInputSegment = mainGaussianInputFileReader.GaussianInputSegment;
        }

        /// <summary>
        /// 读ChemGo型的输入文件
        /// </summary>
        private void ReadChemGoTypeInputFile()
        {
            ChemGoInputFileReadings.MainChemGoInputFileReader mainChemGoInputFileReader = new ChemGoInputFileReadings.MainChemGoInputFileReader(inputList);
            mainChemGoInputFileReader.Run();
            this.inputFile = mainChemGoInputFileReader.InputFile;
        }
    }
}
