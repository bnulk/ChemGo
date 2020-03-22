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
        日期：  2018-08-10

        描述：
            * Householder变换，QL分解。
            * QL算法，或者对应的QR算法（依据数值稳定性做对应的选择），目前无论在理论还是在程序设计上都极为漂亮，Lapack中tred2和tqli可作为平民级的对称矩阵特征值求解器。
              其中tred2是利用Househodler变换转到对称三对角矩阵，然后tqli再进行隐式移位QL迭代，每次迭代收敛甚至达到3阶以上。特征值的QR算法被誉为20世纪最伟大的十大算法之一。
            * 对称矩阵 -(Household变换)-> 三对角矩阵 -(QR迭代)-> 对角矩阵
            * 非对称矩阵 -(Household变换)-> Hessenberg矩阵 -(QR迭代)-> 上三角矩阵
        结构：
            * 
        方法：
            * tred2(Matrix a, Vector d, Vector e)  实对称矩阵A[n,n]的Householder约化，和tqli结合是当前计算实对称矩阵特征值和特征向量最有效的方法。
              输入：a为待处理的实对称矩阵。d，e为空。
              输出：a被产生变换的正交矩阵Q取代。d返回三对角矩阵的对角元素，e返回非对角元素，且e[0]=0。
            * tqli(ref Vector d, ref Vector e, ref Matrix z) 计算特征值和特征向量。
              输入：实对称矩阵，事先由tred2约化后得到的三对角矩阵和正交矩阵Q。d是对角元素，e是次对角元素，e[0]取任意值。z是tred2中的a矩阵。
              输出：d是特征值，e被破坏，z每一列是特征向量。输出z的第k列是与d[k]相对应的规范特征向量。
              另：如果输入z是单位矩阵的话，则输出为三对角矩阵的特征向量。输出z的第k列是与d[k]相对应的规范特征向量。
        代码来源：
            * c++数值计算(第二版)
        ----------------------------------------------------  类注释  结束----------------------------------------------------
        */

        /// <summary>
        /// 实对称矩阵A[n,n]的Householder约化
        /// </summary>
        /// <param name="a">输入：实对称矩阵；输出：变换后的正交矩阵Q</param>
        /// <param name="d">输入：矢量；输出：三对角矩阵的对角元素</param>
        /// <param name="e">输入：矢量；输出：非对角元素且e[0]=0</param>
        public static void tred2(ref Matrix a, ref Vector d, ref Vector e)
        {
            int l, k, j, i;
            double scale, hh, h, g, f;

            int n = d.dim;
            for (i = n - 1; i > 0; i--)
            {
                l = i - 1;
                h = scale = 0.0;
                if (l > 0)
                {
                    for (k = 0; k < l + 1; k++)
                    {
                        scale += Math.Abs(a[i, k]);
                    }
                    if (scale == 0.0)                              //跳过变换
                    {
                        e[i] = a[i, l];
                    }
                    else
                    {
                        for (k = 0; k < l + 1; k++)
                        {
                            a[i, k] /= scale;                      //将标定的a用于变换
                            h += a[i, k] * a[i, k];                //在h中形成σ
                        }
                        f = a[i, l];
                        g = (f >= 0.0 ? -Math.Sqrt(h) : Math.Sqrt(h));
                        e[i] = scale * g;
                        h -= f * g;                                //现在h是(11.2.4)式
                        a[i, l] = f - g;                           //将u存入a的第i行
                        f = 0.0;
                        for (j = 0; j < l + 1; j++)
                        {
                            //如果不需要求本征向量，下面的声明可以省略
                            a[j, i] = a[i, j] / h;                                //将u/H存入a的第i列
                            g = 0.0;                                              //在g中形成A·u的一个元素
                            for (k = 0; k < j + 1; k++)
                            {
                                g += a[j, k] * a[i, k];
                            }
                            for (k = j + 1; k < l + 1; k++)
                            {
                                g += a[k, j] * a[i, k];
                            }
                            e[j] = g / h;                                         //在e的暂时不用的元素中形成p的元素
                            f += e[j] * a[i, j];
                        }
                        hh = f / (h + h);                                         //形成K, (11.2.11)式
                        for (j = 0; j < l + 1; j++)                                        //形成q并存入e中p的位置上
                        {
                            f = a[i, j];
                            e[j] = g = e[j] - hh * f;
                            for (k = 0; k < j + 1; k++)                                    //约化a, (11.2.13)式
                            {
                                a[j, k] -= (f * e[k] + g * a[i, k]);
                            }
                        }
                    }
                }
                else
                {
                    e[i] = a[i, l];
                }
                d[i] = h;
            }
            //如果不需要求本征向量，下面的声明可以省略
            d[0] = 0.0;
            e[0] = 0.0;
            //如果不需要求本征向量，这个循环的内容，除了d[i]=a[i,i]这个声明外，可以省略。
            for (i = 0; i < n; i++)                  //开始矩阵变换的积累
            {
                l = i;
                if (d[i] != 0.0)                 //当i=0时跳过这一块
                {
                    for (j = 0; j < l; j++)
                    {
                        g = 0.0;
                        for (k = 0; k < l; k++)                             //利用a中存储的u和u/H来形成P·Q
                        {
                            g += a[i, k] * a[k, j];
                        }
                        for (k = 0; k < l; k++)
                        {
                            a[k, j] -= g * a[k, i];
                        }
                    }
                }
                d[i] = a[i, i];                                      //保留这一语句
                a[i, i] = 1.0;                                       //为下一次迭代将a重新置成单位矩阵
                for (j = 0; j < l; j++)
                {
                    a[j, i] = a[i, j] = 0.0;
                }
            }
            return;
        }

        /// <summary>
        /// 实对称三对角矩阵，求本征值和本征向量。
        /// </summary>
        /// <param name="d">输入：对角元；输出：本征值。</param>
        /// <param name="e">输入：次对角元素，e[0]任意值；输出：无。</param>
        /// <param name="z">输入：单位矩阵；输出：三对角矩阵的特征向量。</param>
        /// <param name="z">输入：tred2输出的矩阵；输出：tred2已约化的矩阵的特征向量。</param>
        public static void tqli(ref Vector d, ref Vector e, ref Matrix z)
        {
            int m, l, iter, i, k;
            double s, r, p, g, f, dd, c, b;

            int n = d.dim;
            for (i = 1; i < n; i++)
            {
                e[i - 1] = e[i];                             //便于重编号e中的元素。
            }
            e[n - 1] = 0.0;

            for (l = 0; l < n; l++)
            {
                iter = 0;
                do
                {
                    for (m = l; m < n - 1; m++)                                               //寻找一个单一的小次对角元素，用来分裂矩阵。
                    {
                        dd = Math.Abs(d[m]) + Math.Abs(d[m + 1]);
                        if (Math.Abs(e[m]) + dd == dd)
                            break;
                    }
                    if (m != l)
                    {
                        if (iter++ == 30)
                        {
                            //Output.WriteOutput.Error.Append("Too many iterations in ChemGo.LinearAlgebra.Matrix_0_HouseholdAndQR.tqli" + "/n");
                            Console.WriteLine("Too many iterations in ChemGo.LinearAlgebra.Matrix_0_HouseholdAndQR.tqli");
                        }
                        g = (d[l + 1] - d[l]) / (2.0 * e[l]);                                 //形成位移
                        r = Util.pythag(g, 1.0);
                        g = d[m] - d[l] + e[l] / (g + Util.NR_SIGN(r, g));                         //这是dm-ks。
                        s = c = 1.0;
                        p = 0.0;
                        for (i = m - 1; i >= l; i--)                                                   //如同原来QL方法一样的平面转动，随后Givens旋转，用以恢复三对角形式。
                        {
                            f = s * e[i];
                            b = c * e[i];
                            e[i + 1] = (r = Util.pythag(f, g));
                            if (r == 0.0)                                                      //从下溢处恢复
                            {
                                d[i + 1] -= p;
                                e[m] = 0.0;
                                break;
                            }
                            s = f / r;
                            c = g / r;
                            g = d[i + 1] - p;
                            r = (d[i] - g) * s + 2.0 * c * b;
                            d[i + 1] = g + (p = s * r);
                            g = c * r - b;
                            //如果不需要求本征向量，可以忽略下面的循环
                            for (k = 0; k < n; k++)
                            {
                                f = z[k, i + 1];
                                z[k, i + 1] = s * z[k, i] + c * f;
                                z[k, i] = c * z[k, i] - s * f;
                            }
                        }
                        if (r == 0.0 && i >= l) continue;
                        d[l] -= p;
                        e[l] = g;
                        e[m] = 0.0;
                    }
                } while (m != l);
            }

            return;
        }

    }
}
