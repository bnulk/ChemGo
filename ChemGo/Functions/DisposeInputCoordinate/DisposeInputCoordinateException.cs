using System;
using System.Collections.Generic;
using System.Text;

namespace ChemGo.Functions.DisposeInputCoordinate
{
    class DisposeInputCoordinateException:Exception
    {
        /// <summary>
        /// 消息
        /// </summary>
        private readonly string message;
        private readonly string title = "\n\nbnulk@foxmail.com-DisposeInputCoordinateException\n" + "*********************************************\n\n" + "!!!!!!!!!! Error !!!!!!!!!!\n";
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
        public DisposeInputCoordinateException()
        {
        }

        /// <summary>
        /// 字符串参数的构造函数
        /// </summary>
        /// <param name="message">消息</param>
        public DisposeInputCoordinateException(string message)
            : base(message)
        {
            this.message = title + message;
        }
    }
}
