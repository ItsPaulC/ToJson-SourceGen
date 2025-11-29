using FromJson;
using ToJson;
using Xunit;

namespace SourceGen.FromJson.Tests
{
    // Models with both ToJson and FromJson attributes for integration testing
    [ToJson]
    [FromJson]
    public partial class IntegrationSimpleModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    [ToJson]
    [FromJson]
    public partial class IntegrationNumericModel
    {
        public int IntValue { get; set; }
        public long LongValue { get; set; }
        public double DoubleValue { get; set; }
        public decimal DecimalValue { get; set; }
        public float FloatValue { get; set; }
    }

    [ToJson]
    [FromJson]
    public partial class IntegrationNullableModel
    {
        public string? NullableString { get; set; }
        public int? NullableInt { get; set; }
        public bool? NullableBool { get; set; }
    }

    [ToJson]
    [FromJson]
    public partial class IntegrationNestedModel
    {
        public int Id { get; set; }
        public IntegrationSimpleModel? NestedObject { get; set; }
    }

    [ToJson]
    [FromJson]
    public partial class IntegrationArrayModel
    {
        public int[] Numbers { get; set; } = Array.Empty<int>();
        public string[] Names { get; set; } = Array.Empty<string>();
    }

    [ToJson]
    [FromJson]
    public partial class IntegrationListModel
    {
        public List<int> Numbers { get; set; } = new();
        public List<string> Names { get; set; } = new();
    }

    [ToJson]
    [FromJson]
    public partial class IntegrationNestedCollectionModel
    {
        public int Id { get; set; }
        public List<IntegrationSimpleModel> Items { get; set; } = new();
    }

    public class IntegrationTests
    {
        [Fact]
        public void SimpleModel_ToJsonAndFromJson_RoundTrip_Minimized()
        {
            IntegrationSimpleModel original = new()
            {
                Id = 42,
                Name = "Test",
                IsActive = true
            };

            string json = original.ToJson(JsonSerializationStyle.Minimized);
            IntegrationSimpleModel result = IntegrationSimpleModel.FromJson(json);

            Assert.Equal(original.Id, result.Id);
            Assert.Equal(original.Name, result.Name);
            Assert.Equal(original.IsActive, result.IsActive);
        }

        [Fact]
        public void SimpleModel_ToJsonAndFromJson_RoundTrip_Indented()
        {
            IntegrationSimpleModel original = new()
            {
                Id = 42,
                Name = "Test",
                IsActive = true
            };

            string json = original.ToJson(JsonSerializationStyle.Indented);
            IntegrationSimpleModel result = IntegrationSimpleModel.FromJson(json);

            Assert.Equal(original.Id, result.Id);
            Assert.Equal(original.Name, result.Name);
            Assert.Equal(original.IsActive, result.IsActive);
        }

        [Fact]
        public void NumericModel_ToJsonAndFromJson_RoundTrip()
        {
            IntegrationNumericModel original = new()
            {
                IntValue = 123,
                LongValue = 9876543210L,
                DoubleValue = 3.14159,
                DecimalValue = 99.99m,
                FloatValue = 2.5f
            };

            string json = original.ToJson();
            IntegrationNumericModel result = IntegrationNumericModel.FromJson(json);

            Assert.Equal(original.IntValue, result.IntValue);
            Assert.Equal(original.LongValue, result.LongValue);
            Assert.Equal(original.DoubleValue, result.DoubleValue);
            Assert.Equal(original.DecimalValue, result.DecimalValue);
            Assert.Equal(original.FloatValue, result.FloatValue);
        }

        [Fact]
        public void NullableModel_WithNulls_ToJsonAndFromJson_RoundTrip()
        {
            IntegrationNullableModel original = new()
            {
                NullableString = null,
                NullableInt = null,
                NullableBool = null
            };

            string json = original.ToJson();
            IntegrationNullableModel result = IntegrationNullableModel.FromJson(json);

            Assert.Null(result.NullableString);
            Assert.Null(result.NullableInt);
            Assert.Null(result.NullableBool);
        }

        [Fact]
        public void NullableModel_WithValues_ToJsonAndFromJson_RoundTrip()
        {
            IntegrationNullableModel original = new()
            {
                NullableString = "Hello",
                NullableInt = 100,
                NullableBool = true
            };

            string json = original.ToJson();
            IntegrationNullableModel result = IntegrationNullableModel.FromJson(json);

            Assert.Equal(original.NullableString, result.NullableString);
            Assert.Equal(original.NullableInt, result.NullableInt);
            Assert.Equal(original.NullableBool, result.NullableBool);
        }

