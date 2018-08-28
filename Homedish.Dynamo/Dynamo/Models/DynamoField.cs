namespace Homedish.Aws.Dynamo.Models
{
    public enum ColumnType
    {
        Number,
        String,
        Boolean
    }

    public class DynamoField : DynamoFieldSchema
    {
        public string ColumnValue { get; set; }

        public DynamoField()
        {
        }

        public DynamoField(string columnName, ColumnType columnType, string columnValue)
        {
            ColumnValue = columnValue;
            ColumnName = columnName;
            ColumnType = columnType;
        }
    }
}
