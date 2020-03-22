using System;
using System.Collections.Generic;
using System.Text;

namespace ChemGo.Auxiliary.LinearAlgebra
{
    public partial class Matrix
    {
        /*
        ----------------------------------------------------  类注释  开始----------------------------------------------------
        版本：  V1.0
        作者：  刘鲲
        日期：  2019-12-16

        描述：
            * 用完全主元法的Gauss-Jordan消去法求逆阵
            * 
        结构：
            * 
        方法：
            * Matrix InverseMatrix_GaussJ(Matrix originA)。 originA是输入矩阵，返回其逆矩阵。
        代码来源：
            * c++数值计算(第二版)
            * gaussj(ref Matrix a, ref Vector b)  a是输入矩阵，b为包含m个右端向量的输入矩阵。输出时，a被其逆矩阵代替，b为相应的解向量代替。
        ----------------------------------------------------  类注释  结束----------------------------------------------------
        */

        /// <summary>
        /// 用Gauss-Jordan消去法求逆阵
        /// </summary>
        /// <param name="a">输入矩阵,返回其逆阵</param>
		public static void InverseMatrix_GaussJ(ref Matrix a)
        {
            int i, j, k, l, ll, irow = 0, icol = 0;
            double big, pivinv, dum;

            int n = a.row;

            int[] ipiv = new int[n];
            int[] indxr = new int[n];
            int[] indxc = new int[n];                                //这些整型数组用于记录主元。

            for (j = 0; j < n; j++)
            {
                ipiv[j] = 0;
            }

            for (i = 0; i < n; i++)                                  //约化列的主循环
            {
                big = 0.0;
                for (j = 0; j < n; j++)                              //用于寻找主元素的外层循环
                {
                    if (ipiv[j] != 1)
                    {
                        for (k = 0; k < n; k++)
                        {
                            if (ipiv[k] == 0)
                            {
                                if (Math.Abs(a[j,k]) >= big)
                                {
                                    big = Math.Abs(a[j, k]);
                                    irow = j;
                                    icol = k;
                                }
                                else if (ipiv[k] > 1)
                                {
                                    throw new LinearAlgebraException("Matrix_3_InverseMatrix.gaussj died. Error. singular matrix in gaussj process. :: Site 1" + " / n");
                                }
                            }
                        }
                    }
                }
                ipiv[icol] = ipiv[icol] + 1;

                //至此我们已经求得主元素，如果需要，进行行交换把主元素放到对角线上。列并不进行实际交换，只是进行重新标注：
                //indxc[i]为第i+1个主元素所在的列，即被约化的第i+1列；而indxr[i]是主元素原来所在的行，如果indxr[i]不等于indxc[i],意味着要进行列变换。
                //用这种形式记录，解集b最终的次序是正确的，而逆矩阵的列次序被打乱。

                if (irow != icol)
                {
                    for (l = 0; l < n; l++)
                    {
                        Util.SWAP(ref a.ele[irow, l], ref a.ele[icol, l]);
                    }
                }
                indxr[i] = irow;                                                //用位于irow行icol列的主元素去除其所在的行
                indxc[i] = icol;
                if (a[icol,icol] == 0.0)
                {
                    throw new LinearAlgebraException("CalGaussJordan died. Error. singular matrix in gaussj process. :: Site 2" + "/n");
                }
                pivinv = 1.0 / (a[icol,icol]);
                a[icol,icol] = 1.0;
                for (l = 0; l < n; l++)
                {
                    a[icol,l] = a[icol,l] * pivinv;
                }
                for (ll = 0; ll < n; ll++)             //下面进行行约化
                {
                    if (ll != icol)                    //……当然主元素除外
                    {
                        dum = a[ll, icol];
                        a[ll, icol] = 0.0;
                        for (l = 0; l < n ; l++)
                        {
                            a[ll,l] = a[ll, l] - a[icol, l] * dum;
                        }
                    }
                }
            }
            //这是列约化主循环的结尾。考虑已进行过列交换，为使解向量保持原来的次序，再根据其交换的相反顺序交换各列，
            //以整理得到原始方程的解。
            for (l = n - 1; l >= 0; l--)
            {
                if (indxr[l] != indxc[l])
                {
                    for (k = 0; k < n; k++)
                    {
                        Util.SWAP(ref a.ele[k, indxr[l]], ref a.ele[k, indxc[l]]);
                    }
                }
            }
            //程序结束
        }

    }
}
