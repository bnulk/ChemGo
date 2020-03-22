using System;
using System.Collections.Generic;
using System.Text;

namespace ChemGo.Auxiliary.LinearAlgebra
{
    class EigenEquation
    {
        /// <summary>
        /// 用Household变换和QL分解，计算实对称矩阵本征值和本征向量。
        /// </summary>
        /// <param name="A">实对称矩阵，返回本征向量</param>
        /// <param name="eigenValue">本征值</param>
        public static void householdAndQL(ref Matrix A, out Vector eigenValue)
        {
            int n = A.row;
            eigenValue = new Vector(n);
            Vector e = new Vector(n);

            Matrix.tred2(ref A, ref eigenValue, ref e);
            Matrix.tqli(ref eigenValue, ref e, ref A);
        }
    }
}
