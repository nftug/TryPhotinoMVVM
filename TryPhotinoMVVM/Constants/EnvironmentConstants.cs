namespace TryPhotinoMVVM.Constants;

public static class EnvironmentConstants
{

#if DEBUG

    public static readonly bool IsDebugMode = true;

#else

    public static readonly bool IsDebugMode = false;

#endif

}
