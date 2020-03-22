using System;
using System.Collections.Generic;
using System.Text;
using ChemGo.Data;

namespace ChemGo.Auxiliary.ExternalProgram.Gaussian
{
    /// <summary>
    /// 高斯程序输入材料
    /// </summary>
    public struct GaussianInputFileMaterial
    {
        public int numberOfAtom;
        public string[] firstSection;
        public string[] routeSection;
        public string titleSection;
        public string chargeAndMultiplicity;
        public CoordinateType coordinateType;
        public string[] molecularSpecification_ZMatrix;
        public string[] molecularPara_ZMatrix_Name;
        public double[] molecularPara_ZMatrix_Value;
        public string[] molecularCartesian_elements;
        public double[,] molecularCartesian_Value;
        public string[] addition;
    }


    /// <summary>
    /// 输出优化任务的信息包
    /// </summary>
    public struct OptimizationPackage
    {
        public int numberOfPara;
        public double[] x;
        public double[] gradient;
        public double[,] hessian;
    }

    /// <summary>
    /// 输出Mecp任务的信息包
    /// </summary>
    public struct MecpPackage
    {
        public int numberOfX;
        public string[] xName;
        public double[] x;
        public double energy1;
        public double energy2;
        public double[] gradient1;
        public double[,] hessian1;
        public double[] gradient2;
        public double[,] hessian2;
    }


}
