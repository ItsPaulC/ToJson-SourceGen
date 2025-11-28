using System.Text.Json;
using Xunit;

namespace ToJson.SourceGen.Tests
{
    public class ToJsonGeneratorTests
    {
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = null
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

            string generatedJson = model.ToJson();
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

            string generatedJson = model.ToJson();
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

            string generatedJson = model.ToJson();
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

            string generatedJson = model.ToJson();
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

            string generatedJson = model.ToJson();
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

            string generatedJson = model.ToJson();
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

            string generatedJson = model.ToJson();
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

            string generatedJson = model.ToJson();
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

            string generatedJson = model.ToJson();
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

            string generatedJson = model.ToJson();
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

            string generatedJson = model.ToJson();
            string systemTextJson = JsonSerializer.Serialize(model, _jsonOptions);

            Assert.Equal(systemTextJson, generatedJson);
        }
    }
}
