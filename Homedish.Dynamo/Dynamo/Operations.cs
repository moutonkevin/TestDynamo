using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Homedish.Aws.Dynamo.Models;
using Homedish.Aws.Dynamo.Models.Delete;
using Homedish.Aws.Dynamo.Models.Get;
using Homedish.Aws.Dynamo.Models.Insert;
using Homedish.Aws.Dynamo.Models.Query;
using Homedish.Aws.Dynamo.Validation;

namespace Homedish.Aws.Dynamo
{
    public class Operations : IOperations
    {
    
        private readonly IOperationValidation<DeleteModel> _deleteValidation = new DeleteValidation();
        private readonly IOperationValidation<GetModel> _getValidation = new GetValidation();
        private readonly IOperationValidation<InsertModel> _insertValidation = new InsertValidation();
        private readonly IOperationValidation<QueryModel> _queryValidation = new QueryValidation();

        public async Task<InsertResponseModel> InsertAsync(InsertModel value)
        {
            var validationResult = _insertValidation.Validate(value);
            if (!validationResult.IsSuccess)
                return await Task.FromResult((InsertResponseModel) validationResult);

            try
            {
                var dynamoObject = value.Fields.ToDictionary(column => column.ColumnName,
                    column => CreateAttributeValue(column.ColumnType, column.ColumnValue));

                var putItemResponse = await Client.PutItemAsync(new PutItemRequest
                {
                    TableName = value.TableName,
                    Item = dynamoObject
                });

                return new InsertResponseModel
                {
                    HttpStatusCode = putItemResponse.HttpStatusCode
                };
            }
            catch (AmazonDynamoDBException exception)
            {
                return new InsertResponseModel
                {
                    HttpStatusCode = exception.StatusCode,
                    ErrorCode = exception.ErrorCode,
                    ErrorMessage = exception.Message
                };
            }
            catch (Exception exception)
            {
                return new InsertResponseModel
                {
                    ErrorMessage = exception.ToString()
                };
            }
        }

        public async Task<GetResponseModel> GetAsync(GetModel value)
        {
            var validationResult = _getValidation.Validate(value);
            if (!validationResult.IsSuccess)
                return await Task.FromResult((GetResponseModel) validationResult);

            try
            {
                var dynamoObject = ConvertToDynamoKey(value.PartitionKey, value.SortKey);

                var getItemResponse = await Client.GetItemAsync(new GetItemRequest
                {
                    TableName = value.TableName,
                    Key = dynamoObject
                });

                var items = new List<DynamoField>();

                foreach (var field in value.FieldsToRetrieve)
                {
                    var dynamoField = getItemResponse.Item[field.ColumnName];

                    items.Add(new DynamoField(field.ColumnName, field.ColumnType,
                        GetAttributeValue(dynamoField, field.ColumnType)));
                }

                return new GetResponseModel
                {
                    HttpStatusCode = getItemResponse.HttpStatusCode,
                    Items = new List<List<DynamoField>> {items}
                };
            }
            catch (AmazonDynamoDBException exception)
            {
                return new GetResponseModel
                {
                    HttpStatusCode = exception.StatusCode,
                    ErrorCode = exception.ErrorCode,
                    ErrorMessage = exception.Message
                };
            }
            catch (Exception exception)
            {
                return new GetResponseModel
                {
                    ErrorMessage = exception.ToString()
                };
            }
        }

