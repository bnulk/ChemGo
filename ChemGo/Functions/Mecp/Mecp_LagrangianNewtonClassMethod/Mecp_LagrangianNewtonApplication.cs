using System;
using System.Collections.Generic;
using System.Text;
using ChemGo.Data;
using ChemGo.Auxiliary.NumericalOptimization.DelegateData;
using ChemGo.Auxiliary.NumericalOptimization.SingleConstrainedOptimization.SingleLagrangianNewtonSpace;
using ChemGo.Auxiliary.ExternalProgram;
using ChemGo.Auxiliary.FundamentalConstants;

namespace ChemGo.Functions.Mecp.Mecp_LagrangianNewtonClassMethod
{
    partial class Mecp_LagrangianNewtonApplication
    {
        //从MecpApplication接收数据
        private Data_ChemGo data_ChemGo;
        private Output.WriteOutput mainOutput;
        //给拉格朗日牛顿法的参数
        private LagrangianNewton_InputStruct inputStruct;
        //接收拉格朗日牛顿法的结果
        private LagrangianNewton_OutputStruct outputStruct;
        //Data_ChemGo中的Mecp数据。传给MecpApplication数据
        private Data.Mecp mecp;

        /// <summary>
        /// Data_ChemGo中的Mecp数据。
        /// </summary>
        public Data.Mecp Mecp { get => mecp; set => mecp = value; }

        public Mecp_LagrangianNewtonApplication(Data_ChemGo data_ChemGo, Output.WriteOutput mainOutput)
        {
            this.data_ChemGo = data_ChemGo;
            this.mainOutput = mainOutput;
        }

        public void Run()
        {
            ObtainInputStruct();
            if(data_ChemGo.inputFile.labels.control.inputFileType!=InputFileType.ChemGo)
            {
                SingleSimpleLagrangianNewtonApplication app = new SingleSimpleLagrangianNewtonApplication(inputStruct, ref outputStruct, GainDelegateCalculationData);
                app.Run();
                //获取计算完成后的数据
                ObtainCalculationData(app.OutputStruct, ref mecp);
            }
            else
            {
                throw new MecpException("This function requires an external program. \n  " +
                    "ChemGo.Functions.Mecp.Mecp_LagrangianNewtonClassMethod.Mecp_SimpleLagrangianNewtonApplication.Run() Error.");
            }
        }


