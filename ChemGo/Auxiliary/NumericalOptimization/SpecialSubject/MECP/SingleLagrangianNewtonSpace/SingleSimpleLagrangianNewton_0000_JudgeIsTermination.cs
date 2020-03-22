using System;
using System.Collections.Generic;
using System.Text;
using ChemGo.Auxiliary.Statistics;

namespace ChemGo.Auxiliary.NumericalOptimization.MECP.SingleConstrainedOptimization.SingleLagrangianNewtonSpace
{
    class SingleSimpleLagrangianNewton_0000_JudgeIsTermination
    {
        private LagrangianNewton_Criteria criteria;
        private LagrangianNewton_OutputStruct outputStruct;                                                                         
        private bool isConvergence_Force = false;                                                            //力标准是否收敛
        private bool isConvergence_ConstrainedValue = false;                                                 //约束标准是否收敛
        //收敛指标
        private double constrainedValue;
        private double[] lagrangeForce;
        private double maxLagrangeForce;
        private double RMSLagrangeForce;

        private bool isConvergence = false;
        private LagrangianNewton_ConvergenceInfo lagrangianNewton_ConvergenceInfo;
        private List<string> record;
        public bool IsConvergence { get => isConvergence; set => isConvergence = value; }
        public LagrangianNewton_ConvergenceInfo LagrangianNewton_ConvergenceInfo { get => lagrangianNewton_ConvergenceInfo; set => lagrangianNewton_ConvergenceInfo = value; }
        public List<string> Record { get => record; set => record = value; }
        

        public SingleSimpleLagrangianNewton_0000_JudgeIsTermination(LagrangianNewton_Criteria criteria, LagrangianNewton_OutputStruct outputStruct)
        {
            this.criteria = criteria;
            this.outputStruct = outputStruct;

            //初始化数据
            this.constrainedValue = outputStruct.constrainedValue;
            this.lagrangeForce = new double[outputStruct.numberOfX];
        }

        

        public void Run()
        {
            if (Math.Abs(constrainedValue) < criteria.criteriaConstrainedCondition)                //根据约束条件的模，判断是否收敛
            {
                isConvergence_ConstrainedValue = true;
            }
            lagrangianNewton_ConvergenceInfo.constrainedConditionValue = constrainedValue;

            JudgeBasedOnLagrangeForceCriteria();

            if (isConvergence_ConstrainedValue == true && isConvergence_Force == true)
            {
                isConvergence = true;
            }

            //为了集中显示约束值、拉格朗日力和Lambda，给出记录。
            record = new List<string>();
            record.Add(outputStruct.timeI.ToString());                                                                                             //步数标号
            record.Add(Math.Round(constrainedValue, 9).ToString());                                                                                //约束值
            record.Add(Math.Round(outputStruct.lambda, 9).ToString());                                                                             //Lambda
            record.Add(Math.Round(outputStruct.y, 9).ToString());                                                                                  //y值
            record.Add(Math.Round(maxLagrangeForce, 9).ToString());                                                                                //最大Lagrange力
            record.Add(Math.Round(RMSLagrangeForce, 9).ToString());                                                                                //最大均方根Lagrange力
        }

        /// <summary>
        /// 根据拉格朗日力，判断是否收敛
        /// </summary>
        private void JudgeBasedOnLagrangeForceCriteria()
        {
            CalculateLagrangeForce(outputStruct);                          //计算拉格朗日力
            if (maxLagrangeForce <= criteria.criteriaMaxLagrangeForce && RMSLagrangeForce <= criteria.criteriaRMSLagrangeForce)
            {
                isConvergence_Force = true;
                lagrangianNewton_ConvergenceInfo.isConvergence = true;
            }
        }

        /// <summary>
        /// 计算最大拉格朗日力和均方根拉格朗日力
        /// </summary>
        private void CalculateLagrangeForce(LagrangianNewton_OutputStruct outputStruct)
        {
            lagrangeForce = new double[outputStruct.numberOfX];
            for (int i = 0; i < outputStruct.numberOfX; i++)
            {
                lagrangeForce[i] = outputStruct.gradient[i] - outputStruct.lambda * outputStruct.constrainedGradient[i];
            }
            maxLagrangeForce = Statistics.Gadgets.MaxArray(lagrangeForce);
            lagrangianNewton_ConvergenceInfo.MaxLagrangeForce = Math.Round(maxLagrangeForce, 9);                               //保留9位有效数字
            RMSLagrangeForce = Statistics.Gadgets.RMSArray(lagrangeForce);
            lagrangianNewton_ConvergenceInfo.RMSLagrangeForce = Math.Round(RMSLagrangeForce, 9);                               //保留9位有效数字
            return;
        }
    }
}
