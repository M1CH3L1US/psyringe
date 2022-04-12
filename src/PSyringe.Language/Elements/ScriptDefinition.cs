using System.Management.Automation.Language;
using PSyringe.Common.Language.Elements;

namespace PSyringe.Language.Elements;

public class ScriptDefinition : IScriptDefinition {
  private readonly List<ScriptElement> _elements = new();

  public ScriptDefinition(ScriptBlockAst scriptBlock) {
    ScriptBlock = scriptBlock;
  }

  public ScriptBlockAst ScriptBlock { get; }
  public IEnumerable<ScriptElement> Elements => _elements;

  public void AddElement(ScriptElement element) {
    _elements.Add(element);
  }
}