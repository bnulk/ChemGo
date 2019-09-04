using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using ChemGo.Auxiliary.TextTools;
using ChemGo.Auxiliary.FundamentalConstants;
using ChemGo.Data;

namespace ChemGo.Input.InputFileReadings.ChemGoInputFileReadings
{
    class ChemGoInfoReader_Molecule
    {
        /// <summary>
        /// 输入的分子坐标类型，只在这里用。后面都转换成ChemGo标准格式，即zMatrix0和cartesian0格式
        /// </summary>
        private enum InputCoordinateTypeForRead
        {
            zMatrix0,
            zMatrix1,
            cartesian0,
            cartesian1
        }


        private List<string> moleculeList;
        private InputCoordinateTypeForRead inputCoordinateTypeForRead;

        private CoordinateType coordinateType;
        private InputCartesian inputCartesian;
        private InputZmatrix inputZmatrix;


        /// <summary>
        /// 坐标类型
        /// </summary>
        public CoordinateType CoordinateType { get => coordinateType; set => coordinateType = value; }
        /// <summary>
        /// 笛卡尔坐标
        /// </summary>
        public InputCartesian InputCartesian { get => inputCartesian; set => inputCartesian = value; }
        /// <summary>
        /// 内坐标
        /// </summary>
        public InputZmatrix InputZmatrix { get => inputZmatrix; set => inputZmatrix = value; }

        
        public ChemGoInfoReader_Molecule(List<string> moleculeList)
        {
            this.moleculeList = moleculeList;
        }

        /// <summary>
        /// 把分子说明写入ChemGo
        /// </summary>
        public void Run()
        {
            //获取坐标类型
            inputCoordinateTypeForRead = GetInputCoordinateTypeForRead();
            coordinateType = GetChemGoCoordinateType(inputCoordinateTypeForRead);

            //根据坐标类型，选择函数把坐标传递给ChemGo
            switch (inputCoordinateTypeForRead)
            {
                case InputCoordinateTypeForRead.cartesian0:
                    ReadCartesian0();
                    break;
                case InputCoordinateTypeForRead.zMatrix0:
                    ReadZMatrix0();
                    break;
                default:
                    throw new ReadInputFileException("Unknown coordinate type in input file. \n ChemGo.Input.InputFileReadings.ChemGoInputFileReadings.ChemGoInfoReader.ToChemGo_MoleculeList()");
            }
        }


        /// <summary>
        /// 获取输入文件中的坐标类型
        /// </summary>
        /// <returns>输入文件中的坐标类型</returns>
        private InputCoordinateTypeForRead GetInputCoordinateTypeForRead()
        {
            InputCoordinateTypeForRead inputCoordinateTypeForRead;

            //判断是何种坐标
            if (moleculeList[0].Trim().Length > 3)                    //直角坐标
            {
                inputCoordinateTypeForRead = InputCoordinateTypeForRead.cartesian0;
            }
            else                                                       //内坐标
            {
                bool isTwoBlock = false;                                                //内坐标是否分成两块
                for (int i = 0; i < moleculeList.Count; i++)
                {
                    if (moleculeList[i].Trim() == "")
                    {
                        isTwoBlock = true;
                    }
                }
                if (isTwoBlock == true)
                {
                    inputCoordinateTypeForRead = InputCoordinateTypeForRead.zMatrix0;
                }
                else
                {
                    inputCoordinateTypeForRead = InputCoordinateTypeForRead.zMatrix1;
                }
            }

            return inputCoordinateTypeForRead;
        }

        private CoordinateType GetChemGoCoordinateType(InputCoordinateTypeForRead inputCoordinateTypeForRead)
        {
            CoordinateType coordinateType;
            switch(inputCoordinateTypeForRead)
            {
                case InputCoordinateTypeForRead.cartesian0:
                    coordinateType = CoordinateType.Cartesian;
                    break;
                case InputCoordinateTypeForRead.cartesian1:
                    coordinateType = CoordinateType.Cartesian;
                    break;
                case InputCoordinateTypeForRead.zMatrix0:
                    coordinateType = CoordinateType.zMatrix;
                    break;
                case InputCoordinateTypeForRead.zMatrix1:
                    coordinateType = CoordinateType.zMatrix;
                    break;
                default:
                    throw new ReadInputFileException("unknown input molemolar coordinate type.  " +
                        "\n ChemGo.Input.InputFileReadings.ChemGoInputFileReadings.ChemGoInfoReader.GetChemGoCoordinateType() Error.");
            }
            return coordinateType;
        }

