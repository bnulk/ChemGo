using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ChemGo.Data;
using ChemGo.Auxiliary.TextTools;
using ChemGo.Auxiliary.FundamentalConstants;

namespace ChemGo.Input.InputFileReadings.GaussianInputFileReadings
{
    class TransferGaussianInfoToChemGo
    {
        private Data.DataGaussian.GaussianInputSegment gaussianInputSegment;

        private ChargeAndMultiplicity chargeAndMultiplicity;
        private CoordinateType coordinateType;
        private InputCartesian inputCartesian;
        private InputZmatrix inputZmatrix;

        /// <summary>
        /// 电荷和自旋多重度
        /// </summary>
        public ChargeAndMultiplicity ChargeAndMultiplicity { get => chargeAndMultiplicity; set => chargeAndMultiplicity = value; }
        /// <summary>
        /// 坐标类型
        /// </summary>
        public CoordinateType CoordinateType { get => coordinateType; set => coordinateType = value; }
        /// <summary>
        /// 输入的笛卡尔坐标
        /// </summary>
        public InputCartesian InputCartesian { get => inputCartesian; set => inputCartesian = value; }
        /// <summary>
        /// 输入的内坐标
        /// </summary>
        public InputZmatrix InputZmatrix { get => inputZmatrix; set => inputZmatrix = value; }


        public TransferGaussianInfoToChemGo(Data.DataGaussian.GaussianInputSegment gaussianInputSegment)
        {
            this.gaussianInputSegment = gaussianInputSegment;
        }

        public void Run()
        {
            //获取坐标类型
            ObtainCoordinateType();
            //获取电荷和自旋多重度
            ObtainChargeAndMultiplicity();
            if(coordinateType==CoordinateType.zMatrix)
            {
                ObtainInputZMatrix();
            }
            else
            {
                ObtainInputCartesian();
            }
        }

        /// <summary>
        /// 获取电荷和自旋多重度
        /// </summary>
        private void ObtainChargeAndMultiplicity()
        {
            string[] tmpResult;
            List<string> listResult = new List<string>();
            string strChargeAndMultiplicity = gaussianInputSegment.chargeAndMultiplicity;

            tmpResult = strChargeAndMultiplicity.Split(' ');
            for(int i=0;i<tmpResult.Length;i++)
            {
                if(tmpResult[i]!="")
                {
                    listResult.Add(tmpResult[i]);
                }
            }

            if (Convert.ToInt32(listResult.Count / 2) != Convert.ToDouble(listResult.Count) / 2)
            {
                throw new ReadInputFileException("ChargeAndMultiplicity input Error.");
            }
              
            int numberOfPart = listResult.Count / 2;
            chargeAndMultiplicity.numberOfPart = numberOfPart;
            chargeAndMultiplicity.charge = new int[numberOfPart];
            chargeAndMultiplicity.multiplicity = new int[numberOfPart];
            for(int i=0;i<numberOfPart;i++)
            {
                chargeAndMultiplicity.charge[i] = Convert.ToInt32(listResult[i * 2]);
                chargeAndMultiplicity.multiplicity[i] = Convert.ToInt32(listResult[i * 2 + 1]);
            }
        }

        /// <summary>
        /// 获取坐标类型
        /// </summary>
        private void ObtainCoordinateType()
        {
            switch (gaussianInputSegment.coordinateType.ToLower())
            {
                case "zmatrix":
                    coordinateType = CoordinateType.zMatrix;
                    break;
                case "cartesian":
                    coordinateType = CoordinateType.Cartesian;
                    break;
                default:
                    throw new ReadInputFileException("ChemGo.Input.InputFileReadingTools.GaussianInputFileReadingTools.TransferGaussianInfoToChemGo.ObtainChargeAndMultiplicity() Error.");
            }
        }

