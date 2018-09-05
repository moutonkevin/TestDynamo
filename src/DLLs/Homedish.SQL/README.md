# Execute stored procedures

```
var configs = new StoredProcedureConfiguration
{
    ConnectionString = <connection string here>,
    StoredProcedureName = <stored procedure name here>,
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
        Test = await results.GetColumnValue<string>(<column name here>),
    });
});
```