        /// <summary>
        /// 获取Lagrangian-Newton法的输入包
        /// </summary>
        private void ObtainInputStruct()
        {
            int i3;
            inputStruct.output = mainOutput;
            inputStruct.lagrangianNewton_control.initialLambda = data_ChemGo.inputFile.labels.keyword_mecp.lambda;
            inputStruct.lagrangianNewton_control.nStepGradientBeCalculated = data_ChemGo.inputFile.labels.keyword_mecp.gradientN;
            inputStruct.lagrangianNewton_control.nStepHessianBeCalculated = data_ChemGo.inputFile.labels.keyword_mecp.hessianN;
            inputStruct.lagrangianNewton_control.maxCyc = data_ChemGo.inputFile.labels.keyword_mecp.maxCyc;
            inputStruct.lagrangianNewton_control.stepSize = data_ChemGo.inputFile.labels.keyword_mecp.stepSize;
            inputStruct.lagrangianNewton_criteria.criteriaMaxLagrangeForce = data_ChemGo.inputFile.labels.keyword_mecp.maxCon;
            inputStruct.lagrangianNewton_criteria.criteriaRMSLagrangeForce = data_ChemGo.inputFile.labels.keyword_mecp.rmsCon;
            inputStruct.lagrangianNewton_criteria.criteriaConstrainedCondition = data_ChemGo.inputFile.labels.keyword_mecp.energyCon;
            if(data_ChemGo.inputFile.labels.control.coordinateType==CoordinateType.Cartesian)
            {
                inputStruct.numberOfX = data_ChemGo.inputFile.inputCartesian.numberOfAtoms * 3;
                inputStruct.x = new double[data_ChemGo.inputFile.inputCartesian.numberOfAtoms * 3];
                for(int i=0;i<data_ChemGo.inputFile.inputCartesian.numberOfAtoms;i++)
                {
                    i3 = 3 * i;
                    for(int j=0;j<3;j++)
                    {
                        inputStruct.x[i3 + j] = Convert.ToDouble(data_ChemGo.inputFile.inputCartesian.coordinates[i, j + 1]) / PhysConst.bohr2angstroms;
                    }
                }
            }
            else
            {
                inputStruct.numberOfX = data_ChemGo.inputFile.inputZmatrix.parameter.GetLength(0);
                int cycle = data_ChemGo.inputFile.inputZmatrix.parameter.GetLength(0);
                inputStruct.x = new double[cycle];
                int cycle1 = data_ChemGo.inputFile.inputZmatrix.numberOfAtoms - 1;
                for (int i=0;i<cycle1;i++)
                {
                    inputStruct.x[i] = Convert.ToDouble(data_ChemGo.inputFile.inputZmatrix.parameter[i, 1]) / PhysConst.bohr2angstroms;
                }
                double angle2Radian = PhysConst.PI / 180.0;
                for(int i=cycle1;i<cycle;i++)
                {
                    inputStruct.x[i] = Convert.ToDouble(data_ChemGo.inputFile.inputZmatrix.parameter[i, 1]) * angle2Radian;
                }
            }
            inputStruct.lambda = inputStruct.lagrangianNewton_control.initialLambda;
            inputStruct.timeI = 0;
            inputStruct.stepsize = inputStruct.lagrangianNewton_control.stepSize;
        }


        /// <summary>
        /// 获取委托计算数据，包含变量个数、变量值、梯度和Hessian矩阵
        /// </summary>
        /// <param name="data_ChemGo">ChemGo数据</param>
        /// <param name="delegateData">委托计算数据</param>
        private void GainDelegateCalculationData(ref ConstrainedOptimizationDelegateData delegateData)
        {
            MecpServer mecpServer = new MecpServer(data_ChemGo, delegateData);
            mecpServer.Run();
            delegateData.result = mecpServer.DelegateData.result;         
        }

