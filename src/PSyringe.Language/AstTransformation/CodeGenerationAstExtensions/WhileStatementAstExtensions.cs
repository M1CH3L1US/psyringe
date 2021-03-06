using System.Management.Automation.Language;

namespace PSyringe.Language.AstTransformation.CodeGenerationAstExtensions;

public static class WhileStatementAstExtensions {
  public static string ToStringFromAst(this WhileStatementAst ast) {
    var condition = ast.Condition.ToStringFromAst();
    var body = ast.Body.ToStringFromAst();
    var label = ast.GetLabel();

    return $"{label}while ({condition}) {body}";
  }
}