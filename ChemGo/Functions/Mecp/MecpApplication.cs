using System;
using System.Collections.Generic;
using System.Text;
using ChemGo.Data;
using ChemGo.Functions.Mecp.GenerateOtherProgramInputFileMaterial;

namespace ChemGo.Functions.Mecp
{
    class MecpApplication
    {
        private Data_ChemGo data_ChemGo;


        public MecpApplication(Data_ChemGo data_ChemGo)
        {
            this.data_ChemGo = data_ChemGo;
        }

        public void Run()
        {
            if(data_ChemGo.inputFile.labels.control.inputFileType!= InputFileType.ChemGo)
            {
                GenerateOtherProgramInputFileMaterial();
            }            
        }

        /// <summary>
        /// 生成输入文件素材
        /// </summary>
        private void GenerateOtherProgramInputFileMaterial()
        {
            switch(data_ChemGo.inputFile.labels.control.inputFileType)
            {
                case InputFileType.Gaussian:
                    GenerateGaussianInputFileMaterial generateGaussianInputFileMaterial = new GenerateGaussianInputFileMaterial(data_ChemGo.otherProgramData.data_Gaussian.gaussianInputSegment,
                        data_ChemGo.otherProgramData.data_Gaussian.interfaceBetweenGaussianAndChemGo, data_ChemGo.inputFile);
                    generateGaussianInputFileMaterial.Run();
                    break;
                default:
                    throw new MecpException("Unknown fileType.\n  ChemGo.Functions.Mecp.GenerateOtherProgramInputFileMaterial(data_ChemGo.otherProgramData.data_Gaussian.gaussianInputSegment,data_ChemGo.otherProgramData.data_Gaussian.interfaceBetweenGaussianAndChemGo, data_ChemGo.inputFile) Error.\n");
            }
            return;
        }

        /// <summary>
        /// 产生输入文件
        /// </summary>
        private void CreateInputFile()
        {

        }

        /// <summary>
        /// 计算单点
        /// </summary>
        private void CalculateSinglePoint()
        {

        }

        /// <summary>
        /// 是否终止Mecp计算
        /// </summary>
        private bool IsTerminate()
        {
            bool isTerminate = false;
            return isTerminate;
        }

        /// <summary>
        /// 获取单点信息
        /// </summary>
        private void ObtainSinglePointInformation()
        {

        }
        
        /// <summary>
        /// 产生新参数
        /// </summary>
        private void GenerateNewParameter()
        {

        }

        /// <summary>
        /// 更新输入文件素材
        /// </summary>
        private void UpdataInputFileMaterial()
        {
            return;
        }


    }
}
