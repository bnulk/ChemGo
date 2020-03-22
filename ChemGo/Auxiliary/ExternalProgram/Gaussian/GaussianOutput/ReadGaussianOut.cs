using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ChemGo.Auxiliary.TextTools;

namespace ChemGo.Auxiliary.ExternalProgram.Gaussian.GaussianOutput
{
    partial class ReadGaussianOut
    {
        private string filePath;                                //Out文件的物理地址
        private StreamReader OutFileStreamReader;               //读文件对象

        private int numberOfAtom;
        private int numberOfX_ZMatrix;
        private int numberOfX_Cartesian;
        private string focusLine = "";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filepath">GaussianOut文件的物理路径</param>
        public ReadGaussianOut(string filePath)
        {
            this.filePath = filePath;      //全局变量FilePath，文件的物理路径，有初始化参量获得
            OutFileStreamReader = File.OpenText(filePath);
            numberOfAtom = GetNumberOfAtom();
            numberOfX_Cartesian = 3 * numberOfAtom;
            numberOfX_ZMatrix = GetNumberOfParameter_ZMartrix();
        }


        /// <summary>
        /// 重新打开Out文件
        /// </summary>
        public void Update()
        {
            OutFileStreamReader.Close();
            OutFileStreamReader = File.OpenText(filePath);
        }

        /// <summary>
        /// 获取原子个数
        /// </summary>
        /// <returns>原子个数</returns>
        public int GetNumberOfAtom()    //获取原子个数
        {
            int N = -1;
            int indexMark = -1;
            string str = null;
            while (OutFileStreamReader.Peek() > -1)                 //应用一个while循环，条件是文件不结束
            {
                str = OutFileStreamReader.ReadLine();
                indexMark = str.IndexOf("NAtoms=");
                if (indexMark != -1)
                {
                    str = str.Remove(0, indexMark + 7);
                    str = str.Trim();
                    indexMark = str.IndexOf(" ");
                    str = str.Substring(0, indexMark).Trim();
                    try
                    {
                        N = Convert.ToInt32(str);
                    }
                    catch
                    {
                        throw new ReadGaussianOutException("ReadGaussianOut.ObtainN() Error");
                    }
                    return N;
                }
            }
            return N;
        }
        
        /// <summary>
        /// 获取ZMatrix格式的参数个数
        /// </summary>
        /// <returns>ZMatrix格式的参数个数</returns>
        public int GetNumberOfParameter_ZMartrix()
        {
            int numberOfParameter = 0;
            string str = "";

            while (str!= " Initialization pass.")                 //应用一个while循环，条件是文件不结束
            {
                str = OutFileStreamReader.ReadLine();                
            }

            str = OutFileStreamReader.ReadLine();

            if(str== "                       ----------------------------")
            {
                for (int i = 0; i < 5; i++)
                {
                    OutFileStreamReader.ReadLine();
                }

                while (str != " ------------------------------------------------------------------------")
                {
                    str = OutFileStreamReader.ReadLine();
                    numberOfParameter++;
                }
                return numberOfParameter - 1;
            }
            else
            {
                //返回错误值
                return numberOfParameter;
            }
        }

        /// <summary>
        /// 得到TD能量
        /// </summary>
        /// <returns>TD能量</returns>
        public double GetEnergy_TD()
        {
            double energy = 0;
            int indexMark = -1;
            string str = null;
            while (OutFileStreamReader.Peek() > -1)                 //应用一个while循环，条件是文件不结束
            {
                str = OutFileStreamReader.ReadLine();
                indexMark = str.IndexOf("Total Energy, E(TD-HF/TD-KS) =");
                if (indexMark != -1)
                {
                    str = str.Remove(0, indexMark + 30);
                    str = str.Trim();
                    energy = Convert.ToDouble(str);
                    return energy;
                }
            }
            return energy;
        }

        /// <summary>
        /// 获取Archive
        /// </summary>
        /// <returns>Archive</returns>
        public string GetArchive()
        {
            string str = null;
            string archive = "";
            StringBuilder archiveStr = new StringBuilder();

            while (OutFileStreamReader.Peek() > -1)
            {
                str = OutFileStreamReader.ReadLine();
                if (str == " Test job not archived.")
                {
                    for (; str != "";)
                    {
                        str = OutFileStreamReader.ReadLine();
                        archiveStr.Append(str.Trim());
                    }
                }
            }
            archive = archiveStr.ToString();
            if (archive == "")
            {
                throw new ReadGaussianOutException("ReadGaussianOut.GetArchive() Error");
            }
            return archive;
        }

