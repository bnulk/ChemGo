using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;              //使用正则表达式，用于读取多个空格分隔的字符串
using ChemGo.Data;
using ChemGo.Input;

namespace ChemGo.Input.InputFileReadings.ChemGoInputFileReadings
{
    partial class ChemGoInfoReader
    {
        private readonly List<string> inputList;
        private List<string> labelsSummary = new List<string>();                      //读入部分的标签集合
        private List<string> controlList = new List<string>();                        //控制部分
        private List<string> moleculeList = new List<string>();                       //分子说明
        private List<string> spList = new List<string>();                             //单点计算
        private List<string> forceConstantsInCartesianCoordinatesList = new List<string>();                //笛卡尔坐标下的力常数矩阵

        private InputFile inputFile;

        /// <summary>
        /// ChemGo标准格式之输入文件
        /// </summary>
        public InputFile InputFile { get => inputFile; set => inputFile = value; }

        public ChemGoInfoReader(List<string> inputList)
        {
            this.inputList = inputList;
        }

        public void Run()
        {
            ObtainLabelList();

            //读控制部分
            ChemGoInfoReader_Control control = new ChemGoInfoReader_Control(controlList);
            control.Run();
            inputFile.labels.control = control.Control;

            //读分子说明
            ChemGoInfoReader_Molecule molecule = new ChemGoInfoReader_Molecule(moleculeList);
            molecule.Run();
            inputFile.coordinateType = molecule.CoordinateType;
            if (inputFile.coordinateType == CoordinateType.Cartesian)
            {
                inputFile.inputCartesian = molecule.InputCartesian;
            }
            else
            {
                inputFile.inputZmatrix = molecule.InputZmatrix;
            }
        }

        /// <summary>
        /// 获取各个标签的list形式
        /// </summary>
        private void ObtainLabelList()
        {
            string str;                       //临时字符串
            int n = inputList.Count;
            int i;

            for (i = 0; i < n; i++)
            {
                str = inputList[i];

                //控制部分的文本列表
                if (str.ToLower().Trim() == "<control>")
                {
                    labelsSummary.Add("control");
                    i++;
                    str = inputList[i];
                    while (str.ToLower().Trim() != "</control>")
                    {
                        if (str.Trim() != "")
                        {
                            controlList.Add(str);
                        }
                        i++;
                        str = inputList[i];
                    }
                }

                //坐标部分的文本列表
                if (str.ToLower().Trim() == "<molecule>")
                {
                    labelsSummary.Add("molecule");
                    i++;
                    str = inputList[i];
                    while (str.ToLower().Trim() != "</molecule>")
                    {
                        moleculeList.Add(str);
                        i++;
                        str = inputList[i];
                    }
                }

                //笛卡尔坐标系下的力常数
                if (str.ToLower().Trim() == "<forceConstantsInCartesianCoordinates>")
                {
                    labelsSummary.Add("forceConstantsInCartesianCoordinates");
                    i++;
                    str = inputList[i];
                    while (str.ToLower().Trim() != "</forceConstantsInCartesianCoordinates>")
                    {
                        forceConstantsInCartesianCoordinatesList.Add(str);
                        i++;
                        str = inputList[i];
                    }
                }
            }
        }



    }
}
