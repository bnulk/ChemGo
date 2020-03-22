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
        日期：  2018-09-04

        描述：
            * 通过Householder镜像变换方法实现QR分解
        结构：
            * 
        方法：
            * qrdcmp(BnulkMatrix A, out BnulkMatrix Q, out BnulkMatrix R)
        代码来源：
            * C#科学计算讲义，P135
        ----------------------------------------------------  类注释  结束----------------------------------------------------
        */

        public static void qrdcmp(Matrix A, out Matrix Q, out Matrix R)
        {
            int m = A.row;
            int n = A.column;

            Q = new Matrix(m, m);
            R = new Matrix(m, n);

            Matrix H0 = new Matrix(m, m);
            Matrix H1 = new Matrix(m, m);
            Matrix H2 = new Matrix(m, m);

            Matrix A1 = new Matrix(m, n);
            Matrix A2 = new Matrix(m, n);
            Vector u = new Vector(m);

            int i, j;
            double s, du;

            for(i=0;i<m;i++)
            {
                for(j=0;j<n;j++)
                {
                    A1.ele[i, j] = A.ele[i, j];
                }
            }

            //设置H1为单位矩阵
            for (i = 0; i < m; i++)
            {
                for (j = 0; j < m; j++)
                {
                    H1.ele[i, j] = 0.0;
                }
            }
            for (j = 0; j < m; j++)
            {
                H1.ele[j, j] = 1.0;
            }

            for(int k=0;k<n;k++)
            //k表示所有的列
            {
                //设置H0为单位矩阵
                for (i = 0; i < m; i++)
                {
                    for (j = 0; j < m; j++)
                    {
                        H0.ele[i, j] = 0.0;
                    }
                }
                for (j = 0; j < m; j++)
                {
                    H0.ele[j, j] = 1.0;
                }

                s = 0.0;
                for(i=k;i<m;i++)
                {
                    s = s + A1[i, k] * A1[i, k];
                }
                s = Math.Sqrt(s);

                //这段甚为重要，关系到数值稳定性
                //目的是使u的范数尽可能大
                //原则是：如果首元素大于0，则u第一个分量为 正+正
                //如果首元素小于0，则u的第一个分量为 负+负 （更大的负数）
                for (i = 0; i < m; i++)
                {
                    u[i] = 0.0;
                }

                if(A[k,k]>=0.0)
                {
                    u[k] = A1[k, k] + s;
                }
                else
                {
                    u[k] = A1[k, k] - s;
                }

                for(i=k+1;i<m;i++)
                {
                    u[i] = A1[i, k];
                }

                //u的2范数平方，这里引用向量类的范数运算符重载
                du = ~u;

                //计算得到大的H矩阵
                for(i=k;i<m;i++)
                {
                    for(j=k;j<m;j++)
                    {
                        H0[i, j] = -2.0 * u[i] * u[j] / du;
                        if(i==j)
                        {
                            H0[i, j] = 1.0 + H0[i, j];
                        }
                    }
                }

                A2 = H0 * A1;

                for (i = 0; i < m; i++)
                {
                    for (j = 0; j < n; j++)
                    {
                        A1.ele[i, j] = A2.ele[i, j];
                    }
                }

                H1 = H1 * H0;
            }

            for (i = 0; i < m; i++)
            {
                for (j = 0; j < m; j++)
                {
                    Q.ele[i, j] = H1.ele[i, j];
                }
            }
            for (i = 0; i < m; i++)
            {
                for (j = 0; j < n; j++)
                {
                    R.ele[i, j] = A1.ele[i, j];
                }
            }

            return;
        }

        #region 周长发版本

        /// <summary>
        /// qr分解
        /// </summary>
        /// <param name="A">输入矩阵，输出R矩阵</param>
        /// <param name="Q">Q矩阵</param>
        /// <returns>是否分解成功</returns>
        public static bool qrdcmp_ZhouChangFa(ref Matrix A, out Matrix Q)
        {
            int i, j, k, nn, jj;
            double u, alpha, w, t;

            Q = new Matrix(A.row, A.row);

            if(A.row<A.column)
            {
                return false;
            }

            //对角线元素单位化
            for(i=0;i<A.row;i++)
            {
                for(j=0;j<A.row; j++)
                {
                    A[i, j] = 0.0;
                }
                A[i, i] = 0.0;
            }

            //开始分解

            nn = A.column;
            if(A.row==A.column)
            {
                nn = A.row - 1;
            }

            for(k=0;k<=nn-1;k++)
            {
                u = 0.0;
                for(i=k;i<A.row-1;i++)
                {
                    w = Math.Abs(A[i, k]);
                    if(w>u)
                    {
                        u = w;
                    }
                }

                alpha = 0.0;
                for(i=k;i<=A.row-1;i++)
                {
                    t = A[i, k] / u;
                    alpha = alpha + t * t;
                }

                if(A[k,k]>0.0)
                {
                    u = -u;
                }

                alpha = u * Math.Sqrt(alpha);
                if(alpha==0.0)
                {
                    return false;
                }

                u = Math.Sqrt(2.0 * alpha * (alpha - A[k, k]));
                if((u+1.0)!=1.0)
                {
                    A[k, k] = (A[k, k] - alpha) / u;
                    for(i=k+1;i<A.row-1;i++)
                    {
                        A[i, k] = A[i, k] / u;
                    }

                    for(j=0;j<=A.row-1;j++)
                    {
                        t = 0.0;
                        for(jj=k;jj<A.row-1;jj++)
                        {
                            t = t + A[jj, k] * Q[jj, j];
                        }

                        for(i=k;i<=A.row-1;i++)
                        {
                            Q[i, j] = Q[i, j] - 2.0 * t * A[i, k];
                        }
                    }

                    for(j=k+1;j<A.column-1;j++)
                    {
                        t = 0.0;

                        for(jj=k;jj<A.row-1;jj++)
                        {
                            t = t + A[jj, k] * A[jj, j];
                        }

                        for(i=k;i<A.row-1;i++)
                        {
                            A[i, j] = A[i, j] - 2.0 * t * A[i, k];
                        }
                    }

                    A[k, k] = alpha;
                    for(i=k+1;i<A.row-1;i++)
                    {
                        A[i, k] = 0.0;
                    }
                }
            }

            //调整元素
            for(i=0;i<A.row-2;i++)
            {
                for(j=i+1;j<A.row-1;j++)
                {
                    t = Q[i, j];
                    Q[i, j] = Q[j, i];
                    Q[j, i] = t;
                }
            }

            return true;
        }
        #endregion 周长发版本

    }
}
