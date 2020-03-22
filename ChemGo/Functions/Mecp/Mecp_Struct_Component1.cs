using System;
using System.Collections.Generic;
using System.Text;
using ChemGo.Auxiliary.LinearAlgebra;

namespace ChemGo.Functions.Mecp
{
    /// <summary>
    /// 单点数据
    /// </summary>
    public struct SinglePointData
    {
        public int I;                                //第I次计算的结果，为历史数据提供信息。
        public int N;                                //原子个数
        public string coordinateType;                //坐标类型
        public double energy1;
        public string[] para;
        public double[] x;                           //读计算结果得到，所以说原子单位。
        public double[] gradient1;
        public double[,] hessian1;
        public double energy2;
        public double[] gradient2;
        public double[,] hessian2;
    }

    /// <summary>
    /// 猜测更新Hessian阵用到的结构
    /// </summary>
    public struct EstimateHessian
    {
        public int dim;
        public double[] lastMatrixX1, lastMatrixX2;        //旧的坐标矩阵。
        public double[] lastMatrixG1, lastMatrixG2;        //旧的力矩阵。
        public double[,] lastMatrixH1, lastMatrixH2;       //旧的力常数矩阵，二维数组。
        public double[] matrixX1, matrixX2;                //坐标矩阵。
        public double[] matrixG1, matrixG2;                //力矩阵行。
    }

    /// <summary>
    /// LiuFreq
    /// </summary>
    public struct LiuFreq
    {
        public int N;                                //原子个数
        public int[] atomicNumber;                   //原子序数
        public int[] atomicType;                     //原子类型
        public Matrix x;                             //笛卡尔坐标
        public double Lambda;                        //拉格朗日因子
        public double energy1;
        public Vector gradient1;
        public Matrix hessian1;
        public double energy2;
        public Vector gradient2;
        public Matrix hessian2;
        public bool isRealMECP;                      //是否为真正极小
    }

    /// <summary>
    /// 收敛标准
    /// </summary>
    public struct Criteria
    {
        public double deltaEnergy;
        public double[] lagrangeForce;
        public double maxLagrangeForce;
        public double RMSLagrangeForce;
    }
}