        /// <summary>
        /// 获取最后Acheive中“HF=”的能量
        /// </summary>
        /// <returns>HF类型的能量，双精度型</returns>
        public double GetHFTypeEnergy()
        {
            double Energy = -1.0;
            int indexMark = -1;
            string str = null;

            str = GetArchive();
            indexMark = str.IndexOf("HF=");
            //Linux系统，“\”分割；Windows系统“|”分割。
            str = str.Replace("\\", "|");
            str = str.Remove(0, indexMark + 3);
            indexMark = str.IndexOf('|');
            str = str.Substring(0, indexMark);
            Energy = Convert.ToDouble(str);
            return Energy;
        }

        /// <summary>
        /// 得到一行和参数对应的梯度值。 注意：本程序中所有梯度，都是DE/DX，这个一定要当心。
        /// </summary>
        /// <returns>梯度值</returns>
        public double[] GetForceValue_ZMatrix()
        {
            List<string> strGradientValue = new List<string>();
            double[] gradientValue;

            string str;
            string[] tmpStr = new string[2];
            int numberOfX = 0;
            

            while (OutFileStreamReader.Peek() > -1)                //应用一个while循环，条件是文件不结束
            {
                str = OutFileStreamReader.ReadLine();              //读文件的一行

                //获取力
                if (str == " From PutF, contents of force:")           //读梯度的标志
                {
                    while (str != " -----------------------------------------------------------------------------------------------")
                    {
                        str = OutFileStreamReader.ReadLine();
                        tmpStr = TextTools.BasisTextTools.GetStringSeparatedbySpaces(str);
                        strGradientValue.Add(tmpStr[1]);
                    }
                }

                //赋值
                numberOfX = strGradientValue.Count;
                gradientValue = new double[numberOfX];
                for (int i=0;i<numberOfX;i++)
                {
                    gradientValue[i] = Convert.ToDouble(strGradientValue[i]) * (-1);
                }
                //返回值
                return gradientValue;
            }

            //返回错误信息
            gradientValue = new double[1];
            gradientValue[0] = 0.0;
            return gradientValue;
        }

        /// <summary>
        /// 单点计算，获取ZMatrix型的力常数矩阵。形式为string［M,3］。其中M是变量个数，3分别是“变量名”“变量值”“梯度”
        /// </summary>
        /// <returns>string[M,3]二维数组</returns>
		public string[,] GetForceParams_ZMatrix()
        {
            string[,] forceParams;
            int numberOfX = 0;
            List<string> gradientValue = new List<string>();

            string str;
            string[] tmpStr = new string[2];
            string[] tmp3 = new string[3];

            while (OutFileStreamReader.Peek() > -1)                //应用一个while循环，条件是文件不结束
            {
                str = OutFileStreamReader.ReadLine();              //读文件的一行

                //获取力和变量个数
                if (str == " From PutF, contents of force:")           //读梯度的标志
                {
                    str = OutFileStreamReader.ReadLine();
                    while (str!= " -----------------------------------------------------------------------------------------------")
                    {                        
                        tmpStr = TextTools.BasisTextTools.GetStringSeparatedbySpaces(str);
                        numberOfX = Convert.ToInt32(tmpStr[0]);
                        gradientValue.Add(tmpStr[1]);
                        str = OutFileStreamReader.ReadLine();
                    }                                     
                }

                //获取参数名称和当前参数值
                if (str == " Variable       Old X    -DE/DX   Delta X   Delta X   Delta X     New X")           //读梯度的标志
                {
                    forceParams = new string[numberOfX, 3];

                    str = OutFileStreamReader.ReadLine();     //跳过(Linear)    (Quad)   (Total)那一行
                    for (int i = 0; i < (numberOfX); i++)
                    {
                        str = OutFileStreamReader.ReadLine();
                        tmp3[0] = str.Substring(0, 11);
                        tmp3[1] = str.Substring(11, 10);
                        tmp3[2] = str.Substring(21, 10);
                        forceParams[i, 0] = tmp3[0];
                        forceParams[i, 1] = tmp3[1];
                    }

                    //把Out文件力矩阵矩阵元中的D改为E
                    for (int m = 0; m < numberOfX; m++)
                    {
                        forceParams[m, 2] = gradientValue[m].Replace('D', 'E');
                    }

                    //返回值
                    return forceParams;
                }
            }

            //返回错误信息
            forceParams = new string[1, 1];
            forceParams[0, 0] = "Error";
            return forceParams;
        }

