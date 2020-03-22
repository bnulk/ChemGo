using System;
using System.Collections.Generic;
using System.Text;
using ChemGo.Data;
using ChemGo.Auxiliary.ExternalProgram.Gaussian.GaussianOutput;

namespace ChemGo.Auxiliary.ExternalProgram.Gaussian.GaussianService.GaussianMecpService
{
    class GaussianMecpService_ObtainCalculatingData
    {
        private string out1FullPath, out2FullPath;
        private bool isReadHessian;
        private Data_ChemGo data_ChemGo;
        private HessianPackage hessianPackage;
        private GradientPackage gradientPackage;

        private MecpPackage mecpPackage;

        public MecpPackage MecpPackage { get => mecpPackage; set => mecpPackage = value; }

        public GaussianMecpService_ObtainCalculatingData(GaussianMecpVariablePackage variablePackage, Data_ChemGo data_ChemGo)
        {
            this.out1FullPath = variablePackage.out1FullPath;
            this.out2FullPath = variablePackage.out2FullPath;
            this.isReadHessian = variablePackage.isCalculateHessian;
            this.data_ChemGo = data_ChemGo;
        }

        

        public void Run()
        {
            ReadGaussianOut app1 = new ReadGaussianOut(out1FullPath);
            ReadGaussianOut app2 = new ReadGaussianOut(out2FullPath);
            if(data_ChemGo.inputFile.coordinateType==CoordinateType.zMatrix)
            {
                if(isReadHessian==true)
                {
                    hessianPackage = app1.ReadHessianPackage_ZMatrix();
                    HessianPackageToMecpPackage_State1();
                    hessianPackage = app2.ReadHessianPackage_ZMatrix();
                    HessianPackageToMecpPackage_State2();
                }
                else
                {
                    gradientPackage = app1.ReadGradientPackage_ZMatrix();
                    GradientPackageToMecpPackage_State1();
                    gradientPackage = app2.ReadGradientPackage_ZMatrix();
                    GradientPackageToMecpPackage_State2();
                }
            }
            else
            {
                if(isReadHessian==true)
                {
                    hessianPackage = app1.ReadHessianPackage_Cartesian();
                    HessianPackageToMecpPackage_State1();
                    hessianPackage = app2.ReadHessianPackage_Cartesian();
                    HessianPackageToMecpPackage_State2();
                }
                else
                {
                    gradientPackage = app1.ReadGradientPackage_Cartesian();
                    GradientPackageToMecpPackage_State1();
                    gradientPackage = app2.ReadGradientPackage_Cartesian();
                    GradientPackageToMecpPackage_State2();
                }
            }
        }

        private void HessianPackageToMecpPackage_State1()
        {
            mecpPackage.numberOfX = hessianPackage.numberOfX;
            mecpPackage.xName = hessianPackage.xName;
            mecpPackage.x = hessianPackage.x;
            mecpPackage.gradient1 = hessianPackage.gradient;
            mecpPackage.hessian1 = hessianPackage.hessian;
            switch(data_ChemGo.inputFile.labels.keyword_mecp.scfTyp1.ToLower())
            {
                case "hftyp":
                    mecpPackage.energy1 = hessianPackage.energy;
                    break;
                case "tdtyp":
                    ReadGaussianOut app = new ReadGaussianOut(out1FullPath);
                    mecpPackage.energy1 = app.GetEnergy_TD();
                    break;
                default:
                    throw new GaussianMecpServiceException("HessianPackage2MecpPackage_State1() Error.\n\n  + Unrecognized \"scftyp\" type. \n\n");
            }
        }

        private void HessianPackageToMecpPackage_State2()
        {
            mecpPackage.gradient2 = hessianPackage.gradient;
            mecpPackage.hessian2 = hessianPackage.hessian;
            switch (data_ChemGo.inputFile.labels.keyword_mecp.scfTyp1.ToLower())
            {
                case "hftyp":
                    mecpPackage.energy2 = hessianPackage.energy;
                    break;
                case "tdtyp":
                    ReadGaussianOut app = new ReadGaussianOut(out2FullPath);
                    mecpPackage.energy2 = app.GetEnergy_TD();
                    break;
                default:
                    throw new GaussianMecpServiceException("HessianPackage2MecpPackage_State1() Error.\n\n  + Unrecognized \"scftyp\" type. \n\n");
            }
        }

        private void GradientPackageToMecpPackage_State1()
        {
            mecpPackage.numberOfX = gradientPackage.numberOfX;
            mecpPackage.xName = gradientPackage.xName;
            mecpPackage.x = gradientPackage.x;
            mecpPackage.gradient1 = gradientPackage.gradient;
            mecpPackage.hessian1 = new double[gradientPackage.numberOfX, gradientPackage.numberOfX];
            switch (data_ChemGo.inputFile.labels.keyword_mecp.scfTyp1.ToLower())
            {
                case "hftyp":
                    mecpPackage.energy1 = gradientPackage.energy;
                    break;
                case "tdtyp":
                    ReadGaussianOut app = new ReadGaussianOut(out1FullPath);
                    mecpPackage.energy1 = app.GetEnergy_TD();
                    break;
                default:
                    throw new GaussianMecpServiceException("HessianPackage2MecpPackage_State1() Error.\n\n  + Unrecognized \"scftyp\" type. \n\n");
            }
        }

        private void GradientPackageToMecpPackage_State2()
        {
            mecpPackage.gradient2 = gradientPackage.gradient;
            mecpPackage.hessian2 = new double[mecpPackage.numberOfX, mecpPackage.numberOfX];
            switch (data_ChemGo.inputFile.labels.keyword_mecp.scfTyp1.ToLower())
            {
                case "hftyp":
                    mecpPackage.energy2 = gradientPackage.energy;
                    break;
                case "tdtyp":
                    ReadGaussianOut app = new ReadGaussianOut(out2FullPath);
                    mecpPackage.energy2 = app.GetEnergy_TD();
                    break;
                default:
                    throw new GaussianMecpServiceException("HessianPackage2MecpPackage_State1() Error.\n\n  + Unrecognized \"scftyp\" type. \n\n");
            }
        }

    }
}
