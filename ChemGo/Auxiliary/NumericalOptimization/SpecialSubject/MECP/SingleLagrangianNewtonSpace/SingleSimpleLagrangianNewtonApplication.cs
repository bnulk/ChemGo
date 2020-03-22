using System;
using System.Collections.Generic;
using System.Text;
using ChemGo.Auxiliary.LinearAlgebra;
using ChemGo.Auxiliary.Statistics;

namespace ChemGo.Auxiliary.NumericalOptimization.MECP.SingleConstrainedOptimization.SingleLagrangianNewtonSpace
{
    class SingleSimpleLagrangianNewtonApplication
    {
        //声明一个指向函数的委托
        public delegate void Delegate_GainingExternalData(DelegateCalculationParameter delegateCalculationParameter, out DelegateCalculationData delegateCalculationData);
        Delegate_GainingExternalData delegate_GainExternalData;
        private DelegateCalculationParameter delegateCalculationParameter;                             //委托计算参量
        private DelegateCalculationData delegateCalculationData;                                       //委托计算数据
        
        private LagrangianNewton_InputStruct inputStruct;        
        private LagrangianNewton_OutputStruct outputStruct;
                                                                        //迭代次数

        public LagrangianNewton_OutputStruct OutputStruct { get => outputStruct; set => outputStruct = value; }

        public SingleSimpleLagrangianNewtonApplication(LagrangianNewton_InputStruct inputStruct, ref LagrangianNewton_OutputStruct outputStruct, Delegate_GainingExternalData delegate_GainExternalData)
        {
            this.inputStruct = inputStruct;
            this.outputStruct = outputStruct;
            this.outputStruct.timeI = 0;
            this.outputStruct.records = new List<List<string>>();
            this.delegate_GainExternalData = delegate_GainExternalData;
        }


        

        public void Run()
        {
            ObtainFirstDelegateCalculationParameter(0);                                                              //获取第一次委托计算的参数
            delegate_GainExternalData(delegateCalculationParameter, out delegateCalculationData);                    //委托计算

            ObtainOutputStructFromDelegateCalculationData();

            //判断是否收敛
            SingleSimpleLagrangianNewton_0000_JudgeIsTermination isTerminationApp =
                new SingleSimpleLagrangianNewton_0000_JudgeIsTermination(inputStruct.lagrangianNewton_criteria, outputStruct);
            isTerminationApp.Run();
            outputStruct.isConvergence = isTerminationApp.IsConvergence;
            outputStruct.lagrangianNewton_ConvergenceInfo = isTerminationApp.LagrangianNewton_ConvergenceInfo;
            outputStruct.records.Add(isTerminationApp.Record);
            //输出
            SingleSimpleLagrangianNewton_0002_Output.Output(inputStruct, outputStruct);

            //优化循环
            int cycle = 1;                                       
            for (cycle=1; outputStruct.isConvergence == false && cycle <= inputStruct.lagrangianNewton_control.maxCyc; cycle++)
            {
                SingleSimpleLagrangianNewton_0001_Iteration iterationApp = new SingleSimpleLagrangianNewton_0001_Iteration(inputStruct, outputStruct);
                iterationApp.Run();
                ObtainInputStructFromIterationStep(iterationApp.DetParams, cycle, inputStruct.lagrangianNewton_control.stepSize);

                UpdateDelegateCalculationParameter(cycle);                                                               //更新委托计算的参数
                delegate_GainExternalData(delegateCalculationParameter, out delegateCalculationData);                    //委托计算
                ObtainOutputStructFromDelegateCalculationData();                                                         //获取委托计算的数据 

                //判断是否收敛
                isTerminationApp = new SingleSimpleLagrangianNewton_0000_JudgeIsTermination(inputStruct.lagrangianNewton_criteria, outputStruct);
                isTerminationApp.Run();
                outputStruct.isConvergence = isTerminationApp.IsConvergence;
                outputStruct.lagrangianNewton_ConvergenceInfo = isTerminationApp.LagrangianNewton_ConvergenceInfo;
                outputStruct.records.Add(isTerminationApp.Record);

                //输出
                SingleSimpleLagrangianNewton_0002_Output.Output(inputStruct, outputStruct);
            }
        }


