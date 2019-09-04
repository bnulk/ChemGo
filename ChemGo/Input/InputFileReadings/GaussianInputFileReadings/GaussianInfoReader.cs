using System;
using System.Collections.Generic;
using ChemGo.Data.DataGaussian;
using ChemGo.Auxiliary.TextTools;

namespace ChemGo.Input.InputFileReadings.GaussianInputFileReadings
{
    class GaussianInfoReader
    {
        private List<string> inputList;

        private GaussianInputSegment gaussianInputSegment;

        /// <summary>
        /// 高斯程序的输入结构对象
        /// </summary>
        public GaussianInputSegment GaussianInputSegment { get => gaussianInputSegment; set => gaussianInputSegment = value; }

        public GaussianInfoReader(List<string> inputList)
        {
            this.inputList = new List<string>();
            this.inputList = inputList;
        }

        public void Run()
        {
            ObtainGaussianInputSegment();
        }

        private void ObtainGaussianInputSegment()
        {
            string str = "";                                      //读取每行数据
            int iSegment = 0;                                     //分段的标识
            bool isChargeAndMultiplicity = true;                  //是否为电荷和自旋多重度的那一行
            gaussianInputSegment.coordinateType = null;           //坐标类型
            //初始化
            gaussianInputSegment.firstSection = new List<string>();
            gaussianInputSegment.routeSection = new List<string>();
            gaussianInputSegment.titleSection = new List<string>();
            gaussianInputSegment.chargeAndMultiplicity = "";
            gaussianInputSegment.addition = new List<string>();
            //填入数据
            for (int i = 0; i < inputList.Count; i++)
            {
                str = inputList[i];
                if (str.Trim() == "" && iSegment <= 3)
                    iSegment++;
                else
                {
                    switch (iSegment)
                    {
                        case 0:
                            if (str.Substring(0, 1) == "%")
                            {
                                gaussianInputSegment.firstSection.Add(str);
                            }
                            else
                            {
                                gaussianInputSegment.routeSection.Add(str);
                            }
                            break;
                        case 1:
                            gaussianInputSegment.titleSection.Add(str);
                            break;
                        case 2:
                            if (isChargeAndMultiplicity == true)
                            {
                                gaussianInputSegment.chargeAndMultiplicity = str;
                                isChargeAndMultiplicity = false;
                                break;
                            }
                            else
                            {
                                if (gaussianInputSegment.coordinateType == null)                                     //判断坐标类型
                                {
                                    if (str.Length < 4)                                                               //已经去掉前后的“ ”后，第一行的长度
                                    {
                                        gaussianInputSegment.coordinateType = "zmatrix";
                                        gaussianInputSegment.molecularSpecification_ZMatrix = new List<string>();
                                        gaussianInputSegment.molecularPara_ZMatrix = new List<string>();
                                    }
                                    else
                                    {
                                        gaussianInputSegment.coordinateType = "cartesian";
                                        gaussianInputSegment.molecularCartesian = new List<string>();
                                    }
                                }
                                if (gaussianInputSegment.coordinateType == "zmatrix")
                                    gaussianInputSegment.molecularSpecification_ZMatrix.Add(str);
                                else
                                    gaussianInputSegment.molecularCartesian.Add(str);
                            }
                            break;
                        case 3:
                            if (gaussianInputSegment.coordinateType == "zmatrix")
                                gaussianInputSegment.molecularPara_ZMatrix.Add(str);
                            break;
                        default:
                            gaussianInputSegment.addition.Add(str);
                            break;
                    }
                }
            }
            //获取原子个数N
            switch (gaussianInputSegment.coordinateType)
            {
                case "zmatrix":
                    gaussianInputSegment.N = gaussianInputSegment.molecularSpecification_ZMatrix.Count;
                    break;
                case "cartesian":
                    gaussianInputSegment.N = gaussianInputSegment.molecularCartesian.Count;
                    break;
                default:
                    break;
            }
            //去掉大括号的内容
            RemoveChemGoSectionFromRouteSection(ref gaussianInputSegment.routeSection);
            return;
        }

        /// <summary>
        /// 删除routeSection中的ChemGo部分（大括号部分），合并为一个列。
        /// </summary>
        /// <param name="routeSection">routeSection列表</param>
        private void RemoveChemGoSectionFromRouteSection(ref List<string> routeSection)
        {
            string strRouteSection = "";
            for (int i = 0; i < routeSection.Count; i++)
            {
                strRouteSection += routeSection[i];
            }
            strRouteSection = BasisTextTools.GetStringOutsideTwoString(strRouteSection, "{", "}");
            routeSection.Clear();
            routeSection.Add(strRouteSection);
        }
    }
}
