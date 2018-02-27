using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mailer.Services
{
    public static class LoggingService
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetLogger("logger");

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
