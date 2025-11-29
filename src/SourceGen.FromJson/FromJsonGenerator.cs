using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace SourceGen.FromJson
{
    [Generator]
    public class FromJsonGenerator : IIncrementalGenerator
    {
        private const string FromJsonAttributeFullName = "FromJson.FromJsonAttribute";

        private const string AttributeSourceCode = @"
namespace FromJson
{
    [System.AttributeUsage(System.AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class FromJsonAttribute : System.Attribute
    {
    }
}";

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(ctx =>
                ctx.AddSource("FromJsonAttribute.g.cs", SourceText.From(AttributeSourceCode, Encoding.UTF8)));

            IncrementalValuesProvider<ClassDeclarationSyntax> classDeclarations = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: static (s, _) => IsSyntaxTargetForGeneration(s),
                    transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx))
                .Where(static m => m is not null)!;

            IncrementalValueProvider<(Compilation, ImmutableArray<ClassDeclarationSyntax>)> compilationAndClasses
                = context.CompilationProvider.Combine(classDeclarations.Collect());

            context.RegisterSourceOutput(compilationAndClasses,
                static (spc, source) => Execute(source.Item1, source.Item2, spc));
        }

        private static bool IsSyntaxTargetForGeneration(SyntaxNode node)
        {
            return node is ClassDeclarationSyntax classDeclaration
                && classDeclaration.AttributeLists.Count > 0;
        }

        private static ClassDeclarationSyntax? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
        {
            ClassDeclarationSyntax classDeclaration = (ClassDeclarationSyntax)context.Node;

            if (classDeclaration == null || context.SemanticModel == null)
            {
                return null;
            }

            foreach (AttributeListSyntax attributeList in classDeclaration.AttributeLists)
            {
                if (attributeList == null)
                {
                    continue;
                }

                foreach (AttributeSyntax attribute in attributeList.Attributes)
                {
                    if (attribute == null)
                    {
                        continue;
                    }

                    IMethodSymbol? attributeSymbol = context.SemanticModel.GetSymbolInfo(attribute).Symbol as IMethodSymbol;
                    if (attributeSymbol == null)
                    {
                        continue;
                    }

                    INamedTypeSymbol? attributeContainingTypeSymbol = attributeSymbol.ContainingType;
                    if (attributeContainingTypeSymbol == null)
                    {
                        continue;
                    }

                    string fullName = attributeContainingTypeSymbol.ToDisplayString();

                    if (fullName == FromJsonAttributeFullName)
                    {
                        return classDeclaration;
                    }
                }
            }

            return null;
        }

        private static void Execute(Compilation compilation, ImmutableArray<ClassDeclarationSyntax> classes, SourceProductionContext context)
        {
            if (compilation == null)
            {
                return;
            }

            if (classes.IsDefaultOrEmpty)
            {
                return;
            }

            foreach (ClassDeclarationSyntax classDeclaration in classes)
            {
                if (classDeclaration == null || classDeclaration.SyntaxTree == null)
                {
                    continue;
                }

                SemanticModel? semanticModel = compilation.GetSemanticModel(classDeclaration.SyntaxTree);
                if (semanticModel == null)
                {
                    continue;
                }

                INamedTypeSymbol? classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration) as INamedTypeSymbol;

                if (classSymbol == null)
                {
                    continue;
                }

                string namespaceName = classSymbol.ContainingNamespace?.ToDisplayString() ?? "global";
                string className = classSymbol.Name;

                if (string.IsNullOrEmpty(className))
                {
                    continue;
                }

                string source = GenerateFromJsonMethod(classSymbol, namespaceName, className);

                context.AddSource($"{className}.FromJson.g.cs", SourceText.From(source, Encoding.UTF8));
            }
        }

        private static string GenerateFromJsonMethod(INamedTypeSymbol classSymbol, string namespaceName, string className)
        {
            if (classSymbol == null)
            {
                throw new System.ArgumentNullException(nameof(classSymbol));
            }

            if (string.IsNullOrEmpty(namespaceName))
            {
                namespaceName = "global";
            }

            if (string.IsNullOrEmpty(className))
            {
                throw new System.ArgumentNullException(nameof(className));
            }

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("// <auto-generated/>");
            sb.AppendLine("#nullable enable");
            sb.AppendLine();
            sb.AppendLine($"namespace {namespaceName}");
            sb.AppendLine("{");
            sb.AppendLine($"    partial class {className}");
            sb.AppendLine("    {");
            sb.AppendLine("        [System.Runtime.CompilerServices.CompilerGenerated]");
            sb.AppendLine($"        public static {className} FromJson(string json)");
            sb.AppendLine("        {");
            sb.AppendLine("            if (json == null)");
            sb.AppendLine("            {");
            sb.AppendLine("                throw new System.ArgumentNullException(nameof(json));");
            sb.AppendLine("            }");
            sb.AppendLine();
            sb.AppendLine("            using var document = System.Text.Json.JsonDocument.Parse(json);");
            sb.AppendLine("            var root = document.RootElement;");
            sb.AppendLine();
            sb.AppendLine($"            var obj = new {className}();");
            sb.AppendLine();

            // Get all public instance properties and fields
            System.Collections.Generic.List<ISymbol> members = new System.Collections.Generic.List<ISymbol>();
            foreach (ISymbol member in classSymbol.GetMembers())
            {
                if ((member.Kind == SymbolKind.Property || member.Kind == SymbolKind.Field)
                    && !member.IsStatic
                    && member.DeclaredAccessibility == Accessibility.Public)
                {
                    members.Add(member);
                }
            }

            // Generate parsing logic for each member
            foreach (ISymbol member in members)
            {
                string memberName = member.Name;
                ITypeSymbol? memberType = member switch
                {
                    IPropertySymbol prop => prop.Type,
                    IFieldSymbol field => field.Type,
                    _ => null
                };

                if (memberType == null)
                {
                    continue;
                }

                sb.AppendLine($"            if (root.TryGetProperty(\"{memberName}\", out var {memberName}Element))");
                sb.AppendLine("            {");
                sb.AppendLine(GeneratePropertyAssignment(memberName, memberType, $"{memberName}Element"));
                sb.AppendLine("            }");
                sb.AppendLine();
            }

            sb.AppendLine("            return obj;");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private static string GeneratePropertyAssignment(string memberName, ITypeSymbol memberType, string jsonElementVar)
        {
            if (memberType == null)
            {
                throw new System.ArgumentNullException(nameof(memberType));
            }

            StringBuilder sb = new StringBuilder();

            // Handle nullable value types
            if (memberType is INamedTypeSymbol { IsValueType: true, NullableAnnotation: NullableAnnotation.Annotated } namedTypeSymbol)
            {
                ITypeSymbol underlyingType = namedTypeSymbol.TypeArguments.FirstOrDefault() ?? memberType;
                sb.AppendLine($"                if ({jsonElementVar}.ValueKind != System.Text.Json.JsonValueKind.Null)");
                sb.AppendLine("                {");
                sb.AppendLine($"                    obj.{memberName} = {GenerateValueExtraction(underlyingType, jsonElementVar)};");
                sb.AppendLine("                }");
                return sb.ToString();
            }

            // Handle reference types (including nullable reference types)
            bool isNullable = memberType.IsReferenceType || memberType.NullableAnnotation == NullableAnnotation.Annotated;
            if (isNullable)
            {
                sb.AppendLine($"                if ({jsonElementVar}.ValueKind != System.Text.Json.JsonValueKind.Null)");
                sb.AppendLine("                {");
                sb.AppendLine($"                    obj.{memberName} = {GenerateValueExtraction(memberType, jsonElementVar)};");
                sb.AppendLine("                }");
            }
            else
            {
                sb.AppendLine($"                obj.{memberName} = {GenerateValueExtraction(memberType, jsonElementVar)};");
            }

            return sb.ToString();
        }

        private static string GenerateValueExtraction(ITypeSymbol typeSymbol, string jsonElementVar)
        {
            if (typeSymbol == null)
            {
                throw new System.ArgumentNullException(nameof(typeSymbol));
            }

            // Handle arrays
            if (typeSymbol is IArrayTypeSymbol arrayType)
            {
                return GenerateArrayExtraction(arrayType.ElementType, jsonElementVar);
            }

            // Handle collections (List<T>, IEnumerable<T>, etc.)
            if (TryGetEnumerableElementType(typeSymbol, out ITypeSymbol? elementType))
            {
                return GenerateListExtraction(elementType!, jsonElementVar);
            }

            switch (typeSymbol.SpecialType)
            {
                case SpecialType.System_String:
                    // For nullable strings, don't add the null-forgiving operator
                    if (typeSymbol.NullableAnnotation == NullableAnnotation.Annotated)
                    {
                        return $"{jsonElementVar}.GetString()";
                    }
                    return $"{jsonElementVar}.GetString()!";

                case SpecialType.System_Int32:
                    return $"{jsonElementVar}.GetInt32()";

                case SpecialType.System_Int64:
                    return $"{jsonElementVar}.GetInt64()";

                case SpecialType.System_Int16:
                    return $"{jsonElementVar}.GetInt16()";

                case SpecialType.System_Byte:
                    return $"{jsonElementVar}.GetByte()";

                case SpecialType.System_UInt32:
                    return $"{jsonElementVar}.GetUInt32()";

                case SpecialType.System_UInt64:
                    return $"{jsonElementVar}.GetUInt64()";

                case SpecialType.System_UInt16:
                    return $"{jsonElementVar}.GetUInt16()";

                case SpecialType.System_SByte:
                    return $"{jsonElementVar}.GetSByte()";

                case SpecialType.System_Single:
                    return $"{jsonElementVar}.GetSingle()";

                case SpecialType.System_Double:
                    return $"{jsonElementVar}.GetDouble()";

                case SpecialType.System_Decimal:
                    return $"{jsonElementVar}.GetDecimal()";

                case SpecialType.System_Boolean:
                    return $"{jsonElementVar}.GetBoolean()";

                default:
                    // For complex types, check if they have the FromJson attribute
                    if (typeSymbol is INamedTypeSymbol namedType)
                    {
                        bool hasFromJsonAttribute = namedType.GetAttributes()
                            .Any(attr => attr.AttributeClass?.ToDisplayString() == FromJsonAttributeFullName);

                        if (hasFromJsonAttribute)
                        {
                            string typeName = typeSymbol.ToDisplayString();
                            // Remove trailing '?' for nullable reference types
                            if (typeName.EndsWith("?"))
                            {
                                typeName = typeName.Substring(0, typeName.Length - 1);
                            }
                            return $"{typeName}.FromJson({jsonElementVar}.GetRawText())";
                        }
                    }

                    // Fallback: try to deserialize using System.Text.Json
                    string fallbackTypeName = typeSymbol.ToDisplayString();
                    // Remove trailing '?' for nullable reference types
                    if (fallbackTypeName.EndsWith("?"))
                    {
                        fallbackTypeName = fallbackTypeName.Substring(0, fallbackTypeName.Length - 1);
                    }
                    return $"System.Text.Json.JsonSerializer.Deserialize<{fallbackTypeName}>({jsonElementVar}.GetRawText())!";
            }
        }

        private static string GenerateArrayExtraction(ITypeSymbol elementType, string jsonElementVar)
        {
            string tempList = $"tempList_{System.Guid.NewGuid():N}";
            string item = $"item_{System.Guid.NewGuid():N}";

            StringBuilder sb = new StringBuilder();
            sb.Append($"(({jsonElementVar}.ValueKind == System.Text.Json.JsonValueKind.Array) ? ");
            sb.Append($"System.Linq.Enumerable.ToArray(");
            sb.Append($"System.Linq.Enumerable.Select({jsonElementVar}.EnumerateArray(), ");
            sb.Append($"{item} => {GenerateValueExtraction(elementType, item)}");
            sb.Append($")) : System.Array.Empty<{elementType.ToDisplayString()}>())");

            return sb.ToString();
        }

        private static string GenerateListExtraction(ITypeSymbol elementType, string jsonElementVar)
        {
            string item = $"item_{System.Guid.NewGuid():N}";

            StringBuilder sb = new StringBuilder();
            sb.Append($"(({jsonElementVar}.ValueKind == System.Text.Json.JsonValueKind.Array) ? ");
            sb.Append($"System.Linq.Enumerable.ToList(");
            sb.Append($"System.Linq.Enumerable.Select({jsonElementVar}.EnumerateArray(), ");
            sb.Append($"{item} => {GenerateValueExtraction(elementType, item)}");
            sb.Append($")) : new System.Collections.Generic.List<{elementType.ToDisplayString()}>())");

            return sb.ToString();
        }

        private static bool TryGetEnumerableElementType(ITypeSymbol typeSymbol, out ITypeSymbol? elementType)
        {
            elementType = null;

            if (typeSymbol.SpecialType == SpecialType.System_String)
            {
                return false;
            }

            if (typeSymbol is INamedTypeSymbol namedType)
            {
                if (namedType.OriginalDefinition.SpecialType == SpecialType.System_Collections_Generic_IEnumerable_T)
                {
                    elementType = namedType.TypeArguments.FirstOrDefault();
                    return elementType != null;
                }

                foreach (INamedTypeSymbol interfaceType in namedType.AllInterfaces)
                {
                    if (interfaceType.OriginalDefinition.SpecialType == SpecialType.System_Collections_Generic_IEnumerable_T)
                    {
                        elementType = interfaceType.TypeArguments.FirstOrDefault();
                        return elementType != null;
                    }
                }
            }

            return false;
        }
    }
}
