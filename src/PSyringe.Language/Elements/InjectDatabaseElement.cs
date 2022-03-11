using System.Management.Automation.Language;
using PSyringe.Common.Language.Parsing.Elements;

namespace PSyringe.Language.Elements; 

public class InjectDatabaseElement : IInjectDatabaseElement {
  public Ast Ast { get; }

  public InjectDatabaseElement(AttributedExpressionAst injectDatabaseAst) {
    Ast = injectDatabaseAst;
  }

}