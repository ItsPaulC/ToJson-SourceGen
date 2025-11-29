using System;
using System.Collections.Generic;

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

    [ToJson]
    public partial class ArrayModel
    {
        public int[] Numbers { get; set; } = Array.Empty<int>();
        public string[] Names { get; set; } = Array.Empty<string>();
    }

    [ToJson]
    public partial class ListModel
    {
        public List<int> Numbers { get; set; } = new();
        public List<string> Names { get; set; } = new();
    }

    [ToJson]
    public partial class CollectionWithNullsModel
    {
        public int[]? NullableArray { get; set; }
        public List<string?>? NullableList { get; set; }
    }

    [ToJson]
    public partial class NestedCollectionModel
    {
        public int Id { get; set; }
        public List<SimpleModel> Items { get; set; } = new();
    }

    [ToJson]
    public partial class EmptyCollectionModel
    {
        public int[] EmptyArray { get; set; } = Array.Empty<int>();
        public List<string> EmptyList { get; set; } = new();
    }

    [ToJson]
    public partial class CircularParent
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public CircularChild? Child { get; set; }
    }

    [ToJson]
    public partial class CircularChild
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public CircularParent? Parent { get; set; }
    }

    [ToJson]
    public partial class SelfReferencingModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public SelfReferencingModel? Next { get; set; }
    }
}
