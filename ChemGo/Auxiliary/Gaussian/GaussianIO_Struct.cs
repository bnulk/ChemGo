using System;
using System.Collections.Generic;
using System.Text;

namespace ChemGo.Auxiliary
{
    /// <summary>
    /// 高斯程序输入材料
    /// </summary>
    public struct GaussianInputFileMaterial
    {
        public int N;
        public string[] firstSection;
        public string[] routeSection;
        public string titleSection;
        public string chargeAndMultiplicity;
        public string coordinateType;
        public string[] molecularSpecification_ZMatrix;
        public string[] molecularPara_ZMatrix_Name;
        public double[] molecularPara_ZMatrix_Value;
        public string[] molecularCartesian_elements;
        public double[,] molecularCartesian_Value;
        public string[] addition;
    }

    /// <summary>
    /// 输出优化信息的结构
    /// </summary>
    public struct OptimizationPackage
    {
        public int numberOfPara;
        public double[] x;
        public double[] gradient;
        public double[,] hessian;
    }
}
