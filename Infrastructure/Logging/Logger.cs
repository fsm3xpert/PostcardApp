using System;

namespace PostcardApp.Infrastructure.Logging
{
    public static class Logger
    {
        readonly static ILogger _logger;

        static Logger()
        {
            LoggerFactory.SetCurrent(new FileSourceLogFactory());
            _logger = LoggerFactory.CreateLog();
        }

        public static void WriteInfo(string message)
        {
            _logger.Info(message);
        }

        public static void WriteWarning(string message)
        {
            _logger.Warn(message);
        }

        public static void WriteError(string message, Exception ex)
        {
            _logger.Error(message, ex);
        }

        public static void WriteError(Exception ex)
        {
            _logger.Error(ex.Message, ex);
        }
    }
}
