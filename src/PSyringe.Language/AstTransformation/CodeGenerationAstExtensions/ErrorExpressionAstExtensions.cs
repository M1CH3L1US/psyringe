using System.Management.Automation;
using System.Management.Automation.Language;

namespace PSyringe.Language.AstTransformation.CodeGenerationAstExtensions;

public static class ErrorExpressionAstExtensions {
  public static string ToStringFromAst(this ErrorExpressionAst ast) {
    throw new PSInvalidOperationException {
      Source = ast.Extent.Text
    };
  }
}