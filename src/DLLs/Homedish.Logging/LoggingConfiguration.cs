using System.IO;
using NLog;

namespace Homedish.Logging
{
    public class LoggingConfiguration
    {
        public void ConfigureWithFileTarget(string root = "c:/logs")
        {
            var config = new NLog.Config.LoggingConfiguration();

            var logfile = new NLog.Targets.FileTarget("logfile")
            {
                Layout = "${longdate} ${message}${exception:format=ToString}",
                FileName = Path.Combine(root, "${shortdate}.log")
            };

            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            NLog.LogManager.Configuration = config;
        }
    }
}