        /// <summary>
        /// 判断是否终止计算
        /// </summary>
        /// <returns>收否终止计算</returns>
        private bool JudgeIsTermination()
        {
            bool isTerminate = false;                                                                   //是否终止
            bool constrainedValueConvergence = false;                                                   //约束条件是否收敛
            bool lagrangianForceIsConvergence = false;                                                  //拉格朗日力是否收敛

            double maxLagrangianForce = 100;               //最大拉格朗日力
            double rmsLagrangianForce = 100;               //均方根拉格朗日力

            if (outputStruct.constrainedValue<inputStruct.lagrangianNewton_criteria.criteriaConstrainedCondition)
            {
                constrainedValueConvergence = true;
            }

            maxLagrangianForce = Gadgets.MaxArray(outputStruct.constrainedGradient);
            rmsLagrangianForce = Gadgets.RMSArray(outputStruct.constrainedGradient);

            if(maxLagrangianForce<inputStruct.lagrangianNewton_criteria.criteriaMaxLagrangeForce &&
                rmsLagrangianForce<inputStruct.lagrangianNewton_criteria.criteriaRMSLagrangeForce)
            {
                lagrangianForceIsConvergence = true;
            }

            if(constrainedValueConvergence==true && lagrangianForceIsConvergence==true)
            {
                isTerminate = true;
            }

            return isTerminate;
        }

        /// <summary>
        /// 获取第一次委托计算的参数
        /// </summary>
        private void ObtainFirstDelegateCalculationParameter(int timeI)
        {
            delegateCalculationParameter.numberOfX = inputStruct.numberOfX;
            delegateCalculationParameter.x = new double[delegateCalculationParameter.numberOfX];
            for(int i=0;i< delegateCalculationParameter.numberOfX;i++)
            {
                delegateCalculationParameter.x[i] = inputStruct.x[i];
            }
            delegateCalculationParameter.timeI = timeI;
            delegateCalculationParameter.isReadGradient = true;
            delegateCalculationParameter.isReadHessian = true;
        }

        private void UpdateDelegateCalculationParameter(int timeI)
        {
            delegateCalculationData.numberOfX = inputStruct.numberOfX;
            for (int i = 0; i < delegateCalculationParameter.numberOfX; i++)
            {
                delegateCalculationParameter.x[i] = inputStruct.x[i];
            }
            delegateCalculationParameter.timeI = timeI;
            delegateCalculationParameter.isReadGradient = true;
            if(Math.IEEERemainder(timeI, inputStruct.lagrangianNewton_control.nStepHessianBeCalculated) == 0)
            {
                delegateCalculationParameter.isReadHessian = true;
            }
            else
            {
                delegateCalculationParameter.isReadHessian = false;
            }
        }

        /// <summary>
        /// 把委托计算的结果，赋值给outputStruct
        /// </summary>
        private void ObtainOutputStructFromDelegateCalculationData()
        {
            outputStruct.timeI = inputStruct.timeI;
            outputStruct.lambda = inputStruct.lambda;
            outputStruct.numberOfX = delegateCalculationData.numberOfX;
            outputStruct.x = delegateCalculationData.x;
            outputStruct.y = delegateCalculationData.y;
            outputStruct.constrainedValue = delegateCalculationData.constrainedValue;
            if(delegateCalculationData.isExistGradient)
            {
                outputStruct.gradient = delegateCalculationData.gradient;
            }
            if(delegateCalculationData.isExistHessian)
            {
                outputStruct.hessian = delegateCalculationData.hessian;
            }
            if(delegateCalculationData.isExistConstrainedGradient)
            {
                outputStruct.constrainedGradient = delegateCalculationData.constrainedGradient;
            }
            if(delegateCalculationData.isExistConstrainedHessian)
            {
                outputStruct.constrainedHessian = delegateCalculationData.constrainedHessian;
            }
        }

        /// <summary>
        /// 根据每次迭代计算，更新数据。
        /// </summary>
        /// <param name="detParam">迭代变量</param>
        /// <param name="cycle">循环次数</param>
        /// <param name="stepsize">步长</param>
        private void ObtainInputStructFromIterationStep(Vector detParam, int cycle, double stepsize)
        {
            for(int i=0;i<inputStruct.numberOfX;i++)
            {
                inputStruct.x[i] += detParam[i] * stepsize;
            }
            inputStruct.lambda = detParam[inputStruct.numberOfX];
            inputStruct.timeI = cycle;
        }
    }
}
