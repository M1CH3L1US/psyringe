using System.Management.Automation.Language;
using System.Text;
using PSyringe.Common.Language.Elements;
using PSyringe.Common.Language.Parsing;
using PSyringe.Common.Providers;
using PSyringe.Language.Attributes;

namespace PSyringe.Language.Parsing;

public class ScriptParser : IScriptParser {
  internal readonly IElementFactory ElementFactory;

  public ScriptParser(IElementFactory elementFactory) {
    ElementFactory = elementFactory;
  }

  public IScriptDefinition Parse(string script) {
    var visitor = new ScriptParserVisitor();
    return Parse(script, visitor);
  }

  public IScriptDefinition Parse(string script, IScriptParserVisitor visitor) {
    var scriptBlockAst = PrepareAndParseScript(script);
    visitor.Visit(scriptBlockAst);

    var scriptElement = ElementFactory.CreateScript(scriptBlockAst);

    AddAllFunctionDefinitionElementsToScript(scriptElement, visitor.FunctionDefinitions);
    AddAllVariableExpressionElementsToScript(scriptElement, visitor.AttributedVariableExpressions);

    return scriptElement;
  }

  private void AddAllFunctionDefinitionElementsToScript(
    IScriptDefinition scriptElement,
    IEnumerable<IAttributedScriptElement<FunctionDefinitionAst>> functionDefinitionAsts
  ) {
    foreach (var functionAst in functionDefinitionAsts) {
      var functionDefinitionElement =
        ElementFactory.CreateElement(functionAst);
      scriptElement.AddElement(functionDefinitionElement);
    }
  }

  private void AddAllVariableExpressionElementsToScript(
    IScriptDefinition scriptElement,
    IEnumerable<IAttributedScriptElement<AttributedExpressionAst>> variableExpressionAsts
  ) {
    foreach (var variableAst in variableExpressionAsts) {
      var variableExpressionElement =
        ElementFactory.CreateElement(variableAst);
      scriptElement.AddElement(variableExpressionElement);
    }
  }

  private ScriptBlockAst PrepareAndParseScript(string script) {
    PrependAssemblyReference(ref script);
    var scriptAst = Parser.ParseInput(script, out _, out _);
    return scriptAst;
  }

  internal static void PrependAssemblyReference(ref string script) {
    var sb = new StringBuilder();
    var attributeNamespace = GetAttributeAssemblyNamespace<InjectAttribute>();
    var genericProviderNamespace = GetAttributeAssemblyNamespace<ILogger>();

    sb.AppendLine($"using namespace {attributeNamespace};");
    sb.AppendLine($"using namespace {genericProviderNamespace};");
    sb.AppendLine(script);

    script = sb.ToString();
  }

  private static string GetAttributeAssemblyNamespace<T>() {
    var type = typeof(T);
    return type.Namespace!;
  }
}