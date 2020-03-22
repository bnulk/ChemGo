using System;
using System.Collections.Generic;
using System.Text;
using ChemGo.Data;
using ChemGo.Data.DataGaussian;
using ChemGo.Auxiliary.TextTools;

namespace ChemGo.Input.InputFileReadings.GaussianInputFileReadings
{
    class ChemGoInfoReader
    {
        private List<string> inputList;
        private string chemGoPart;
        private string segmentPart;
        private string keywordPart;
        private string[] segment;
        private string[,] keywordAndParameter;

        private Labels labels;
        private Data.DataGaussian.InterfaceBetweenGaussianAndChemGo interfaceBetweenGaussianAndChemGo;

        /// <summary>
        /// ChemGo的标签
        /// </summary>
        public Labels Labels { get => labels; set => labels = value; }
        /// <summary>
        /// Gaussian和ChemGo交互信息
        /// </summary>
        public InterfaceBetweenGaussianAndChemGo InterfaceBetweenGaussianAndChemGo { get => interfaceBetweenGaussianAndChemGo; set => interfaceBetweenGaussianAndChemGo = value; }
        

        public ChemGoInfoReader(List<string> inputList)
        {
            this.inputList = inputList;
            labels.control.inputFileType = InputFileType.Gaussian;
        }

        public void Run()
        {
            //从输入文件中得到ChemGo的输入部分
            ObtainChemGoPart();
            //获取碎片部分
            ObtainSegmentPart();
            //获取关键词部分
            ObtainKeywordPart();
            //获取Segment
            ObtainSegment();
            //获取关键词
            ObtainKeyword();

            //获取ChemGo和Gaussian的交互信息
            ObtainInterfaceBetweenGaussianAndChemGo();
            //初始化任务标签            
            InitializeTaskLabel();
            //获取任务的标签信息
            ObtainTaskLabel();
            //获取控制部分的关键词信息
            ObtainControlParameter();
        }

