using System;

using log4net;

namespace IBVD.Digital.Logic.Component
{
    public static class LoggingComponent
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(LoggingComponent));

        public static void LogError(string message, Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            logger.Error(message, ex);

        }

        public static void LogInfo(string message)
        {
            log4net.Config.XmlConfigurator.Configure();

            logger.Info(message);
        }
    }
}
