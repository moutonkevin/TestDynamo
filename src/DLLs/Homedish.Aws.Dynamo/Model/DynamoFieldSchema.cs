namespace Homedish.Aws.Dynamo.Model
{
    public class DynamoFieldSchema
    {
        public DynamoFieldSchema()
        {
        }

        public DynamoFieldSchema(string columnName, ColumnType columnType)
        {
            ColumnName = columnName;
            ColumnType = columnType;
        }

        public string ColumnName { get; set; }
        public ColumnType ColumnType { get; set; }
    }
}