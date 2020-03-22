using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ChemGo.Data;
using ChemGo.Auxiliary.NumericalOptimization.SingleConstrainedOptimization;

namespace ChemGo.Auxiliary.ExternalProgram.Gaussian.GaussianService.GaussianMecpService
{
    class GaussianMecpService_CreateInputFile
    {
        private GaussianInputFileMaterial[] gaussianInputFileMaterials = new GaussianInputFileMaterial[2];
        private GaussianMecpVariablePackage variablePackage;



        public GaussianMecpService_CreateInputFile(GaussianMecpVariablePackage variablePackage, GaussianInputFileMaterial[] gaussianInputFileMaterials)
        {
            this.variablePackage = variablePackage;
            this.gaussianInputFileMaterials = gaussianInputFileMaterials;
        }

        

        public void Run()
        {
            CreateTmpDirectory();
            //如果有liuk文件夹，则从文件夹liuk中读取chk文件。
            ReadChkFromLiukFold();
            CreateGjf1File();
            CreateGjf2File();
            
        }




        /// <summary>
        /// 把双精度数字转换成高斯输入文件的格式
        /// </summary>
        /// <param name="parameterValue"></param>
        /// <returns></returns>
        private string ConvertToGaussianWritingFormat(double parameterValue)
        {
            string result;

            //整数问题            
            if (Math.Floor(parameterValue) == parameterValue)                  //如果parameterValue为整数，加上小数点
            {
                result = parameterValue.ToString() + ".0";
            }
            else
            {
                result = parameterValue.ToString();
            }
            //科学计数法问题
            if (Math.Abs(Convert.ToDouble(parameterValue)) < 1E-4)
            {
                result = "0.0";
            }

            return result;
        }


        /// <summary>
        /// 创建tmp临时文件目录
        /// </summary>
        private void CreateTmpDirectory()
        {
            //创造一个tmp目录，用来写临时文件
            Directory.CreateDirectory(variablePackage.workPath);
        }

        /// <summary>
        /// 复制临时文件夹liuk中的chk文件。
        /// </summary>
        private void ReadChkFromLiukFold()
        {
            if (Directory.Exists("liuk"))
            {
                try
                {
                    string[] chkList = Directory.GetFiles("liuk", "*.chk");
                    foreach (string f in chkList)
                    {
                        //remove path from the file name
                        string fName = f.Substring(5);
                        if (OperatingSystem.OS_BasisFunction.ObtianOSName() == "windows")
                        {
                            File.Copy(@variablePackage.workPath + "\\liuk\\" + fName, @variablePackage.workPath + "\\tmp\\" + fName, true);
                        }
                        else
                        {
                            File.Copy(@variablePackage.workPath + "//liuk//" + fName, @variablePackage.workPath + "//tmp//" + fName, true);
                        }

                    }
                }
                catch
                {
                    throw new ExternalProgramException("Chk file not found in the liuk folder.\n" +
                        "ChemGo.Auxiliary.ExternalProgram.Gaussian.GaussianService.GaussianMecpService.GaussianMecpService_CreateInputFile.ReadChkFromLiukFold()");
                }
            }
            return;
        }


        /// <summary>
        /// 创建gjf1文件
        /// </summary>
        private void CreateGjf1File()
        {
            int i, cycle;
            StreamWriter gjf1;
            gjf1 = File.CreateText(variablePackage.gjf1FullPath);

            cycle = gaussianInputFileMaterials[0].firstSection.Length;
            for (i=0;i<cycle;i++)
            {
                gjf1.Write(gaussianInputFileMaterials[0].firstSection[i] + "\n");
            }

            cycle = gaussianInputFileMaterials[0].routeSection.Length;
            for (i = 0; i < cycle; i++)
            {
                if (i == cycle - 1)
                {
                    if (variablePackage.isCalculateHessian==true)
                        gjf1.Write(gaussianInputFileMaterials[0].routeSection[i] + " freq=noraman IOP(7/33=1)" + "\n");
                    else
                        gjf1.Write(gaussianInputFileMaterials[0].routeSection[i] + " force IOP(7/33=1)" + "\n");
                }
                else
                {
                    gjf1.Write(gaussianInputFileMaterials[0].routeSection[i] + "\n");
                }
            }
            gjf1.Write("\n");
            gjf1.Flush();

            //Title部分
            gjf1.Write(gaussianInputFileMaterials[0].titleSection + "\n");
            gjf1.Write("\n");
            gjf1.Flush();

            //电荷和自旋多重度
            gjf1.Write(gaussianInputFileMaterials[0].chargeAndMultiplicity + "\n");


            //坐标
            if (gaussianInputFileMaterials[0].coordinateType == CoordinateType.zMatrix)
            {
                cycle = gaussianInputFileMaterials[0].molecularSpecification_ZMatrix.Length;
                for (i = 0; i < cycle; i++)
                {
                    gjf1.Write(gaussianInputFileMaterials[0].molecularSpecification_ZMatrix[i] + "\n");
                }
                gjf1.Write("\n");
                cycle = gaussianInputFileMaterials[0].molecularPara_ZMatrix_Value.Length;
                for (i = 0; i < cycle; i++)
                {
                    gjf1.Write(gaussianInputFileMaterials[0].molecularPara_ZMatrix_Name[i] + "     =     "
                        + ConvertToGaussianWritingFormat(gaussianInputFileMaterials[0].molecularPara_ZMatrix_Value[i]) + "\n");
                }
            }

            if (gaussianInputFileMaterials[0].coordinateType == CoordinateType.Cartesian)
            {
                cycle = gaussianInputFileMaterials[0].molecularCartesian_elements.Length;
                for (i = 0; i < cycle; i++)
                {
                    gjf1.Write(gaussianInputFileMaterials[0].molecularCartesian_elements[i].PadRight(10) 
                        + gaussianInputFileMaterials[0].molecularCartesian_Value[i,0].ToString("0.00000000").PadLeft(15)
                        + gaussianInputFileMaterials[0].molecularCartesian_Value[i, 1].ToString("0.00000000").PadLeft(15)
                        + gaussianInputFileMaterials[0].molecularCartesian_Value[i, 2].ToString("0.00000000").PadLeft(15) + "\n");
                }
            }

            //附加部分
            gjf1.Write("\n");
            cycle = gaussianInputFileMaterials[0].addition.Length;
            for (i = 0; i < cycle; i++)
            {
                gjf1.Write(gaussianInputFileMaterials[0].addition[i] + "\n");
            }

            gjf1.Flush();
            gjf1.Close();
        }


