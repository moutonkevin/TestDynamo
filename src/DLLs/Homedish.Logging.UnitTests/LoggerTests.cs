using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Homedish.Logging.UnitTests
{
    public class LoggerTests
    {
        private string logFilePath = "c:/logs";

        [Fact]
        public void Logger_WriteMessageToFile_Correct()
        {
            string message = $"test-{DateTime.Now}";

            new LoggingConfiguration().ConfigureWithFileTarget(logFilePath);
            var logger = LogManager.GetLogger();

            logger.Info(message);

            var logFiles = Directory.GetFiles(logFilePath, "*.log");

            Assert.NotEmpty(logFiles);

            var logFile = logFiles[0];

            var logFileLines = File.ReadAllLines(logFile);

            Assert.NotEmpty(logFileLines);

            var lastLine = logFileLines.LastOrDefault();

            Assert.NotNull(lastLine);
            Assert.EndsWith(message, lastLine);
        }
    }
}
