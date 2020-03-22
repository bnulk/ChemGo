using System;
using System.Collections.Generic;
using System.Text;
using ChemGo.Data;

namespace ChemGo.Functions.Mecp
{
    /// <summary>
    /// Lagrangian法的Mecp变量
    /// </summary>
    public struct MecpLagrangianNewtonVariable
    {
        public bool isCalculateHessian;                        //是否加freq
        public int cycleI;                                     //第I次循环
        public double stepSize;                                //步长
        public bool isConvergence;                             //是否收敛
        public string mecpFreq;                                //振动分析选项
        public double[] newX;                                  //新坐标数组
        public double lambda;                                  //拉格朗日因子

        public SinglePointData singlePointData;                //单点数据
        public List<SinglePointData> historySinglePointData;   //历史单点数据
        public EstimateHessian estimateHessian;                //猜测更新Hessian阵用到的结构

        public LiuFreq liuFreq;                                //LiuFreq数据
        public Criteria criteria;                              //收敛标准
    }
    
}
