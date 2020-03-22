using System.Text;
using ChemGo.Data;
using ChemGo.Functions.Mecp;

namespace ChemGo.Drive
{
    partial class Drive_0101_Mecp
    {
        private Data_ChemGo data_ChemGo;
        private Output.WriteOutput mainOutput;

        private Mecp mecp;
        private SinglePoint singlePoint;

        public Mecp Mecp { get => mecp; set => mecp = value; }
        public SinglePoint SinglePoint { get => singlePoint; set => singlePoint = value; }

        public Drive_0101_Mecp(Data_ChemGo data_ChemGo, Output.WriteOutput mainOutput)
        {
            this.data_ChemGo = data_ChemGo;
            this.mainOutput = mainOutput;

            StringBuilder result = new StringBuilder();
            result.Append("bnulk@foxmail.com-Mecp" + "\n");
            result.Append("*********************************************" + "\n\n");

            mainOutput.WriteOutputStr(result);
        }
        

        public void Run()
        {
            try
            {
                //运行程序
                MecpApplication app = new MecpApplication(data_ChemGo, mainOutput);
                app.Run();
                Mecp = app.Mecp;
            }
            catch (MecpException e)
            {
                mainOutput.WriteOutputStr(e.Message);
                return;
            }
        }
    }
}
