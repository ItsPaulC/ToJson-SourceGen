# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

ToJson-SourceGen is a C# source generator project that creates a ToJson attribute. This is a learning/test project created with Claude.

The solution uses the modern `.slnx` format (XML-based solution file introduced in Visual Studio 2022 17.10).

## Build and Development Commands

Since this is a source generator project, typical commands will include:

**Building:**
```bash
dotnet build
```

**Testing:**
```bash
dotnet test
```

**Running a single test:**
```bash
dotnet test --filter "FullyQualifiedName~TestNamespace.TestClass.TestMethod"
```

**Packaging (for source generators):**
```bash
dotnet pack
```

## Code Style and Conventions

The project follows strict .NET coding standards defined in `.editorconfig`:

- **Indentation:** 4 spaces
- **Bracing:** Allman style (braces on new lines)
- **Naming conventions:**
  - Private/internal fields: `_camelCase` prefix
  - Static fields: `s_camelCase` prefix
  - Constants: `PascalCase`
- **var usage:** Prefer explicit types; use `var` only when type is apparent
- **Expression-bodied members:** Preferred for all member types
- **Null checking:** Use modern C# patterns (null-conditional operators, throw expressions)
- **Pattern matching:** Preferred over is/as with type checks

## Architecture Notes

**Source Generator Structure:**
When implementing the source generator, follow the standard pattern:
- Generator class implements `IIncrementalGenerator`
- Use incremental generation pipeline for performance
- Attribute definition should be emitted into the consuming assembly
- Generate `ToJson()` methods for types decorated with the attribute

**Testing Source Generators:**
- Unit tests should verify generated source code
- Integration tests should compile and execute generated code
- Use `Microsoft.CodeAnalysis.CSharp.Testing` for generator testing

## Important Considerations

- Source generators run during compilation, so performance is critical
- Generated code must handle various type scenarios (primitives, collections, nested objects, nullability)
- Consider JSON serialization edge cases (circular references, infinite recursion)
- Follow Roslyn source generator best practices for diagnostics and error reporting
