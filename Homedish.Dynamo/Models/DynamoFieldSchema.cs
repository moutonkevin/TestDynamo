namespace Homedish.Dynamo.Models
{
    public class DynamoFieldSchema
    {
        public string ColumnName { get; set; }
        public ColumnType ColumnType { get; set; }

        public DynamoFieldSchema()
        {
        }

        public DynamoFieldSchema(string columnName, ColumnType columnType)
        {
            ColumnName = columnName;
            ColumnType = columnType;
        }
    }
}
