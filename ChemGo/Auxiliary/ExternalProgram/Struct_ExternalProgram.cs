

namespace ChemGo.Auxiliary.ExternalProgram
{
    /// <summary>
    /// 输出优化任务的信息包
    /// </summary>
    public struct OptimizationPackage
    {
        public int numberOfPara;
        public double[] x;
        public double energy;        
        public double[] gradient;
        public double[,] hessian;
    }

    public struct MecpPackage
    {
        public int numberOfPara;
        public double[] x;
        public double energy1;
        public double energy2;        
        public double[] gradient1;
        public double[,] hessian1;
        public double[] gradient2;
        public double[,] hessian2;
    }

}