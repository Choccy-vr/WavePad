public static class TableBuilder
{
    // Quick column creation methods
    public static ColumnDefinition Id(string name = "id")
    {
        return new ColumnDefinition(name, SQLiteDataType.INTEGER)
            .PrimaryKey()
            .AutoIncrement();
    }

    public static ColumnDefinition Text(string name)
    {
        return new ColumnDefinition(name, SQLiteDataType.TEXT);
    }

    public static ColumnDefinition Integer(string name)
    {
        return new ColumnDefinition(name, SQLiteDataType.INTEGER);
    }

    public static ColumnDefinition Real(string name)
    {
        return new ColumnDefinition(name, SQLiteDataType.REAL);
    }

    public static ColumnDefinition Timestamp(string name = "created_at")
    {
        return new ColumnDefinition(name, SQLiteDataType.TEXT)
            .WithDefault("CURRENT_TIMESTAMP");
    }

    public static ColumnDefinition Bool(string name)
    {
        return new ColumnDefinition(name, SQLiteDataType.INTEGER)
            .WithDefault("0");
    }
}