        public double[] GetForceValue_Cartesian()
        {
            string str;
            int numberOfX = 0;
            List<string> strGradientValue = new List<string>();
            string[] tmp3 = new string[3];

            double[] gradientValue;

            while (OutFileStreamReader.Peek() > -1)                //应用一个while循环，条件是文件不结束
            {
                str = OutFileStreamReader.ReadLine();              //读文件的一行

                //获取力
                if (str == " ***** Axes restored to original set *****")           //读梯度的标志
                {
                    str = OutFileStreamReader.ReadLine();
                    str = OutFileStreamReader.ReadLine();
                    str = OutFileStreamReader.ReadLine();
                    str = OutFileStreamReader.ReadLine();
                    str = OutFileStreamReader.ReadLine();                          //跳过5行

                    str = OutFileStreamReader.ReadLine();
                    while (str != " -------------------------------------------------------------------")
                    {
                        tmp3[0] = str.Substring(26, 12);
                        tmp3[1] = str.Substring(41, 12);
                        tmp3[2] = str.Substring(56, 12);
                        strGradientValue.Add(tmp3[0].Trim());
                        strGradientValue.Add(tmp3[1].Trim());
                        strGradientValue.Add(tmp3[2].Trim());
                        str = OutFileStreamReader.ReadLine();
                    }
                }

                numberOfX = strGradientValue.Count;
                gradientValue = new double[numberOfX];
                for(int i=0;i<numberOfX;i++)
                {
                    gradientValue[i] = Convert.ToDouble(strGradientValue[i]);
                }
            }

            //返回错误信息
            gradientValue = new double[1];
            gradientValue[0] = 0.0;
            return gradientValue;
        }

        /// <summary>
        /// string［M,3］。其中M=3*N是变量个数，3分别是“变量名”“变量值”“梯度”
        /// </summary>
        /// <returns>string[M,3]二维数组</returns>
        public string[,] GetForceParams_Cartesian()
        {
            string[,] forceParams;
            int numberOfX = 0;
            List<string> gradientValue = new List<string>();

            string str;
            string[] tmpStr = new string[7];
            string[] tmp3 = new string[3];

            while (OutFileStreamReader.Peek() > -1)                //应用一个while循环，条件是文件不结束
            {
                str = OutFileStreamReader.ReadLine();              //读文件的一行

                //获取力
                if (str == " ***** Axes restored to original set *****")           //读梯度的标志
                {
                    str = OutFileStreamReader.ReadLine();
                    str = OutFileStreamReader.ReadLine();
                    str = OutFileStreamReader.ReadLine();
                    str = OutFileStreamReader.ReadLine();
                    str = OutFileStreamReader.ReadLine();                          //跳过5行

                    str = OutFileStreamReader.ReadLine();
                    while (str!= " -------------------------------------------------------------------")
                    {                        
                        tmp3[0] = str.Substring(26, 12);
                        tmp3[1] = str.Substring(41, 12);
                        tmp3[2] = str.Substring(56, 12);
                        gradientValue.Add(tmp3[0].Trim());
                        gradientValue.Add(tmp3[1].Trim());
                        gradientValue.Add(tmp3[2].Trim());
                        numberOfX = numberOfX + 3;
                        str = OutFileStreamReader.ReadLine();
                    }
                }
                                

                //获取参数名称和当前参数值
                if (str == " Variable       Old X    -DE/DX   Delta X   Delta X   Delta X     New X")           //读梯度的标志
                {
                    forceParams = new string[numberOfX, 3];
                    str = OutFileStreamReader.ReadLine();     //跳过(Linear)    (Quad)   (Total)那一行

                    for (int i = 0; i < numberOfX; i++)
                    {
                        str = OutFileStreamReader.ReadLine();
                        tmp3[0] = str.Substring(0, 11);
                        tmp3[1] = str.Substring(11, 10);
                        tmp3[2] = str.Substring(21, 10);
                        forceParams[i, 0] = tmp3[0];
                        forceParams[i, 1] = tmp3[1];
                    }

                    //把Out文件力矩阵矩阵元中的D改为E
                    for (int m = 0; m < numberOfX; m++)
                    {
                        forceParams[m, 2] = gradientValue[m].Replace('D', 'E');
                    }

                    //返回值
                    return forceParams;
                }               
            }

            //返回错误信息
            forceParams = new string[1, 1];
            forceParams[0, 0] = "Error";
            return forceParams;
        }

