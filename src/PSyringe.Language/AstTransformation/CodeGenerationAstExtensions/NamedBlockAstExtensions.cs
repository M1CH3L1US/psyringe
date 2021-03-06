using System.Management.Automation.Language;
using System.Text;
using static PSyringe.Language.AstTransformation.CodeGenConstants;

namespace PSyringe.Language.AstTransformation.CodeGenerationAstExtensions;

public static class NamedBlockAstExtensions {
  public static string ToStringFromAst(this NamedBlockAst ast) {
    var statements = ast.Statements?.ToStringFromAstJoinBy($";{NewLine}");
    var traps = ast.Traps?.ToStringFromAstJoinBy(NewLine);

    var namedBlock = new StringBuilder();

    if (!ast.Unnamed) {
      var name = ast.BlockKind.Text().ToLower();
      namedBlock.Append(name);
      namedBlock.Append(' ');
    }

    namedBlock.AppendLine("{");

    if (statements is not null) {
      if (!statements.EndsWith(';')) {
        statements += ';';
      }

      namedBlock.AppendLine(statements);
    }

    if (traps is not null) {
      namedBlock.AppendLine(traps);
    }

    namedBlock.Append('}');

    return namedBlock.ToString();
  }

  private static bool ReplaceChildCore(this NamedBlockAst ast, StatementAst child, Ast replacement) {
    var idx = ast.Statements.IndexOf(child);

    if (idx == -1) {
      return false;
    }

    var newStatements = ast.Statements.ToList();
    newStatements[idx] = (StatementAst) replacement;

    ast.SetPrivateProperty(nameof(ast.Statements), newStatements.AsReadOnly());
    return true;
  }
}