namespace ChemGo.CommandLine
{
    /// <summary>
    /// 命令行类型
    /// </summary>
    public enum CommandLineType
    {
        multiArgs,
        doubleArgs,
        singleArgs,
        zeroArgs
    }

    /// <summary>
    /// 帮助选项类型
    /// </summary>
    public enum HelpOptionType
    {
        noPrint,
        unknown,
        help,
        about
    }
}
