using System.Text;
using ChemGo.Data;
using ChemGo.Functions.DisposeInputCoordinate;

namespace ChemGo.Drive
{
    partial class Drive_0002_DisposeInputCoordinate
    {
        private InputFile inputFile;
        private Data_ChemGo data_ChemGo;
        private Output.WriteOutput mainOutput;

        private int numberOfAtoms;
        private Geometry geometry;
        private ZMatrix zMatrix;

        public Drive_0002_DisposeInputCoordinate(InputFile inputFile, Data_ChemGo data_ChemGo, Output.WriteOutput mainOutput)
        {
            this.inputFile = inputFile;
            this.data_ChemGo = data_ChemGo;
            this.mainOutput = mainOutput;

            StringBuilder result = new StringBuilder();
            result.Append("bnulk@foxmail.com-DisposeInputCoordinate" + "\n");
            result.Append("*********************************************" + "\n\n");
            mainOutput.WriteOutputStr(result);
        }

        public void Run()
        {
            try
            {
                //运行程序
                DisposeInputCoordinateApplication app = new DisposeInputCoordinateApplication(inputFile);
                app.Run(); ;
                //拿到运行应用的数据
                numberOfAtoms = app.NumberOfAtoms;
                geometry = app.Geometry;
                zMatrix = app.ZMatrix;
            }
            catch (DisposeInputCoordinateException e)
            {
                mainOutput.WriteOutputStr(e.Message);
            }
        }
    }
}
