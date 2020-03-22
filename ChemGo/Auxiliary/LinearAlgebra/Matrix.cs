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
        日期：  2018-07-29

        描述：
            * 矩阵类
        结构：
            * row，column 矩阵的行列数
            * ele 矩阵元素
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

        public int row, column;            //矩阵的行列数
        public double[,] ele;              //矩阵的数据

        #region 构造函数
        public Matrix(int rowNum, int columnNum)
        {
            row = rowNum;
            column = columnNum;
            ele = new double[row, column];
        }

        public Matrix(double[,] members)
        {
            row = members.GetUpperBound(0) + 1;
            column = members.GetUpperBound(1) + 1;
            ele = new double[row, column];
            Array.Copy(members, ele, row * column);
        }

        public Matrix(double[] vector)
        {
            row = vector.Length;
            column = 1;
            ele = new double[row, column];
            for (int i = 0; i < row; i++)
            {
                ele[i, 0] = vector[i];
            }
        }
        #endregion

        #region 属性和索引器
        public int rowNum { get { return row; } }
        public int columnNum { get { return column; } }
        public double[,] dataTwoDimArray { get { return ele; } }

        public double this[int r, int c]
        {
            get { return ele[r, c]; }
            set { ele[r, c] = value; }
        }
        #endregion

        #region 基本运算
        /// <summary>
        /// 将矩阵转置，得到一个新矩阵（此操作不影响原矩阵）
        /// </summary>
        /// <param name="input">要转置的矩阵</param>
        /// <returns>原矩阵经过转置得到的新矩阵</returns>
        public static Matrix Transpose(Matrix input)
        {
            double[,] inverseMatrix = new double[input.column, input.row];
            for (int r = 0; r < input.row; r++)
                for (int c = 0; c < input.column; c++)
                    inverseMatrix[c, r] = input[r, c];
            return new Matrix(inverseMatrix);
        }

        /// <summary>
        /// 得到行向量
        /// </summary>
        /// <param name="r">行数</param>
        /// <returns>该行的行向量</returns>
        public Matrix getRow(int r)
        {
            if (r > row || r <= 0)
                throw new Exception("没有这一行。");
            double[] a = new double[column];
            Array.Copy(ele, column * (row - 1), a, 0, column);
            Matrix m = new Matrix(a);
            return m;
        }

        /// <summary>
        /// 得到列向量
        /// </summary>
        /// <param name="r">列数</param>
        /// <returns>该行的列向量</returns>
        public Matrix getColumn(int c)
        {
            if (c > column || c < 0) throw new IndexOutOfRangeException("没有这一列。");
            double[,] a = new double[row, 1];
            for (int i = 0; i < row; i++)
                a[i, 0] = ele[i, c];
            return new Matrix(a);
        }

        /// <summary>
        /// 两个矩阵的元素分别相加
        /// </summary>
        /// <param name="a">矩阵1</param>
        /// <param name="b">矩阵2</param>
        /// <returns>矩阵元素分别相加得到的新矩阵</returns>
        public static Matrix operator +(Matrix a, Matrix b)
        {
            if (a.row != b.row || a.column != b.column)
                throw new IndexOutOfRangeException("矩阵维数不匹配。");
            Matrix result = new Matrix(a.row, a.column);
            for (int i = 0; i < a.row; i++)
                for (int j = 0; j < a.column; j++)
                    result[i, j] = a[i, j] + b[i, j];
            return result;
        }

        /// <summary>
        /// 矩阵的元素分别加实数
        /// </summary>
        /// <param name="x">实数</param>
        /// <param name="a1">矩阵</param>
        /// <returns>矩阵元素分别加实数后的新矩阵</returns>
        public static Matrix operator +(double x, Matrix a1)
        {
            int m = a1.row;
            int n = a1.column;

            Matrix a2 = new Matrix(m, n);

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    a2.ele[i, j] = a1.ele[i, j] + x;
                }
            }

            return a2;
        }

        public static Matrix operator -(Matrix a, Matrix b)
        {
            return a + b * (-1);
        }

        public static Matrix operator -(double x, Matrix a1)
        {
            int m = a1.row;
            int n = a1.column;

            Matrix a2 = new Matrix(m, n);

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    a2.ele[i, j] = a1.ele[i, j] - x;
                }
            }

            return a2;
        }

        public static Matrix operator *(Matrix matrix, double factor)
        {
            Matrix result = new Matrix(matrix.row, matrix.column);
            for (int i = 0; i < matrix.row; i++)
                for (int j = 0; j < matrix.column; j++)
                    matrix[i, j] = matrix[i, j] * factor;
            return matrix;
        }
        public static Matrix operator *(double factor, Matrix matrix)
        {
            return matrix * factor;
        }
        public static Matrix operator *(Matrix a, Matrix b)
        {
            if (a.column != b.row)
                throw new IndexOutOfRangeException("矩阵维数不匹配。");
            Matrix result = new Matrix(a.row, b.column);
            for (int i = 0; i < a.row; i++)
                for (int j = 0; j < b.column; j++)
                    for (int k = 0; k < a.column; k++)
                        result[i, j] += a[i, k] * b[k, j];

            return result;
        }

        public static bool operator ==(Matrix a, Matrix b)
        {
            if (object.Equals(a, b)) return true;
            if (object.Equals(null, b))
                return a.Equals(b);
            return b.Equals(a);
        }

        public static bool operator !=(Matrix a, Matrix b)
        {
            return !(a == b);
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is Matrix)) return false;
            Matrix t = obj as Matrix;
            if (row != t.row || column != t.column) return false;
            return this.Equals(t, 10);
        }

        /// <summary>
        /// 按照给定的精度比较两个矩阵是否相等
        /// </summary>
        /// <param name="matrix">要比较的另外一个矩阵</param>
        /// <param name="precision">比较精度（小数位）</param>
        /// <returns>是否相等</returns>
        public bool Equals(Matrix matrix, int precision)
        {
            if (precision < 0) throw new IndexOutOfRangeException("小数位不能是负数");
            double test = Math.Pow(10.0, -precision);
            if (test < double.Epsilon)
                throw new IndexOutOfRangeException("所要求的精度太高，不被支持。");
            for (int r = 0; r < this.row; r++)
                for (int c = 0; c < this.column; c++)
                    if (Math.Abs(this[r, c] - matrix[r, c]) >= test)
                        return false;

            return true;
        }

        public static Matrix Copy(Matrix a)
        {
            int m = a.row;
            int n = a.column;
            Matrix b = new Matrix(m, n);

            for(int i=0;i<m;i++)
            {
                for(int j=0;j<m;j++)
                {
                    b[i, j] = a[i, j];
                }
            }

            return b;
        }

        #endregion


    }
}
