using System;
using System.Collections.Generic;
using System.Text;

namespace ChemGo.Auxiliary.LinearAlgebra
{
    public partial class Vector
    {
        /*
        ----------------------------------------------------  类注释  开始----------------------------------------------------
        版本：  V1.0
        作者：  刘鲲
        日期：  2018-07-26

        描述：
            * 向量类
        结构：
            * dim 向量维数
            * ele 向量元素
        方法：
            * = 向量相等
                注意该方法不需要自行编写重载函数，
                c#已经实现了对类的等号运算符重载，
                并禁止用户编写等号（=）重载函数
            * + 两向量对应元素相加
            * - 两向量对应元素相减
            * * 两向量对应元素相乘
            * / 两向量对应元素相除
            * 
            * + 重载向量加一实数（所有元素）
            * - 重载向量减一实数（所有元素）
            * * 重载向量乘一实数（所有元素）
            * / 重载向量除一实数（所有元素）
            * 
            * + 重载实数加向量（所有元素）
            * - 重载实数减向量（所有元素）
            * * 重载实数乘以向量（所有元素）
            * / <无> 不重载 <即不能用实数除以向量>
            * 
            * | （VEC1|VEC2） 向量内积
            * ~ （~VEC1） 向量2范数的平方
            * - （-VEC1） 向量取负
            * ^ （VEC1^VEC2） 向量外积
        ----------------------------------------------------  类注释  结束----------------------------------------------------
    */

        public int dim;                  //向量维数
        public double[] ele;             //向量元素

        #region 构造函数
        /// <summary>
        /// 向量构造函数
        /// </summary>
        /// <param name="dim">向量维数</param>
        public Vector(int dim)
        {
            this.dim = dim;
            ele = new double[dim];
        }

        /// <summary>
        /// 向量构造函数
        /// </summary>
        /// <param name="ele">一维数组</param>
        public Vector(double[] ele)
        {
            dim = ele.Length;
            this.ele = new double[dim];
            for(int i=0;i<dim;i++)
            {
                this.ele[i] = ele[i];
            }
        }
        #endregion 构造函数

        #region 属性和索引器
        public int dimNum { get { return dim; } }
        public double[] dataOneDimArray { get { return ele; } }

        public double this[int d]
        {
            get { return ele[d]; }
            set { ele[d] = value; }
        }
        #endregion

        /// <summary>
        /// 向量加法重载
        /// 分量分别相加
        /// </summary>
        /// <param name="v1">第一个向量</param>
        /// <param name="v2">第二个向量</param>
        /// <returns>两个向量之和</returns>
        public static Vector operator +(Vector v1, Vector v2)
        {
            //向量维数
            int n = v1.dim;
            //检查相加的两个向量维数是否相同
            if(n!=v2.dim)
            {
                throw new IndexOutOfRangeException("向量维数不匹配。");
            }

            Vector v3 = new Vector(n);
            for(int i=0;i<n;i++)
            {
                v3.ele[i] = v1.ele[i] + v2.ele[i];
            }

            return v3;
        }

        /// <summary>
        /// 向量减法重载
        /// 分量分别相减
        /// </summary>
        /// <param name="v1">第一个向量</param>
        /// <param name="v2">第二个向量</param>
        /// <returns>两个向量之差</returns>
        public static Vector operator -(Vector v1, Vector v2)
        {
            //向量维数
            int n = v1.dim;
            //检查相加的两个向量维数是否相同
            if (n != v2.dim)
            {
                throw new IndexOutOfRangeException("向量维数不匹配。");
            }

            Vector v3 = new Vector(n);
            for (int i = 0; i < n; i++)
            {
                v3.ele[i] = v1.ele[i] - v2.ele[i];
            }

            return v3;
        }

        /// <summary>
        /// 向量数乘
        /// 分量分别乘以实数
        /// </summary>
        /// <param name="v1">向量</param>
        /// <param name="a">乘数</param>
        /// <returns>向量数乘之积</returns>
        public static Vector operator *(Vector v1, double a)
        {
            //向量维数
            int n = v1.dim;

            Vector v2 = new Vector(n);
            for (int i = 0; i < n; i++)
            {
                v2.ele[i] = v1.ele[i] * a;
            }

            return v2;
        }

        /// <summary>
        /// 向量数乘
        /// 分量分别乘以实数
        /// </summary>
        /// <param name="a">乘数</param>
        /// <param name="v1">向量</param>
        /// <returns>向量数乘之积</returns>
        public static Vector operator *(double a, Vector v1)
        {
            //向量维数
            int n = v1.dim;

            Vector v2 = new Vector(n);
            for (int i = 0; i < n; i++)
            {
                v2.ele[i] = v1.ele[i] * a;
            }

            return v2;
        }

        /// <summary>
        /// 向量数除
        /// 分量分别除以实数
        /// </summary>
        /// <param name="v1">向量</param>
        /// <param name="a">除数</param>
        /// <returns>向量数除之商</returns>
        public static Vector operator /(Vector v1, double a)
        {
            //向量维数
            int n = v1.dim;

            Vector v2 = new Vector(n);
            for (int i = 0; i < n; i++)
            {
                v2.ele[i] = v1.ele[i] / a;
            }

            return v2;
        }

        /// <summary>
        /// 标量积
        /// </summary>
        /// <param name="v1">第一个向量</param>
        /// <param name="v2">第二个向量</param>
        /// <returns>两个向量的标量积</returns>
        public static double operator |(Vector v1, Vector v2)
        {
            int m = v1.dim;
            int n = v2.dim;
            //检查相加的两个向量维数是否相同
            if (m != n)
            {
                throw new IndexOutOfRangeException("向量维数不匹配。");
            }

            double dotMul = 0.0;

            for(int i=0;i<m;i++)
            {
                dotMul += v1.ele[i] * v2.ele[i];
            }

            return dotMul;
        }

        /// <summary>
        /// 张量积
        /// </summary>
        /// <param name="v1">第一个向量</param>
        /// <param name="v2">第二个向量</param>
        /// <returns>两个向量的张量积</returns>
        public static Matrix operator *(Vector v1, Vector v2)
        {
            int row = v1.dim;
            int column = v2.dim;
            //检查相加的两个向量维数是否相同
            if (row!=column)
            {
                throw new IndexOutOfRangeException("向量维数不匹配。");
            }

            Matrix matrix = new Matrix(row,column);

            for(int i=0;i<row;i++)
            {
                for(int j=0;j<column;j++)
                {
                    matrix.ele[i,j] = v1.ele[i] * v2.ele[j];
                }
            }

            return matrix;
        }

        /// <summary>
        /// 负向量
        /// </summary>
        /// <param name="v1">向量</param>
        /// <returns>该向量的负向量</returns>
        public static Vector operator -(Vector v1)
        {
            int n = v1.dim;
            Vector v2 = new Vector(n);

            for(int i=0;i<n;i++)
            {
                v2.ele[i] = -v1.ele[i];
            }

            return v2;
        }

        /// <summary>
        /// 向量模的平方
        /// </summary>
        /// <param name="v1">向量</param>
        /// <returns>向量模的平方</returns>
        public static double operator ~(Vector v1)
        {
            int NO;

            //获取变量维数
            NO = v1.dim;
            double sum = 0.0;

            for (int i = 0; i < NO; i++)
            {
                sum = sum + v1.ele[i] * v1.ele[i];
            }
            return sum;
        }
    }
}
