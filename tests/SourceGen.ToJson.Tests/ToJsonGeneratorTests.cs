using System;
using System.Collections.Generic;
using System.Text.Json;
using ToJson;
using Xunit;

namespace SourceGen.ToJson.Tests
{
    public class ToJsonGeneratorTests
    {
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = null
        };

        private readonly JsonSerializerOptions _indentedJsonOptions = new()
        {
            PropertyNamingPolicy = null,
            WriteIndented = true
        };

        [Fact]
        public void SimpleModel_ToJson_MatchesSystemTextJson()
        {
            SimpleModel model = new()
            {
                Id = 42,
                Name = "Test",
                IsActive = true
            };

            string generatedJson = model.ToJson(JsonSerializationStyle.Minimized);
            string systemTextJson = JsonSerializer.Serialize(model, _jsonOptions);

            Assert.Equal(systemTextJson, generatedJson);
        }

        [Fact]
        public void SimpleModel_WithEmptyString_ToJson_MatchesSystemTextJson()
        {
            SimpleModel model = new()
            {
                Id = 1,
                Name = "",
                IsActive = false
            };

            string generatedJson = model.ToJson(JsonSerializationStyle.Minimized);
            string systemTextJson = JsonSerializer.Serialize(model, _jsonOptions);

            Assert.Equal(systemTextJson, generatedJson);
        }

        [Fact]
        public void NumericModel_ToJson_MatchesSystemTextJson()
        {
            NumericModel model = new()
            {
                IntValue = 123,
                LongValue = 9876543210L,
                DoubleValue = 3.14159,
                DecimalValue = 99.99m,
                FloatValue = 2.5f
            };

            string generatedJson = model.ToJson(JsonSerializationStyle.Minimized);
            string systemTextJson = JsonSerializer.Serialize(model, _jsonOptions);

            Assert.Equal(systemTextJson, generatedJson);
        }

        [Fact]
        public void NumericModel_WithZeroValues_ToJson_MatchesSystemTextJson()
        {
            NumericModel model = new()
            {
                IntValue = 0,
                LongValue = 0L,
                DoubleValue = 0.0,
                DecimalValue = 0m,
                FloatValue = 0f
            };

            string generatedJson = model.ToJson(JsonSerializationStyle.Minimized);
            string systemTextJson = JsonSerializer.Serialize(model, _jsonOptions);

            Assert.Equal(systemTextJson, generatedJson);
        }

        [Fact]
        public void NullableModel_WithNullValues_ToJson_MatchesSystemTextJson()
        {
            NullableModel model = new()
            {
                NullableString = null,
                NullableInt = null,
                NullableBool = null
            };

            string generatedJson = model.ToJson(JsonSerializationStyle.Minimized);
            string systemTextJson = JsonSerializer.Serialize(model, _jsonOptions);

            Assert.Equal(systemTextJson, generatedJson);
        }

        [Fact]
        public void NullableModel_WithValues_ToJson_MatchesSystemTextJson()
        {
            NullableModel model = new()
            {
                NullableString = "Hello",
                NullableInt = 100,
                NullableBool = true
            };

            string generatedJson = model.ToJson(JsonSerializationStyle.Minimized);
            string systemTextJson = JsonSerializer.Serialize(model, _jsonOptions);

            Assert.Equal(systemTextJson, generatedJson);
        }

        [Fact]
        public void NestedModel_WithNestedObject_ToJson_MatchesSystemTextJson()
        {
            NestedModel model = new()
            {
                Id = 1,
                NestedObject = new SimpleModel
                {
                    Id = 2,
                    Name = "Nested",
                    IsActive = true
                }
            };

            string generatedJson = model.ToJson(JsonSerializationStyle.Minimized);
            string systemTextJson = JsonSerializer.Serialize(model, _jsonOptions);

            Assert.Equal(systemTextJson, generatedJson);
        }

