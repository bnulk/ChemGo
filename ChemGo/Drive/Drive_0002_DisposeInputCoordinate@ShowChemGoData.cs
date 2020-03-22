using System;
using System.Text;
using ChemGo.Data;
using ChemGo.Functions.DisposeInputCoordinate;

namespace ChemGo.Drive
{
    partial class Drive_0002_DisposeInputCoordinate
    {
        public void ShowChemGoData(Data_ChemGo data_ChemGo)
        {
            StringBuilder result = new StringBuilder();
            int i, j, cycle;

            result.Append("bnulk@foxmail.com-Dispose Input Coordinate" + "\n");
            result.Append("*********************************************" + "\n\n");
            result.Append("Number of Atoms: " + data_ChemGo.singlePoint.numberOfAtoms.ToString() + "\n");

            if (data_ChemGo.singlePoint.zMatrix.atomicNumbers != null)
            {
                result.Append("ZMatrix:" + "\n");
                cycle = data_ChemGo.singlePoint.zMatrix.coordinates.GetLength(0);
                for (i = 0; i < cycle; i++)
                {
                    for (j = 0; j < 7; j++)
                    {
                        result.Append("     " + data_ChemGo.singlePoint.zMatrix.coordinates[i, j] + "  ");
                    }
                    result.Append("\n");
                }
                result.Append("\n");
                cycle = data_ChemGo.singlePoint.zMatrix.parameterName.Length;
                for (i = 0; i < cycle; i++)
                {
                    result.Append("     " + data_ChemGo.singlePoint.zMatrix.parameterName[i] + "=" + data_ChemGo.singlePoint.zMatrix.parameterValue[i] + "\n");
                }
            }                

            if(data_ChemGo.singlePoint.geometry.atomicNumbers!=null)
            {
                result.Append("\n");
                result.Append("                          Input orientation:                          " + "\n");
                result.Append(" ---------------------------------------------------------------------" + "\n");
                result.Append(" Center     Atomic      Atomic             Coordinates (Angstroms)" + "\n");
                result.Append(" Number     Number       Type             X           Y           Z" + "\n");
                result.Append(" ---------------------------------------------------------------------" + "\n");
                cycle = data_ChemGo.singlePoint.geometry.atomicNumbers.Length;
                for (i = 0; i < cycle; i++)
                {
                    result.Append("     " + i.ToString().PadLeft(7) + data_ChemGo.singlePoint.geometry.atomicNumbers[i].ToString().PadLeft(11) + "0".PadLeft(12)
                        + data_ChemGo.singlePoint.geometry.standardOrientationCoordinates[i, 0].ToString("0.000000").PadLeft(13)
                        + data_ChemGo.singlePoint.geometry.standardOrientationCoordinates[i, 1].ToString("0.000000").PadLeft(12)
                        + data_ChemGo.singlePoint.geometry.standardOrientationCoordinates[i, 2].ToString("0.000000").PadLeft(12) + "\n");
                }
            }
            result.Append("\n");

            mainOutput.WriteOutputStr(result);
        }
    }
}
