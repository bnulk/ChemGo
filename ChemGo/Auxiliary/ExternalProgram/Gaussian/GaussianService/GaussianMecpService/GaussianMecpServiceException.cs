using System;
using System.Collections.Generic;
using System.Text;

namespace ChemGo.Auxiliary.ExternalProgram.Gaussian.GaussianService.GaussianMecpService
{
    class GaussianMecpServiceException : Exception
    {
        /*
        ----------------------------------------------------  类注释  开始----------------------------------------------------
        版本：  V1.0
        作者：  刘鲲
        日期：  2019-08-23

        描述：
            * 引发的异常。
        ----------------------------------------------------  类注释  结束----------------------------------------------------
        */

        /// <summary>
        /// 消息
        /// </summary>
        private readonly string message;
        private readonly string title = "\n\nbnulk@foxmail.com-ChemGo.Auxiliary.ExternalProgram.Gaussian.GaussianService.GaussianMecpService\n" 
            + "*********************************************\n\n" + "!!!!!!!!!! Error !!!!!!!!!!\n";
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
        public GaussianMecpServiceException()
        {
        }

        /// <summary>
        /// 字符串参数的构造函数
        /// </summary>
        /// <param name="message">消息</param>
        public GaussianMecpServiceException(string message)
            : base(message)
        {
            this.message = title + message;
        }
    }
}