        [Fact]
        public void NestedModel_WithNullNestedObject_ToJson_MatchesSystemTextJson()
        {
            NestedModel model = new()
            {
                Id = 1,
                NestedObject = null
            };

            string generatedJson = model.ToJson(JsonSerializationStyle.Minimized);
            string systemTextJson = JsonSerializer.Serialize(model, _jsonOptions);

            Assert.Equal(systemTextJson, generatedJson);
        }

        [Fact]
        public void ComplexModel_ToJson_MatchesSystemTextJson()
        {
            ComplexModel model = new()
            {
                Id = 999,
                Name = "Complex Test",
                Price = 49.99,
                Available = true,
                Description = "A complex model with multiple property types"
            };

            string generatedJson = model.ToJson(JsonSerializationStyle.Minimized);
            string systemTextJson = JsonSerializer.Serialize(model, _jsonOptions);

            Assert.Equal(systemTextJson, generatedJson);
        }

        [Fact]
        public void ComplexModel_WithNullDescription_ToJson_MatchesSystemTextJson()
        {
            ComplexModel model = new()
            {
                Id = 888,
                Name = "No Description",
                Price = 19.99,
                Available = false,
                Description = null
            };

            string generatedJson = model.ToJson(JsonSerializationStyle.Minimized);
            string systemTextJson = JsonSerializer.Serialize(model, _jsonOptions);

            Assert.Equal(systemTextJson, generatedJson);
        }

        [Fact]
        public void SimpleModel_WithSpecialCharacters_ToJson_MatchesSystemTextJson()
        {
            SimpleModel model = new()
            {
                Id = 5,
                Name = "Test \"quotes\" and \\backslashes\\",
                IsActive = true
            };

            string generatedJson = model.ToJson(JsonSerializationStyle.Minimized);
            string systemTextJson = JsonSerializer.Serialize(model, _jsonOptions);

            Assert.Equal(systemTextJson, generatedJson);
        }

        [Fact]
        public void ArrayModel_WithIntArray_ToJson_MatchesSystemTextJson()
        {
            ArrayModel model = new()
            {
                Numbers = new[] { 1, 2, 3, 4, 5 },
                Names = new[] { "Alice", "Bob", "Charlie" }
            };

            string generatedJson = model.ToJson(JsonSerializationStyle.Minimized);
            string systemTextJson = JsonSerializer.Serialize(model, _jsonOptions);

            Assert.Equal(systemTextJson, generatedJson);
        }

        [Fact]
        public void ListModel_WithList_ToJson_MatchesSystemTextJson()
        {
            ListModel model = new()
            {
                Numbers = new List<int> { 10, 20, 30 },
                Names = new List<string> { "One", "Two", "Three" }
            };

            string generatedJson = model.ToJson(JsonSerializationStyle.Minimized);
            string systemTextJson = JsonSerializer.Serialize(model, _jsonOptions);

            Assert.Equal(systemTextJson, generatedJson);
        }

        [Fact]
        public void EmptyCollectionModel_ToJson_MatchesSystemTextJson()
        {
            EmptyCollectionModel model = new()
            {
                EmptyArray = Array.Empty<int>(),
                EmptyList = new List<string>()
            };

            string generatedJson = model.ToJson(JsonSerializationStyle.Minimized);
            string systemTextJson = JsonSerializer.Serialize(model, _jsonOptions);

            Assert.Equal(systemTextJson, generatedJson);
        }

        [Fact]
        public void CollectionWithNullsModel_WithNullCollection_ToJson_MatchesSystemTextJson()
        {
            CollectionWithNullsModel model = new()
            {
                NullableArray = null,
                NullableList = null
            };

            string generatedJson = model.ToJson(JsonSerializationStyle.Minimized);
            string systemTextJson = JsonSerializer.Serialize(model, _jsonOptions);

            Assert.Equal(systemTextJson, generatedJson);
        }

