using System;
using System.Collections.Generic;
using System.Text;

namespace ChemGo.CommandLine
{
    class CommandLineException : Exception
    {
        /*
        ----------------------------------------------------  类注释  开始----------------------------------------------------
        版本：  V1.0
        作者：  刘鲲
        日期：  2019-07-23

        描述：
            * 无法分析命令行参数时引发的异常。
        ----------------------------------------------------  类注释  结束----------------------------------------------------
        */

        /// <summary>
        /// 消息
        /// </summary>
        private readonly string message;
        public override string Message
        {
            get
            {
                return message;
            }
        }

        /// <summary>
        /// 无参数构造函数
        /// </summary>
        public CommandLineException()
        {
        }

        /// <summary>
        /// 字符串参数的构造函数
        /// </summary>
        /// <param name="message">消息</param>
        public CommandLineException(string message)
            : base(message)
        {
            this.message = message;
        }

    }
}
