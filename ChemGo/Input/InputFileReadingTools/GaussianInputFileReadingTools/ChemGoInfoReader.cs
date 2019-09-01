using System;
using System.Collections.Generic;
using System.Text;
using ChemGo.Data;
using ChemGo.Data.DataGaussian;
using ChemGo.Auxiliary.TextTools;

namespace ChemGo.Input.InputFileReadingTools.GaussianInputFileReadingTools
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
                        labels.mecp.opt = keywordAndParameter[i, 1];
                        break;
                    case "coordinatetype":
                        labels.mecp.coordinateType = keywordAndParameter[i, 1];
                        break;
                    case "file1":
                        labels.mecp.file1 = keywordAndParameter[i, 1];
                        break;
                    case "file2":
                        labels.mecp.file2 = keywordAndParameter[i, 1];
                        break;
                    case "scftyp1":
                        labels.mecp.scfTyp1 = keywordAndParameter[i, 1];
                        break;
                    case "scftyp2":
                        labels.mecp.scfTyp2 = keywordAndParameter[i, 1];
                        break;
                    case "maxcyc":
                        labels.mecp.maxCyc = Convert.ToInt32(keywordAndParameter[i, 1]);
                        break;
                    case "stepsize":
                        labels.mecp.stepSize = Convert.ToDouble(keywordAndParameter[i, 1]);
                        break;
                    case "hessiann":
                        labels.mecp.hessianN = Convert.ToInt32(keywordAndParameter[i, 1]);
                        break;
                    case "energycon":
                        labels.mecp.energyCon = Convert.ToDouble(keywordAndParameter[i, 1]);
                        break;
                    case "maxcon":
                        labels.mecp.maxCon = Convert.ToDouble(keywordAndParameter[i, 1]);
                        break;
                    case "rmscon":
                        labels.mecp.rmsCon = Convert.ToDouble(keywordAndParameter[i, 1]);
                        break;
                    case "lambda":
                        labels.mecp.lambda = Convert.ToDouble(keywordAndParameter[i, 1]);
                        break;
                    case "isreadfirst":
                        labels.mecp.isReadFirst = Convert.ToBoolean(keywordAndParameter[i, 1]);
                        break;
                    case "showgradratiocriterionn":
                        labels.mecp.showGradRatioCriterionN = Convert.ToDouble(keywordAndParameter[i, 1]);
                        break;
                    case "showgradratiocriterion":
                        labels.mecp.showGradRatioCriterion = Convert.ToDouble(keywordAndParameter[i, 1]);
                        break;
                    case "judgement":
                        labels.mecp.judgement = keywordAndParameter[i, 1];
                        break;
                    case "mecpfreq":
                        labels.mecp.mecpFreq = keywordAndParameter[i, 1];
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
            labels.mecp.coordinateType = "zmatrix";
            labels.mecp.energyCon = 0.00001;
            labels.mecp.file1 = "State1.gjf";
            labels.mecp.file2 = "State2.gjf";
            labels.mecp.guessHessian = "bfgs";
            labels.mecp.hessianN = 1;
            labels.mecp.isReadFirst = false;
            labels.mecp.judgement = "global";
            labels.mecp.lambda = 0.1;
            labels.mecp.maxCon = 0.001;
            labels.mecp.maxCyc = 100;
            labels.mecp.mecpFreq = "liu";
            labels.mecp.opt = "ln";
            labels.mecp.rmsCon = 0.0005;
            labels.mecp.scfTyp1 = "hftyp";
            labels.mecp.scfTyp2 = "hftyp";
            labels.mecp.showGradRatioCriterion = 0.0001;
            labels.mecp.showGradRatioCriterionN = 4;
            labels.mecp.sqp_tao = 0.01;
            labels.mecp.stepSize = 0.1;
        }

    }
}
