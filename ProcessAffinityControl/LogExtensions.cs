namespace ProcessAffinityControl;

public static class LogExtensions
{
    public static void Log(this string message)
    {
        Console.WriteLine(message);
    }

    public static void Log(this string message, object arg0)
    {
        Console.WriteLine(message, arg0);
    }
}