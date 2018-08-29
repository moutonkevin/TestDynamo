using System.IO;
using NLog;

namespace Homedish.Logging
{
    public class LoggingConfiguration
    {
        public static void ConfigureWithFileTarget(string path = "c:/logs")
        {
            var config = new NLog.Config.LoggingConfiguration();

            var logfile = new NLog.Targets.FileTarget("logfile")
            {
                Layout = "${longdate} ${logger} ${message}${exception:format=ToString}",
                FileName = Path.Combine(path, "${shortdate}.log")
            };

            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            NLog.LogManager.Configuration = config;
        }
    }
}
