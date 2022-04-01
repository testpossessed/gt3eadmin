using System;
using NLog;
using NLog.Extensions.Logging;

namespace GT3e.Admin.Services
{
  internal static class LogWriter
  {
    private static Logger logger = null!;

    internal static void Init()
    {
      logger = LogManager.Setup()
                         .LoadConfigurationFromSection(Configuration.CurrentConfig)
                         .GetCurrentClassLogger();
      LogManager.Configuration.Variables["appDataFolder"] = PathProvider.AppDataFolderPath;
    }

    internal static void Error(Exception exception, string message, params object[] args)
    {
        logger.Error(exception, message, args);
    }

    internal static void Info(string message)
    {
        logger.Info(message);
    }

  }
}