        public async Task<QueryResponseModel> QueryAsync(QueryModel value)
        {
            var validationResult = _queryValidation.Validate(value);
            if (!validationResult.IsSuccess)
                return await Task.FromResult((QueryResponseModel) validationResult);

            try
            {
                var values = new Dictionary<string, AttributeValue>();

                foreach (var cev in value.ConditionExpressionValues)
                    values.Add(cev.ColumnName, CreateAttributeValue(cev.ColumnType, cev.ColumnValue));

                var queryResponse = await Client.QueryAsync(new QueryRequest
                {
                    TableName = value.TableName,
                    IndexName = value.IndexName,
                    KeyConditionExpression = value.ConditionExpression,
                    ExpressionAttributeValues = values,
                    ProjectionExpression = string.Join(",", value.FieldsToRetrieve.Select(s => s.ColumnName))
                });

                var items = new List<List<DynamoField>>();

                foreach (var row in queryResponse.Items)
                {
                    var columns = new List<DynamoField>();

                    foreach (var fieldsToRetrieve in value.FieldsToRetrieve)
                    foreach (var column in row)
                        if (string.Compare(fieldsToRetrieve.ColumnName, column.Key,
                                StringComparison.InvariantCulture) == 0)
                            columns.Add(new DynamoField(column.Key, fieldsToRetrieve.ColumnType,
                                GetAttributeValue(column.Value, fieldsToRetrieve.ColumnType)));
                    items.Add(columns);
                }

                return new QueryResponseModel
                {
                    HttpStatusCode = queryResponse.HttpStatusCode,
                    Items = items
                };
            }
            catch (AmazonDynamoDBException exception)
            {
                return new QueryResponseModel
                {
                    HttpStatusCode = exception.StatusCode,
                    ErrorCode = exception.ErrorCode,
                    ErrorMessage = exception.Message
                };
            }
            catch (Exception exception)
            {
                return new QueryResponseModel
                {
                    ErrorMessage = exception.ToString()
                };
            }
        }

        public async Task<DeleteResponseModel> DeleteAsync(DeleteModel value)
        {
            var validationResult = _deleteValidation.Validate(value);
            if (!validationResult.IsSuccess)
                return await Task.FromResult((DeleteResponseModel) validationResult);

            try
            {
                var dynamoObject = value.Keys.ToDictionary(column => column.ColumnName,
                    column => CreateAttributeValue(column.ColumnType, column.ColumnValue));

                var deleteItemResponse = await Client.DeleteItemAsync(new DeleteItemRequest
                {
                    TableName = value.TableName,
                    Key = dynamoObject
                });

                return new DeleteResponseModel
                {
                    HttpStatusCode = deleteItemResponse.HttpStatusCode
                };
            }
            catch (AmazonDynamoDBException exception)
            {
                return new DeleteResponseModel
                {
                    HttpStatusCode = exception.StatusCode,
                    ErrorCode = exception.ErrorCode,
                    ErrorMessage = exception.Message
                };
            }
            catch (Exception exception)
            {
                return new DeleteResponseModel
                {
                    ErrorMessage = exception.ToString()
                };
            }
        }

        private AttributeValue CreateAttributeValue(ColumnType columnType, string columnValue)
        {
            switch (columnType)
            {
                case ColumnType.Number:
                    return new AttributeValue {N = columnValue};

                case ColumnType.String:
                    return new AttributeValue {S = columnValue};

                case ColumnType.Boolean:
                    return new AttributeValue {BOOL = bool.Parse(columnValue)};

                default:
                    return null;
            }
        }

        private string GetAttributeValue(AttributeValue attributeValue, ColumnType columnType)
        {
            switch (columnType)
            {
                case ColumnType.Number:
                    return attributeValue.N;

                case ColumnType.String:
                    return attributeValue.S;

                case ColumnType.Boolean:
                    return attributeValue.BOOL.ToString();

                default:
                    return null;
            }
        }

        private Dictionary<string, AttributeValue> ConvertToDynamoKey(DynamoField partitionKey, DynamoField sortKey)
        {
            var dynamoKeys = new Dictionary<string, AttributeValue>();

            if (partitionKey != null)
                dynamoKeys.Add(partitionKey.ColumnName,
                    CreateAttributeValue(partitionKey.ColumnType, partitionKey.ColumnValue));
            if (sortKey != null)
                dynamoKeys.Add(sortKey.ColumnName, CreateAttributeValue(sortKey.ColumnType, sortKey.ColumnValue));

            return dynamoKeys;
        }
    }
}