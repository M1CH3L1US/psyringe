using System.Linq;
using System.Management.Automation.Language;
using FluentAssertions;
using PSyringe.Common.Test.Scripts;
using PSyringe.Language.Parsing;
using PSyringe.Language.Test.Parsing.Utils;
using Xunit;

namespace PSyringe.Language.Test.Parsing;

public class ScriptVisitorTest {
  [Fact]
  public void Preperation_PrependsAssemblyReference_WhenCalled() {
    var sut = MakeVisitorAndVisitScript(ScriptTemplates.EmptyScript);
    var usingStatementName = sut.UsingStatements.Select(e => e.Name.Value).FirstOrDefault();

    usingStatementName.Should().StartWith("PSyringe.Language.Attributes");
  }

  [Fact]
  public void VisitFunctionDefinition_AddsInjectionSite_WhenFunctionIsInjectionSite() {
    var sut = MakeVisitorAndVisitScript(ScriptTemplates.WithInjectionSiteFunction);

    sut.FunctionDefinitions.Should().NotBeEmpty();
  }

  [Fact]
  public void VisitFunctionDefinition_AddsInjectionSite_WhenFunctionIsStartupFunction() {
    var sut = MakeVisitorAndVisitScript(ScriptTemplates.WithStartupFunction);

    sut.FunctionDefinitions.Should().NotBeEmpty();
  }

  [Fact]
  public void VisitFunctionDefinition_AddsCallbackFunction_WhenFunctionIsOnLoaded() {
    var sut = MakeVisitorAndVisitScript(ScriptTemplates.WithOnLoadedFunction);

    sut.FunctionDefinitions.Should().NotBeEmpty();
  }

  [Fact]
  public void VisitFunctionDefinition_AddsCallbackFunction_WhenFunctionIsOnError() {
    var sut = MakeVisitorAndVisitScript(ScriptTemplates.WithOnErrorFunction);

    sut.FunctionDefinitions.Should().NotBeEmpty();
  }

  [Fact]
  public void VisitFunctionDefinition_AddsCallbackFunction_WhenFunctionIsBeforeUnload() {
    var sut = MakeVisitorAndVisitScript(ScriptTemplates.WithBeforeUnloadFunction);

    sut.FunctionDefinitions.Should().NotBeEmpty();
  }


  [Fact]
  public void VisitAttributedExpression_AddsInjectExpression_WhenExpressionIsInjectVariable() {
    var sut = MakeVisitorAndVisitScript(ScriptTemplates.WithInjectVariableExpression_NoTarget);

    sut.AttributedVariableExpressions.Should().NotBeEmpty();
  }

  [Fact]
  public void VisitAttributedExpression_AddsInjectExpression_WhenExpressionIsInjectVariableWithDefault() {
    var sut = MakeVisitorAndVisitScript(ScriptTemplates.WithInjectVariableAssigment_NoTarget);

    sut.AttributedVariableExpressions.Should().NotBeEmpty();
  }

  [Fact]
  public void VisitAttributedExpression_AddsInjectExpression_WhenExpressionIsInjectCredential() {
    var sut = MakeVisitorAndVisitScript(ScriptTemplates.WithInjectCredentialVariable_NoTarget);

    sut.AttributedVariableExpressions.Should().NotBeEmpty();
  }


  private ScriptParserVisitor MakeVisitorAndVisitScript(string script) {
    var visitor = new ScriptParserVisitor();
    var ast = GetAst(script);
    visitor.Visit(ast);
    return visitor;
  }

  private ScriptBlockAst GetAst(string script) {
    return ParsingUtil.ParseScript(script);
  }
}