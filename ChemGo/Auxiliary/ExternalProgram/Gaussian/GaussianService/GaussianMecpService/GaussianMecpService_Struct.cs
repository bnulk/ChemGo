using System;
using System.Collections.Generic;
using System.Text;

namespace ChemGo.Auxiliary.ExternalProgram.Gaussian.GaussianService.GaussianMecpService
{
    /// <summary>
    /// 输入信息
    /// </summary>
    public struct GaussianMecpVariablePackage
    {
        public int timeI;
        public bool isCalculateHessian;
        /// <summary>
        /// 建立tmp目录的物理地址
        /// </summary>
        public string workPath;
        public string gjf1FullPath;
        public string gjf2FullPath;
        public string out1FullPath;
        public string out2FullPath;
    }
}
