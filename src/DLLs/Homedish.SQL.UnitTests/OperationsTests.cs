using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Homedish.SQL.Models;
using Homedish.SQL.UnitTests.Models;
using Xunit;

namespace Homedish.SQL.UnitTests
{
    public class OperationsTests : IClassFixture<Prerequisites>
    {
        private readonly Prerequisites _fixture;
        private readonly IOperations _operations = new Operations();

        public OperationsTests(Prerequisites fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ExecuteSpWithParametersWithOneResult_Correct()
        {
            var configs = new StoredProcedureConfiguration
            {
                ConnectionString = Constants.ConnectionString,
                StoredProcedureName = "usp_test",
                StoredProcedureParameters = new Dictionary<string, object>()
                {
                    {"@status", 1},
                    {"@boolean", true},
                }
            };

            var response = await _operations.ExecuteStoredProcedureAsync(configs, async (results) =>
            {
                return await results.Read(async () => new SpResponse
                {
                    Test = await results.GetColumnValue<string>("test"),
                    Status = await results.GetColumnValue<int>("status"),
                    Boolean = await results.GetColumnValue<bool>("boolean"),
                });
            });

            Assert.Equal("hello", response.Test);
            Assert.Equal(1, response.Status);
            Assert.True(response.Boolean);
        }

        [Fact]
        public async Task ExecuteSpWithParametersWithMultipleResults_Correct()
        {
            var configs = new StoredProcedureConfiguration
            {
                ConnectionString = Constants.ConnectionString,
                StoredProcedureName = "usp_test",
                StoredProcedureParameters = new Dictionary<string, object>()
                {
                    {"@status", 1},
                    {"@boolean", true},
                }
            };

            var response = await _operations.ExecuteStoredProcedureAsync(configs, async (results) =>
            {
                return await results.ReadAll(async () => new SpResponse
                {
                    Test = await results.GetColumnValue<string>("test"),
                    Status = await results.GetColumnValue<int>("status"),
                    Boolean = await results.GetColumnValue<bool>("boolean"),
                });
            });

            Assert.True(response.Count() > 1);
        }

        [Fact]
        public async Task ExecuteSpWithParametersWithoutResults_Correct()
        {
            var configs = new StoredProcedureConfiguration
            {
                ConnectionString = Constants.ConnectionString,
                StoredProcedureName = "usp_test",
                StoredProcedureParameters = new Dictionary<string, object>()
                {
                    {"@status", 4654},
                    {"@boolean", true},
                }
            };

            var response = await _operations.ExecuteStoredProcedureAsync(configs, async (results) =>
            {
#pragma warning disable 1998
                return await results.Read(async () => results.HasRows() ? new SpResponse() : null);
#pragma warning restore 1998
            });

            Assert.Null(response);
        }

        [Fact]
        public async Task ExecuteSpWithoutParametersWithOneResult_Correct()
        {
            var configs = new StoredProcedureConfiguration
            {
                ConnectionString = Constants.ConnectionString,
                StoredProcedureName = "usp_test2",
                StoredProcedureParameters =  null
            };

            var response = await _operations.ExecuteStoredProcedureAsync(configs, async (results) =>
            {
                return await results.ReadAll(async () => new SpResponse
                {
                    Test = await results.GetColumnValue<string>("test"),
                    Status = await results.GetColumnValue<int>("status"),
                    Boolean = await results.GetColumnValue<bool>("boolean"),
                });
            });

            Assert.True(response.Count() > 1);
        }
    }
}
