using Xunit;

namespace Homedish.Logging.UnitTests
{
    public class LogManagerTests
    {
        [Fact]
        public void ConfigureWithFileTarget_Exists()
        {
            var logger = LogManager.GetCurrentClassLogger();

            Assert.Equal(GetType().FullName, logger.CallingClassFullName);
        }
    }
}
