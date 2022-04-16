using System.Management.Automation.Language;
using PSyringe.Language.Compiler;

namespace PSyringe.Language.AstTransformation;

public static class ExpressionAstExtensions {
  public static string GetAstAsString(this BinaryExpressionAst ast) {
    var left = ast.Left.GetAstAsString();
    var right = ast.Right.GetAstAsString();
    var @operator = ast.Operator.Text();

    return $"{left} {@operator} {right}";
  }

  public static string GetAstAsString(this ConstantExpressionAst ast) {
    var value = ast.Value;

    // The value is only a boolean when an attribute
    // has an implicit parameter e.g. [Parameter(ValueFromPipeline)]
    // in which case the value is true.
    if (ast.StaticType == typeof(bool)) {
      return CompilerScriptText.TrueVariable;
    }

    return value.ToString();
  }

  public static string GetAstAsString(this TernaryExpressionAst ast) {
    var conditionExpression = ast.Condition.GetAstAsString();
    var ifTrue = ast.IfTrue.GetAstAsString();
    var ifFalse = ast.IfFalse.GetAstAsString();

    return $"{conditionExpression} ? {ifTrue} : {ifFalse}";
  }

  public static string GetAstAsString(this VariableExpressionAst ast) {
    var variableName = ast.VariablePath.UserPath;
    var variableSing = ast.Splatted ? '@' : '$';

    return $"{variableSing}{variableName}";
  }

  public static string GetAstAsString(this StringConstantExpressionAst ast) {
    return QuoteStringExpression(ast.Value, ast.StringConstantType);
  }

  /// <summary>
  ///   The expression is evaluated at runtime which means we
  ///   can process this the same as a string constant.
  /// </summary>
  public static string GetAstAsString(this ExpandableStringExpressionAst ast) {
    return QuoteStringExpression(ast.Value, ast.StringConstantType);
  }

  private static string QuoteStringExpression(string value, StringConstantType type) {
    return type switch {
      StringConstantType.BareWord => value,
      StringConstantType.DoubleQuoted => DoubleQuote(value),
      StringConstantType.DoubleQuotedHereString => $"@{DoubleQuote(value)}@",
      StringConstantType.SingleQuoted => SingleQuote(value),
      StringConstantType.SingleQuotedHereString => $"@{SingleQuote(value)}@",
      _ => ""
    };
  }

  private static string SingleQuote(object value) {
    return $"'{value}'";
  }

  private static string DoubleQuote(object value) {
    return $"\"{value}\"";
  }

  /// <summary>
  ///   An array literal would usually be represented as
  ///   an expression without @() but, for the sake of
  ///   simplicity, we don't differentiate them from
  ///   `ArrayExpressionAst`.
  /// </summary>
  public static string GetAstAsString(this ArrayLiteralAst ast) {
    return MakeArrayExpression(ast.Elements);
  }

  public static string GetAstAsString(this ArrayExpressionAst ast) {
    return MakeArrayExpression(ast.SubExpression.Statements);
  }

  private static string MakeArrayExpression(IEnumerable<Ast> elements) {
    var elementsAsString = elements.Select(e => e.GetAstAsString()).ToList();
    var elementsAsStringJoined = string.Join(", ", elementsAsString);

    return $"@({elementsAsStringJoined})";
  }

  public static string GetAstAsString(this UsingExpressionAst ast) {
    // TODO: ${ using:SomeVar } might brake this
    var variable = (VariableExpressionAst) ast.SubExpression;
    var variableName = variable.VariablePath.UserPath;

    return $"$using:{variableName}";
  }

  public static string GetAstAsString(this IndexExpressionAst ast) {
    var nullConditional = ast.NullConditional ? "?" : "";
    var target = ast.Target.GetAstAsString();
    var index = ast.Index.GetAstAsString();

    return $"{target}{nullConditional}[{index}]";
  }

  public static string GetAstAsString(this TypeExpressionAst ast) {
    return ast.TypeName.InvokeExtensionMethodInAssemblyForConcreteType<string>(nameof(GetAstAsString));
  }

  public static string GetAstAsString(this ITypeName type) {
    return type.InvokeExtensionMethodInAssemblyForConcreteType<string>(nameof(GetAstAsString));
  }

  public static string GetAstAsString(this TypeName type) {
    return FullTypeNameAsString(type);
  }

  public static string GetAstAsString(this ReflectionTypeName type) {
    return FullTypeNameAsString(type);
  }

  /// <summary>
  ///   The FullName of the type already includes the generics
  ///   e.g. System.Collections.Generic.List would be
  ///   System.Collections.Generic.List[System.String]
  /// </summary>
  public static string GetAstAsString(this GenericTypeName type) {
    return FullTypeNameAsString(type);
  }

  private static string FullTypeNameAsString(this ITypeName type) {
    return $"[{type.FullName}]";
  }

  public static string GetAstAsString(this ArrayTypeName type) {
    var brackets = string.Concat(Enumerable.Repeat("[]", type.Rank));
    var typeName = type.ElementType.GetAstAsString();
    // The GetString method will return the type name with the brackets
    // which we need to remove to add the array brackets.
    var typeNameWithoutEndBrackets = typeName.TrimEnd(']');

    return $"{typeNameWithoutEndBrackets}{brackets}]";
  }

  public static string GetAstAsString(this UnaryExpressionAst ast) {
    var token = ast.TokenKind;
    var shouldBeAfterExpression = token.HasTrait(TokenFlags.PrefixOrPostfixOperator);
    var childExpression = ast.Child.GetAstAsString();

    if (shouldBeAfterExpression) {
      return $"{childExpression}{token.Text()}";
    }

    return $"{token.Text()}{childExpression}";
  }

  public static string GetAstAsString(this ErrorExpressionAst ast) {
    return string.Empty;
  }
}