        /// <summary>
        /// 从输入文件中获取ChemGo的输入部分
        /// </summary>
        private void ObtainChemGoPart()
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < inputList.Count; i++)
            {
                stringBuilder.Append(inputList[i]);
            }
            chemGoPart = stringBuilder.ToString();
            chemGoPart = BasisTextTools.GetStringBetweenTwoString(chemGoPart, "{", "}");
        }

        /// <summary>
        /// 从chemGoPart中获取segmentPart部分
        /// </summary>
        private void ObtainSegmentPart()
        {
            segmentPart = BasisTextTools.GetStringBetweenTwoString(chemGoPart, "<", ">");
        }

        /// <summary>
        /// 从chemGoPart中获取keywordPart部分
        /// </summary>
        private void ObtainKeywordPart()
        {
            keywordPart = BasisTextTools.GetStringOutsideTwoString(chemGoPart, "<", ">");
        }

        /// <summary>
        /// segment[]接收segmentPart中的数据。
        /// </summary>
        private void ObtainSegment()
        {
            string[] tmpSegment;
            tmpSegment = segmentPart.Split('|');
            segment = new string[tmpSegment.Length];
            for (int i = 0; i < tmpSegment.Length; i++)
            {
                segment[i] = tmpSegment[i].Trim();
            }

        }

        /// <summary>
        /// keywordAndParameter[,]接收keywordPart中的数据。
        /// </summary>
        private void ObtainKeyword()
        {
            string[] tmpKeyword;
            string[] tmpKeywordAndParameter;
            List<string> tmpListKeyword = new List<string>();
            tmpKeyword = keywordPart.Split(' ');
            for (int i = 0; i < tmpKeyword.Length; i++)
            {
                int j = 0;
                if (tmpKeyword[i] != "")
                {
                    j++;
                    tmpListKeyword.Add(tmpKeyword[i]);
                }
            }

            keywordAndParameter = new string[tmpListKeyword.Count, 2];
            for (int i = 0; i < tmpListKeyword.Count; i++)
            {
                if (tmpListKeyword[i].Contains('='))
                {
                    tmpKeywordAndParameter = tmpListKeyword[i].Split('=');
                    keywordAndParameter[i, 0] = tmpKeywordAndParameter[0].Trim();
                    keywordAndParameter[i, 1] = tmpKeywordAndParameter[1].Trim();
                }
                else
                {
                    keywordAndParameter[i, 0] = tmpListKeyword[i].Trim();
                    keywordAndParameter[i, 1] = null;
                }
            }
        }

        /// <summary>
        /// 获取ChemGo和Gaussian的交互信息
        /// </summary>
        private void ObtainInterfaceBetweenGaussianAndChemGo()
        {
            int n = keywordAndParameter.GetLength(0);
            interfaceBetweenGaussianAndChemGo.task = Task.sp;
            interfaceBetweenGaussianAndChemGo.cmd = "g09";

            for (int i = 0; i < n; i++)
            {
                if (keywordAndParameter[i, 0].ToLower() == "task")
                {
                    interfaceBetweenGaussianAndChemGo.task = GetTask(keywordAndParameter[i, 1]);
                }
                if (keywordAndParameter[i, 0].ToLower() == "cmd")
                {
                    interfaceBetweenGaussianAndChemGo.cmd = keywordAndParameter[i, 1];
                }
            }

            n = segment.Length;
            interfaceBetweenGaussianAndChemGo.segment = new string[n];
            for (int i = 0; i < n; i++)
            {
                InterfaceBetweenGaussianAndChemGo.segment[i] = segment[i];
            }
        }

        /// <summary>
        /// 获取任务的标签信息
        /// </summary>
        private void ObtainTaskLabel()
        {
            switch (InterfaceBetweenGaussianAndChemGo.task)
            {
                case Task.mecp:
                    ObtainMecpLabel();
                    break;
                default:
                    throw new ReadInputFileException("Can't find legitimate \"task\"");
            }
        }

        /// <summary>
        /// 根据参数，获取任务类型
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <returns>任务类型</returns>
        private Task GetTask(string parameter)
        {
            Task task;
            switch (parameter.ToLower())
            {
                case "sp":
                    task = Task.sp;
                    break;
                case "min":
                    task = Task.min;
                    break;
                case "mecp":
                    task = Task.mecp;
                    break;
                default:
                    throw new ReadInputFileException("Incorrect parameters in \"task\" keyword. ");
            }
            return task;
        }

        /// <summary>
        /// 获取Control的关键词参数
        /// </summary>
        private void ObtainControlParameter()
        {
            //初始化
            labels.control.cmd = interfaceBetweenGaussianAndChemGo.cmd;
            labels.control.task = interfaceBetweenGaussianAndChemGo.task;
            labels.control.inputFileType = InputFileType.Gaussian;
            //从输入文件中获取
            int n = keywordAndParameter.GetLength(0);
            for (int i = 0; i < n; i++)
            {
                switch (keywordAndParameter[i, 0].ToLower())
                {
                    case "cmd":
                        labels.control.cmd = keywordAndParameter[i, 1];
                        break;
                    case "coordinatetype":
                        switch(keywordAndParameter[i, 1].ToLower())
                        {
                            case "zmatrix":
                                labels.control.coordinateType = CoordinateType.zMatrix;
                                break;
                            case "cartesian":
                                labels.control.coordinateType = CoordinateType.Cartesian;
                                break;
                            default:
                                labels.control.coordinateType = CoordinateType.noInfo;
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 获取Mecp关键词
        /// </summary>
        private void ObtainMecpLabel()
        {

            int n = keywordAndParameter.GetLength(0);
            for (int i = 0; i < n; i++)
            {
                switch (keywordAndParameter[i, 0].ToLower())
                {
                    case "opt":
                        labels.keyword_mecp.opt = keywordAndParameter[i, 1];
                        break;
                    case "coordinatetype":
                        labels.keyword_mecp.coordinateType = keywordAndParameter[i, 1];
                        break;
                    case "scftyp1":
                        labels.keyword_mecp.scfTyp1 = keywordAndParameter[i, 1];
                        break;
                    case "scftyp2":
                        labels.keyword_mecp.scfTyp2 = keywordAndParameter[i, 1];
                        break;
                    case "maxcyc":
                        labels.keyword_mecp.maxCyc = Convert.ToInt32(keywordAndParameter[i, 1]);
                        break;
                    case "stepsize":
                        labels.keyword_mecp.stepSize = Convert.ToDouble(keywordAndParameter[i, 1]);
                        break;
                    case "gradientn":
                        labels.keyword_mecp.gradientN = Convert.ToInt32(keywordAndParameter[i, 1]);
                        break;
                    case "hessiann":
                        labels.keyword_mecp.hessianN = Convert.ToInt32(keywordAndParameter[i, 1]);
                        break;
                    case "energycon":
                        labels.keyword_mecp.energyCon = Convert.ToDouble(keywordAndParameter[i, 1]);
                        break;
                    case "maxcon":
                        labels.keyword_mecp.maxCon = Convert.ToDouble(keywordAndParameter[i, 1]);
                        break;
                    case "rmscon":
                        labels.keyword_mecp.rmsCon = Convert.ToDouble(keywordAndParameter[i, 1]);
                        break;
                    case "lambda":
                        labels.keyword_mecp.lambda = Convert.ToDouble(keywordAndParameter[i, 1]);
                        break;
                    case "isreadfirst":
                        labels.keyword_mecp.isReadFirst = Convert.ToBoolean(keywordAndParameter[i, 1]);
                        break;
                    case "showgradratiocriterionn":
                        labels.keyword_mecp.showGradRatioCriterionN = Convert.ToDouble(keywordAndParameter[i, 1]);
                        break;
                    case "showgradratiocriterion":
                        labels.keyword_mecp.showGradRatioCriterion = Convert.ToDouble(keywordAndParameter[i, 1]);
                        break;
                    case "judgement":
                        labels.keyword_mecp.judgement = keywordAndParameter[i, 1];
                        break;
                    case "mecpfreq":
                        if(keywordAndParameter[i, 1]!=null)
                        {
                            labels.keyword_mecp.mecpFreq = keywordAndParameter[i, 1];
                        }
                        else
                        {
                            labels.keyword_mecp.mecpFreq = "liu";
                        }
                        break;
                    default:
                        break;
                }
            }
        }


        /// <summary>
        /// 初始化interfaceBetweenGaussianAndChemGo
        /// </summary>
        private void InitializeInterfaceBetweenGaussianAndChemGo()
        {
            interfaceBetweenGaussianAndChemGo.cmd = "g09";
            interfaceBetweenGaussianAndChemGo.segment = null;
            interfaceBetweenGaussianAndChemGo.task = Task.sp;
        }

        /// <summary>
        /// 初始化任务标签
        /// </summary>
        private void InitializeTaskLabel()
        {
            switch (InterfaceBetweenGaussianAndChemGo.task)
            {
                case Task.mecp:
                    InitializeMecpLabel();
                    break;
                default:
                    throw new ReadInputFileException("Can't find legitimate \"task\"");
            }
        }

        private void InitializeMecpLabel()
        {
            labels.keyword_mecp.coordinateType = "noInfo";
            labels.keyword_mecp.energyCon = 0.00001;
            labels.keyword_mecp.guessHessian = "bfgs";
            labels.keyword_mecp.gradientN = 1;
            labels.keyword_mecp.hessianN = 1;
            labels.keyword_mecp.isReadFirst = false;
            labels.keyword_mecp.judgement = "global";
            labels.keyword_mecp.lambda = 0.1;
            labels.keyword_mecp.maxCon = 0.001;
            labels.keyword_mecp.maxCyc = 100;
            labels.keyword_mecp.mecpFreq = "liu";
            labels.keyword_mecp.opt = "ln";
            labels.keyword_mecp.rmsCon = 0.0005;
            labels.keyword_mecp.scfTyp1 = "hftyp";
            labels.keyword_mecp.scfTyp2 = "hftyp";
            labels.keyword_mecp.showGradRatioCriterion = 0.0001;
            labels.keyword_mecp.showGradRatioCriterionN = 4;
            labels.keyword_mecp.sqp_tao = 0.01;
            labels.keyword_mecp.stepSize = 0.1;
        }

    }
}
