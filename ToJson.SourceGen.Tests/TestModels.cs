namespace ToJson.SourceGen.Tests
{
    [ToJson]
    public partial class SimpleModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    [ToJson]
    public partial class NumericModel
    {
        public int IntValue { get; set; }
        public long LongValue { get; set; }
        public double DoubleValue { get; set; }
        public decimal DecimalValue { get; set; }
        public float FloatValue { get; set; }
    }

    [ToJson]
    public partial class NullableModel
    {
        public string? NullableString { get; set; }
        public int? NullableInt { get; set; }
        public bool? NullableBool { get; set; }
    }

    [ToJson]
    public partial class NestedModel
    {
        public int Id { get; set; }
        public SimpleModel? NestedObject { get; set; }
    }

    [ToJson]
    public partial class ComplexModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public bool Available { get; set; }
        public string? Description { get; set; }
    }
}
