using Xunit;

namespace Homedish.Template.Worker.IntegrationTests
{
    public class TestBase : IClassFixture<DependencyInjection>
    {
        protected readonly DependencyInjection Container;

        public TestBase(DependencyInjection container)
        {
            Container = container;
        }
    }
}
