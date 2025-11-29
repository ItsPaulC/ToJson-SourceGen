using Xunit;

namespace SourceGen.FromJson.Tests
{
    public class FromJsonGeneratorTests
    {
        [Fact]
        public void SimpleModel_FromJson_ParsesCorrectly()
        {
            string json = "{\"Id\":42,\"Name\":\"Test\",\"IsActive\":true}";

            SimpleModel result = SimpleModel.FromJson(json);

            Assert.Equal(42, result.Id);
            Assert.Equal("Test", result.Name);
            Assert.True(result.IsActive);
        }

        [Fact]
        public void SimpleModel_FromJsonIndented_ParsesCorrectly()
        {
            string json = @"{
  ""Id"": 42,
  ""Name"": ""Test"",
  ""IsActive"": true
}";

            SimpleModel result = SimpleModel.FromJson(json);

            Assert.Equal(42, result.Id);
            Assert.Equal("Test", result.Name);
            Assert.True(result.IsActive);
        }

        [Fact]
        public void SimpleModel_WithEmptyString_FromJson_ParsesCorrectly()
        {
            string json = "{\"Id\":1,\"Name\":\"\",\"IsActive\":false}";

            SimpleModel result = SimpleModel.FromJson(json);

            Assert.Equal(1, result.Id);
            Assert.Equal("", result.Name);
            Assert.False(result.IsActive);
        }

        [Fact]
        public void NumericModel_FromJson_ParsesCorrectly()
        {
            string json = "{\"IntValue\":123,\"LongValue\":9876543210,\"DoubleValue\":3.14159,\"DecimalValue\":99.99,\"FloatValue\":2.5}";

            NumericModel result = NumericModel.FromJson(json);

            Assert.Equal(123, result.IntValue);
            Assert.Equal(9876543210L, result.LongValue);
            Assert.Equal(3.14159, result.DoubleValue);
            Assert.Equal(99.99m, result.DecimalValue);
            Assert.Equal(2.5f, result.FloatValue);
        }

        [Fact]
        public void NumericModel_WithZeroValues_FromJson_ParsesCorrectly()
        {
            string json = "{\"IntValue\":0,\"LongValue\":0,\"DoubleValue\":0,\"DecimalValue\":0,\"FloatValue\":0}";

            NumericModel result = NumericModel.FromJson(json);

            Assert.Equal(0, result.IntValue);
            Assert.Equal(0L, result.LongValue);
            Assert.Equal(0.0, result.DoubleValue);
            Assert.Equal(0m, result.DecimalValue);
            Assert.Equal(0f, result.FloatValue);
        }

        [Fact]
        public void NullableModel_WithNullValues_FromJson_ParsesCorrectly()
        {
            string json = "{\"NullableString\":null,\"NullableInt\":null,\"NullableBool\":null}";

            NullableModel result = NullableModel.FromJson(json);

            Assert.Null(result.NullableString);
            Assert.Null(result.NullableInt);
            Assert.Null(result.NullableBool);
        }

        [Fact]
        public void NullableModel_WithValues_FromJson_ParsesCorrectly()
        {
            string json = "{\"NullableString\":\"Hello\",\"NullableInt\":100,\"NullableBool\":true}";

            NullableModel result = NullableModel.FromJson(json);

            Assert.Equal("Hello", result.NullableString);
            Assert.Equal(100, result.NullableInt);
            Assert.True(result.NullableBool);
        }

        [Fact]
        public void NestedModel_WithNestedObject_FromJson_ParsesCorrectly()
        {
            string json = "{\"Id\":1,\"NestedObject\":{\"Id\":2,\"Name\":\"Nested\",\"IsActive\":true}}";

            NestedModel result = NestedModel.FromJson(json);

            Assert.Equal(1, result.Id);
            Assert.NotNull(result.NestedObject);
            Assert.Equal(2, result.NestedObject.Id);
            Assert.Equal("Nested", result.NestedObject.Name);
            Assert.True(result.NestedObject.IsActive);
        }

        [Fact]
        public void NestedModel_WithNullNestedObject_FromJson_ParsesCorrectly()
        {
            string json = "{\"Id\":1,\"NestedObject\":null}";

            NestedModel result = NestedModel.FromJson(json);

            Assert.Equal(1, result.Id);
            Assert.Null(result.NestedObject);
        }

        [Fact]
        public void ComplexModel_FromJson_ParsesCorrectly()
        {
            string json = "{\"Id\":999,\"Name\":\"Complex Test\",\"Price\":49.99,\"Available\":true,\"Description\":\"A complex model with multiple property types\"}";

            ComplexModel result = ComplexModel.FromJson(json);

            Assert.Equal(999, result.Id);
            Assert.Equal("Complex Test", result.Name);
            Assert.Equal(49.99, result.Price);
            Assert.True(result.Available);
            Assert.Equal("A complex model with multiple property types", result.Description);
        }

        [Fact]
        public void ComplexModel_WithNullDescription_FromJson_ParsesCorrectly()
        {
            string json = "{\"Id\":888,\"Name\":\"No Description\",\"Price\":19.99,\"Available\":false,\"Description\":null}";

            ComplexModel result = ComplexModel.FromJson(json);

            Assert.Equal(888, result.Id);
            Assert.Equal("No Description", result.Name);
            Assert.Equal(19.99, result.Price);
            Assert.False(result.Available);
            Assert.Null(result.Description);
        }

