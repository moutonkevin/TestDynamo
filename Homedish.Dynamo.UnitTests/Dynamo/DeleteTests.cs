using System.Collections.Generic;
using System.Threading.Tasks;
using Homedish.Aws.Dynamo;
using Homedish.Aws.Dynamo.Models;
using Homedish.Aws.Dynamo.Models.Delete;
using Xunit;

namespace Homedish.Aws.UnitTests.Dynamo
{
    public class DeleteTests
    {
        private readonly IOperations _operations = new Operations();
        private readonly string _tableName = "kevin-test";

        [Fact]
        public async Task DeleteValidOperation_Success()
        {
            var model = new DeleteModel()
            {
                TableName = _tableName,
                Keys = new List<DynamoField>
                {
                    new DynamoField("id", ColumnType.Number, "1"),
                }
            };

            var response = await _operations.DeleteAsync(model);

            Assert.True(response.IsSuccess);
        }

        [Fact]
        public async Task DeleteEmptyTableValidOperation_Failure()
        {
            var model = new DeleteModel()
            {
                Keys = new List<DynamoField>
                {
                    new DynamoField {ColumnName = "id", ColumnType = ColumnType.Number, ColumnValue = "1"},
                }
            };

            var response = await _operations.DeleteAsync(model);

            Assert.False(response.IsSuccess);
            Assert.Equal("Table name cannot be empty", response.ErrorMessage);
        }

        [Fact]
        public async Task DeleteNoPrimaryKeyValidOperation_Failure()
        {
            var model = new DeleteModel()
            {
                Keys = new List<DynamoField>
                {
                }
            };

            var response = await _operations.DeleteAsync(model);

            Assert.False(response.IsSuccess);
            Assert.Equal("The Primary Partition Key is required", response.ErrorMessage);
        }

        [Fact]
        public async Task DeleteNoPrimaryKeyAtAllValidOperation_Failure()
        {
            var model = new DeleteModel()
            {
            };

            var response = await _operations.DeleteAsync(model);

            Assert.False(response.IsSuccess);
            Assert.Equal("The Primary Partition Key is required", response.ErrorMessage);
        }
    }
}
