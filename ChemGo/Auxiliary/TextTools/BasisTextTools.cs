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
        /// 得到两个字符串之间的部分
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
                Console.WriteLine("More than one "+ start + " are found" + "/n");
            }

            if (indexStart == -1 || indexEnd == -1)
            {
                Console.WriteLine("Can not find " + start + " and " + end + " in the input file." + "/n");
            }

            result = original.Remove(indexEnd, original.Length - indexEnd);
            result = result.Remove(0, indexStart + start.Length);

            return result;

        }

        /// <summary>
        /// 获取两个字符串之外的部分
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

            result = original.Remove(indexStart, indexEnd - indexStart + end.Length);

            return result;
        }
    }
}
