using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Homedish.Aws.Dynamo.Model;
using Homedish.Aws.Dynamo.Model.Insert;
using Xunit;

namespace Homedish.Aws.Dynamo.UnitTests
{
    public class InsertTests
    {

        private readonly IDynamoOperations _operations = new Operations();
        private readonly string _tableName = "kevin-test2";

        [Fact]
        public async Task InsertBoolValidOperation_Success()
        {
            var model = new InsertModel
            {
                TableName = _tableName,
                Fields = new List<DynamoField>
                {
                    new DynamoField {ColumnName = "id", ColumnType = ColumnType.Number, ColumnValue = "1"},
                    new DynamoField {ColumnName = "bool", ColumnType = ColumnType.Boolean, ColumnValue = "True"}
                }
            };

            var response = await _operations.InsertAsync(model);

            Assert.True(response.IsSuccess);
        }

        [Fact]
        public async Task InsertValidOperation_Success()
        {
            var model = new InsertModel
            {
                TableName = _tableName,
                Fields = new List<DynamoField>
                {
                    new DynamoField {ColumnName = "id", ColumnType = ColumnType.Number, ColumnValue = "1"},
                    new DynamoField {ColumnName = "test", ColumnType = ColumnType.String, ColumnValue = "hello2"}
                }
            };

            var response = await _operations.InsertAsync(model);

            Assert.True(response.IsSuccess);
        }

        [Fact]
        public async Task InsertNoPrimaryKey_Failure()
        {
            var model = new InsertModel
            {
                TableName = _tableName,
                Fields = new List<DynamoField>
                {
                    new DynamoField {ColumnName = "test", ColumnType = ColumnType.String, ColumnValue = "hello2"}
                }
            };

            var response = await _operations.InsertAsync(model);

            Assert.False(response.IsSuccess);
            Assert.Equal(HttpStatusCode.BadRequest, response.HttpStatusCode);
            Assert.Equal("ValidationException", response.ErrorCode);
        }

        [Fact]
        public async Task InsertNoTableName_Failure()
        {
            var model = new InsertModel
            {
                Fields = new List<DynamoField>
                {
                    new DynamoField {ColumnName = "id", ColumnType = ColumnType.Number, ColumnValue = "1"},
                    new DynamoField {ColumnName = "test", ColumnType = ColumnType.String, ColumnValue = "hello2"}
                }
            };

            var response = await _operations.InsertAsync(model);

            Assert.False(response.IsSuccess);
            Assert.Equal("Table name cannot be empty", response.ErrorMessage);
        }
    }
}
