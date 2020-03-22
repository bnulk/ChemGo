using System.Text;
using ChemGo.Data;

namespace ChemGo.Drive
{
    partial class Drive_0001_ReadInputFile
    {
        /// <summary>
        /// 展示最终结果
        /// </summary>
        public void ShowChemGoData(Data_ChemGo data_ChemGo)
        {
            ShowControlInfo();

            switch(data_ChemGo.inputFile.labels.control.inputFileType)
            {
                case InputFileType.ChemGo:
                    ShowChemGoInfo();
                    break;
                case InputFileType.Gaussian:
                    ShowInterfaceBetweenGaussianAndChemGoInfo();
                    ShowGaussianInfo();
                    break;
                default:
                    throw new DriveException("unknown InputFileType. \n @ChemGo.Drive.Drive_0001_ReadInputFile;  ShowChemGoData(Data_ChemGo data_ChemGo)");
            }
        }

        /// <summary>
        /// 显示控制部分信息
        /// </summary>
        private void ShowControlInfo()
        {
            StringBuilder result = new StringBuilder();

            result.Append("bnulk@foxmail.com-InputFile Summary" + "\n");
            result.Append("*********************************************" + "\n\n");

            result.Append("Control Section:" + "\n");
            result.Append("     " + "The Input File Type is:  " + data_ChemGo.inputFile.labels.control.inputFileType.ToString() + "\n");
            result.Append("     " + "Charge and Spin Multiplicity:  " + "\n");
            for (int i = 0; i < data_ChemGo.inputFile.chargeAndMultiplicity.numberOfPart; i++)
            {
                result.Append("     " + "     " + data_ChemGo.inputFile.chargeAndMultiplicity.charge[i].ToString() + "  "
                + data_ChemGo.inputFile.chargeAndMultiplicity.multiplicity[i].ToString() + "\n");
            }
            result.Append("     " + "Input Coordinate Type:  " + data_ChemGo.inputFile.labels.control.coordinateType.ToString() + "\n");
            result.Append("     " + "Task:  " + data_ChemGo.inputFile.labels.control.task.ToString() + "\n");

            switch (data_ChemGo.inputFile.labels.control.task)
            {
                case Task.mecp:
                    result.Append("MECP Keyword:" + "\n");
                    result.Append("     " + MecpInfo());
                    break;
                default:
                    result.Append("Single Point Keyword:" + "\n");
                    result.Append("     " + SpInfo());
                    break;
            }

            mainOutput.WriteOutputStr(result);
        }

        /// <summary>
        /// MECP的信息
        /// </summary>
        /// <returns></returns>
        private string MecpInfo()
        {
            string mecpInfo = "CoordinateType=" + data_ChemGo.inputFile.labels.keyword_mecp.coordinateType.ToString() + "  "
                + "guessHessian=" + data_ChemGo.inputFile.labels.keyword_mecp.guessHessian.ToString() + "  "
                + "hessianN=" + data_ChemGo.inputFile.labels.keyword_mecp.hessianN.ToString() + "  "
                + "isReadFirst=" + data_ChemGo.inputFile.labels.keyword_mecp.isReadFirst.ToString() + "  "
                + "judgement=" + data_ChemGo.inputFile.labels.keyword_mecp.judgement.ToString() + "  " + "\n" + "     "
                + "lambda=" + data_ChemGo.inputFile.labels.keyword_mecp.lambda.ToString() + "  "
                + "maxCon=" + data_ChemGo.inputFile.labels.keyword_mecp.maxCon.ToString() + "  "
                + "rmsCon=" + data_ChemGo.inputFile.labels.keyword_mecp.rmsCon.ToString() + "  "
                + "maxCyc=" + data_ChemGo.inputFile.labels.keyword_mecp.maxCyc.ToString() + "  "
                + "mecpFreq=" + data_ChemGo.inputFile.labels.keyword_mecp.mecpFreq.ToString() + "  "
                + "opt=" + data_ChemGo.inputFile.labels.keyword_mecp.opt.ToString() + "  " + "\n" + "     "
                + "scfTyp1=" + data_ChemGo.inputFile.labels.keyword_mecp.scfTyp1.ToString() + "  "
                + "scfTyp2=" + data_ChemGo.inputFile.labels.keyword_mecp.scfTyp2.ToString() + "  "
                + "stepSize=" + data_ChemGo.inputFile.labels.keyword_mecp.stepSize.ToString() + "\n";
            return mecpInfo;
        }

        /// <summary>
        /// 单点信息
        /// </summary>
        /// <returns></returns>
        private string SpInfo()
        {
            string spInfo = "";
            return spInfo;
        }

