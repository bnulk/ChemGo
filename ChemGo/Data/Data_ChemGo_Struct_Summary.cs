using System;
using System.Collections.Generic;
using System.Text;

namespace ChemGo.Data
{
    /// <summary>
    /// 命令行信息
    /// </summary>
    public struct CommandLineInformation
    {
        public string currentDirectory;                         //当前目录
        public string inputFileDirectory;                       //输入文件所在目录
        public string inputFileFullPath;                        //输入文件路径
        public string outputFileFullPath;                       //输出文件路径
        public InputFileType inputFileType;                     //输入文件类型
    }

    /// <summary>
    /// 输入文件信息
    /// </summary>
    public struct InputFile
    {
        public Labels labels;
        public ChargeAndMultiplicity chargeAndMultiplicity;
        public CoordinateType coordinateType;
        public InputCartesian inputCartesian;
        public InputZmatrix inputZmatrix;
    }
    
    /// <summary>
    /// 其它程序的数据
    /// </summary>
    public struct OtherProgramData
    {
        public DataGaussian.Data_Gaussian data_Gaussian;
    }

    /// <summary>
    /// 单点数据
    /// </summary>
    public struct SinglePoint
    {
        public int numberOfAtoms;
        public Geometry geometry;
        public ZMatrix zMatrix;
        public double energy;
        public DerivativeInfo_Cartesian derivativeInfo_Cartesian;
        public DerivativeInfo_ZMatrix derivativeInfo_ZMatrix;
    }

    /// <summary>
    /// 极小势能面交叉点信息
    /// </summary>
    public struct Mecp
    {
        public int numberOfAtoms;
        public int numberOfX;
        public CoordinateType coordinateType;
        public Geometry geometry;
        public ZMatrix zMatrix;
        public double lambda;
        public double energy1;
        public double energy2;
        public DerivativeInfo_Cartesian derivativeInfo_Cartesian1;
        public DerivativeInfo_ZMatrix derivativeInfo_ZMatrix1;
        public DerivativeInfo_Cartesian derivativeInfo_Cartesian2;
        public DerivativeInfo_ZMatrix derivativeInfo_ZMatrix2;
        public bool isConvergence;
    }
}
