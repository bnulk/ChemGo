using System;
using System.Text;
using ChemGo.Data;
using ChemGo.Functions.Mecp;

namespace ChemGo.Drive
{
    partial class Drive_0101_Mecp
    {
        public void ShowChemGoData(Mecp mecp)
        {
            StringBuilder result = new StringBuilder();

            result.Append("bnulk@foxmail.com-MECP Result" + "\n");
            result.Append("*********************************************" + "\n\n");
            result.Append("Energy=" + ((mecp.energy1+mecp.energy2)/2).ToString() + "\n");
            result.Append("Lambda=" + mecp.lambda.ToString() + "\n");
            result.Append("-Lambda/(1-Lambda)=" + ((-1)*mecp.lambda/(1-mecp.lambda)).ToString() + "\n");
            result.Append("Gradient ratio between two states: " + "\n");

            switch(data_ChemGo.inputFile.labels.control.coordinateType)
            {
                case CoordinateType.zMatrix:
                    result.Append(ShowGradientRatio_ZMatrix(mecp));
                    break;
                case CoordinateType.Cartesian:
                    result.Append(ShowGradientRatio_Geometry(mecp));
                    break;
                default:
                    throw new DriveException("unknown InputFileType. \n @ChemGo.Drive.Drive_0101_Mecp;  UpdateSinglePointData(ref Data_ChemGo data_ChemGo):site1 CoordinateType Error");
            }


            result.Append("*****************************************************************" + "\n");
            if(mecp.isConvergence==true)
            {
                result.Append("Congratulations! the KKT point was found." + "\n");
            }
            else
            {
                result.Append("Unfortunately, the KKT point was not found" + "\n");
            }
            result.Append("*****************************************************************" + "\n");


            mainOutput.WriteOutputStr(result);

        }

        private StringBuilder ShowGradientRatio_ZMatrix(Mecp mecp)
        {
            StringBuilder result = new StringBuilder();

            for(int i=0;i<mecp.numberOfX;i++)
            {
                if (Math.Abs(mecp.derivativeInfo_ZMatrix1.gradient[i]) > 0.01 && Math.Abs(mecp.derivativeInfo_ZMatrix2.gradient[i]) > 0.01)
                {
                    result.Append(mecp.zMatrix.parameterName[i].PadRight(10) + "=     " + Math.Round(mecp.derivativeInfo_ZMatrix1.gradient[i] / mecp.derivativeInfo_ZMatrix2.gradient[i], 2).ToString().PadRight(10) + "\n");
                }
                else
                {
                    result.Append(mecp.zMatrix.parameterName[i].PadRight(10) + "=     " + "smallGradient".PadRight(10) + "\n");
                }
            }

            return result;
        }


        private StringBuilder ShowGradientRatio_Geometry(Mecp mecp)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < mecp.numberOfAtoms; i++)
            {
                int i3 = 3 * i;

                if (Math.Abs(mecp.derivativeInfo_Cartesian1.gradient[i3]) > 0.01 && Math.Abs(mecp.derivativeInfo_Cartesian2.gradient[i3]) > 0.01)
                {
                    result.Append((i3.ToString()+"X").PadRight(10)
                        +"=     " + Math.Round(mecp.derivativeInfo_Cartesian1.gradient[i] / mecp.derivativeInfo_Cartesian2.gradient[i], 2).ToString().PadRight(10) + "\n");
                }
                else
                {
                    result.Append((i3.ToString() + "X").PadRight(10) + "=     " + "smallGradient".PadRight(10) + "\n");
                }

                if (Math.Abs(mecp.derivativeInfo_Cartesian1.gradient[i3+1]) > 0.01 && Math.Abs(mecp.derivativeInfo_Cartesian2.gradient[i3+1]) > 0.01)
                {
                    result.Append((i3.ToString() + "Y").PadRight(10)
                        + "=     " + Math.Round(mecp.derivativeInfo_Cartesian1.gradient[i] / mecp.derivativeInfo_Cartesian2.gradient[i], 2).ToString().PadRight(10) + "\n");
                }
                else
                {
                    result.Append((i3.ToString() + "Y").PadRight(10) + "=     " + "smallGradient".PadRight(10) + "\n");
                }

                if (Math.Abs(mecp.derivativeInfo_Cartesian1.gradient[i3+2]) > 0.01 && Math.Abs(mecp.derivativeInfo_Cartesian2.gradient[i3+2]) > 0.01)
                {
                    result.Append((i3.ToString() + "Z").PadRight(10)
                        + "=     " + Math.Round(mecp.derivativeInfo_Cartesian1.gradient[i] / mecp.derivativeInfo_Cartesian2.gradient[i], 2).ToString().PadRight(10) + "\n");
                }
                else
                {
                    result.Append((i3.ToString() + "Z").PadRight(10) + "=     " + "smallGradient".PadRight(10) + "\n");
                }
            }

            return result;
        }

    }
}