        /// <summary>
        /// 获取输入内坐标
        /// </summary>
        private void ObtainInputZMatrix()
        {
            int i, j;
            string[] tmpStr;

            //原子个数
            inputZmatrix.numberOfAtoms = gaussianInputSegment.molecularSpecification_ZMatrix.Count;

            inputZmatrix.parameter = new string[gaussianInputSegment.molecularPara_ZMatrix.Count, 2];
            inputZmatrix.coordinates = new string[inputZmatrix.numberOfAtoms, 7];
            inputZmatrix.atomicNumbers = new int[inputZmatrix.numberOfAtoms];
            inputZmatrix.realAtomicWeights = new double[inputZmatrix.numberOfAtoms];


            //分子说明            
            tmpStr = BasisTextTools.GetStringSeparatedbySpaces(gaussianInputSegment.molecularSpecification_ZMatrix[0]);           //第一行
            inputZmatrix.coordinates[0, 0] = tmpStr[0];
            tmpStr = BasisTextTools.GetStringSeparatedbySpaces(gaussianInputSegment.molecularSpecification_ZMatrix[1]);           //第二行
            inputZmatrix.coordinates[1, 0] = tmpStr[0];
            inputZmatrix.coordinates[1, 1] = tmpStr[1];
            inputZmatrix.coordinates[1, 2] = tmpStr[2];
            tmpStr = BasisTextTools.GetStringSeparatedbySpaces(gaussianInputSegment.molecularSpecification_ZMatrix[2]);           //第三行
            inputZmatrix.coordinates[2, 0] = tmpStr[0];
            inputZmatrix.coordinates[2, 1] = tmpStr[1];
            inputZmatrix.coordinates[2, 2] = tmpStr[2];
            inputZmatrix.coordinates[2, 3] = tmpStr[3];
            inputZmatrix.coordinates[2, 4] = tmpStr[4];
            for (i=3;i<inputZmatrix.numberOfAtoms;i++)
            {
                tmpStr = BasisTextTools.GetStringSeparatedbySpaces(gaussianInputSegment.molecularSpecification_ZMatrix[i]);
                for (j=0;j<7;j++)
                {
                    inputZmatrix.coordinates[i, j] = tmpStr[j];
                }
            }

            //分子参数            
            for(i=0;i<gaussianInputSegment.molecularPara_ZMatrix.Count;i++)
            {
                if (gaussianInputSegment.molecularPara_ZMatrix[i].Contains('='))
                {
                    tmpStr = gaussianInputSegment.molecularPara_ZMatrix[i].Split('=');
                    inputZmatrix.parameter[i, 0] = tmpStr[0].Trim();
                    inputZmatrix.parameter[i, 1] = tmpStr[1].Trim();
                }
                else
                {
                    tmpStr = Regex.Split(gaussianInputSegment.molecularPara_ZMatrix[i], "\\s+", RegexOptions.IgnoreCase);
                    inputZmatrix.parameter[i, 0] = tmpStr[0].Trim();
                    inputZmatrix.parameter[i, 1] = tmpStr[1].Trim();
                }
            }

            //原子序号（核电荷数）数组
            //原子量数组
            for (i=0;i< inputZmatrix.numberOfAtoms;i++)
            {
                inputZmatrix.atomicNumbers[i] = GetAtomicNumber(inputZmatrix.coordinates[i, 0]);
                inputZmatrix.realAtomicWeights[i] = Masses.NumberToMass(inputZmatrix.atomicNumbers[i]);
            }
        }

        /// <summary>
        /// 获取笛卡尔坐标
        /// </summary>
        private void ObtainInputCartesian()
        {
            int i, j;
            string[] tmpStr;

            //原子个数
            inputCartesian.numberOfAtoms = gaussianInputSegment.molecularCartesian.Count;

            inputCartesian.coordinates = new string[inputCartesian.numberOfAtoms, 4];
            inputCartesian.atomicNumbers = new int[inputCartesian.numberOfAtoms];
            inputCartesian.realAtomicWeights = new double[inputCartesian.numberOfAtoms];

            //分子说明            
            for (i=0;i<inputCartesian.numberOfAtoms;i++)
            {
                tmpStr = BasisTextTools.GetStringSeparatedbySpaces(gaussianInputSegment.molecularCartesian[i]);
                for(j=0;j<4;j++)
                {
                    inputCartesian.coordinates[i, j] = tmpStr[j];
                }
            }


            //原子序号（核电荷数）数组
            //原子量数组
            for(i=0;i<inputCartesian.numberOfAtoms;i++)
            {
                inputCartesian.atomicNumbers[i] = GetAtomicNumber(inputCartesian.coordinates[i, 0]);
                inputCartesian.realAtomicWeights[i] = Masses.NumberToMass(inputCartesian.atomicNumbers[i]);
            }
        }

        /// <summary>
        /// 根据字符串型元素符号或原子序号，得到整型原子序号。
        /// </summary>
        /// <param name="textAtomicNumberOrElementalSymbol">字符串型元素符号或原子序号</param>
        /// <returns>整型原子序号</returns>
        private int GetAtomicNumber(string textAtomicNumberOrElementalSymbol)
        {
            int atomicNumber;
            if(BasisTextTools.IsNumber(textAtomicNumberOrElementalSymbol))
            {
                atomicNumber = Convert.ToInt32(textAtomicNumberOrElementalSymbol);
            }
            else
            {
                atomicNumber = Masses.SymbolToNumber(textAtomicNumberOrElementalSymbol);
            }
            return atomicNumber;
        }
    }
}
