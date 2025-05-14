using ProjectBlack.Debugging;

namespace ProjectBlack;

public class ProjectBlackConsts
{
    public const string LocalizationSourceName = "ProjectBlack";

    public const string ConnectionStringName = "Default";

    public const bool MultiTenancyEnabled = false;


    /// <summary>
    /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
    /// </summary>
    public static readonly string DefaultPassPhrase =
        DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "d39e890e832449fa8416267c8511a5eb";
}
