using System;
using System.Collections.Generic;
using System.Text;
using ChemGo.Data;
using ChemGo.Data.DataGaussian;
using ChemGo.Auxiliary.Gaussian.InputFile;
using ChemGo.Auxiliary.TextTools;

namespace ChemGo.Functions.Mecp.GenerateOtherProgramInputFileMaterial
{
    class GenerateGaussianInputFileMaterial
    {
        private GaussianInputSegment gaussianInputSegment;
        private InterfaceBetweenGaussianAndChemGo interfaceBetweenGaussianAndChemGo;
        private InputFile inputFile;

        int i, n;
        string tmpStr;
        string[] tmpArray;


        private GaussianInputFileMaterial[] gaussianInputFileMaterial;
        /// <summary>
        /// 高斯输入文件材料数组
        /// </summary>
        public GaussianInputFileMaterial[] GaussianInputFileMaterial { get => gaussianInputFileMaterial; set => gaussianInputFileMaterial = value; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gaussianInputSegment">高斯输入片段</param>
        /// <param name="interfaceBetweenGaussianAndChemGo">高斯和ChemGo衔接部分</param>
        /// <param name="inputFile">ChemGo输入文件对象</param>
        public GenerateGaussianInputFileMaterial(GaussianInputSegment gaussianInputSegment, InterfaceBetweenGaussianAndChemGo interfaceBetweenGaussianAndChemGo, InputFile inputFile)
        {
            this.gaussianInputSegment = gaussianInputSegment;
            this.interfaceBetweenGaussianAndChemGo = interfaceBetweenGaussianAndChemGo;
            this.inputFile = inputFile;
            gaussianInputFileMaterial = new GaussianInputFileMaterial[2];
        }

        public void Run()
        {
            ObtainFirstSection();
            ObtainRouteSection();
            ObtainTitleSection();
            ObtainChargeAndMultiplicity();
            ObtainCoordinate();
            ObtainAddition();
        }

        /// <summary>
        /// 获取第一部分
        /// </summary>
        private void ObtainFirstSection()
        {
            n = gaussianInputSegment.firstSection.Count;
            GaussianInputFileMaterial[0].firstSection = new string[n];
            GaussianInputFileMaterial[1].firstSection = new string[n];
            for (i = 0; i < n; i++)
            {
                if (gaussianInputSegment.firstSection[i].StartsWith("%chk"))
                {
                    tmpStr = gaussianInputSegment.firstSection[i].Remove(0, 4).Trim();
                    tmpArray = BasisTextTools.GetStringSeparatedbySpaces(tmpStr);
                    gaussianInputFileMaterial[0].firstSection[i] = tmpArray[0];
                    gaussianInputFileMaterial[1].firstSection[i] = tmpArray[1];
                }
                else
                {
                    gaussianInputFileMaterial[0].firstSection[i] = gaussianInputSegment.firstSection[i];
                    gaussianInputFileMaterial[1].firstSection[i] = gaussianInputSegment.firstSection[i];
                }
            }
        }

        /// <summary>
        /// 获取RouteSection部分
        /// </summary>
        private void ObtainRouteSection()
        {
            n = gaussianInputSegment.routeSection.Count;
            gaussianInputFileMaterial[0].routeSection = new string[n];
            gaussianInputFileMaterial[1].routeSection = new string[n];
            for (i = 0; i < n; i++)
            {
                gaussianInputFileMaterial[0].routeSection[i] = gaussianInputSegment.routeSection[i];
                if(interfaceBetweenGaussianAndChemGo.segment[0]!=null)
                {
                    gaussianInputFileMaterial[0].routeSection[i] += " " + interfaceBetweenGaussianAndChemGo.segment[0];
                }
                gaussianInputFileMaterial[1].routeSection[i] = gaussianInputSegment.routeSection[i];
                if (interfaceBetweenGaussianAndChemGo.segment[1] != null)
                {
                    gaussianInputFileMaterial[1].routeSection[i] += " " + interfaceBetweenGaussianAndChemGo.segment[1];
                }
            }
        }

        /// <summary>
        /// 获取TitleSection部分
        /// </summary>
        private void ObtainTitleSection()
        {
            n = gaussianInputSegment.titleSection.Count;
            for (i = 0; i < n; i++)
            {
                gaussianInputFileMaterial[0].titleSection += gaussianInputSegment.titleSection[i];
                gaussianInputFileMaterial[1].titleSection += gaussianInputSegment.titleSection[i];
            }
        }

        /// <summary>
        /// 获取电荷和自旋多重度
        /// </summary>
        private void ObtainChargeAndMultiplicity()
        {
            gaussianInputFileMaterial[0].chargeAndMultiplicity = inputFile.chargeAndMultiplicity.charge[0].ToString() + " " + inputFile.chargeAndMultiplicity.multiplicity[0].ToString();
            gaussianInputFileMaterial[1].chargeAndMultiplicity = inputFile.chargeAndMultiplicity.charge[1].ToString() + " " + inputFile.chargeAndMultiplicity.multiplicity[1].ToString();
        }

        /// <summary>
        /// 获取坐标
        /// </summary>
        private void ObtainCoordinate()
        {
            if (inputFile.coordinateType == CoordinateType.zMatrix)
            {
                gaussianInputFileMaterial[0].coordinateType = gaussianInputFileMaterial[1].coordinateType = "zMatrix";
            }
            else
            {
                gaussianInputFileMaterial[0].coordinateType = gaussianInputFileMaterial[1].coordinateType = "cartesian";
            }

            switch (inputFile.coordinateType)
            {
                case CoordinateType.zMatrix:
                    ObtainZMatrix();
                    break;
                case CoordinateType.Cartesian:
                    break;
                default:
                    throw new MecpException("Unknown coordinateType.\n  ChemGo.Functions.Mecp.GenerateOtherProgramInputFileMaterial.GenerateOtherProgramInputFileMaterial.ObtainCoordinate() Error. \n");
            }
        }

        /// <summary>
        /// 获取内坐标
        /// </summary>
        private void ObtainZMatrix()
        {
            gaussianInputFileMaterial[0].N = gaussianInputFileMaterial[1].N = inputFile.inputZmatrix.numberOfAtoms;
            n = inputFile.inputZmatrix.numberOfAtoms;
            int m = inputFile.inputZmatrix.parameter.GetLength(0);
            gaussianInputFileMaterial[0].molecularSpecification_ZMatrix = new string[n];
            gaussianInputFileMaterial[1].molecularSpecification_ZMatrix = new string[n];
            gaussianInputFileMaterial[0].molecularPara_ZMatrix_Name = new string[m];
            gaussianInputFileMaterial[0].molecularPara_ZMatrix_Value = new double[m];
            gaussianInputFileMaterial[1].molecularPara_ZMatrix_Name = new string[m];
            gaussianInputFileMaterial[1].molecularPara_ZMatrix_Value = new double[m];

            for (i = 0; i < n; i++)
            {
                gaussianInputFileMaterial[0].molecularSpecification_ZMatrix[i] = gaussianInputSegment.molecularSpecification_ZMatrix[i];
                gaussianInputFileMaterial[1].molecularSpecification_ZMatrix[i] = gaussianInputSegment.molecularSpecification_ZMatrix[i];
            }
            for (i = 0; i < m; i++)
            {
                gaussianInputFileMaterial[0].molecularPara_ZMatrix_Name[i] = inputFile.inputZmatrix.parameter[i, 0];
                gaussianInputFileMaterial[0].molecularPara_ZMatrix_Value[i] = Convert.ToDouble(inputFile.inputZmatrix.parameter[i, 1]);
                gaussianInputFileMaterial[1].molecularPara_ZMatrix_Name[i] = gaussianInputFileMaterial[0].molecularPara_ZMatrix_Name[i];
                gaussianInputFileMaterial[1].molecularPara_ZMatrix_Value[i] = gaussianInputFileMaterial[0].molecularPara_ZMatrix_Value[i];
            }
        }

        /// <summary>
        /// 获取笛卡尔坐标
        /// </summary>
        private void ObtainCartesian()
        {
            gaussianInputFileMaterial[0].N = gaussianInputFileMaterial[1].N = inputFile.inputCartesian.numberOfAtoms;
            n = inputFile.inputCartesian.numberOfAtoms;
            gaussianInputFileMaterial[0].molecularCartesian_elements = new string[n];
            gaussianInputFileMaterial[0].molecularCartesian_Value = new double[n, 4];
            gaussianInputFileMaterial[1].molecularCartesian_elements = new string[n];
            gaussianInputFileMaterial[1].molecularCartesian_Value = new double[n, 4];

            for (i = 0; i < n; i++)
            {
                gaussianInputFileMaterial[0].molecularCartesian_elements[i] = inputFile.inputCartesian.coordinates[i, 0];
                gaussianInputFileMaterial[0].molecularCartesian_Value[i, 0] = Convert.ToDouble(inputFile.inputCartesian.coordinates[i, 1]);
                gaussianInputFileMaterial[0].molecularCartesian_Value[i, 1] = Convert.ToDouble(inputFile.inputCartesian.coordinates[i, 2]);
                gaussianInputFileMaterial[0].molecularCartesian_Value[i, 2] = Convert.ToDouble(inputFile.inputCartesian.coordinates[i, 3]);
                gaussianInputFileMaterial[1].molecularCartesian_elements[i] = inputFile.inputCartesian.coordinates[i, 0];
                gaussianInputFileMaterial[1].molecularCartesian_Value[i, 0] = Convert.ToDouble(inputFile.inputCartesian.coordinates[i, 1]);
                gaussianInputFileMaterial[1].molecularCartesian_Value[i, 1] = Convert.ToDouble(inputFile.inputCartesian.coordinates[i, 2]);
                gaussianInputFileMaterial[1].molecularCartesian_Value[i, 2] = Convert.ToDouble(inputFile.inputCartesian.coordinates[i, 3]);
            }
        }

        /// <summary>
        /// 获取附加部分
        /// </summary>
        private void ObtainAddition()
        {
            n = gaussianInputSegment.addition.Count;
            gaussianInputFileMaterial[0].addition = new string[n];
            gaussianInputFileMaterial[1].addition = new string[n];
            for (i = 0; i < n; i++)
            {
                gaussianInputFileMaterial[0].addition[i] = gaussianInputSegment.addition[i];
                gaussianInputFileMaterial[1].addition[i] = gaussianInputSegment.addition[i];
            }
        }


    }
}
