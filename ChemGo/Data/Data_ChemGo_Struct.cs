using System.IO;

namespace ChemGo.Data
{
    /// <summary>
    /// 命令行信息
    /// </summary>
    public struct CommandLineInformation
    {
        public string currentDirectory;                         //当前目录
        public string inputFilePath;                            //输入文件路径
        public string outputFilePath;                           //输出文件路径
        public InputFileType inputFileType;                     //输入文件类型
    }

    /// <summary>
    /// 输入文件
    /// </summary>
    public struct InputFile
    {
        public Labels labels;
        public ChargeAndMultiplicity chargeAndMultiplicity;
        public InputCartesian inputCartesian;
        public InputZmatrix inputZmatrix;
    }

    /// <summary>
    /// 标签合集
    /// </summary>
    public struct Labels
    {
        public Control control;
        public Mecp mecp;
    }

    /// <summary>
    /// 控制关键词
    /// </summary>
    public struct Control
    {
        /// <summary>
        /// 计算任务
        /// </summary>
        public Task task;
        /// <summary>
        /// 使用的计算程序
        /// </summary>
        public string calProgram;
        /// <summary>
        /// 计算过程中使用的坐标类型
        /// </summary>
        public string coordinateType;
    }

    /// <summary>
    /// Mecp关键词
    /// </summary>
    public struct Mecp
    {
        public string opt;                                   //优化方法
        public string coordinateType;                        //坐标类型，包括"z-matrix"和"cartesian"
        public string file1;                                 //第一个态的文件名
        public string file2;                                 //第二个态的文件名
        public string scfTyp1;                               //第一个态的自洽场类型
        public string scfTyp2;                               //第二个态的自洽场类型
        public int maxCyc;                                   //最大循环次数
        public double stepSize;                              //步长
        public string guessHessian;                          //估计Hessian阵的方式，包括两种方式，默认为1、"BFGS"，即BFGS方法； 另一种是2、"Powell"，即Powell方法
        public int hessianN;                                 //计算Hessian阵的间隔步，即每隔hessianStep步计算一次力常数矩阵
        public double energyCon;                             //收敛限能量
        public double maxCon;                                //最大拉格朗日力
        public double rmsCon;                                //均方根拉格朗日力
        public double lambda;                                //计算中所用拉格朗日参量λ
        public bool isReadFirst;                             //是否读第零步Labuta、能量、梯度和Hessian阵，第一步的构型；"true"表示读，"false"表示不读 
        public double showGradRatioCriterionN;               //最终显示梯度比的梯度阚值，10^-N
        public double showGradRatioCriterion;                //最终显示梯度比的梯度阚值
        public string judgement;                             //判据。可以是能量"energy"或者综合"global".
        public string mecpFreq;                              //mecp的振动分析

        public double sqp_tao;                               //SQP方法中的tao
    }

    /// <summary>
    /// 电荷和自旋多重度
    /// </summary>
    public struct ChargeAndMultiplicity
    {
        /// <summary>
        /// 总电荷
        /// </summary>
        public int charge;
        /// <summary>
        /// 总自旋角动量
        /// </summary>
        public int multiplicity;
    }

    /// <summary>
    /// 输入的笛卡尔坐标
    /// </summary>
    public struct InputCartesian
    {
        /// <summary>
        /// 原子个数
        /// </summary>
        public int numberOfAtoms;
        /// <summary>
        /// 原子序号（核电荷数）数组
        /// </summary>
        public int[] atomicNumbers;
        /// <summary>
        /// 原子量数组
        /// </summary>
        public double[] realAtomicWeights;
        /// <summary>
        /// 坐标矩阵，N行4列。
        /// </summary>
        public double[,] Coordinates;
    }

    /// <summary>
    /// 输入的内坐标
    /// </summary>
    public struct InputZmatrix
    {
        /// <summary>
        /// 原子个数
        /// </summary>
        public int numberOfAtoms;
        /// <summary>
        /// 原子序号（核电荷数）数组
        /// </summary>
        public int[] atomicNumbers;
        /// <summary>
        /// 原子量数组
        /// </summary>
        public double[] realAtomicWeights;
        /// <summary>
        /// 坐标矩阵，N行7列。
        /// </summary>
        public double[,] Coordinates;
        /// <summary>
        /// 参数矩阵，3N-6行2列。
        /// </summary>
        public double[,] Parameter;
    }

    /// <summary>
    /// 分子标准取向的几何结构
    /// </summary>
    public struct Geometry
    {
        /// <summary>
        /// 原子个数
        /// </summary>
        public int numberOfAtoms;
        /// <summary>
        /// 原子序号（核电荷数）数组
        /// </summary>
        public int[] atomicNumbers;
        /// <summary>
        /// 原子量数组
        /// </summary>
        public double[] realAtomicWeights;
        /// <summary>
        /// 标准取向坐标矩阵，N行4列
        /// </summary>
        public double[,] StandardOrientationCoordinates;
    }


    /// <summary>
    /// 梯度和力常数相关的信息
    /// </summary>
    public struct DerivativeInfo
    {
        /// <summary>
        /// 坐标
        /// </summary>
        public double[] coordinates;
        /// <summary>
        /// 梯度
        /// </summary>
        public double[] gradient;
        /// <summary>
        /// 力常数矩阵
        /// </summary>
        public double[,] forceConstants;
    }
}
