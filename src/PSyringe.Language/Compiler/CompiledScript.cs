using System.Management.Automation.Language;
using PSyringe.Common.Language.Compiler;
using PSyringe.Common.Language.Elements;

namespace PSyringe.Language.Compiler;

public class CompiledScript : ICompiledScript {
  public ScriptBlockAst ScriptBlock { get; }
  public IScriptDefinition ScriptDefinition { init; get; }

  public string GetScriptCode() {
    throw new NotImplementedException();
  }

  public IList<IScriptVariableDependency> Dependencies { get; } = new List<IScriptVariableDependency>();
}