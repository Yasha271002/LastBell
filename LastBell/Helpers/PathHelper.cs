using System.IO;
using Serilog;

namespace LastBell.Helpers;

public class PathHelper
{
    public string ResolveImagePath(string relativePath, string directoryPath, ILogger logger)
    {
        if (string.IsNullOrEmpty(relativePath))
        {
            logger.Warning("Путь до изображения пустой или null");
            return string.Empty;
        }

        var basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, directoryPath);
        var fullPath = Path.Combine(basePath, relativePath);

        if (File.Exists(fullPath))
        {
            return fullPath;
        }

        logger.Warning($"Изображение не найдено: {fullPath}");
        return string.Empty;
    }
}