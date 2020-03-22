using System;
using System.Collections.Generic;
using System.Text;

namespace ChemGo.Auxiliary.NumericalOptimization.MECP.SingleConstrainedOptimization.SingleLagrangianNewtonSpace
{
    class SingleSimpleLagrangianNewton_0002_Output
    {
        public static void Output(LagrangianNewton_InputStruct inputStruct, LagrangianNewton_OutputStruct outputStruct)
        {
            StringBuilder m_Result = new StringBuilder();

            m_Result.Append("##########     I is:" + outputStruct.timeI.ToString() + "     ##########" + "\n");
            m_Result.Append("The Value of Main Function is:" + outputStruct.y.ToString() + "\n");
            m_Result.Append("The Constraint Function Value is:" + outputStruct.constrainedValue.ToString() + "\n");
            m_Result.Append("Lambda is:" + outputStruct.lambda.ToString() + "\n");
            m_Result.Append("Stepsize is:" + inputStruct.lagrangianNewton_control.stepSize.ToString() + "\n");
            //集中显示重要结果
            m_Result.Append("----------" + "\n");

            m_Result.Append("I".PadLeft(1).PadRight(10) + "ConstraintValue".PadLeft(10).PadRight(20) + "Lambda".PadLeft(6).PadRight(20) + "FunctionValue".PadRight(30) + "MaxForce".PadRight(20) + "RMSForce".PadRight(20) + "\n");
            for (int i = 0; i < outputStruct.records.Count; i++)
            {
                m_Result.Append(outputStruct.records[i][0].PadRight(10));
                m_Result.Append(outputStruct.records[i][1].PadRight(20));
                m_Result.Append(outputStruct.records[i][2].PadRight(20));
                m_Result.Append(outputStruct.records[i][3].PadRight(30));
                m_Result.Append(outputStruct.records[i][4].PadRight(20));
                m_Result.Append(outputStruct.records[i][5].PadRight(20));
                m_Result.Append("\n");
            }

            /*
            //输出梯度和力常数
            for (int i=0;i< data_MECP.functionData.gradient1.Length; i++)
            {
                m_Result.Append(data_MECP.functionData.gradient1[i].ToString().PadLeft(20));
                if ((i+1)%3==0)
                    m_Result.Append("\n");
            }

            for (int i = 0; i < data_MECP.functionData.gradient1.Length; i++)
            {
                for (int j = 0; j < data_MECP.functionData.gradient1.Length; j++)
                {
                    if(i>=j)
                    {
                        m_Result.Append(data_MECP.functionData.hessian1[i, j].ToString().PadLeft(20));
                    }
                    if ((j+1)%5 == 0)
                        m_Result.Append("\n");
                }
                m_Result.Append("\n");
            }
            */
            string constrainedConditionConverged = "No";
            if (Math.Abs(outputStruct.lagrangianNewton_ConvergenceInfo.constrainedConditionValue) < inputStruct.lagrangianNewton_criteria.criteriaConstrainedCondition)
                constrainedConditionConverged = "Yes";
            string maxConverged = "No";
            if ( outputStruct.lagrangianNewton_ConvergenceInfo.MaxLagrangeForce< inputStruct.lagrangianNewton_criteria.criteriaMaxLagrangeForce)
                maxConverged = "Yes";
            string RMSConverged = "No";
            if (outputStruct.lagrangianNewton_ConvergenceInfo.RMSLagrangeForce < inputStruct.lagrangianNewton_criteria.criteriaRMSLagrangeForce)
                RMSConverged = "Yes";
            m_Result.Append("         Item    ".PadRight(25) + "   Value ".PadRight(15) + "  Threshold".PadRight(15) + "     Converged?".PadRight(15) + "\n");
            m_Result.Append(" Constrained Condition".PadRight(25) + outputStruct.lagrangianNewton_ConvergenceInfo.constrainedConditionValue.ToString("E5").PadRight(15) + ("  " + inputStruct.lagrangianNewton_criteria.criteriaConstrainedCondition.ToString()).PadRight(15) + "         " + constrainedConditionConverged.PadRight(15) + "\n");
            m_Result.Append(" Maximum KKT Force".PadRight(25) + outputStruct.lagrangianNewton_ConvergenceInfo.MaxLagrangeForce.ToString("0.000000").PadRight(15) + ("  " + inputStruct.lagrangianNewton_criteria.criteriaMaxLagrangeForce.ToString()).PadRight(15) + "         " + maxConverged.PadRight(15) + "\n");
            m_Result.Append(" RMS KKT Force".PadRight(25) + outputStruct.lagrangianNewton_ConvergenceInfo.RMSLagrangeForce.ToString("0.000000").PadRight(15) + ("  " + inputStruct.lagrangianNewton_criteria.criteriaRMSLagrangeForce.ToString()).PadRight(15) + "         " + RMSConverged.PadRight(15) + "\n");

            inputStruct.output.WriteOutputStr(m_Result);
        }
    }
}
