using System.Management.Automation.Language;
using PSyringe.Language.TypeLoader;

namespace PSyringe.Language.AstTransformation.CodeGenerationAstExtensions;

public static class TypeExpressionAstExtensions {
  private static readonly ExtensionMethodFinder _extensionMethodFinder = new(nameof(ToStringFromAst));

  public static string ToStringFromAst(this TypeExpressionAst ast) {
    return _extensionMethodFinder.InvokeExtensionMethodInAssemblyForConcreteType<string>(ast.TypeName);
  }

  public static string ToStringFromAst(this ITypeName type) {
    return _extensionMethodFinder.InvokeExtensionMethodInAssemblyForConcreteType<string>(type);
  }

  public static string ToStringFromAst(this TypeName type) {
    return FullTypeNameAsString(type);
  }

  public static string ToStringFromAst(this ReflectionTypeName type) {
    return FullTypeNameAsString(type);
  }

  /// <summary>
  ///   The FullName of the type already includes the generics
  ///   e.g. System.Collections.Generic.List would be
  ///   System.Collections.Generic.List[System.String]
  /// </summary>
  public static string ToStringFromAst(this GenericTypeName type) {
    return FullTypeNameAsString(type);
  }

  private static string FullTypeNameAsString(this ITypeName type) {
    return $"[{type.FullName}]";
  }

  public static string ToStringFromAst(this ArrayTypeName type) {
    var brackets = string.Concat(Enumerable.Repeat("[]", type.Rank));
    var typeName = type.ElementType.ToStringFromAst();
    // The GetString method will return the type name with the brackets
    // which we need to remove to add the array brackets.
    var typeNameWithoutEndBrackets = typeName.TrimEnd(']');

    return $"{typeNameWithoutEndBrackets}{brackets}]";
  }
}