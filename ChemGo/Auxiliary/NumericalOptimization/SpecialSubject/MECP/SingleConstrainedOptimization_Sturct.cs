using System;
using System.Collections.Generic;
using System.Text;

namespace ChemGo.Auxiliary.NumericalOptimization.MECP.SingleConstrainedOptimization
{
    /// <summary>
    /// 委托计算参量
    /// </summary>
    public struct DelegateCalculationParameter
    {
        public bool isReadGradient;
        public bool isReadHessian;
        public int timeI;
        public int numberOfX;
        public double[] x;
    }


    /// <summary>
    /// 委托计算数据
    /// </summary>
    public struct DelegateCalculationData
    {
        public int numberOfX;
        public double[] x;
        public double y;
        public double constrainedValue;                                       //约束函数的值
        public bool isExistGradient;
        public double[] gradient;
        public bool isExistHessian;
        public double[,] hessian;
        public bool isExistConstrainedGradient;
        public double[] constrainedGradient;
        public bool isExistConstrainedHessian;
        public double[,] constrainedHessian;
    }
}
