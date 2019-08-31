using System;
using System.Collections.Generic;
using System.Text;

namespace ChemGo.Data
{
    /// <summary>
    /// 计算任务类型
    /// </summary>
    public enum Task
    {
        mecp,
        ts,
        min,
        sp
    }

    /// <summary>
    /// 输入文件类型
    /// </summary>
    public enum InputFileType
    {
        unknown,
        Gaussian,
        ChemGo
    }
}
