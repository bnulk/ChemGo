using System;
using System.Collections.Generic;
using System.Text;
using ChemGo.Data;
using ChemGo.Input.InputFileReadingTools;

namespace ChemGo.Drive
{
    class Drive_1_ReadInputFile
    {
        private Data.InputFile inputfile;
        private Data.DataGaussian.InterfaceBetweenGaussianAndChemGo interfaceBetweenGaussianAndChemGo;
        private Data.DataGaussian.GaussianInputSegment gaussianInputSegment;


        /// <summary>
        /// 读输入文件内容
        /// </summary>
        public Drive_1_ReadInputFile(CommandLineInformation commandLineInformation)
        {
            try
            {
                InputFileReader inputFileReader = new InputFileReader(commandLineInformation);
                inputFileReader.Run();
                inputfile = inputFileReader.InputFile;
                interfaceBetweenGaussianAndChemGo = inputFileReader.InterfaceBetweenGaussianAndChemGo;
                gaussianInputSegment = inputFileReader.GaussianInputSegment;
            }
            catch (ReadInputFileException e)
            {
                Console.Write(e.Message);
                return;
            }
        }

        /// <summary>
        /// 更新data_ChemGo中的输入数据
        /// </summary>
        /// <param name="data_ChemGo">数据类对象data_ChemGo</param>
        public void UpdateInputData(ref Data_ChemGo data_ChemGo)
        {
            data_ChemGo.inputFile = this.inputfile;

            switch (data_ChemGo.commandLineInformation.inputFileType)
            {
                case InputFileType.ChemGo:
                    break;
                case InputFileType.Gaussian:
                    UpdateDataGaussian(ref data_ChemGo);
                    break;
                case InputFileType.unknown:
                    throw new ReadInputFileException(" InputFileType Error. ChemGo.Drive.Drive_1_ReadInputFile.UpdateInputData(ref Data_ChemGo data_ChemGo) ");
                default:
                    throw new ReadInputFileException(" InputFileType Error. ChemGo.Drive.Drive_1_ReadInputFile.UpdateInputData(ref Data_ChemGo data_ChemGo) ");
            }

        }

        /// <summary>
        /// 更新ChemGo中的Gaussian相关数据
        /// </summary>
        /// <param name="data_ChemGo">数据类对象data_ChemGo</param>
        private void UpdateDataGaussian(ref Data_ChemGo data_ChemGo)
        {
            data_ChemGo.otherProgramData.data_Gaussian.interfaceBetweenGaussianAndChemGo = this.interfaceBetweenGaussianAndChemGo;
            data_ChemGo.otherProgramData.data_Gaussian.gaussianInputSegment = this.gaussianInputSegment;
        }



    }
}