        /// <summary>
        /// 创建gjf2文件
        /// </summary>
        private void CreateGjf2File()
        {
            int i, cycle;
            StreamWriter gjf2;
            gjf2 = File.CreateText(variablePackage.gjf2FullPath);

            cycle = gaussianInputFileMaterials[1].firstSection.Length;
            for (i = 0; i < cycle; i++)
            {
                gjf2.Write(gaussianInputFileMaterials[1].firstSection[i] + "\n");
            }

            cycle = gaussianInputFileMaterials[1].routeSection.Length;
            for (i = 0; i < cycle; i++)
            {
                if (i == cycle - 1)
                {
                    if (variablePackage.isCalculateHessian == true)
                        gjf2.Write(gaussianInputFileMaterials[1].routeSection[i] + " freq=noraman IOP(7/33=1)" + "\n");
                    else
                        gjf2.Write(gaussianInputFileMaterials[1].routeSection[i] + " force IOP(7/33=1)" + "\n");
                }
                else
                {
                    gjf2.Write(gaussianInputFileMaterials[1].routeSection[i] + "\n");
                }
            }
            gjf2.Write("\n");

            //Title部分
            gjf2.Write(gaussianInputFileMaterials[1].titleSection + "\n");
            gjf2.Write("\n");

            //电荷和自旋多重度
            gjf2.Write(gaussianInputFileMaterials[1].chargeAndMultiplicity + "\n");


            //坐标
            if (gaussianInputFileMaterials[1].coordinateType == CoordinateType.zMatrix)
            {
                cycle = gaussianInputFileMaterials[1].molecularSpecification_ZMatrix.Length;
                for (i = 0; i < cycle; i++)
                {
                    gjf2.Write(gaussianInputFileMaterials[1].molecularSpecification_ZMatrix[i] + "\n");
                }
                gjf2.Write("\n");
                cycle = gaussianInputFileMaterials[1].molecularPara_ZMatrix_Value.Length;
                for (i = 0; i < cycle; i++)
                {
                    gjf2.Write(gaussianInputFileMaterials[1].molecularPara_ZMatrix_Name[i] + "     =     "
                        + ConvertToGaussianWritingFormat(gaussianInputFileMaterials[0].molecularPara_ZMatrix_Value[i]) + "\n");
                }
            }

            if (gaussianInputFileMaterials[1].coordinateType == CoordinateType.Cartesian)
            {
                cycle = gaussianInputFileMaterials[1].molecularCartesian_elements.Length;
                for (i = 0; i < cycle; i++)
                {
                    gjf2.Write(gaussianInputFileMaterials[1].molecularCartesian_elements[i].PadRight(10)
                        + gaussianInputFileMaterials[1].molecularCartesian_Value[i, 0].ToString("0.00000000").PadLeft(15)
                        + gaussianInputFileMaterials[1].molecularCartesian_Value[i, 1].ToString("0.00000000").PadLeft(15)
                        + gaussianInputFileMaterials[1].molecularCartesian_Value[i, 2].ToString("0.00000000").PadLeft(15) + "\n");
                }
            }

            //附加部分
            gjf2.Write("\n");
            cycle = gaussianInputFileMaterials[1].addition.Length;
            for (i = 0; i < cycle; i++)
            {
                gjf2.Write(gaussianInputFileMaterials[1].addition[i] + "\n");
            }

            gjf2.Flush();
            gjf2.Close();
        }

    }
}
