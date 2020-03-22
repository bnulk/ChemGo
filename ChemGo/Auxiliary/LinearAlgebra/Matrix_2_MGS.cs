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
       日期：  2019-03-18

       描述：
           * 修正的Gram-Schmidt正交化方法
       结构：
           * 
       方法：
           * MGS(Matrix A, out Matrix Q, out Matrix R)
       代码来源：
           * C#科学计算讲义，P142
       ----------------------------------------------------  类注释  结束----------------------------------------------------
       */

        /// <summary>
        /// 修正的Gram-Schmidt正交化方法
        /// </summary>
        /// <param name="A">原矩阵</param>
        /// <param name="Q">正交基组成的向量空间</param>
        /// <param name="R">方阵</param>
        public static void MGS(Matrix A, out Matrix Q, out Matrix R)
        {
            //获取A矩阵维数
            int M, N;
            M = A.row;
            N = A.column;

            //为Q和R分配地址
            Q = new Matrix(M, N);
            R = new Matrix(N, N);

            Vector vtmp = new Vector(M);

            double s = 0.0;

            int i, j;

            for(i=0;i<M;i++)
            {
                s = s + A[i, 0] * A[i, 0];
            }

            R[0, 0] = Math.Sqrt(s);

            for(i=0;i<M;i++)
            {
                Q[i, 0] = A[i, 0] / R[0, 0];
            }

            for(int k=1;k<N;k++)
            {
                for(j=0;j<=k-1;j++)
                {
                    s = 0.0;
                    for(int p=0;p<M;p++)
                    {
                        s = s + Q[p, j] * A[p, k];
                    }
                    R[j, k] = s;
                }

                for (i = 0; i < M; i++)
                {
                    vtmp[i] = A[i, k];
                }

                for(j=0;j<=k-1;j++)
                {
                    for(i=0;i<M;i++)
                    {
                        vtmp[i] = vtmp[i] - Q[i, j] * R[j, k];
                    }
                }
                //注意：这里是对分量操作


                //这里使用了向量类的运算符重载，~用于求模的平方。
                R[k, k] = Math.Sqrt(~vtmp);

                for(i=0;i<M;i++)
                {
                    Q[i, k] = vtmp[i] / R[k, k];
                }
            
            }
        }



    }
}