        /// <summary>
        /// 得到力常数矩阵，参数的标号按Out文件中的顺序
        /// </summary>
        /// <returns>二维数组，力常数矩阵string[3*N-6,3*N-6]</returns>
        //public string[,] GetForceConstant()
        public string[,] GetForceConstant_Zmatrix()
        {
            //数据准备
            string str = "";         //临时存放每一行数据
            string[] Data = new string[6];           //把每一行数据按空格分成六份,每一份为一个力常数数据
            //初始化每行数据Data
            for (int i = 0; i < 6; i++)
            {
                Data[i] = "ForceConstant";
            }
            int Block = 0;                            //力常数矩阵在Out文件中被分成的块数
            Block = Convert.ToInt32(Math.Floor(Convert.ToDouble(numberOfX_ZMatrix) / 5) + 1);         //力常数矩阵的块数
            string[,] forceConstant = new string[numberOfX_ZMatrix, numberOfX_ZMatrix];        //定义力常数矩阵数组



            //正式开始操作

            while (OutFileStreamReader.Peek() > -1)               //应用一个while循环，条件是文件不结束
            {
                str = OutFileStreamReader.ReadLine();             //读文件的一行
                if (str == " Force constants in internal coordinates: ")           //读梯度的标志
                {
                    //跳过没有数据的一行
                    str = OutFileStreamReader.ReadLine();

                    for (int i = 0; i < Block; i++)              //按块读数据
                    {
                        for (int m = 5 * i; m < numberOfX_ZMatrix; m++)      //读行
                        {
                            //读文本中每一行数据
                            str = OutFileStreamReader.ReadLine();
                            Data = TextTools.BasisTextTools.GetStringSeparatedbySpaces(str);

                            if (i * 5 + 5 <= numberOfX_ZMatrix)                          //判断是否为最后一个力常数块
                            //不是最后一个力常数块
                            {
                                for (int n = 5 * i; n < 5 + 5 * i; n++) //读列
                                {
                                    if (n <= m)
                                    {
                                        forceConstant[m, n] = Data[n - 5 * i + 1];
                                    }
                                }
                            }
                            else
                            //最后一个力常数块
                            {
                                for (int n = 5 * i; n < numberOfX_ZMatrix; n++) //读列
                                {
                                    if (n <= m)
                                    {
                                        forceConstant[m, n] = Data[n - 5 * i + 1];
                                    }
                                }
                            }
                        }

                        str = OutFileStreamReader.ReadLine();     //跳过Block之间的构型参数
                    }
                    for (int m = 0; m < numberOfX_ZMatrix; m++)
                    {
                        for (int n = 0; n < numberOfX_ZMatrix; n++)
                        {
                            if (m < n)
                            {
                                forceConstant[m, n] = forceConstant[n, m];
                            }
                        }
                    }
                    //把Out文件力常数矩阵矩阵元中的D改为E
                    for (int m = 0; m < numberOfX_ZMatrix; m++)
                    {
                        for (int n = 0; n < numberOfX_ZMatrix; n++)
                        {
                            forceConstant[m, n] = forceConstant[m, n].Replace('D', 'E');
                        }
                    }
                }
            }
            return forceConstant;
        }

