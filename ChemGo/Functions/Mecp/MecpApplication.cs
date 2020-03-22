using System;
using System.Collections.Generic;
using System.Text;
using ChemGo.Data;

namespace ChemGo.Functions.Mecp
{
    class MecpApplication
    {
        private Data_ChemGo data_ChemGo;
        private Output.WriteOutput mainOutput;
        private bool isTermination;
        private ChemGo.Data.Mecp mecp;
        private ChemGo.Data.SinglePoint singlepoint;


        public bool IsTermination { get => isTermination; set => isTermination = value; }
        public Data.Mecp Mecp { get => mecp; set => mecp = value; }
        public SinglePoint Singlepoint { get => singlepoint; set => singlepoint = value; }

        public MecpApplication(Data_ChemGo data_ChemGo, Output.WriteOutput mainOutput)
        {
            this.data_ChemGo = data_ChemGo;
            this.mainOutput = mainOutput;
            IsTermination = false;
        }

        public void Run()
        {
            //根据优化方法运行
            RunningBasedOnOptimizationOptions();
        }


        /// <summary>
        /// 运行优化MECP
        /// </summary>
        private void RunningBasedOnOptimizationOptions()
        {
            switch (data_ChemGo.inputFile.labels.keyword_mecp.opt.ToLower())
            {
                case "ln":
                    Mecp_LagrangianNewtonClassMethod.Mecp_LagrangianNewtonApplication app = new Mecp_LagrangianNewtonClassMethod.Mecp_LagrangianNewtonApplication(data_ChemGo, mainOutput);
                    app.Run();
                    mecp = app.Mecp;
                    break;
                default:
                    throw new MecpException("unknown MECP opt parameter. \n ChemGo.Functions.Mecp.OptimizeMecp() Error.");
            }
        }
    }
}
