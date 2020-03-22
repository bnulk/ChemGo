using System;
using System.Collections.Generic;
using System.Text;

namespace ChemGo.Auxiliary.LinearAlgebra
{
    class LinearAlgebraException : Exception
    {
        /*
        ----------------------------------------------------  类注释  开始----------------------------------------------------
        版本：  V1.0
        作者：  刘鲲
        日期：  2019-08-23

        描述：
            * 无法驱动任务时引发的异常。
        ----------------------------------------------------  类注释  结束----------------------------------------------------
        */

        /// <summary>
        /// 消息
        /// </summary>
        private readonly string message;
        private readonly string title = "\n\nbnulk@foxmail.com-LinearAlgebraException\n" + "*********************************************\n\n" + "!!!!!!!!!! Error !!!!!!!!!!\n";
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
        public LinearAlgebraException()
        {
        }

        /// <summary>
        /// 字符串参数的构造函数
        /// </summary>
        /// <param name="message">消息</param>
        public LinearAlgebraException(string message)
            : base(message)
        {
            this.message = title + message;
        }
    }
}
