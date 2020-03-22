using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ChemGo.Auxiliary.NumericalOptimization.DelegateData;
using ChemGo.Auxiliary.FundamentalConstants;
using ChemGo.Data;

namespace ChemGo.Auxiliary.ExternalProgram.Gaussian.GaussianService.GaussianMecpService
{
    class GaussianMecpServiceApplication
    {
        private Data_ChemGo data_ChemGo;
        private GaussianInputFileMaterial[] gaussianInputFileMaterial = new GaussianInputFileMaterial[2];
        private GaussianMecpVariablePackage variablePackage;

        private ConstrainedOptimizationDelegateData delegateData;

        /// <summary>
        /// 约束优化的数据包
        /// </summary>
        public ConstrainedOptimizationDelegateData DelegateData { get => delegateData; set => delegateData = value; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data_ChemGo"></param>
        /// <param name="delegateCalculationParameter"></param>
        public GaussianMecpServiceApplication(Data_ChemGo data_ChemGo, ConstrainedOptimizationDelegateData delegateData)
        {
            this.data_ChemGo = data_ChemGo;
            this.delegateData = delegateData;
            ObtainGaussianInputFileMaterial();
        }


        public void Run()
        {
            delegateData.result.numberOfX = delegateData.parameter.numberOfX;
            delegateData.result.x = delegateData.parameter.x;

            if(delegateData.parameter.timeI!=0)
            {
                UpdateGaussianInputFileMaterial();
            }
            ObtainVariablePackage();
            CreateInputFile();
            RunGaussianCalculation();
            ReadGaussianOutputData2DelegateCalculationData(data_ChemGo);
        }



        /// <summary>
        /// 获取高斯输入文件材料
        /// </summary>
        private void ObtainGaussianInputFileMaterial()
        {
            GaussianMecpService_GenerateInputFileMaterial generateInputFileMaterial_Mecp =
                new GaussianMecpService_GenerateInputFileMaterial(data_ChemGo.otherProgramData.data_Gaussian.gaussianInputSegment,
                data_ChemGo.otherProgramData.data_Gaussian.interfaceBetweenGaussianAndChemGo, data_ChemGo.inputFile);
            generateInputFileMaterial_Mecp.Run();
            gaussianInputFileMaterial[0] = generateInputFileMaterial_Mecp.GaussianInputFileMaterial[0];
            gaussianInputFileMaterial[1] = generateInputFileMaterial_Mecp.GaussianInputFileMaterial[1];
        }

        /// <summary>
        /// 更新高斯输入文件材料中的变量
        /// </summary>
        private void UpdateGaussianInputFileMaterial()
        {
            if(gaussianInputFileMaterial[0].coordinateType==CoordinateType.zMatrix)
            {
                int numberOfValue = gaussianInputFileMaterial[0].molecularPara_ZMatrix_Value.Length;
                int cycle1 = gaussianInputFileMaterial[0].numberOfAtom - 1;
                double radian2Angle = 180.0 / PhysConst.PI;
                for (int i=0;i<cycle1;i++)
                {
                    gaussianInputFileMaterial[0].molecularPara_ZMatrix_Value[i] = gaussianInputFileMaterial[1].molecularPara_ZMatrix_Value[i] = delegateData.parameter.x[i] * PhysConst.bohr2angstroms;
                }
                for(int i=cycle1;i<numberOfValue;i++)
                {
                    gaussianInputFileMaterial[0].molecularPara_ZMatrix_Value[i] = gaussianInputFileMaterial[1].molecularPara_ZMatrix_Value[i] = delegateData.parameter.x[i] * radian2Angle;
;               }
            }
            else
            {
                int numberOfValue = gaussianInputFileMaterial[0].numberOfAtom;
                int tripleI;
                for(int i=0;i<numberOfValue;i++)
                {
                    tripleI = 3 * i;
                    for(int j=0;j<3;j++)
                    {
                        gaussianInputFileMaterial[0].molecularCartesian_Value[i, j] = gaussianInputFileMaterial[1].molecularCartesian_Value[i, j] = delegateData.parameter.x[tripleI + j] * PhysConst.bohr2angstroms;
                    }
                }
            }            
        }


        /// <summary>
        /// 获取计算的临时包
        /// </summary>
        private void ObtainVariablePackage()
        {
            variablePackage.timeI = delegateData.parameter.timeI;
            
            variablePackage.isCalculateHessian = delegateData.parameter.isCalcConstraintHessian;
            if (OperatingSystem.OS_BasisFunction.ObtianOSName() == "windows")
            {
                variablePackage.gjf1FullPath = data_ChemGo.commandLineInformation.inputFileDirectory + "\\tmp\\"
                     + "State1_" + variablePackage.timeI.ToString() + ".gjf";
                variablePackage.gjf2FullPath = data_ChemGo.commandLineInformation.inputFileDirectory + "\\tmp\\"
                    + "State2_" + variablePackage.timeI.ToString() + ".gjf";
                variablePackage.out1FullPath = data_ChemGo.commandLineInformation.inputFileDirectory + "\\tmp\\"
                     + "State1_" + variablePackage.timeI.ToString() + ".out";
                variablePackage.out2FullPath = data_ChemGo.commandLineInformation.inputFileDirectory + "\\tmp\\"
                    + "State2_" + variablePackage.timeI.ToString() + ".out";
            }
            else
            {
                variablePackage.gjf1FullPath = data_ChemGo.commandLineInformation.inputFileDirectory + "//tmp//"
                     + "State1_" + variablePackage.timeI.ToString() + ".gjf";
                variablePackage.gjf2FullPath = data_ChemGo.commandLineInformation.inputFileDirectory + "//tmp//"
                    + "State2_" + variablePackage.timeI.ToString() + ".gjf";
                variablePackage.out1FullPath = data_ChemGo.commandLineInformation.inputFileDirectory + "//tmp//"
                     + "State1_" + variablePackage.timeI.ToString() + ".out";
                variablePackage.out2FullPath = data_ChemGo.commandLineInformation.inputFileDirectory + "//tmp//"
                    + "State2_" + variablePackage.timeI.ToString() + ".out";
            }
            variablePackage.workPath = Path.GetDirectoryName(variablePackage.gjf1FullPath);
        }


        private void CreateInputFile()
        {
            GaussianMecpService_CreateInputFile app = new GaussianMecpService_CreateInputFile(variablePackage, gaussianInputFileMaterial);
            app.Run();
        }

        private void RunGaussianCalculation()
        {
            GaussianMecpService_CalculateSinglePoints app = new GaussianMecpService_CalculateSinglePoints(data_ChemGo, variablePackage);
            app.Run();
        }

        private void ReadGaussianOutputData2DelegateCalculationData(Data_ChemGo data_ChemGo)
        {
            GaussianMecpService_ObtainCalculatingData app = new GaussianMecpService_ObtainCalculatingData(variablePackage, data_ChemGo);
            app.Run();


            delegateData.result.numberOfX = app.MecpPackage.numberOfX;
            delegateData.result.isReadGradient = true;
            delegateData.result.isReadConstraintGradient = true;

            delegateData.result.x = new double[delegateData.result.numberOfX];
            delegateData.result.gradient = new double[delegateData.result.numberOfX];
            delegateData.result.constraintGradient = new List<double[]>();
            delegateData.result.constraintGradient.Add(new double[delegateData.result.numberOfX]);
            delegateData.result.hessian = new double[delegateData.result.numberOfX, delegateData.result.numberOfX];
            delegateData.result.constraintHessian = new List<double[,]>();
            delegateData.result.constraintHessian.Add(new double[delegateData.result.numberOfX, delegateData.result.numberOfX]);


            for (int i = 0; i < app.MecpPackage.numberOfX; i++) 
            {
                delegateData.result.x[i] = app.MecpPackage.x[i];
                delegateData.result.y = app.MecpPackage.energy1;
                delegateData.result.constraintFunctionValue = app.MecpPackage.energy1 - app.MecpPackage.energy2;
                delegateData.result.gradient[i] = app.MecpPackage.gradient1[i];
                delegateData.result.constraintGradient[0][i] = app.MecpPackage.gradient1[i] - app.MecpPackage.gradient2[i];
            }

            if (variablePackage.isCalculateHessian==true)
            {
                delegateData.result.isReadHessian = true;
                delegateData.result.isReadConstraintHessian = true;

                for(int i=0;i<app.MecpPackage.numberOfX;i++)
                {
                    for (int j=0;j<app.MecpPackage.numberOfX;j++)
                    {
                        delegateData.result.hessian[i, j] = app.MecpPackage.hessian1[i, j];
                        delegateData.result.constraintHessian[0][i, j] = app.MecpPackage.hessian1[i, j] - app.MecpPackage.hessian2[i, j];
                    }
                }
            }
            else
            {
                delegateData.result.isReadHessian = false;
                delegateData.result.isReadConstraintHessian = false;
            }

        }
    }
}
