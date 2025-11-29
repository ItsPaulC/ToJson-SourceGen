using FromJson;

namespace SourceGen.FromJson.Tests
{
    [FromJson]
    public partial class SimpleModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    [FromJson]
    public partial class NumericModel
    {
        public int IntValue { get; set; }
        public long LongValue { get; set; }
        public double DoubleValue { get; set; }
        public decimal DecimalValue { get; set; }
        public float FloatValue { get; set; }
    }

    [FromJson]
    public partial class NullableModel
    {
        public string? NullableString { get; set; }
        public int? NullableInt { get; set; }
        public bool? NullableBool { get; set; }
    }

    [FromJson]
    public partial class NestedModel
    {
        public int Id { get; set; }
        public SimpleModel? NestedObject { get; set; }
    }

    [FromJson]
    public partial class ComplexModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public bool Available { get; set; }
        public string? Description { get; set; }
    }

    [FromJson]
    public partial class ArrayModel
    {
        public int[] Numbers { get; set; } = Array.Empty<int>();
        public string[] Names { get; set; } = Array.Empty<string>();
    }

    [FromJson]
    public partial class ListModel
    {
        public List<int> Numbers { get; set; } = new();
        public List<string> Names { get; set; } = new();
    }

    [FromJson]
    public partial class CollectionWithNullsModel
    {
        public int[]? NullableArray { get; set; }
        public List<string?>? NullableList { get; set; }
    }

    [FromJson]
    public partial class NestedCollectionModel
    {
        public int Id { get; set; }
        public List<SimpleModel> Items { get; set; } = new();
    }

    [FromJson]
    public partial class EmptyCollectionModel
    {
        public int[] EmptyArray { get; set; } = Array.Empty<int>();
        public List<string> EmptyList { get; set; } = new();
    }
}
