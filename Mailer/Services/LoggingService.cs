using System;
using System.Diagnostics;
using NLog;

namespace Mailer.Services
{
    public static class LoggingService
    {
        private static readonly Logger _logger = LogManager.GetLogger("logger");

        public static void Log(string message)
        {
            Debug.WriteLine(message);

            _logger.Info(message);
        }

        public static void Log(Exception ex)
        {
            Debug.WriteLine(ex);

            _logger.Error(ex);
        }
    }
}