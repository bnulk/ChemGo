using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;              //使用正则表达式

namespace ChemGo.Auxiliary.TextTools
{
    partial class BasisTextTools
    {
        /*
        ----------------------------------------------------  类注释  开始----------------------------------------------------
        版本：  V1.0
        作者：  刘鲲
        日期：  2018-06-16

        描述：
            * 一些处理文本的小工具
        方法：
            * IsNumber --- 判断字符串是否是数字。
        ----------------------------------------------------  类注释  结束----------------------------------------------------
        */

        /// <summary>
        /// 判断字符串是否是数字
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns></returns>
        public static bool IsNumber(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            const string pattern = "^[0-9]*$";
            Regex rx = new Regex(pattern);
            return rx.IsMatch(s);
        }

        /// <summary>
        /// 得到两个字符串之间的部分。如果不存在两个字符串，则返回全部内容。
        /// </summary>
        /// <param name="original">原始字符串</param>
        /// <param name="start">开始字符串</param>
        /// <param name="end">结束字符串</param>
        /// <returns>两个字符串之间的部分</returns>
        public static string GetStringBetweenTwoString(string original, string start, string end)
        {
            string result;

            int indexStart;
            int indexEnd;

            indexStart = original.IndexOf(start);
            indexEnd = original.IndexOf(end);

            if (original.LastIndexOf(start) != indexStart)
            {
                result = "";
                return original;
            }

            if (indexStart == -1 || indexEnd == -1)
            {
                result = "";
                return original;
            }

            result = original.Remove(indexEnd, original.Length - indexEnd);
            result = result.Remove(0, indexStart + start.Length);

            return result;

        }

        /// <summary>
        /// 获取两个字符串之外的部分。如果不存在两个字符串，则返回全部内容。
        /// </summary>
        /// <param name="original">原始字符串</param>
        /// <param name="start">开始字符串</param>
        /// <param name="end">结束字符串</param>
        /// <returns>两个字符串之外的部分</returns>
        public static string GetStringOutsideTwoString(string original, string start, string end)
        {
            string result;

            int indexStart;
            int indexEnd;

            indexStart = original.IndexOf(start);
            indexEnd = original.IndexOf(end);

            if (original.LastIndexOf(start) != indexStart)
            {
                result = "";
                return original;
            }

            if (indexStart == -1 || indexEnd == -1)
            {
                result = "";
                return original;
            }

            result = original.Remove(indexStart, indexEnd - indexStart + end.Length);

            return result;
        }

        /// <summary>
        /// 以字符串数组形式，返回被空格分隔的字符串。
        /// </summary>
        /// <param name="original">字符串</param>
        /// <returns>被空格分离的字符串构成的数组</returns>
        public static string[] GetStringSeparatedbySpaces(string original)
        {
            string[] result;
            original = original.Trim();
            result = Regex.Split(original, "\\s+", RegexOptions.IgnoreCase);

            /*
            string[] result;
            string[] tmpResult;
            List<string> tmpList = new List<string>();
            int i;
            int n;

            tmpResult = original.Split(' ');
            n = tmpResult.Length;
            for(i=0;i<n;i++)
            {
                if(tmpResult[i]!="")
                {
                    tmpList.Add(tmpResult[i]);
                }
            }

            n = tmpList.Count;
            result = new string[n];
            for(i=0;i<n;i++)
            {
                result[i] = tmpList[i];
            }
            */
            return result;
        }
    }
}
