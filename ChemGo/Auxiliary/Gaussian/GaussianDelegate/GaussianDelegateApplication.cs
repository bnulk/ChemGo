using System;
using System.Collections.Generic;
using System.Text;

namespace ChemGo.Auxiliary.Gaussian.GaussianDelegate
{
    partial class GaussianDelegateApplication
    {
        private string fullFilePath;
        private GaussianInputFileMaterial gaussianInputFileMaterial;

        //声明一个指向函数的委托
        public delegate void DelegateOptimizationPackage();

        public GaussianDelegateApplication(string fullFilePath, GaussianInputFileMaterial gaussianInputFileMaterial)
        {
            this.gaussianInputFileMaterial = gaussianInputFileMaterial;
            this.fullFilePath = fullFilePath;
        }

        
        
    }
}