        [Fact]
        public void CollectionWithNullsModel_WithNullElements_ToJson_MatchesSystemTextJson()
        {
            CollectionWithNullsModel model = new()
            {
                NullableArray = new[] { 1, 2, 3 },
                NullableList = new List<string?> { "Hello", null, "World" }
            };

            string generatedJson = model.ToJson(JsonSerializationStyle.Minimized);
            string systemTextJson = JsonSerializer.Serialize(model, _jsonOptions);

            Assert.Equal(systemTextJson, generatedJson);
        }

        [Fact]
        public void NestedCollectionModel_ToJson_MatchesSystemTextJson()
        {
            NestedCollectionModel model = new()
            {
                Id = 100,
                Items = new List<SimpleModel>
                {
                    new() { Id = 1, Name = "First", IsActive = true },
                    new() { Id = 2, Name = "Second", IsActive = false },
                    new() { Id = 3, Name = "Third", IsActive = true }
                }
            };

            string generatedJson = model.ToJson(JsonSerializationStyle.Minimized);
            string systemTextJson = JsonSerializer.Serialize(model, _jsonOptions);

            Assert.Equal(systemTextJson, generatedJson);
        }

        [Fact]
        public void ArrayModel_WithEmptyArrays_ToJson_MatchesSystemTextJson()
        {
            ArrayModel model = new()
            {
                Numbers = Array.Empty<int>(),
                Names = Array.Empty<string>()
            };

            string generatedJson = model.ToJson(JsonSerializationStyle.Minimized);
            string systemTextJson = JsonSerializer.Serialize(model, _jsonOptions);

            Assert.Equal(systemTextJson, generatedJson);
        }

        // Indented formatting tests

        [Fact]
        public void SimpleModel_ToJsonIndented_MatchesSystemTextJson()
        {
            SimpleModel model = new()
            {
                Id = 42,
                Name = "Test",
                IsActive = true
            };

            string generatedJson = model.ToJson(JsonSerializationStyle.Indented);
            string systemTextJson = JsonSerializer.Serialize(model, _indentedJsonOptions);

            Assert.Equal(systemTextJson, generatedJson);
        }

        [Fact]
        public void ComplexModel_ToJsonIndented_MatchesSystemTextJson()
        {
            ComplexModel model = new()
            {
                Id = 999,
                Name = "Complex Test",
                Price = 49.99,
                Available = true,
                Description = "A complex model"
            };

            string generatedJson = model.ToJson(JsonSerializationStyle.Indented);
            string systemTextJson = JsonSerializer.Serialize(model, _indentedJsonOptions);

            Assert.Equal(systemTextJson, generatedJson);
        }

        [Fact]
        public void NestedModel_ToJsonIndented_MatchesSystemTextJson()
        {
            NestedModel model = new()
            {
                Id = 1,
                NestedObject = new SimpleModel
                {
                    Id = 2,
                    Name = "Nested",
                    IsActive = true
                }
            };

            string generatedJson = model.ToJson(JsonSerializationStyle.Indented);
            string systemTextJson = JsonSerializer.Serialize(model, _indentedJsonOptions);

            Assert.Equal(systemTextJson, generatedJson);
        }

        [Fact]
        public void ArrayModel_ToJsonIndented_MatchesSystemTextJson()
        {
            ArrayModel model = new()
            {
                Numbers = new[] { 1, 2, 3, 4, 5 },
                Names = new[] { "Alice", "Bob", "Charlie" }
            };

            string generatedJson = model.ToJson(JsonSerializationStyle.Indented);
            string systemTextJson = JsonSerializer.Serialize(model, _indentedJsonOptions);

            Assert.Equal(systemTextJson, generatedJson);
        }

        [Fact]
        public void NestedCollectionModel_ToJsonIndented_MatchesSystemTextJson()
        {
            NestedCollectionModel model = new()
            {
                Id = 100,
                Items = new List<SimpleModel>
                {
                    new() { Id = 1, Name = "First", IsActive = true },
                    new() { Id = 2, Name = "Second", IsActive = false }
                }
            };

            string generatedJson = model.ToJson(JsonSerializationStyle.Indented);
            string systemTextJson = JsonSerializer.Serialize(model, _indentedJsonOptions);

            Assert.Equal(systemTextJson, generatedJson);
        }