        private void ShowInterfaceBetweenGaussianAndChemGoInfo()
        {
            StringBuilder result = new StringBuilder();

            result.Append("Interface between Gaussian and ChemGo Info:" + "\n");
            result.Append("     " + "Command is:  " + data_ChemGo.otherProgramData.data_Gaussian.interfaceBetweenGaussianAndChemGo.cmd + "\n");
            result.Append("     " + "Task is:  " + data_ChemGo.otherProgramData.data_Gaussian.interfaceBetweenGaussianAndChemGo.task + "\n");

            mainOutput.WriteOutputStr(result);

        }

        /// <summary>
        /// 显示Gaussian类型输入文件的信息
        /// </summary>
        private void ShowGaussianInfo()
        {
            StringBuilder result = new StringBuilder();
            int i;

            result.Append("Gaussian Info:" + "\n");
            result.Append("     " + "Coordinate Type is:  " + data_ChemGo.otherProgramData.data_Gaussian.gaussianInputSegment.coordinateType.ToString() + "\n");
            result.Append("     " + "Number Of Segment is:  " + data_ChemGo.otherProgramData.data_Gaussian.gaussianInputSegment.N.ToString() + "\n");

            result.Append("Segment Info:" + "\n");
            //第一部分%Section
            result.Append("     " + "-----FirstSection(%Section)-----" + "\n");
            for (i=0;i<data_ChemGo.otherProgramData.data_Gaussian.gaussianInputSegment.firstSection.Count;i++)
            {
                result.Append("     " + data_ChemGo.otherProgramData.data_Gaussian.gaussianInputSegment.firstSection[i] + "\n");
            }
            //第二部分RouteSection
            result.Append("     " + "-----SecondSection(Route Section)-----" + "\n");
            for (i = 0; i < data_ChemGo.otherProgramData.data_Gaussian.gaussianInputSegment.routeSection.Count; i++)
            {
                result.Append("     " + data_ChemGo.otherProgramData.data_Gaussian.gaussianInputSegment.routeSection[i] + "\n");
            }
            //第三部分Title
            result.Append("     " + "-----ThirdSection(Title Section)-----" + "\n");
            for (i = 0; i < data_ChemGo.otherProgramData.data_Gaussian.gaussianInputSegment.titleSection.Count; i++)
            {
                result.Append("     " + data_ChemGo.otherProgramData.data_Gaussian.gaussianInputSegment.titleSection[i] + "\n");
            }
            //第四部分Charge and Multiplicity
            result.Append("     " + "-----FourthSection(Charge and Multiplicity)-----" + "\n");
            result.Append("     " + data_ChemGo.otherProgramData.data_Gaussian.gaussianInputSegment.chargeAndMultiplicity[i] + "\n");
            //第五部分Molecular Specification
            switch (data_ChemGo.otherProgramData.data_Gaussian.gaussianInputSegment.coordinateType.ToLower())
            {
                case "cartesian":
                    result.Append("     " + "-----FifthSection(Molecular Specification Section)-----" + "\n");
                    for (i = 0; i < data_ChemGo.otherProgramData.data_Gaussian.gaussianInputSegment.molecularCartesian.Count; i++)
                    {
                        result.Append("     " + data_ChemGo.otherProgramData.data_Gaussian.gaussianInputSegment.molecularCartesian[i] + "\n");
                    }
                    break;
                case "zmatrix":
                    result.Append("     " + "-----FifthSection(Molecular Specification Section)-----" + "\n");
                    for (i = 0; i < data_ChemGo.otherProgramData.data_Gaussian.gaussianInputSegment.molecularSpecification_ZMatrix.Count; i++)
                    {
                        result.Append("     " + data_ChemGo.otherProgramData.data_Gaussian.gaussianInputSegment.molecularSpecification_ZMatrix[i] + "\n");
                    }
                    result.Append("\n");
                    for (i = 0; i < data_ChemGo.otherProgramData.data_Gaussian.gaussianInputSegment.molecularPara_ZMatrix.Count; i++)
                    {
                        result.Append("     " + data_ChemGo.otherProgramData.data_Gaussian.gaussianInputSegment.molecularPara_ZMatrix[i] + "\n");
                    }
                    break;
                default:
                    throw new DriveException("unknown InputFileType. \n @ChemGo.Drive.Drive_0001_ReadInputFile;  ShowGaussianInfo():site1 CoordinateType Error");
            }
            //第六部分Addition
            result.Append("     " + "-----SixthSection(Addition Section)-----" + "\n");
            for (i = 0; i < data_ChemGo.otherProgramData.data_Gaussian.gaussianInputSegment.addition.Count; i++)
            {
                result.Append("     " + data_ChemGo.otherProgramData.data_Gaussian.gaussianInputSegment.addition[i] + "\n");
            }

            mainOutput.WriteOutputStr(result);
        }

        /// <summary>
        /// 显示ChemGo类型输入文件的信息
        /// </summary>
        private void ShowChemGoInfo()
        {
            
        }
    }
}