        /// <summary>
        /// 读Cartesian0型坐标
        /// </summary>
        private void ReadCartesian0()
        {
            inputCartesian.numberOfAtoms = moleculeList.Count;
            inputCartesian.atomicNumbers = new int[inputCartesian.numberOfAtoms];
            inputCartesian.coordinates = new string[inputCartesian.numberOfAtoms, 4];
            inputCartesian.realAtomicWeights = new double[inputCartesian.numberOfAtoms];

            string[] tmpData = new string[6];                        //一行四部分，多出的部分无用，为了兼容其它程序的坐标形式。
            for (int i = 0; i < inputCartesian.numberOfAtoms; i++)
            {
                tmpData = Regex.Split(moleculeList[i], "\\s+", RegexOptions.IgnoreCase);
                //元素的原子序数或者元素符号
                if (BasisTextTools.IsNumber(tmpData[0]) == true)                                           //原子序数
                {
                    inputCartesian.atomicNumbers[i] = Convert.ToInt32(tmpData[0]);               //读取原子序数
                    inputCartesian.realAtomicWeights[i] = Masses.NumberToMass(inputCartesian.atomicNumbers[i]);           //原子量
                }
                else                                                                                       //元素符号
                {
                    inputCartesian.atomicNumbers[i] = Masses.SymbolToNumber(tmpData[0]);         //根据元素符号，填写原子序数。
                    inputCartesian.realAtomicWeights[i] = Masses.NumberToMass(inputCartesian.atomicNumbers[i]);           //原子量
                }
                //坐标
                for (int j = 0; j < 4; j++)
                {
                    inputCartesian.coordinates[i, j] = tmpData[j];
                }
            }
        }

        /// <summary>
        /// 读Zmatrix0型坐标
        /// </summary>
        private void ReadZMatrix0()
        {
            string[] tmpStr = new string[10];                         //一行七部分，多出的部分无用，为了兼容其它程序的坐标形式。
            int moleculeListRowsNumber = moleculeList.Count;          //分子列表行数
            List<string> molecularSpecification = new List<string>();
            List<string> molecularParameter = new List<string>();
            bool isMolecularParameter = false;

            //扫描分子列表，获取原子个数，填充分子说明和分子参数列表
            for (int i = 0; i < moleculeListRowsNumber; i++)
            {
                if (moleculeList[i].Trim() == "")
                {
                    isMolecularParameter = true;
                    inputZmatrix.numberOfAtoms = i;
                }
                if (isMolecularParameter == false)
                {
                    molecularSpecification.Add(moleculeList[i]);
                }
                else
                {
                    molecularParameter.Add(moleculeList[i]);
                }
            }
            molecularParameter.RemoveAt(0);

            //原子个数
            inputZmatrix.numberOfAtoms = molecularSpecification.Count;
            
            inputZmatrix.coordinates = new string[inputZmatrix.numberOfAtoms, 7];
            inputZmatrix.parameter = new string[molecularParameter.Count, 2];
            inputZmatrix.atomicNumbers = new int[inputZmatrix.numberOfAtoms];
            inputZmatrix.realAtomicWeights = new double[inputZmatrix.numberOfAtoms];


            //分子说明            
            tmpStr = BasisTextTools.GetStringSeparatedbySpaces(molecularSpecification[0]);           //第一行
            inputZmatrix.coordinates[0, 0] = tmpStr[0];
            tmpStr = BasisTextTools.GetStringSeparatedbySpaces(molecularSpecification[1]);           //第二行
            inputZmatrix.coordinates[1, 0] = tmpStr[0];
            inputZmatrix.coordinates[1, 1] = tmpStr[1];
            inputZmatrix.coordinates[1, 2] = tmpStr[2];
            tmpStr = BasisTextTools.GetStringSeparatedbySpaces(molecularSpecification[2]);           //第三行
            inputZmatrix.coordinates[2, 0] = tmpStr[0];
            inputZmatrix.coordinates[2, 1] = tmpStr[1];
            inputZmatrix.coordinates[2, 2] = tmpStr[2];
            inputZmatrix.coordinates[2, 3] = tmpStr[3];
            inputZmatrix.coordinates[2, 4] = tmpStr[4];
            for (int i = 3; i < inputZmatrix.numberOfAtoms; i++)
            {
                tmpStr = BasisTextTools.GetStringSeparatedbySpaces(molecularSpecification[i]);
                for (int j = 0; j < 7; j++)
                {
                    InputZmatrix.coordinates[i, j] = tmpStr[j];
                }
            }

            //分子参数            
            for (int i = 0; i <molecularParameter.Count; i++)
            {
                if(molecularParameter[i].Contains('='))
                {
                    tmpStr = molecularParameter[i].Split('=');
                    inputZmatrix.parameter[i, 0] = tmpStr[0].Trim();
                    inputZmatrix.parameter[i, 1] = tmpStr[1].Trim();
                }
                else
                {
                    tmpStr = Regex.Split(molecularParameter[i], "\\s+", RegexOptions.IgnoreCase);
                    inputZmatrix.parameter[i, 0] = tmpStr[0].Trim();
                    inputZmatrix.parameter[i, 1] = tmpStr[1].Trim();
                }
                
            }

            //原子序号（核电荷数）数组
            //原子量数组
            for (int i = 0; i < inputZmatrix.numberOfAtoms; i++)
            {
                inputZmatrix.atomicNumbers[i] = GetAtomicNumber(inputZmatrix.coordinates[i, 0]);
                inputZmatrix.realAtomicWeights[i] = Masses.NumberToMass(inputZmatrix.atomicNumbers[i]);
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
            if (BasisTextTools.IsNumber(textAtomicNumberOrElementalSymbol))
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
