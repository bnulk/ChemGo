using System;
using System.Collections.Generic;
using System.Text;
using ChemGo.Data;
using ChemGo.Auxiliary.NumericalOptimization.DelegateData;
using ChemGo.Auxiliary.ExternalProgram.Gaussian.GaussianService.GaussianMecpService;

namespace ChemGo.Auxiliary.ExternalProgram
{
    class MecpServer
    {
        private Data_ChemGo data_ChemGo;
        private ConstrainedOptimizationDelegateData delegateData;

        public ConstrainedOptimizationDelegateData DelegateData { get => delegateData; set => delegateData = value; }


        public MecpServer(Data_ChemGo data_ChemGo, ConstrainedOptimizationDelegateData delegateData)
        {
            this.data_ChemGo = data_ChemGo;
            this.delegateData = delegateData;
        }

        

        public void Run()
        {
            switch(data_ChemGo.commandLineInformation.inputFileType)
            {
                case InputFileType.Gaussian:
                    GaussianMecpServiceApplication app = new GaussianMecpServiceApplication(data_ChemGo, delegateData);
                    app.Run();
                    delegateData = app.DelegateData;
                    break;
                default:
                    throw new ExternalProgramException("Currently, MECP calculation is only supported by Gaussian program. \n ChemGo.Auxiliary.ExternalProgram.MecpServer.Run()");
            }

        }


    }
}
