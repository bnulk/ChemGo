using System.Text;
using ChemGo.Data;
using ChemGo.Input.InputFileReadings;

namespace ChemGo.Drive
{
    partial class Drive_0001_ReadInputFile
    {
        public void UpdateChemGoData(ref Data_ChemGo data_ChemGo)
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