        /// <summary>
        /// 获取计算完成后的数据
        /// </summary>
        /// <param name="outputStruct">“数值计算空间”的输出结构</param>
        /// <param name="mecp">mecp数据</param>
        private void ObtainCalculationData(LagrangianNewton_OutputStruct outputStruct, ref Data.Mecp mecp)
        {
            mecp.lambda = outputStruct.lambda;
            mecp.numberOfAtoms = data_ChemGo.singlePoint.numberOfAtoms;
            mecp.numberOfX = outputStruct.numberOfX;
            mecp.isConvergence = outputStruct.isConvergence;
            mecp.energy1 = outputStruct.y;
            mecp.energy2 = outputStruct.y - outputStruct.constrainedValue;
            switch(data_ChemGo.inputFile.labels.control.coordinateType)
            {
                case CoordinateType.zMatrix:
                    mecp.coordinateType = CoordinateType.zMatrix;
                    break;
                case CoordinateType.Cartesian:
                    mecp.coordinateType = CoordinateType.Cartesian;
                    break;
                default:
                    throw new MecpException("unknown InputFileType. \n " +
                        "@ChemGo.Functions.Mecp.Mecp_LagrangianNewtonClassMethod.Mecp_LagrangianNewtonApplication; ObtainCalculationData(LagrangianNewton_OutputStruct outputStruct, ref Data.Mecp mecp):site1 CoordinateType Error");
            }
            
            if(mecp.coordinateType==CoordinateType.zMatrix)
            {
                mecp.zMatrix = data_ChemGo.singlePoint.zMatrix;
                mecp.zMatrix.parameterName = new string[mecp.numberOfX];
                mecp.derivativeInfo_ZMatrix1.coordinates = new double[mecp.numberOfX];
                mecp.derivativeInfo_ZMatrix2.coordinates = new double[mecp.numberOfX];
                mecp.derivativeInfo_ZMatrix1.gradient = new double[mecp.numberOfX];
                mecp.derivativeInfo_ZMatrix2.gradient = new double[mecp.numberOfX];
                mecp.derivativeInfo_ZMatrix1.forceConstants = new double[mecp.numberOfX, mecp.numberOfX];
                mecp.derivativeInfo_ZMatrix2.forceConstants = new double[mecp.numberOfX, mecp.numberOfX];

                for (int i=0;i<mecp.numberOfX;i++)
                {
                    mecp.zMatrix.parameterName[i] = data_ChemGo.singlePoint.zMatrix.parameterName[i];
                    mecp.zMatrix.parameterValue[i] = outputStruct.x[i];
                    mecp.derivativeInfo_ZMatrix1.coordinates[i] = outputStruct.x[i];
                    mecp.derivativeInfo_ZMatrix2.coordinates[i] = outputStruct.x[i];
                    mecp.derivativeInfo_ZMatrix1.gradient[i] = outputStruct.gradient[i];
                    mecp.derivativeInfo_ZMatrix2.gradient[i] = outputStruct.gradient[i] - outputStruct.constrainedGradient[i];
                    for(int j=0;j<mecp.numberOfX;j++)
                    {
                        mecp.derivativeInfo_ZMatrix1.forceConstants[i, j] = outputStruct.hessian[i, j];
                        mecp.derivativeInfo_ZMatrix2.forceConstants[i, j] = outputStruct.hessian[i, j] - outputStruct.constrainedHessian[i, j];
                    }
                }
            }
            if(mecp.coordinateType==CoordinateType.Cartesian)
            {
                mecp.geometry = data_ChemGo.singlePoint.geometry;

                mecp.geometry.standardOrientationCoordinates = new double[mecp.numberOfX, 3];
                mecp.derivativeInfo_Cartesian1.coordinates = new double[mecp.numberOfX];
                mecp.derivativeInfo_Cartesian2.coordinates = new double[mecp.numberOfX];
                mecp.derivativeInfo_Cartesian1.gradient = new double[mecp.numberOfX];
                mecp.derivativeInfo_Cartesian2.gradient = new double[mecp.numberOfX];
                mecp.derivativeInfo_Cartesian1.forceConstants = new double[mecp.numberOfX, mecp.numberOfX];
                mecp.derivativeInfo_Cartesian2.forceConstants = new double[mecp.numberOfX, mecp.numberOfX];

                int i3 = 0;
                for(int i=0;i<data_ChemGo.singlePoint.geometry.numberOfAtoms;i++)
                {
                    i3 = 3 * i;
                    mecp.geometry.standardOrientationCoordinates[i, 0] = outputStruct.x[i3];
                    mecp.geometry.standardOrientationCoordinates[i, 1] = outputStruct.x[i3 + 1];
                    mecp.geometry.standardOrientationCoordinates[i, 2] = outputStruct.x[i3 + 2];
                }

                for (int i = 0; i < mecp.numberOfX; i++)
                {
                    mecp.derivativeInfo_Cartesian1.coordinates[i] = outputStruct.x[i];
                    mecp.derivativeInfo_Cartesian1.gradient[i] = outputStruct.gradient[i];
                    mecp.derivativeInfo_Cartesian2.gradient[i] = outputStruct.gradient[i] - outputStruct.constrainedGradient[i];
                    for (int j = 0; j < mecp.numberOfX; j++)
                    {
                        mecp.derivativeInfo_Cartesian1.forceConstants[i, j] = outputStruct.hessian[i, j];
                        mecp.derivativeInfo_Cartesian2.forceConstants[i, j] = outputStruct.hessian[i, j] - outputStruct.constrainedHessian[i, j];
                    }
                }
            }
        }
    }
}
