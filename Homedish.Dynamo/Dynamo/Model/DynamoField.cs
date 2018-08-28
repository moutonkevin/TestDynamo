namespace Homedish.Aws.Dynamo.Model
{
    public enum ColumnType
    {
        Number,
        String,
        Boolean
    }

    public class DynamoField : DynamoFieldSchema
    {
        public DynamoField()
        {
        }

        public DynamoField(string columnName, ColumnType columnType, string columnValue)
        {
            ColumnValue = columnValue;
            ColumnName = columnName;
            ColumnType = columnType;
        }

        public string ColumnValue { get; set; }
    }
}