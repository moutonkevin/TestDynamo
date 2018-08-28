using System.Collections.Generic;
using System.Threading.Tasks;
using Homedish.Dynamo.Models;
using Homedish.Dynamo.Models.Get;
using Xunit;

namespace Homedish.Dynamo.UnitTests
{
    public class GetTests
    {
        private readonly IOperations _operations = new Operations();
        private const string _tableName = "kevin-test2";

        [Fact]
        public async Task GetBoolValidOperation_Success()
        {
            var model = new GetModel
            {
                TableName = _tableName,
                PartitionKey = new DynamoField("id", ColumnType.Number, "1"),
                FieldsToRetrieve = new List<DynamoFieldSchema>
                {
                    new DynamoFieldSchema("id", ColumnType.Number),
                    new DynamoFieldSchema("bool", ColumnType.String),
                },
            };

            var response = await _operations.GetAsync(model);

            Assert.True(response.IsSuccess);
        }

        [Fact]
        public async Task GetValidOperation_Success()
        {
            var model = new GetModel
            {
                TableName = _tableName,
                PartitionKey = new DynamoField("id", ColumnType.Number, "1"),
                FieldsToRetrieve = new List<DynamoFieldSchema>
                {
                    new DynamoFieldSchema("id", ColumnType.Number),
                    new DynamoFieldSchema("test", ColumnType.String),
                },
            };

            var response = await _operations.GetAsync(model);

            Assert.True(response.IsSuccess);
        }

        [Fact]
        public async Task GetNoPrimaryKey_Failure()
        {
            var model = new GetModel
            {
                TableName = _tableName,
                FieldsToRetrieve = new List<DynamoFieldSchema>
                {
                    new DynamoFieldSchema {ColumnName = "test", ColumnType = ColumnType.String},
                },
            };

            var response = await _operations.GetAsync(model);

            Assert.False(response.IsSuccess);
            Assert.Equal("The Primary Partition Key is required", response.ErrorMessage);
        }

        [Fact]
        public async Task InsertNoTableName_Failure()
        {
            var model = new GetModel
            {
                PartitionKey = new DynamoField("id", ColumnType.Number, "1"),
                FieldsToRetrieve = new List<DynamoFieldSchema>
                {
                    new DynamoFieldSchema {ColumnName = "id", ColumnType = ColumnType.Number},
                    new DynamoFieldSchema {ColumnName = "test", ColumnType = ColumnType.String},
                },
            };

            var response = await _operations.GetAsync(model);

            Assert.False(response.IsSuccess);
            Assert.Equal("Table name cannot be empty", response.ErrorMessage);
        }
    }
}
