# ToJson.SourceGen

A C# source generator that automatically creates JSON serialization methods for your classes at compile time.

## Features

- **Zero runtime overhead** - Code is generated at compile time
- **Simple API** - Just add the `[ToJson]` attribute to your class
- **Type-safe** - Generated code is strongly typed
- **Formatting options** - Supports both compact and indented JSON output
- **Collections and arrays** - Supports arrays, `List<T>`, and other `IEnumerable<T>` types
- **Nested object support** - Automatically handles nested objects with `[ToJson]` attribute
- **Proper JSON escaping** - Uses `System.Text.Json.JsonEncodedText` for string encoding
- **Nullable support** - Correctly handles nullable value types and reference types

## Supported Types

- Primitives: `int`, `long`, `short`, `byte`, `uint`, `ulong`, `ushort`, `sbyte`
- Floating point: `float`, `double`, `decimal`
- `bool` - serialized as `true`/`false`
- `string` - with proper JSON character escaping
- Nullable value types: `int?`, `bool?`, etc.
- Nullable reference types: `string?`, etc.
- Arrays: `int[]`, `string[]`, etc.
- Collections: `List<T>`, `IEnumerable<T>`, and other generic collections
- Nested objects with `[ToJson]` attribute
- Collections of nested objects: `List<MyClass>` where `MyClass` has `[ToJson]`

## Installation

### From NuGet (when published)

```bash
dotnet add package ToJson.SourceGen
```

### From Local Build

1. Build the project:
   ```bash
   dotnet pack
   ```

2. Reference the generator in your project:
   ```xml
   <ItemGroup>
     <ProjectReference Include="..\ToJson.SourceGen\ToJson.SourceGen.csproj"
                       OutputItemType="Analyzer"
                       ReferenceOutputAssembly="false" />
   </ItemGroup>
   ```

## Usage

### Basic Example

**Important:** You must add `using ToJson;` at the top of your file to use the `[ToJson]` attribute.

```csharp
using ToJson;  // Required!

[ToJson]
public partial class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
}

// Usage
var person = new Person
{
    Id = 1,
    Name = "John Doe",
    IsActive = true
};

string json = person.ToJson();
// Output: {"Id":1,"Name":"John Doe","IsActive":true}
```

### Nested Objects

```csharp
[ToJson]
public partial class Address
{
    public string Street { get; set; }
    public string City { get; set; }
}

[ToJson]
public partial class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Address? HomeAddress { get; set; }
}

// Usage
var employee = new Employee
{
    Id = 42,
    Name = "Jane Smith",
    HomeAddress = new Address
    {
        Street = "123 Main St",
        City = "Springfield"
    }
};

string json = employee.ToJson();
// Output: {"Id":42,"Name":"Jane Smith","HomeAddress":{"Street":"123 Main St","City":"Springfield"}}
```

### Nullable Types

```csharp
[ToJson]
public partial class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal? Price { get; set; }
    public string? Description { get; set; }
}

// Usage with null values
var product = new Product
{
    Id = 100,
    Name = "Widget",
    Price = null,
    Description = null
};

string json = product.ToJson();
// Output: {"Id":100,"Name":"Widget","Price":null,"Description":null}
```

### Collections and Arrays

```csharp
[ToJson]
public partial class Team
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int[] Scores { get; set; }
    public List<string> Members { get; set; }
}

// Usage
var team = new Team
{
    Id = 1,
    Name = "Alpha Team",
    Scores = new[] { 100, 95, 87, 92 },
    Members = new List<string> { "Alice", "Bob", "Charlie" }
};

string json = team.ToJson();
// Output: {"Id":1,"Name":"Alpha Team","Scores":[100,95,87,92],"Members":["Alice","Bob","Charlie"]}
```

### Collections of Nested Objects

```csharp
[ToJson]
public partial class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
}

[ToJson]
public partial class Company
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Person> Employees { get; set; }
}

// Usage
var company = new Company
{
    Id = 1,
    Name = "Acme Corp",
    Employees = new List<Person>
    {
        new Person { Id = 1, Name = "Alice" },
        new Person { Id = 2, Name = "Bob" }
    }
};

string json = company.ToJson();
// Output: {"Id":1,"Name":"Acme Corp","Employees":[{"Id":1,"Name":"Alice"},{"Id":2,"Name":"Bob"}]}
```

### Indented Formatting

```csharp
[ToJson]
public partial class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}

var product = new Product
{
    Id = 1,
    Name = "Widget",
    Price = 19.99m
};

// Compact output (default)
string compact = product.ToJson();
// Output: {"Id":1,"Name":"Widget","Price":19.99}

// Indented output
string indented = product.ToJson(true);
/* Output:
{
  "Id": 1,
  "Name": "Widget",
  "Price": 19.99
}
*/
```

## Important Notes

### Classes Must Be Partial

Classes decorated with `[ToJson]` **must** be declared as `partial`:

```csharp
// ✓ Correct
[ToJson]
public partial class MyClass { }

// ✗ Wrong - will not generate code
[ToJson]
public class MyClass { }
```

### Only Public Properties and Fields

The generator only serializes public properties and fields. Private, protected, and internal members are ignored.

### Special Characters

Strings are automatically escaped using `System.Text.Json.JsonEncodedText`:

```csharp
var obj = new MyClass { Text = "Hello \"World\"" };
string json = obj.ToJson();
// Output: {"Text":"Hello \"World\""}
```

### No Circular References

The generator does not handle circular references. Attempting to serialize objects with circular references will cause a stack overflow.

## Requirements

- .NET Standard 2.0 or later for the generator
- C# 9.0 or later for consuming projects (for init-only properties and records support)

## Comparison with System.Text.Json

The generated `ToJson()` method produces output identical to `System.Text.Json.JsonSerializer.Serialize()` with default options:

```csharp
using System.Text.Json;

var model = new MyModel { Id = 1, Name = "Test" };

string generated = model.ToJson();
string systemTextJson = JsonSerializer.Serialize(model);

// These produce identical output
Assert.Equal(systemTextJson, generated);
```

## Performance

Since code is generated at compile time, there is:
- **No reflection overhead** at runtime
- **No dynamic code generation** at runtime
- **Minimal memory allocations** - uses `StringBuilder` for efficient string building

## Limitations

- No custom naming policies (always uses property names as-is)
- No circular reference detection
- No polymorphism support
- No support for dictionaries or non-generic collections


## Contributing

This is a learning project. Feel free to fork and experiment!
