using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Homedish.Aws.Dynamo.Model;
using Homedish.Aws.Dynamo.Model.Query;
using Xunit;

namespace Homedish.Aws.Dynamo.UnitTests.Dynamo
{
    public class QueryTests
    {
        private readonly IOperations _operations = new Operations();
        private readonly string _tableName = "kevin-test";
        private readonly string _indexName = "id-index";

        [Fact]
        public async Task QueryValidIndexValidOperation_Success()
        {
            var model = new QueryModel
            {
                TableName = _tableName,
                IndexName = _indexName,
                ConditionExpression = "id = :id",
                ConditionExpressionValues = new List<DynamoField>
                {
                    new DynamoField(":id", ColumnType.Number, "1"),
                },
                FieldsToRetrieve = new List<DynamoFieldSchema>
                {
                    new DynamoFieldSchema("test", ColumnType.String)
                },
            };

            var response = await _operations.QueryAsync(model);

            Assert.True(response.IsSuccess);
        }

        [Fact]
        public async Task QueryValidIndexWithExtraUnexistingFieldValidOperation_Success()
        {
            var model = new QueryModel
            {
                TableName = _tableName,
                IndexName = _indexName,
                ConditionExpression = "id = :id",
                ConditionExpressionValues = new List<DynamoField>
                {
                    new DynamoField(":id", ColumnType.Number, "1"),
                },
                FieldsToRetrieve = new List<DynamoFieldSchema>
                {
                    new DynamoFieldSchema("test", ColumnType.String),
                    new DynamoFieldSchema("test2", ColumnType.String)
                },
            };

            var response = await _operations.QueryAsync(model);

            Assert.True(response.IsSuccess);
        }

        [Fact]
        public async Task QueryInvalidIndexValidOperation_Failure()
        {
            var model = new QueryModel
            {
                TableName = _tableName,
                IndexName = "wrong-index",
                ConditionExpression = "id = :id",
                ConditionExpressionValues = new List<DynamoField>
                {
                    new DynamoField {ColumnName = ":id", ColumnType = ColumnType.Number, ColumnValue = "1"},
                },
                FieldsToRetrieve = new List<DynamoFieldSchema>
                {
                    new DynamoFieldSchema {ColumnName = "test", ColumnType = ColumnType.String},
                },
            };

            var response = await _operations.QueryAsync(model);

            Assert.False(response.IsSuccess);
            Assert.Equal(HttpStatusCode.BadRequest, response.HttpStatusCode);
            Assert.Equal("ValidationException", response.ErrorCode);
        }

        [Fact]
        public async Task QueryEmptyTableOperation_Failure()
        {
            var model = new QueryModel
            {
                IndexName = _indexName,
                ConditionExpression = "id = :id",
                ConditionExpressionValues = new List<DynamoField>
                {
                    new DynamoField {ColumnName = ":id", ColumnType = ColumnType.Number, ColumnValue = "1"},
                },
                FieldsToRetrieve = new List<DynamoFieldSchema>
                {
                    new DynamoFieldSchema {ColumnName = "test", ColumnType = ColumnType.String},
                },
            };

            var response = await _operations.QueryAsync(model);

            Assert.False(response.IsSuccess);
            Assert.Equal("Table name cannot be empty", response.ErrorMessage);
        }

        [Fact]
        public async Task QueryEmptyIndexOperation_Failure()
        {
            var model = new QueryModel
            {
                TableName = _indexName,
                ConditionExpression = "id = :id",
                ConditionExpressionValues = new List<DynamoField>
                {
                    new DynamoField {ColumnName = ":id", ColumnType = ColumnType.Number, ColumnValue = "1"},
                },
                FieldsToRetrieve = new List<DynamoFieldSchema>
                {
                    new DynamoFieldSchema {ColumnName = "test", ColumnType = ColumnType.String},
                },
            };

            var response = await _operations.QueryAsync(model);

            Assert.False(response.IsSuccess);
            Assert.Equal("Index name cannot be empty", response.ErrorMessage);
        }
    }
}
