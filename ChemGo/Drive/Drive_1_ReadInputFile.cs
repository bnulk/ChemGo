using System;
using System.Collections.Generic;
using System.Text;
using ChemGo.Data;
using ChemGo.Input.ReadInputFile;

namespace ChemGo.Drive
{
    class Drive_1_ReadInputFile
    {
        private Data.Labels labels;
        private Data.DataGaussian.InterfaceBetweenGaussianAndChemGo interfaceBetweenGaussianAndChemGo;
        private Data.DataGaussian.GaussianInputSegment gaussianInputSegment;


        /// <summary>
        /// 读输入文件内容
        /// </summary>
        public Drive_1_ReadInputFile(CommandLineInformation commandLineInformation)
        {
            try
            {
                ReadInputFile readInputFile = new ReadInputFile(commandLineInformation);
                readInputFile.Run();
                labels = readInputFile.Labels;
                interfaceBetweenGaussianAndChemGo = readInputFile.InterfaceBetweenGaussianAndChemGo;
                gaussianInputSegment = readInputFile.GaussianInputSegment;
            }
            catch (ReadInputFileException e)
            {
                Console.Write(e.Message);
                return;
            }
        }

        public void UpdateToChemGo(ref Data_ChemGo data_ChemGo)
        {
            data_ChemGo.labels = this.labels;
        }

        public void UpdateToGaussian(ref Data.DataGaussian.Data_Gaussian data_Gaussian)
        {
            data_Gaussian.interfaceBetweenGaussianAndChemGo = this.interfaceBetweenGaussianAndChemGo;
            data_Gaussian.gaussianInputSegment = this.gaussianInputSegment;
        }



    }
}
