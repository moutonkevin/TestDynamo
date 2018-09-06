# Get from dynamo

1. Add to DI

`services.AddTransient<IOperations, Operations>();`

2. Example

```
var getOperation = new GetModel
{
    TableName = <table name here>,
    PartitionKey = new DynamoField("id", ColumnType.Number, "1"),
    FieldsToRetrieve = new List<DynamoFieldSchema>
    {
        new DynamoFieldSchema("id", ColumnType.Number),
        new DynamoFieldSchema("bool", ColumnType.String),
    },
};

var response = await _operations.GetAsync(model);
```

# Inserto to dynamo

1. Add to DI

`services.AddTransient<IOperations, Operations>();`

2. Example

```
var model = new InsertModel
{
    TableName = <table name here>,
    Fields = new List<DynamoField>
    {
        new DynamoField {ColumnName = "id", ColumnType = ColumnType.Number, ColumnValue = "1"},
        new DynamoField {ColumnName = "bool", ColumnType = ColumnType.Boolean, ColumnValue = "True"}
    }
};

var response = await _operations.InsertAsync(model);
```

# Inserto to dynamo

1. Add to DI

`services.AddTransient<IOperations, Operations>();`

2. Example

```
var model = new DeleteModel()
{
    TableName = <table name here>,
    Keys = new List<DynamoField>
    {
        new DynamoField("id", ColumnType.Number, "1"),
    }
};

var response = await _operations.DeleteAsync(model);
```

# Query dynamo

1. Add to DI

`services.AddTransient<IOperations, Operations>();`

2. Example

```
var model = new QueryModel
{
    TableName = <table name here>,
    IndexName = <index name here>,
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
```