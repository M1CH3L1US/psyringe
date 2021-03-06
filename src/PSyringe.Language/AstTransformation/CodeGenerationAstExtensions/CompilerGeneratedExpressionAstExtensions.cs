using System.Text;
using PSyringe.Language.AstTransformation.SyntheticAsts;
using static PSyringe.Language.AstTransformation.CodeGenConstants;

namespace PSyringe.Language.AstTransformation.CodeGenerationAstExtensions;

public static class CompilerGeneratedExpressionAstExtensions {
  public static string ToStringFromAst(this SyntheticExpressionAst ast) {
    var generatedExpression = ast.Expression.ToStringFromAst();

    var expression = new StringBuilder();
    expression.Append(InlineExpression);
    expression.Append(' ');
    expression.Append(generatedExpression);

    return expression.ToString();
  }
}