        /// <summary>
        /// 得到力常数矩阵，参数的标号按Out文件中的顺序
        /// </summary>
        /// <returns>二维数组，力常数矩阵string[3*N,3*N]</returns>
        public string[,] GetForceConstant_Cartesian()
        {
            //数据准备
            string str = "";         //临时存放每一行数据
            string[] temp_Data = new string[1000];   //临时存放数据行被分割的部分
            string[] Data = new string[6];           //把每一行数据按空格分成六份,每一份为一个力常数数据
            //初始化每行数据Data
            for (int i = 0; i < 6; i++)
            {
                Data[i] = "ForceConstant";
            }
            int Block = 0;                            //力常数矩阵在Out文件中被分成的块数
            Block = Convert.ToInt32(Math.Floor(Convert.ToDouble(numberOfX_Cartesian) / 5) + 1);         //力常数矩阵的块数
            string[,] forceConstant = new string[numberOfX_Cartesian, numberOfX_Cartesian];        //定义力常数矩阵数组


            //正式开始操作

            while (OutFileStreamReader.Peek() > -1)               //应用一个while循环，条件是文件不结束
            {
                str = OutFileStreamReader.ReadLine();             //读文件的一行
                if (str == " Force constants in Cartesian coordinates: ")           //读梯度的标志
                {
                    //跳过没有数据的一行
                    str = OutFileStreamReader.ReadLine();

                    for (int i = 0; i < Block; i++)              //按块读数据
                    {
                        for (int m = 5 * i; m < numberOfX_Cartesian; m++)      //读行
                        {
                            //读文本中每一行数据
                            str = OutFileStreamReader.ReadLine();
                            Data = TextTools.BasisTextTools.GetStringSeparatedbySpaces(str);
                            if (i * 5 + 5 <= numberOfX_Cartesian)                          //判断是否为最后一个力常数块
                            //不是最后一个力常数块
                            {
                                for (int n = 5 * i; n < 5 + 5 * i; n++) //读列
                                {
                                    if (n <= m)
                                    {
                                        forceConstant[m, n] = Data[n - 5 * i + 1];
                                    }
                                }
                            }
                            else
                            //最后一个力常数块
                            {
                                for (int n = 5 * i; n < numberOfX_Cartesian; n++) //读列
                                {
                                    if (n <= m)
                                    {
                                        forceConstant[m, n] = Data[n - 5 * i + 1];
                                    }
                                }
                            }
                        }

                        str = OutFileStreamReader.ReadLine();     //跳过Block之间的构型参数
                    }
                    for (int m = 0; m < numberOfX_Cartesian; m++)
                    {
                        for (int n = 0; n < numberOfX_Cartesian; n++)
                        {
                            if (m < n)
                            {
                                forceConstant[m, n] = forceConstant[n, m];
                            }
                        }
                    }
                    //把Out文件力常数矩阵矩阵元中的D改为E
                    for (int m = 0; m < numberOfX_Cartesian; m++)
                    {
                        for (int n = 0; n < numberOfX_Cartesian; n++)
                        {
                            forceConstant[m, n] = forceConstant[m, n].Replace('D', 'E');
                        }
                    }
                }
            }
            return forceConstant;
        }


        /// <summary>
        /// 读输入取向坐标
        /// </summary>
        /// <param name="atomicNumber">原子序数</param>
        /// <param name="atomicType">原子类型</param>
        /// <param name="coordinates_Angstroms">坐标</param>
        public void ReadInputOrientation(out int[] atomicNumber, out int[] atomicType, out double[,] coordinates_Angstroms)
        {
            atomicNumber = new int[numberOfAtom];
            atomicType = new int[numberOfAtom];
            coordinates_Angstroms = new double[numberOfX_Cartesian, 3];
            string[] tmpWords = new string[6];

            //正式开始操作
            string str;

            //定位
            str = OutFileStreamReader.ReadLine();             //读文件的一行
            while (str.Trim() != "Input orientation:" && OutFileStreamReader.Peek() > -1)               //应用一个while循环，条件是文件不结束
            {
                str = OutFileStreamReader.ReadLine();
            }
            for (int i = 0; i < 4; i++)
            {
                str = OutFileStreamReader.ReadLine();
            }

            //读取数据
            for (int i = 0; i < numberOfAtom; i++)
            {
                str = OutFileStreamReader.ReadLine();
                tmpWords = TextTools.BasisTextTools.GetStringSeparatedbySpaces(str);
                atomicNumber[i] = Convert.ToInt32(tmpWords[1]);
                atomicType[i] = Convert.ToInt32(tmpWords[2]);
                coordinates_Angstroms[i, 0] = Convert.ToDouble(tmpWords[3]);
                coordinates_Angstroms[i, 1] = Convert.ToDouble(tmpWords[4]);
                coordinates_Angstroms[i, 2] = Convert.ToDouble(tmpWords[5]);
            }

            return;
        }

