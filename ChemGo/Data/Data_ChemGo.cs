using System;
using System.Collections.Generic;
using System.Text;

namespace ChemGo.Data
{
    class Data_ChemGo
    {
        /// <summary>
        /// 命令行信息
        /// </summary>
        public CommandLineInformation commandLineInformation;
        /// <summary>
        /// 标签合集
        /// </summary>
        public Labels labels;
        /// <summary>
        /// 控制部分
        /// </summary>
        public Control control;
        /// <summary>
        /// 输入文件中的内坐标
        /// </summary>
        public InputZmatrix inputZmatrix;
        /// <summary>
        /// 几何构型
        /// </summary>
        public Geometry geometry;
        /// <summary>
        /// 梯度综合信息
        /// </summary>
        public DerivativeInfo derivativeInfo;
        /// <summary>
        /// 自洽场能量
        /// </summary>
        public double scfEnergy;
        /// <summary>
        /// 总能量
        /// </summary>
        public double energy;
        /// <summary>
        /// MECP关键词合集
        /// </summary>
        public Mecp mecp;
    }
}
