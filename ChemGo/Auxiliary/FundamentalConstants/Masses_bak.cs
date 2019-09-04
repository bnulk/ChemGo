using System;
using System.Collections.Generic;
using System.Text;

namespace ChemGo.Auxiliary.FundamentalConstants
{
    static class Masses
    {
        /*
        ----------------------------------------------------  类注释  开始----------------------------------------------------
        版本：  V1.0
        作者：  刘鲲
        日期：  2018-06-18

        描述：
            * 原子量；数据来自www.pml.nist.gov。（2017）
        ----------------------------------------------------  类注释  结束----------------------------------------------------
        */

        static readonly string[,] element = new string[,]
        {
            {   "0",     "x",            "dummy",                 "0.0" },
            {   "1",     "h",         "hydrogen",               "1.008" },
            {   "2",    "he",           "helium",            "4.002602" },
            {   "3",    "li",          "lithium",                "6.94" },
            {   "4",    "be",        "beryllium",           "9.0121831" },
            {   "5",     "b",            "boron",               "10.81" },
            {   "6",     "c",           "carbon",              "12.011" },
            {   "7",     "n",         "nitrogen",              "14.007" },
            {   "8",     "o",           "oxygen",              "15.999" },
            {   "9",     "f",         "fluorine",         "18.99840316" },
            {   "10",   "ne",             "neon",             "20.1797" },
            {   "11",   "na",           "sodium",         "22.98976928" },
            {   "12",   "mg",        "magnesium",              "24.305" },
            {   "13",   "al",         "aluminum",          "26.9815385" },
            {   "14",   "si",          "silicon",              "28.085" },
            {   "15",    "p",       "phosphorus",         "30.97376199" },
            {   "16",    "s",           "sulfur",               "32.06" },
            {   "17",   "cl",         "chlorine",               "35.45" },
            {   "18",   "ar",            "argon",              "39.948" },
            {   "19",    "k",        "potassium",             "39.0983" },
            {   "20",   "ca",          "calcium",              "40.078" },
            {   "21",   "sc",         "scandium",           "44.955908" },
            {   "22",   "ti",         "titanium",              "47.867" },
            {   "23",    "v",         "vanadium",             "50.9415" },
            {   "24",   "cr",         "chromium",             "51.9961" },
            {   "25",   "mn",        "manganese",           "54.938044" },
            {   "26",   "fe",             "iron",              "55.845" },
            {   "27",   "co",           "cobalt",           "58.933194" },
            {   "28",   "ni",           "nickel",             "58.6934" },
            {   "29",   "cu",           "copper",              "63.546" },
            {   "30",   "zn",             "zinc",               "65.38" },
            {   "31",   "ga",          "gallium",              "69.723" },
            {   "32",   "ge",        "germanium",              "72.630" },
            {   "33",   "as",          "arsenic",           "74.921595" },
            {   "34",   "se",         "selenium",              "78.971" },
            {   "35",   "br",          "bromine",              "79.904" },
            {   "36",   "kr",          "krypton",              "83.798" },
            {   "37",   "rb",         "rubidium",             "85.4678" },
            {   "38",   "sr",        "strontium",               "87.62" },
            {   "39",    "y",          "yttrium",            "88.90584" },
            {   "40",   "zr",        "zirconium",              "91.224" },
            {   "41",   "nb",          "niobium",            "92.90637" },
            {   "42",   "mo",       "molybdenum",               "95.95" },
            {   "43",   "tc",       "technetium",                 "-98" },
            {   "44",   "ru",        "ruthenium",              "101.07" },
            {   "45",   "rh",          "rhodium",           "102.90550" },
            {   "46",   "pd",        "palladium",              "106.42" },
            {   "47",   "ag",           "silver",            "107.8682" },
            {   "48",   "cd",          "cadmium",             "112.414" },
            {   "49",   "in",           "indium",             "114.818" },
            {   "50",   "sn",              "tin",             "118.710" },
            {   "51",   "sb",         "antimony",             "121.760" },
            {   "52",   "te",        "tellurium",              "127.60" },
            {   "53",    "i",           "iodine",           "126.90447" },
            {   "54",   "xe",            "xenon",             "131.293" },
        };

        /// <summary>
        /// 根据元素符号得到原子序数
        /// </summary>
        /// <param name="Symbol">元素符号</param>
        /// <returns></returns>
        public static int SymbolToNumber(string symbol)
        {
            int number = 0;
            int sumNumber = element.GetLength(0);
            for(int i=0;i<sumNumber;i++)
            {
                if(symbol.ToLower()==element[i,1])
                {
                    number = Convert.ToInt32(element[i, 0]);
                }
            }
            return number;
        }

        /// <summary>
        /// 根据原子序数得到元素符号
        /// </summary>
        /// <param name="number">原子序数</param>
        /// <returns></returns>
        public static string NumberToSymbol(int number)
        {
            string symbol = "x";
            int sumSymbol = element.GetLength(0);
            for (int i = 0; i < sumSymbol; i++)
            {
                if (number == Convert.ToInt32(element[i, 0]))
                {
                    symbol = element[i, 1];
                }
            }
            return symbol;
        }

        /// <summary>
        /// 根据原子序数得到平均原子量
        /// </summary>
        /// <param name="Symbol">元素符号</param>
        /// <returns></returns>
        public static double NumberToMass(int number)
        {
            double mass = 0;
            int sumNumber = element.GetLength(0);
            for (int i = 0; i < sumNumber; i++)
            {
                if (number == Convert.ToInt32(element[i, 0]))
                {
                    mass = Convert.ToDouble(element[i, 3]);
                }
            }
            return mass;
        }

    }
}
