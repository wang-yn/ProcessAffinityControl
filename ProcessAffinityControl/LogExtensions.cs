using System.Text;

namespace ProcessAffinityControl;

public static class LogExtensions
{
    private static StreamWriter _streamWriter;
    private static readonly string LOG_FILE_NAME = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs/log.txt");

    static LogExtensions()
    {
        Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs"));

        if (File.Exists(LOG_FILE_NAME))
            File.Delete(LOG_FILE_NAME);

        _streamWriter = new StreamWriter(LOG_FILE_NAME, true, Encoding.Default, 1024 * 4);
        _streamWriter.AutoFlush = true;
    }

    public static void Log(this string message, object? arg0 = null)
    {
        _streamWriter.WriteLine(message, arg0);
    }
}