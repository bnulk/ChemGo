using System;
using System.Collections.Generic;
using System.Text;
using ChemGo.Data;

namespace ChemGo.Input.InputFileReadingTools.GaussianInputFileReadingTools
{
    class MainGaussianInputFileReader
    {
        private List<string> inputList;

        private InputFile inputFile;
        private Data.DataGaussian.InterfaceBetweenGaussianAndChemGo interfaceBetweenGaussianAndChemGo;
        private Data.DataGaussian.GaussianInputSegment gaussianInputSegment;


        /// <summary>
        /// ChemGo标准格式之输入文件
        /// </summary>
        public InputFile InputFile { get => inputFile; set => inputFile = value; }
        /// <summary>
        /// ChemGo和Gaussian交互信息
        /// </summary>
        public Data.DataGaussian.InterfaceBetweenGaussianAndChemGo InterfaceBetweenGaussianAndChemGo { get => interfaceBetweenGaussianAndChemGo; set => interfaceBetweenGaussianAndChemGo = value; }
        /// <summary>
        /// Gaussian输入的碎片格式
        /// </summary>
        public Data.DataGaussian.GaussianInputSegment GaussianInputSegment { get => gaussianInputSegment; set => gaussianInputSegment = value; }


        public MainGaussianInputFileReader(List<string> inputList)
        {
            this.inputList = inputList;
        }

        public void Run()
        {
            //读输入文件中和ChemGo直接相关的信息（大括号里的内容）
            ChemGoInfoReader chemGoInfoReader = new ChemGoInfoReader(inputList);
            chemGoInfoReader.Run();
            this.interfaceBetweenGaussianAndChemGo = chemGoInfoReader.InterfaceBetweenGaussianAndChemGo;
            this.inputFile.labels = chemGoInfoReader.Labels;
            //读输入文件在的Gaussian部分
            GaussianInfoReader gaussianInfoReader = new GaussianInfoReader(inputList);
            gaussianInfoReader.Run();
            this.gaussianInputSegment = gaussianInfoReader.GaussianInputSegment;
        }

    }
}
