using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ChemGo.Auxiliary.TextTools;
using ChemGo.Data;

namespace ChemGo.Input.InputFileReadings.ChemGoInputFileReadings
{
    class ChemGoInfoReader_Control
    {
        private List<string> controlList = new List<string>();

        private Control control;

        /// <summary>
        /// 输入文件数据的控制部分
        /// </summary>
        public Control Control { get => control; set => control = value; }


        public ChemGoInfoReader_Control(List<string> controlList)
        {
            this.controlList = controlList;
        }

        /// <summary>
        /// 把控制部分写入ChemGo
        /// </summary>
        public void Run()
        {
            string str = "";                                        //控制部分文本的字符串形式
            string[] keyWordAndPara;                                //全部关键词和参数组成的数组
            string[] inputKeyWord = new string[2];                  //单个指令数组。被“=”分成两部分，或者没有等号的一部分。

            //初始化
            control.task = Task.sp;
            control.coordinateType = CoordinateType.Cartesian;
            control.inputFileType = InputFileType.ChemGo;

            //控制部分字符串
            for (int i = 0; i < controlList.Count; i++)
            {
                str += controlList[i].Trim();
            }

            //分割所有的关键词
            keyWordAndPara = Regex.Split(str, "\\s+", RegexOptions.IgnoreCase);

            for (int i = 0; i < keyWordAndPara.Length; i++)
            {
                try
                {
                    inputKeyWord = keyWordAndPara[i].Split('=');
                }
                catch
                {
                    throw new ReadInputFileException("Input Error" + "\n" + inputKeyWord.ToString() + "\n");
                }
                if (inputKeyWord.Length >= 2)
                {
                    //根据输入文件的关键词初始化指令
                    switch (inputKeyWord[0].ToLower())
                    {
                        case "task":
                            control.task = ReadInputFileTools.GetTask(inputKeyWord[1].ToLower());
                            break;
                        case "coordinate":
                            control.coordinateType = ReadInputFileTools.GetCoordinateType(inputKeyWord[1].ToLower());
                            break;
                        default:
                            break;
                    }
                }
            }
            return;
        }


        /// <summary>
        /// 从字符串中读取电荷数组
        /// </summary>
        /// <param name="strCharge">字符串</param>
        /// <returns>电荷数组</returns>
        private int[] ReadCharge(string strCharge)
        {
            string[] tmpResult;
            int[] result;
            strCharge = BasisTextTools.GetStringBetweenTwoString(strCharge, "(", ")");
            if(strCharge.Contains(','))
            {
                tmpResult = strCharge.Split(',');
            }
            else
            {
                result = new int[1];
                result[0] = Convert.ToInt32(strCharge);
                return result;
            }
            int n = tmpResult.Length;
            result = new int[n];
            for(int i=0;i<n;i++)
            {
                result[i] = Convert.ToInt32(tmpResult[i]);
            }
            return result;
        }

        /// <summary>
        /// 从字符串中读取自旋多重度数组
        /// </summary>
        /// <param name="strCharge">字符串</param>
        /// <returns>自旋多重度</returns>
        private int[] ReadMultiplicity(string strMultiplicity)
        {
            string[] tmpResult;
            int[] result;
            strMultiplicity = BasisTextTools.GetStringBetweenTwoString(strMultiplicity, "(", ")");
            if (strMultiplicity.Contains(','))
            {
                tmpResult = strMultiplicity.Split(',');
            }
            else
            {
                result = new int[1];
                result[0] = Convert.ToInt32(strMultiplicity);
                return result;
            }
            int n = tmpResult.Length;
            result = new int[n];
            for (int i = 0; i < n; i++)
            {
                result[i] = Convert.ToInt32(tmpResult[i]);
            }
            return result;
        }

    }
}
