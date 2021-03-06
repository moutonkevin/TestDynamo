﻿using System.Collections.Generic;

namespace Homedish.Aws.Dynamo.Model.Get
{
    public class GetModel : OperationModel
    {
        public DynamoField PartitionKey { get; set; }
        public DynamoField SortKey { get; set; }
        public List<DynamoFieldSchema> FieldsToRetrieve { get; set; }
    }
}