        [Fact]
        public void NestedModel_ToJsonAndFromJson_RoundTrip()
        {
            IntegrationNestedModel original = new()
            {
                Id = 1,
                NestedObject = new IntegrationSimpleModel
                {
                    Id = 2,
                    Name = "Nested",
                    IsActive = true
                }
            };

            string json = original.ToJson();
            IntegrationNestedModel result = IntegrationNestedModel.FromJson(json);

            Assert.Equal(original.Id, result.Id);
            Assert.NotNull(result.NestedObject);
            Assert.Equal(original.NestedObject.Id, result.NestedObject.Id);
            Assert.Equal(original.NestedObject.Name, result.NestedObject.Name);
            Assert.Equal(original.NestedObject.IsActive, result.NestedObject.IsActive);
        }

        [Fact]
        public void NestedModel_WithNull_ToJsonAndFromJson_RoundTrip()
        {
            IntegrationNestedModel original = new()
            {
                Id = 1,
                NestedObject = null
            };

            string json = original.ToJson();
            IntegrationNestedModel result = IntegrationNestedModel.FromJson(json);

            Assert.Equal(original.Id, result.Id);
            Assert.Null(result.NestedObject);
        }

        [Fact]
        public void ArrayModel_ToJsonAndFromJson_RoundTrip()
        {
            IntegrationArrayModel original = new()
            {
                Numbers = new[] { 1, 2, 3, 4, 5 },
                Names = new[] { "Alice", "Bob", "Charlie" }
            };

            string json = original.ToJson();
            IntegrationArrayModel result = IntegrationArrayModel.FromJson(json);

            Assert.Equal(original.Numbers, result.Numbers);
            Assert.Equal(original.Names, result.Names);
        }

        [Fact]
        public void ListModel_ToJsonAndFromJson_RoundTrip()
        {
            IntegrationListModel original = new()
            {
                Numbers = new List<int> { 10, 20, 30 },
                Names = new List<string> { "One", "Two", "Three" }
            };

            string json = original.ToJson();
            IntegrationListModel result = IntegrationListModel.FromJson(json);

            Assert.Equal(original.Numbers, result.Numbers);
            Assert.Equal(original.Names, result.Names);
        }

        [Fact]
        public void NestedCollectionModel_ToJsonAndFromJson_RoundTrip()
        {
            IntegrationNestedCollectionModel original = new()
            {
                Id = 100,
                Items = new List<IntegrationSimpleModel>
                {
                    new() { Id = 1, Name = "First", IsActive = true },
                    new() { Id = 2, Name = "Second", IsActive = false },
                    new() { Id = 3, Name = "Third", IsActive = true }
                }
            };

            string json = original.ToJson();
            IntegrationNestedCollectionModel result = IntegrationNestedCollectionModel.FromJson(json);

            Assert.Equal(original.Id, result.Id);
            Assert.Equal(original.Items.Count, result.Items.Count);
            for (int i = 0; i < original.Items.Count; i++)
            {
                Assert.Equal(original.Items[i].Id, result.Items[i].Id);
                Assert.Equal(original.Items[i].Name, result.Items[i].Name);
                Assert.Equal(original.Items[i].IsActive, result.Items[i].IsActive);
            }
        }

        [Fact]
        public void SimpleModel_WithSpecialCharacters_ToJsonAndFromJson_RoundTrip()
        {
            IntegrationSimpleModel original = new()
            {
                Id = 5,
                Name = "Test \"quotes\" and \\backslashes\\",
                IsActive = true
            };

            string json = original.ToJson();
            IntegrationSimpleModel result = IntegrationSimpleModel.FromJson(json);

            Assert.Equal(original.Id, result.Id);
            Assert.Equal(original.Name, result.Name);
            Assert.Equal(original.IsActive, result.IsActive);
        }

        [Fact]
        public void ArrayModel_WithEmptyArrays_ToJsonAndFromJson_RoundTrip()
        {
            IntegrationArrayModel original = new()
            {
                Numbers = Array.Empty<int>(),
                Names = Array.Empty<string>()
            };

            string json = original.ToJson();
            IntegrationArrayModel result = IntegrationArrayModel.FromJson(json);

            Assert.Empty(result.Numbers);
            Assert.Empty(result.Names);
        }

        [Fact]
        public void NumericModel_WithZeroValues_ToJsonAndFromJson_RoundTrip()
        {
            IntegrationNumericModel original = new()
            {
                IntValue = 0,
                LongValue = 0L,
                DoubleValue = 0.0,
                DecimalValue = 0m,
                FloatValue = 0f
            };

            string json = original.ToJson();
            IntegrationNumericModel result = IntegrationNumericModel.FromJson(json);

            Assert.Equal(original.IntValue, result.IntValue);
            Assert.Equal(original.LongValue, result.LongValue);
            Assert.Equal(original.DoubleValue, result.DoubleValue);
            Assert.Equal(original.DecimalValue, result.DecimalValue);
            Assert.Equal(original.FloatValue, result.FloatValue);
        }
    }
}
