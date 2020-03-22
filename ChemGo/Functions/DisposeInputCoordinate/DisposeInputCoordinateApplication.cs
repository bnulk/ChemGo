using System;
using System.Collections.Generic;
using System.Text;
using ChemGo.Data;

namespace ChemGo.Functions.DisposeInputCoordinate
{
    class DisposeInputCoordinateApplication
    {
        private InputFile inputFile;

        private int numberOfAtoms;                                                 //原子个数
        private Geometry geometry;                                                 //标准取向笛卡尔坐标数据
        private ZMatrix zMatrix;                                                   //内坐标

        /// <summary>
        /// 原子个数
        /// </summary>
        public int NumberOfAtoms { get => numberOfAtoms; set => numberOfAtoms = value; }
        /// <summary>
        /// 标准取向笛卡尔坐标数据
        /// </summary>
        public Geometry Geometry { get => geometry; set => geometry = value; }
        /// <summary>
        /// 输入内坐标的参数
        /// </summary>
        public ZMatrix ZMatrix { get => zMatrix; set => zMatrix = value; }

        public DisposeInputCoordinateApplication(InputFile inputFile)
        {
            this.inputFile = inputFile;
        }


        public void Run()
        {
            ObtainNumberOfAtoms();
            if (inputFile.labels.control.coordinateType == CoordinateType.Cartesian)
            {
                if (inputFile.coordinateType == CoordinateType.Cartesian)
                {
                    ObtainStandardOrientationCartesianBasedOnInputCartesian();
                }
                else
                {
                    throw new DisposeInputCoordinateException("incorrect coordinateType");
                    //ObtainStandardOrientationCartesianBasedOnInputZMatrix();
                }
            }
            if (inputFile.labels.control.coordinateType == CoordinateType.zMatrix)
            {
                if (inputFile.coordinateType == CoordinateType.Cartesian)
                {
                    throw new DisposeInputCoordinateException("incorrect coordinateType");
                    //inputFile.labels.control.coordinateType = CoordinateType.Cartesian;   //仍用输入的内坐标
                    //ObtainStandardOrientationCartesianBasedOnInputCartesian();
                }
                else
                {
                    ObtainStandardOrientationCartesianBasedOnInputZMatrix();
                    ObtainZMatrix();
                }
            }
        }


        /// <summary>
        /// 获取原子个数
        /// </summary>
        private void ObtainNumberOfAtoms()
        {
            if (inputFile.coordinateType == CoordinateType.zMatrix)
            {
                numberOfAtoms = inputFile.inputZmatrix.numberOfAtoms;
            }
            else
            {
                numberOfAtoms = inputFile.inputCartesian.numberOfAtoms;
            }
        }

        /// <summary>
        /// 根据输入的笛卡尔坐标，获取标准取向的笛卡尔坐标（暂无）
        /// </summary>
        private void ObtainStandardOrientationCartesianBasedOnInputCartesian()
        {
            geometry.numberOfAtoms = inputFile.inputCartesian.numberOfAtoms;
            geometry.atomicNumbers = new int[numberOfAtoms];
            geometry.realAtomicWeights = new double[numberOfAtoms];
            geometry.standardOrientationCoordinates = new double[numberOfAtoms, 3];
            for (int i = 0; i < numberOfAtoms; i++)
            {
                geometry.atomicNumbers[i] = inputFile.inputCartesian.atomicNumbers[i];
                geometry.realAtomicWeights[i] = inputFile.inputCartesian.realAtomicWeights[i];
                for (int j = 0; j < 3; j++)
                {
                    geometry.standardOrientationCoordinates[i, j] = Convert.ToDouble(inputFile.inputCartesian.coordinates[i, j + 1]);
                }
            }
        }

        /// <summary>
        /// 根据输入的内坐标，获取标准取向的笛卡尔坐标（暂无）
        /// </summary>
        private void ObtainStandardOrientationCartesianBasedOnInputZMatrix()
        {
            return;
        }

        /// <summary>
        /// 获取内坐标参数
        /// </summary>
        private void ObtainZMatrix()
        {
            zMatrix.numberOfAtoms = inputFile.inputZmatrix.numberOfAtoms;
            zMatrix.coordinates = new string[numberOfAtoms, 7];
            zMatrix.atomicNumbers = new int[numberOfAtoms];
            zMatrix.realAtomicWeights = new double[numberOfAtoms];

            int numberOfParameter = inputFile.inputZmatrix.parameter.GetLength(0);
            zMatrix.parameterName = new string[numberOfParameter];
            zMatrix.parameterValue = new double[numberOfParameter];

            for (int i = 0; i < numberOfAtoms; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    zMatrix.coordinates[i, j] = inputFile.inputZmatrix.coordinates[i, j];
                }
                zMatrix.atomicNumbers[i] = inputFile.inputZmatrix.atomicNumbers[i];
                zMatrix.realAtomicWeights[i] = inputFile.inputZmatrix.realAtomicWeights[i];
            }

            for (int i = 0; i < numberOfParameter; i++)
            {
                zMatrix.parameterName[i] = inputFile.inputZmatrix.parameter[i, 0];
                zMatrix.parameterValue[i] = Convert.ToDouble(inputFile.inputZmatrix.parameter[i, 1]);
            }
        }


    }
}
