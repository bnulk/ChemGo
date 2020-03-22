using System;
using System.Collections.Generic;
using System.Text;
using ChemGo.Auxiliary.LinearAlgebra;

namespace ChemGo.Auxiliary.NumericalOptimization.MECP.SingleConstrainedOptimization.SingleLagrangianNewtonSpace
{

    /*
        ----------------------------------------------------  类注释  开始----------------------------------------------------
        版本：  V1.0
        作者：  刘鲲
        日期：  2020-02-18

        描述：
            * 
        结构：
            * 
        方法：
            *
        代码来源：
            * Chemical Physics Letters 1985年， volume 119, 371
        ----------------------------------------------------  类注释  结束----------------------------------------------------
        */
    class SingleSimpleLagrangianNewton_0001_Iteration
    {
        //参数变量
        private LagrangianNewton_InputStruct inputStruct;
        private LagrangianNewton_OutputStruct outputStruct;

        //本类中的变量
        private Matrix omiga_Z;                      //ω_Z阵。
        private Matrix inverseOmiga_Z;               //ω_Z阵的逆矩阵。
        private Vector f_Z;                          //F阵，numberOfX+1行。前numberOfX行是梯度，最后一行是约束函数值。
        private Vector detParams;                    //参数的Det值，numberOfX+1行。其中前numberOfX是变量，最后一行是拉格朗日λ值。

        /// <summary>
        /// 迭代后得到的计算参数改变值。
        /// </summary>
        public Vector DetParams { get => detParams; set => detParams = value; }

        public SingleSimpleLagrangianNewton_0001_Iteration(LagrangianNewton_InputStruct inputStruct, LagrangianNewton_OutputStruct outputStruct)
        {
            this.inputStruct = inputStruct;
            this.outputStruct = outputStruct;
        }

        public void Run()
        {
            Initialize();
            newtLagr();
        }


        /// <summary>
        /// 初始化本类中的变量
        /// </summary>
        private void Initialize()
        {
            //初始化三个计算中的矩阵
            omiga_Z = new Matrix(outputStruct.numberOfX + 1, outputStruct.numberOfX + 1);
            inverseOmiga_Z = new Matrix(outputStruct.numberOfX + 1, outputStruct.numberOfX + 1);
            f_Z = new Vector(outputStruct.numberOfX + 1);
            DetParams = new Vector(outputStruct.numberOfX + 1);
        }

        /// <summary>
        /// 拉格朗日牛顿法
        /// </summary>
        private void newtLagr()
        {
            //由能量，力矩阵和力常数矩阵产生F_Z列和Omiga_Z矩阵
            CalFZAndOmiga_Z();
            //调用高斯-约当函数求方程的解
            SolveEquation();
        }

        /// <summary>
        /// 由能量，力矩阵和力常数矩阵产生F_Z列和Omiga_Z矩阵
        /// </summary>
        public void CalFZAndOmiga_Z()
        {
            for (int i = 0; i < outputStruct.numberOfX; i++)
            {
                f_Z[i] = Convert.ToDouble(outputStruct.gradient[i]) * (-1);
            }

            f_Z[outputStruct.numberOfX] = outputStruct.constrainedValue;

            for (int i = 0; i < outputStruct.numberOfX; i++)
            {
                for (int j = 0; j < outputStruct.numberOfX; j++)
                {
                    omiga_Z[i, j] = outputStruct.hessian[i, j] - outputStruct.lambda * outputStruct.constrainedHessian[i, j];
                }
            }
            for (int i = 0; i < outputStruct.numberOfX; i++)
            {
                omiga_Z[i, outputStruct.numberOfX] = outputStruct.constrainedGradient[i] * (-1);
            }
            for (int i = 0; i < outputStruct.numberOfX; i++)
            {
                omiga_Z[outputStruct.numberOfX, i] = outputStruct.constrainedGradient[i] * (-1);
            }
            omiga_Z[outputStruct.numberOfX, outputStruct.numberOfX] = 0;

            return;
        }

        /// <summary>
        /// 调用高斯-约当函数求方程的解
        /// </summary>
        public void SolveEquation()
        {
            int cycle = outputStruct.numberOfX + 1;
            for (int i = 0; i < cycle; i++)
            {
                DetParams[i] = f_Z[i];
            }
            inverseOmiga_Z = new Matrix(cycle, cycle);
            for (int i = 0; i < cycle; i++)
            {
                for (int j = 0; j < cycle; j++)
                {
                    inverseOmiga_Z[i,j] = omiga_Z[i, j];
                }
            }
            //调用函数
            LinearEquation.gaussj(ref inverseOmiga_Z, ref detParams);
        }

    }
}