        /// <summary>
        /// 读标准取向的力
        /// </summary>
        /// <param name="force">力</param>
        public void ReadInputOrientationForce(out double[] force)
        {
            force = new double[numberOfX_Cartesian];
            string[] tmpWords = new string[5];

            //正式开始操作
            string str;

            //定位
            str = OutFileStreamReader.ReadLine();             //读文件的一行
            while (str.Trim() != "***** Axes restored to original set *****" && OutFileStreamReader.Peek() > -1)               //应用一个while循环，条件是文件不结束
            {
                str = OutFileStreamReader.ReadLine();
            }
            for (int i = 0; i < 5; i++)
            {
                str = OutFileStreamReader.ReadLine();
            }

            //读取数据
            for (int i = 0; i < numberOfAtom; i++)
            {
                str = OutFileStreamReader.ReadLine();
                tmpWords = TextTools.BasisTextTools.GetStringSeparatedbySpaces(str);
                force[3 * i] = Convert.ToDouble(tmpWords[2]);
                force[3 * i + 1] = Convert.ToDouble(tmpWords[3]);
                force[3 * i + 2] = Convert.ToDouble(tmpWords[4]);
            }
            return;
        }


        /// <summary>
        /// 读L703后的力常数矩阵，即输入坐标下的力常数矩阵
        /// </summary>
        /// <param name="forceConstant">力常数矩阵</param>
        public string[,] GetL703Hessian()
        {
            string[,] strForceConstant = new string[numberOfX_Cartesian, numberOfX_Cartesian];
            //数据准备
            string str = "";         //临时存放每一行数据
            string[] Data = new string[6];           //把每一行数据按空格分成六份,每一份为一个力常数数据
            //初始化每行数据Data
            for (int i = 0; i < 6; i++)
            {
                Data[i] = "ForceConstant";
            }
            int Block = 0;                            //力常数矩阵在Out文件中被分成的块数
            Block = Convert.ToInt32(Math.Floor(Convert.ToDouble(numberOfX_Cartesian) / 5) + 1);         //力常数矩阵的块数
            //初始化力常数矩阵
            for (int i = 0; i < numberOfX_Cartesian; i++)
            {
                for (int j = 0; j < numberOfX_Cartesian; j++)
                {
                    strForceConstant[i, j] = "bnulk";
                }
            }

            //正式开始操作

            //定位
            while (str.Trim() != "Hessian after L703:" && OutFileStreamReader.Peek() > -1)
            {
                str = OutFileStreamReader.ReadLine();             //读文件的一行
            }

            //读取数据
            str = OutFileStreamReader.ReadLine();                        //跳过没有数据的一行
            for (int i = 0; i < Block; i++)              //按块读数据
            {
                for (int m = 5 * i; m < numberOfX_Cartesian; m++)      //读行
                {
                    //读文本中每一行数据
                    str = OutFileStreamReader.ReadLine();
                    Data = TextTools.BasisTextTools.GetStringSeparatedbySpaces(str);
                    if (i * 5 + 5 <= numberOfX_Cartesian)                    //判断是否为最后一个力常数块
                                                                   //不是最后一个力常数块
                    {
                        for (int n = 5 * i; n < 5 + 5 * i; n++) //读列
                        {
                            if (n <= m)
                            {
                                strForceConstant[m, n] = Data[n - 5 * i + 1];
                            }
                        }
                    }
                    else
                    //最后一个力常数块
                    {
                        for (int n = 5 * i; n < numberOfX_Cartesian; n++) //读列
                        {
                            if (n <= m)
                            {
                                strForceConstant[m, n] = Data[n - 5 * i + 1];
                            }
                        }
                    }
                }
                str = OutFileStreamReader.ReadLine();     //跳过Block之间的构型参数
            }

            //补齐力常数矩阵
            for (int m = 0; m < numberOfX_Cartesian; m++)
            {
                for (int n = 0; n < numberOfX_Cartesian; n++)
                {
                    if (m < n)
                    {
                        strForceConstant[m, n] = strForceConstant[n, m];
                    }
                }
            }
            //把Out文件力常数矩阵矩阵元中的D改为E，然后把字符串转为双精度的数
            for (int m = 0; m < numberOfX_Cartesian; m++)
            {
                for (int n = 0; n < numberOfX_Cartesian; n++)
                {
                    strForceConstant[m, n] = strForceConstant[m, n].Replace('D', 'E');
                }
            }

            return strForceConstant;
        }






    }

 
}
