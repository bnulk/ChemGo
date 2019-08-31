using System;
using System.Collections.Generic;
using System.Text;
using ChemGo.Data;
using ChemGo.Data.DataGaussian;

namespace ChemGo.Input.ReadInputFile.ReadLabels
{
    class ReadLabelsApplication
    {
        private CommandLineInformation commandLineInformation;
        private List<string> inputList;

        private Labels labels;
        private Data.DataGaussian.InterfaceBetweenGaussianAndChemGo interfaceBetweenGaussianAndChemGo;
                
        /// <summary>
        /// ChemGo的标签
        /// </summary>
        public Labels Labels { get => labels; set => labels = value; }

        /// <summary>
        /// Gaussian和ChemGo交互信息
        /// </summary>
        public InterfaceBetweenGaussianAndChemGo InterfaceBetweenGaussianAndChemGo { get => interfaceBetweenGaussianAndChemGo; set => interfaceBetweenGaussianAndChemGo = value; }


        public ReadLabelsApplication(CommandLineInformation commandLineInformation, List<string> inputList)
        {
            this.commandLineInformation = commandLineInformation;
            this.inputList = inputList;
        }

        

        public void Run()
        {
            switch(commandLineInformation.inputFileType)
            {
                case InputFileType.Gaussian:
                    ReadLabels_Gaussian readLabels_Gaussian = new ReadLabels_Gaussian(inputList);
                    readLabels_Gaussian.Run();
                    break;
                case InputFileType.unknown:
                    break;
                default:
                    break;
            }
        }
    }
}