        [Fact]
        public void NullableModel_WithNulls_ToJsonIndented_MatchesSystemTextJson()
        {
            NullableModel model = new()
            {
                NullableString = null,
                NullableInt = null,
                NullableBool = null
            };

            string generatedJson = model.ToJson(JsonSerializationStyle.Indented);
            string systemTextJson = JsonSerializer.Serialize(model, _indentedJsonOptions);

            Assert.Equal(systemTextJson, generatedJson);
        }

        // Circular reference detection tests

        [Fact]
        public void CircularParent_WithCircularChild_ThrowsJsonException()
        {
            CircularParent parent = new()
            {
                Id = 1,
                Name = "Parent"
            };

            CircularChild child = new()
            {
                Id = 2,
                Name = "Child",
                Parent = parent
            };

            parent.Child = child;

            Assert.Throws<System.Text.Json.JsonException>(() => parent.ToJson());
        }

        [Fact]
        public void CircularChild_WithCircularParent_ThrowsJsonException()
        {
            CircularParent parent = new()
            {
                Id = 1,
                Name = "Parent"
            };

            CircularChild child = new()
            {
                Id = 2,
                Name = "Child",
                Parent = parent
            };

            parent.Child = child;

            Assert.Throws<System.Text.Json.JsonException>(() => child.ToJson());
        }

        [Fact]
        public void SelfReferencingModel_WithCircularReference_ThrowsJsonException()
        {
            SelfReferencingModel model = new()
            {
                Id = 1,
                Name = "Node1"
            };

            // Create circular reference to itself
            model.Next = model;

            Assert.Throws<System.Text.Json.JsonException>(() => model.ToJson());
        }

        [Fact]
        public void SelfReferencingModel_WithChainedCircularReference_ThrowsJsonException()
        {
            SelfReferencingModel node1 = new()
            {
                Id = 1,
                Name = "Node1"
            };

            SelfReferencingModel node2 = new()
            {
                Id = 2,
                Name = "Node2"
            };

            SelfReferencingModel node3 = new()
            {
                Id = 3,
                Name = "Node3"
            };

            // Create a chain that loops back
            node1.Next = node2;
            node2.Next = node3;
            node3.Next = node1; // Circular!

            Assert.Throws<System.Text.Json.JsonException>(() => node1.ToJson());
        }

        [Fact]
        public void SelfReferencingModel_WithoutCircularReference_Serializes()
        {
            SelfReferencingModel node1 = new()
            {
                Id = 1,
                Name = "Node1"
            };

            SelfReferencingModel node2 = new()
            {
                Id = 2,
                Name = "Node2"
            };

            SelfReferencingModel node3 = new()
            {
                Id = 3,
                Name = "Node3"
            };

            // Create a chain without circular reference
            node1.Next = node2;
            node2.Next = node3;
            // node3.Next is null

            // Should not throw
            string json = node1.ToJson(JsonSerializationStyle.Minimized);
            Assert.NotNull(json);
            Assert.Contains("\"Id\":1", json);
            Assert.Contains("\"Id\":2", json);
            Assert.Contains("\"Id\":3", json);
        }

        [Fact]
        public void CircularParent_WithoutCircularReference_Serializes()
        {
            CircularParent parent = new()
            {
                Id = 1,
                Name = "Parent",
                Child = new CircularChild
                {
                    Id = 2,
                    Name = "Child",
                    Parent = null // No circular reference
                }
            };

            // Should not throw
            string json = parent.ToJson(JsonSerializationStyle.Minimized);
            Assert.NotNull(json);
            Assert.Contains("\"Id\":1", json);
            Assert.Contains("\"Id\":2", json);
        }
    }
}
