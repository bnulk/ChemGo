using System;
using System.Collections.Generic;
using System.Text;
using ChemGo.Auxiliary.LinearAlgebra;
using ChemGo.Output;

namespace ChemGo.Auxiliary.NumericalOptimization.MECP.SingleConstrainedOptimization.SingleLagrangianNewtonSpace
{
    /// <summary>
    /// 输入结构
    /// </summary>
    public struct LagrangianNewton_InputStruct
    {
        public WriteOutput output;
        public LagrangianNewton_Criteria lagrangianNewton_criteria;
        public LagrangianNewton_Control lagrangianNewton_control;
        public int numberOfX;
        public double[] x;
        public int timeI;
        public double stepsize;
        public double lambda;
    }

    /// <summary>
    /// 输出结构
    /// </summary>
    public struct LagrangianNewton_OutputStruct
    {
        public bool isConvergence;
        public int timeI;
        public int numberOfX;
        public double[] x;
        public double y;
        public double constrainedValue;                                       //约束函数的值
        public double[] gradient;
        public double[,] hessian;
        public double[] constrainedGradient;
        public double[,] constrainedHessian;
        public double lambda;
        public LagrangianNewton_ConvergenceInfo lagrangianNewton_ConvergenceInfo;
        public List<List<string>> records;
    }

    /// <summary>
    /// 收敛标准,输入结构的一部分
    /// </summary>
    public struct LagrangianNewton_Criteria
    {
        public double criteriaConstrainedCondition;
        public double criteriaMaxLagrangeForce;
        public double criteriaRMSLagrangeForce;
    }

    /// <summary>
    /// 收敛信息
    /// </summary>
    public struct LagrangianNewton_ConvergenceInfo
    {
        public bool isConvergence;
        public double constrainedConditionValue;
        public double MaxLagrangeForce;
        public double RMSLagrangeForce;
    }

    /// <summary>
    /// 计算控制，输入结构的一部分
    /// </summary>
    public struct LagrangianNewton_Control
    {
        public double initialLambda;                              //Lambda初值
        public int maxCyc;                                        //最大步数
        public double stepSize;                                   //步长
        public int nStepHessianBeCalculated;                      //每n步计算一次Hessian
        public int nStepGradientBeCalculated;                     //每n步计算一次梯度
        public GetHessianWay getHessianWay;                       //获取Hessian的方法
    }


    /// <summary>
    /// 获取Hessian的方法类型
    /// </summary>
    public enum GetHessianWay
    {
        read,
        bfgs,
        powell
    }
}
