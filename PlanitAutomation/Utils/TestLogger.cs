using System.Text;

namespace PlanitAutomation.Utils;

/// <summary>
/// Simple thread-safe file logger. Writes timestamped log entries both to the
/// console and to a per-run log file under TestResults/Logs/.
/// </summary>
public static class TestLogger
{
    private static readonly string LogDirectory =
        Path.Combine(AppContext.BaseDirectory, "TestResults", "Logs");

    private static readonly string LogFilePath;
    private static readonly object FileLock = new();

    static TestLogger()
    {
        Directory.CreateDirectory(LogDirectory);
        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        LogFilePath = Path.Combine(LogDirectory, $"test-run_{timestamp}.log");
    }

    /// <summary>Writes an INFO-level message.</summary>
    public static void Info(string message) => Write("INFO", message);

    /// <summary>Writes an ERROR-level message.</summary>
    public static void Error(string message) => Write("ERROR", message);

    /// <summary>Writes an ERROR-level message with full exception details.</summary>
    public static void Error(string message, Exception ex) =>
        Write("ERROR", $"{message}{Environment.NewLine}{ex}");

    private static void Write(string level, string message)
    {
        var line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [{level,-5}] {message}";
        Console.WriteLine(line);
        lock (FileLock)
        {
            File.AppendAllText(LogFilePath, line + Environment.NewLine, Encoding.UTF8);
        }
    }
}
