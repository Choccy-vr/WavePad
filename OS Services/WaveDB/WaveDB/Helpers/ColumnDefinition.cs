public class ColumnDefinition
{
    public string Name { get; set; }
    public SQLiteDataType DataType { get; set; }
    public List<SQLiteConstraint> Constraints { get; set; } = new List<SQLiteConstraint>();
    public string DefaultValue { get; set; }

    public ColumnDefinition(string name, SQLiteDataType dataType)
    {
        Name = name;
        DataType = dataType;
    }

    // Fluent methods for easy chaining
    public ColumnDefinition PrimaryKey()
    {
        Constraints.Add(SQLiteConstraint.PRIMARY_KEY);
        return this;
    }

    public ColumnDefinition AutoIncrement()
    {
        Constraints.Add(SQLiteConstraint.AUTOINCREMENT);
        return this;
    }

    public ColumnDefinition NotNull()
    {
        Constraints.Add(SQLiteConstraint.NOT_NULL);
        return this;
    }

    public ColumnDefinition Unique()
    {
        Constraints.Add(SQLiteConstraint.UNIQUE);
        return this;
    }

    public ColumnDefinition WithDefault(string defaultValue)
    {
        DefaultValue = defaultValue;
        Constraints.Add(SQLiteConstraint.DEFAULT);
        return this;
    }

    // Convert to SQL string
    public override string ToString()
    {
        var parts = new List<string> { Name, DataType.ToString() };

        foreach (var constraint in Constraints)
        {
            switch (constraint)
            {
                case SQLiteConstraint.PRIMARY_KEY:
                    parts.Add("PRIMARY KEY");
                    break;
                case SQLiteConstraint.AUTOINCREMENT:
                    parts.Add("AUTOINCREMENT");
                    break;
                case SQLiteConstraint.NOT_NULL:
                    parts.Add("NOT NULL");
                    break;
                case SQLiteConstraint.UNIQUE:
                    parts.Add("UNIQUE");
                    break;
                case SQLiteConstraint.DEFAULT:
                    if (!string.IsNullOrEmpty(DefaultValue))
                    {
                        parts.Add($"DEFAULT {DefaultValue}");
                    }
                    break;
            }
        }

        return string.Join(" ", parts);
    }
}