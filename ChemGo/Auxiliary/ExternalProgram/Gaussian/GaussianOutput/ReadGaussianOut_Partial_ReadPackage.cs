using System;
using System.Collections.Generic;
using System.Text;

namespace ChemGo.Auxiliary.ExternalProgram.Gaussian.GaussianOutput
{
    partial class ReadGaussianOut
    {
        /// <summary>
        /// 输入取向的坐标、梯度和力常数矩阵
        /// </summary>
        /// <returns>Hessian包</returns>
        public HessianPackage ReadHessianPackage_ZMatrix()
        {
            HessianPackage hessianPackage = new HessianPackage();
            hessianPackage.numberOfX = numberOfX_ZMatrix;
            hessianPackage.xName = new string[numberOfX_ZMatrix];
            hessianPackage.x = new double[numberOfX_ZMatrix];
            hessianPackage.gradient = new double[numberOfX_ZMatrix];
            hessianPackage.hessian = new double[numberOfX_ZMatrix, numberOfX_ZMatrix];

            string[,] strGradient = new string[numberOfX_ZMatrix, 3];
            string[,] strHessian = new string[numberOfX_ZMatrix, numberOfX_ZMatrix];

            //读梯度
            strGradient = GetForceParams_ZMatrix();
            //读HF=后面的能量
            hessianPackage.energy = GetHFTypeEnergy();
            Update();
            //读Hessian
            strHessian = GetForceConstant_Zmatrix();

            for(int i=0;i< numberOfX_ZMatrix; i++)
            {
                hessianPackage.xName[i] = strGradient[i, 0];
                hessianPackage.x[i] = Convert.ToDouble(strGradient[i, 1]);
                hessianPackage.gradient[i] = Convert.ToDouble(strGradient[i, 2]) * (-1);
                for (int j=0;j< numberOfX_ZMatrix; j++)
                {
                    hessianPackage.hessian[i, j] = Convert.ToDouble(strHessian[i, j]);
                }
            }

            return hessianPackage;
        }

        /// <summary>
        /// 输入取向的坐标、梯度和力常数矩阵
        /// </summary>
        /// <returns>Hessian包</returns>
        public HessianPackage ReadHessianPackage_Cartesian()
        {
            HessianPackage hessianPackage = new HessianPackage();
            hessianPackage.numberOfX = numberOfX_Cartesian;
            hessianPackage.xName = new string[numberOfX_Cartesian];
            hessianPackage.x = new double[numberOfX_Cartesian];
            hessianPackage.gradient = new double[numberOfX_Cartesian];
            hessianPackage.hessian = new double[numberOfX_Cartesian, numberOfX_Cartesian];

            string[,] strGradient = new string[numberOfX_Cartesian, 3];
            string[,] strHessian = new string[numberOfX_Cartesian, numberOfX_Cartesian];

            //读梯度
            strGradient = GetForceParams_Cartesian();
            //读HF = 后面的能量
            hessianPackage.energy = GetHFTypeEnergy();
            Update();
            //读Hessian
            //strHessian = GetL703Hessian();
            strHessian = GetForceConstant_Cartesian();

            for (int i = 0; i < numberOfX_Cartesian; i++)
            {
                hessianPackage.xName[i] = strGradient[i, 0];
                hessianPackage.x[i] = Convert.ToDouble(strGradient[i, 1]);
                hessianPackage.gradient[i] = Convert.ToDouble(strGradient[i, 2]) * (-1);
                for (int j = 0; j < numberOfX_Cartesian; j++)
                {
                    hessianPackage.hessian[i, j] = Convert.ToDouble(strHessian[i, j]);
                }
            }
            return hessianPackage;
        }

        /// <summary>
        /// 输入取向的坐标、梯度
        /// </summary>
        /// <returns>梯度包</returns>
        public GradientPackage ReadGradientPackage_ZMatrix()
        {
            GradientPackage gradientPackage = new GradientPackage();
            gradientPackage.numberOfX = numberOfX_ZMatrix;
            gradientPackage.xName = new string[numberOfX_ZMatrix];
            gradientPackage.x = new double[numberOfX_ZMatrix];
            gradientPackage.gradient = new double[numberOfX_ZMatrix];

            string[,] strGradient = new string[numberOfX_ZMatrix, 3];

            //读梯度
            strGradient = GetForceParams_ZMatrix();
            //读HF = 后面的能量
            gradientPackage.energy = GetHFTypeEnergy();

            for (int i = 0; i < numberOfX_ZMatrix; i++)
            {
                gradientPackage.xName[i] = strGradient[i, 0];
                gradientPackage.x[i] = Convert.ToDouble(strGradient[i, 1]);
                gradientPackage.gradient[i] = Convert.ToDouble(strGradient[i, 2]) * (-1);
            }

            return gradientPackage;
        }

        /// <summary>
        /// 输入取向的坐标、梯度
        /// </summary>
        /// <returns>梯度包</returns>
        public GradientPackage ReadGradientPackage_Cartesian()
        {
            GradientPackage gradientPackage = new GradientPackage();
            gradientPackage.numberOfX = numberOfX_Cartesian;
            gradientPackage.xName = new string[numberOfX_Cartesian];
            gradientPackage.x = new double[numberOfX_Cartesian];
            gradientPackage.gradient = new double[numberOfX_Cartesian];

            string[,] strGradient = new string[numberOfX_Cartesian, 3];

            //读梯度
            strGradient = GetForceParams_Cartesian();
            //读HF = 后面的能量
            gradientPackage.energy = GetHFTypeEnergy();

            for (int i = 0; i < numberOfX_Cartesian; i++)
            {
                gradientPackage.xName[i] = strGradient[i, 0];
                gradientPackage.x[i] = Convert.ToDouble(strGradient[i, 1]);
                gradientPackage.gradient[i] = Convert.ToDouble(strGradient[i, 2]) * (-1);
            }
            return gradientPackage;
        }
    }
}
