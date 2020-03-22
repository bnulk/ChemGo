using System;
using System.Collections.Generic;
using System.Text;

namespace ChemGo.Auxiliary.NumericalOptimization.DelegateData
{
    public struct ConstrainedOptimizationDelegateData
    {
        public DelegateCalculationParameter parameter;
        public DelegateCalculationResult result;
    }

    /// <summary>
    /// 委托计算参量
    /// </summary>
    public struct DelegateCalculationParameter
    {
        public int timeI;
        public bool isCalcGradient;
        public bool isCalcConstraintGradient;
        public bool isCalcHessian;
        public bool isCalcConstraintHessian;
        public int numberOfX;
        public int numberOfConstraintCondition;
        public double[] x;
    }


    /// <summary>
    /// 委托计算结果
    /// </summary>
    public struct DelegateCalculationResult
    {
        public int numberOfX;
        public double[] x;
        public double y;
        public double constraintFunctionValue;                                         //约束函数的值
        public bool isReadGradient;
        public double[] gradient;
        public bool isReadHessian;
        public double[,] hessian;
        public bool isReadConstraintGradient;
        public List<double[]> constraintGradient;
        public bool isReadConstraintHessian;
        public List<double[,]> constraintHessian;
    }
}
