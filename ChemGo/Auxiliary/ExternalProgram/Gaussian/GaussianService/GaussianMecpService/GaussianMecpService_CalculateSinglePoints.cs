using System;
using System.Collections.Generic;
using System.Text;
using ChemGo.Data;
using ChemGo.Auxiliary.ExternalProgram.Gaussian.GaussianRun;

namespace ChemGo.Auxiliary.ExternalProgram.Gaussian.GaussianService.GaussianMecpService
{
    class GaussianMecpService_CalculateSinglePoints
    {
        private Data_ChemGo data_ChemGo;
        private GaussianMecpVariablePackage variablePackage;

        public GaussianMecpService_CalculateSinglePoints(Data_ChemGo data_ChemGo, GaussianMecpVariablePackage variablePackage)
        {
            this.data_ChemGo = data_ChemGo;
            this.variablePackage = variablePackage;
        }


        public void Run()
        {
            RunningGaussian runningGaussian = new RunningGaussian(data_ChemGo.otherProgramData.data_Gaussian.interfaceBetweenGaussianAndChemGo.cmd,
                variablePackage.gjf1FullPath, variablePackage.out1FullPath);
            runningGaussian.Run();

            runningGaussian=new RunningGaussian(data_ChemGo.otherProgramData.data_Gaussian.interfaceBetweenGaussianAndChemGo.cmd,
                variablePackage.gjf2FullPath, variablePackage.out2FullPath);
            runningGaussian.Run();
        }

    }
}
