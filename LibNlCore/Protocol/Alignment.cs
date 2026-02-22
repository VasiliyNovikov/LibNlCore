namespace LibNlCore.Protocol;

internal static class Alignment
{
    public static int Align(int length) => (length + 3) & ~3;
}