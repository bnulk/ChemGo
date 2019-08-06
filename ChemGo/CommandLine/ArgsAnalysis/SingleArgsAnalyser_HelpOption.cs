using System;
using System.Collections.Generic;
using System.Text;

namespace ChemGo.CommandLine.ArgsAnalysis
{
    class SingleArgsAnalyser_HelpOption
    {
        /// <summary>
        /// 参数
        /// </summary>
        private string arg;
        /// <summary>
        /// 帮助选项名字
        /// </summary>
        private string helpOptionName = null;
        /// <summary>
        /// 帮助选项名字的全称
        /// </summary>
        private string helpOptionFullName = null;

        /// <summary>
        /// 帮助信息的类型
        /// </summary>
        private HelpOptionType helpOptionType;
        /// <summary>
        /// 帮助信息的类型
        /// </summary>
        public HelpOptionType HelpOptionType { get => helpOptionType; set => helpOptionType = value; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="args"></param>
        public SingleArgsAnalyser_HelpOption(string arg)
        {
            this.arg = arg;
        }

        /// <summary>
        /// 运行单参数分析器的帮助选项
        /// </summary>
        public void Run()
        {
            ObtainHelpOptionString();
            if(helpOptionFullName!=null)
            {
                helpOptionName = ConvertHelpOptionFullNameToHelpOptionName(helpOptionFullName);
            }            
            ObtainHelpOptionType();
        }

        /// <summary>
        /// 获取帮助选项字符串
        /// </summary>
        private void ObtainHelpOptionString()
        {
            if (arg.StartsWith("--"))
            {
                helpOptionFullName = arg.Substring(2); 
            }
            else if (arg.StartsWith("-"))
            {
                helpOptionName = arg.Substring(1);
            }
        }

        /// <summary>
        /// 把信息中的全名改为名字
        /// </summary>
        /// <param name="helpOptionFullName">帮助信息的全名</param>
        /// <returns></returns>
        private string ConvertHelpOptionFullNameToHelpOptionName(string helpOptionFullName)
        {
            string helpOptionName;
            switch (helpOptionFullName)
            {
                case "help":
                    helpOptionName = "h";
                    break;
                case "about":
                    helpOptionName = "a";
                    break;
                default:
                    helpOptionName = helpOptionFullName;
                    break;
            }
            return helpOptionName;
        }

        /// <summary>
        /// 获取帮助选项。
        /// </summary>
        private void ObtainHelpOptionType()
        {
            switch (helpOptionName)
            {
                case "h":
                    helpOptionType = HelpOptionType.help;
                    break;
                case "a":
                    helpOptionType = HelpOptionType.about;
                    break;
                default:
                    helpOptionType = HelpOptionType.unknown;
                    break;
            }
        }



    }
}
