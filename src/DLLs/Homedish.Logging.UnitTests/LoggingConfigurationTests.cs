using NLog.Targets;
using Xunit;

namespace Homedish.Logging.UnitTests
{
    public class LoggingConfigurationTests
    {
        [Fact]
        public void ConfigureWithFileTarget_Exists()
        {
            new LoggingConfiguration().ConfigureWithFileTarget();

            Assert.NotNull(NLog.LogManager.Configuration);
        }

        [Fact]
        public void ConfigureWithFileTarget_OneRuleIsAdded()
        {
            new LoggingConfiguration().ConfigureWithFileTarget();

            Assert.Equal(1, NLog.LogManager.Configuration.LoggingRules.Count);
        }

        [Fact]
        public void ConfigureWithFileTarget_CorrectFileTarget()
        {
            new LoggingConfiguration().ConfigureWithFileTarget();

            Assert.Equal(1, NLog.LogManager.Configuration.LoggingRules[0].Targets.Count);
            Assert.IsType<FileTarget>(NLog.LogManager.Configuration.LoggingRules[0].Targets[0]);
        }
    }
}