        [Fact]
        public void SimpleModel_WithSpecialCharacters_FromJson_ParsesCorrectly()
        {
            string json = "{\"Id\":5,\"Name\":\"Test \\\"quotes\\\" and \\\\backslashes\\\\\",\"IsActive\":true}";

            SimpleModel result = SimpleModel.FromJson(json);

            Assert.Equal(5, result.Id);
            Assert.Equal("Test \"quotes\" and \\backslashes\\", result.Name);
            Assert.True(result.IsActive);
        }

        [Fact]
        public void ArrayModel_WithIntArray_FromJson_ParsesCorrectly()
        {
            string json = "{\"Numbers\":[1,2,3,4,5],\"Names\":[\"Alice\",\"Bob\",\"Charlie\"]}";

            ArrayModel result = ArrayModel.FromJson(json);

            Assert.Equal(new[] { 1, 2, 3, 4, 5 }, result.Numbers);
            Assert.Equal(new[] { "Alice", "Bob", "Charlie" }, result.Names);
        }

        [Fact]
        public void ListModel_WithList_FromJson_ParsesCorrectly()
        {
            string json = "{\"Numbers\":[10,20,30],\"Names\":[\"One\",\"Two\",\"Three\"]}";

            ListModel result = ListModel.FromJson(json);

            Assert.Equal(new List<int> { 10, 20, 30 }, result.Numbers);
            Assert.Equal(new List<string> { "One", "Two", "Three" }, result.Names);
        }

        [Fact]
        public void EmptyCollectionModel_FromJson_ParsesCorrectly()
        {
            string json = "{\"EmptyArray\":[],\"EmptyList\":[]}";

            EmptyCollectionModel result = EmptyCollectionModel.FromJson(json);

            Assert.Empty(result.EmptyArray);
            Assert.Empty(result.EmptyList);
        }

        [Fact]
        public void CollectionWithNullsModel_WithNullCollection_FromJson_ParsesCorrectly()
        {
            string json = "{\"NullableArray\":null,\"NullableList\":null}";

            CollectionWithNullsModel result = CollectionWithNullsModel.FromJson(json);

            Assert.Null(result.NullableArray);
            Assert.Null(result.NullableList);
        }

        [Fact]
        public void CollectionWithNullsModel_WithNullElements_FromJson_ParsesCorrectly()
        {
            string json = "{\"NullableArray\":[1,2,3],\"NullableList\":[\"Hello\",null,\"World\"]}";

            CollectionWithNullsModel result = CollectionWithNullsModel.FromJson(json);

            Assert.Equal(new[] { 1, 2, 3 }, result.NullableArray);
            Assert.Equal(3, result.NullableList!.Count);
            Assert.Equal("Hello", result.NullableList[0]);
            Assert.Null(result.NullableList[1]);
            Assert.Equal("World", result.NullableList[2]);
        }

        [Fact]
        public void NestedCollectionModel_FromJson_ParsesCorrectly()
        {
            string json = "{\"Id\":100,\"Items\":[{\"Id\":1,\"Name\":\"First\",\"IsActive\":true},{\"Id\":2,\"Name\":\"Second\",\"IsActive\":false},{\"Id\":3,\"Name\":\"Third\",\"IsActive\":true}]}";

            NestedCollectionModel result = NestedCollectionModel.FromJson(json);

            Assert.Equal(100, result.Id);
            Assert.Equal(3, result.Items.Count);
            Assert.Equal(1, result.Items[0].Id);
            Assert.Equal("First", result.Items[0].Name);
            Assert.True(result.Items[0].IsActive);
            Assert.Equal(2, result.Items[1].Id);
            Assert.Equal("Second", result.Items[1].Name);
            Assert.False(result.Items[1].IsActive);
            Assert.Equal(3, result.Items[2].Id);
            Assert.Equal("Third", result.Items[2].Name);
            Assert.True(result.Items[2].IsActive);
        }

        [Fact]
        public void ArrayModel_WithEmptyArrays_FromJson_ParsesCorrectly()
        {
            string json = "{\"Numbers\":[],\"Names\":[]}";

            ArrayModel result = ArrayModel.FromJson(json);

            Assert.Empty(result.Numbers);
            Assert.Empty(result.Names);
        }

        [Fact]
        public void SimpleModel_WithMissingProperties_FromJson_UsesDefaults()
        {
            string json = "{\"Id\":42}";

            SimpleModel result = SimpleModel.FromJson(json);

            Assert.Equal(42, result.Id);
            Assert.Equal(string.Empty, result.Name);
            Assert.False(result.IsActive);
        }

        [Fact]
        public void SimpleModel_WithNullJson_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => SimpleModel.FromJson(null!));
        }

        [Fact]
        public void NestedCollectionModel_WithIndentedJson_ParsesCorrectly()
        {
            string json = @"{
  ""Id"": 100,
  ""Items"": [
    {
      ""Id"": 1,
      ""Name"": ""First"",
      ""IsActive"": true
    },
    {
      ""Id"": 2,
      ""Name"": ""Second"",
      ""IsActive"": false
    }
  ]
}";

            NestedCollectionModel result = NestedCollectionModel.FromJson(json);

            Assert.Equal(100, result.Id);
            Assert.Equal(2, result.Items.Count);
            Assert.Equal(1, result.Items[0].Id);
            Assert.Equal("First", result.Items[0].Name);
            Assert.True(result.Items[0].IsActive);
            Assert.Equal(2, result.Items[1].Id);
            Assert.Equal("Second", result.Items[1].Name);
            Assert.False(result.Items[1].IsActive);
        }
    }
}
