using System;
using System.Collections.Generic;
using System.Text;

namespace ChemGo.Auxiliary.ExternalProgram.Gaussian.GaussianOutput
{

    struct HessianPackage
    {
        public int numberOfX;        
        public double energy;
        public string[] xName;
        public double[] x;
        public double[] gradient;
        public double[,] hessian;
    }

    struct GradientPackage
    {
        public int numberOfX;
        public double energy;
        public string[] xName;
        public double[] x;
        public double[] gradient;
    }
}
