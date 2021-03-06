using System.Management.Automation.Language;
using PSyringe.Common.Compiler;
using PSyringe.Common.Language.Elements;

namespace PSyringe.Language.Elements;

public class OnErrorCallbackElement : ScriptElement {
  public OnErrorCallbackElement(Ast ast) : base(ast) {
  }

  public OnErrorCallbackElement(Ast ast, AttributeAst attribute) : base(ast, attribute) {
  }


  public override void TransformAst(IScriptTransformer transformer) {
    throw new NotImplementedException();
  }
}