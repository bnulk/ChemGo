using System;
using System.Collections.Generic;
using System.Text;

namespace ChemGo.Data.DataGaussian
{
    /// <summary>
    /// 和ChemGo衔接的关键词
    /// </summary>
    public struct InterfaceBetweenGaussianAndChemGo
    {
        public Task task;
        public string cmd;
        public string[] segment;
    }

    /// <summary>
    /// 高斯程序的输入文件
    /// </summary>
    public struct GaussianInputSegment
    {
        public int N;
        public List<string> firstSection;
        public List<string> routeSection;
        public List<string> titleSection;
        public string chargeAndMultiplicity;
        public string coordinateType;
        public List<string> molecularSpecification_ZMatrix;
        public List<string> molecularPara_ZMatrix;
        public List<string> molecularCartesian;
        public List<string> addition;
    }


}
