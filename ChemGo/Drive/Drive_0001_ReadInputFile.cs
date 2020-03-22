using System.Text;
using ChemGo.Data;
using ChemGo.Input.InputFileReadings;

namespace ChemGo.Drive
{
    partial class Drive_0001_ReadInputFile
    {
        private Data_ChemGo data_ChemGo;
        private CommandLineInformation commandLineInformation;
        private Output.WriteOutput mainOutput;

        private Data.InputFile inputfile;
        private Data.DataGaussian.InterfaceBetweenGaussianAndChemGo interfaceBetweenGaussianAndChemGo;
        private Data.DataGaussian.GaussianInputSegment gaussianInputSegment;


        /// <summary>
        /// 读输入文件内容
        /// </summary>
        public Drive_0001_ReadInputFile(CommandLineInformation commandLineInformation, Data_ChemGo data_ChemGo, Output.WriteOutput mainOutput)
        {
            this.commandLineInformation = commandLineInformation;
            this.data_ChemGo = data_ChemGo;            
            this.mainOutput = mainOutput;

            StringBuilder result = new StringBuilder();
            result.Append("bnulk@foxmail.com-InputFile" + "\n");
            result.Append("*********************************************" + "\n\n");
            mainOutput.WriteOutputStr(result);
        }

        /// <summary>
        /// 更新data_ChemGo中的输入数据
        /// </summary>
        public void Run()
        {
            try
            {
                //运行应用
                InputFileReadingApplication inputFileReadingApplication = new InputFileReadingApplication(commandLineInformation);
                inputFileReadingApplication.Run();
                //拿到运行应用的数据
                inputfile = inputFileReadingApplication.InputFile;
                interfaceBetweenGaussianAndChemGo = inputFileReadingApplication.InterfaceBetweenGaussianAndChemGo;
                gaussianInputSegment = inputFileReadingApplication.GaussianInputSegment;
            }
            catch (ReadInputFileException e)
            {
                mainOutput.WriteOutputStr(e.Message);
                return;
            }         
        }

    }
}
