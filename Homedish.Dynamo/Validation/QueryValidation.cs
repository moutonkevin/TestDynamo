﻿using Homedish.Dynamo.Models;
using Homedish.Dynamo.Models.Query;

namespace Homedish.Dynamo.Validation
{
    internal class QueryValidation : IOperationValidation<QueryModel>
    {
        public ResponseBase Validate(QueryModel query)
        {
            var response = new QueryResponseModel();

            if (string.IsNullOrWhiteSpace(query.TableName))
            {
                response.ErrorMessage = "Table name cannot be empty";
            }

            if (string.IsNullOrWhiteSpace(query.IndexName))
            {
                response.ErrorMessage = "Index name cannot be empty";
            }

            return response;
        }
    }
}
