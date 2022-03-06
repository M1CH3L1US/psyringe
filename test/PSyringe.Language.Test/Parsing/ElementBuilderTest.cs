using System;
using System.Linq;
using FluentAssertions;
using PSyringe.Common.Language.Parsing;
using PSyringe.Common.Language.Parsing.Elements;
using PSyringe.Common.Test.Scripts;
using PSyringe.Language.Parsing;
using PSyringe.Language.Test.Parsing.Utils;
using Xunit;

namespace PSyringe.Language.Test.Parsing;

public class ElementFactoryTest {
  [Fact]
  public void Build_ShouldReturnOfInternalScriptElement_WhenCalled() {
    var builder = MakeScriptElementBuilder(ScriptTemplates.EmptyScript, out var visitor);

    var script = builder.Build();

    script.Should().BeAssignableTo<IScriptElement>();
  }

  [Fact]
  public void AddInjectionSite_AddsInjectionSiteElementToScript_WhenCalled() {
    var builder = MakeScriptElementBuilder(ScriptTemplates.WithInjectionSiteFunction, out var visitor);

    var siteAst = visitor.InjectionSites.First();
    builder.AddInjectionSite(siteAst);
    var script = builder.Build();

    var injectionSite = script.InjectionSites.First();

    injectionSite.Should().BeAssignableTo<IInjectionSiteElement>();
  }


  [Fact]
  public void AddInjectionSiteParameter_CreatesAndAddsParameterToInjectionSite_WhenCalled() {
    var builder = MakeScriptElementBuilder(ScriptTemplates.WithInjectParameterFunction_NoTarget, out var visitor);

    var siteAst = visitor.InjectionSites.First();
    builder.AddInjectionSite(siteAst);
    var siteParameters = visitor.FunctionParameters[siteAst];
    builder.AddParameterToInjectionSite(siteAst, siteParameters.First());
    var script = builder.Build();

    var injectionSite = script.InjectionSites.First();
    var parameter = injectionSite.Parameters.First();

    parameter.Should().BeAssignableTo<IInjectionSiteParameter>();
  }

  [Fact]
  public void SetStartupFunction_SetsStartupElementInScript_WhenCalled() {
    var builder = MakeScriptElementBuilder(ScriptTemplates.WithStartupFunction, out var visitor);

    var startupFunctionAst = visitor.InjectionSites.First();
    builder.SetStartupFunction(startupFunctionAst);
    var script = builder.Build();

    script.StartupFunction.Should().NotBeNull();
  }

  [Fact]
  public void SetStartupFunction_ThrowsError_WhenStartupFunctionIsAlreadySet() {
    var builder = MakeScriptElementBuilder(ScriptTemplates.WithStartupFunction, out var visitor);

    var startupFunctionAst = visitor.InjectionSites.First();
    builder.SetStartupFunction(startupFunctionAst);
    var action = () => builder.SetStartupFunction(startupFunctionAst);

    action.Should().Throw<InvalidOperationException>();
  }

  [Fact]
  public void AddInjectVariable_AddsInjectVariableToScript_WhenCalled() {
    var builder = MakeScriptElementBuilder(ScriptTemplates.WithInjectVariableExpression_NoTarget, out var visitor);

    var injectVariableAst = visitor.InjectExpressions.First();
    builder.AddInjectVariable(injectVariableAst);
    var script = builder.Build();

    var injectVariable = script.InjectVariables.First();
    injectVariable.Should().BeAssignableTo<IInjectVariableElement>();
  }

  [Fact]
  public void AddInjectCredential_AddsInjectCredentialToScript_WhenCalled() {
    var builder = MakeScriptElementBuilder(ScriptTemplates.WithInjectCredentialVariable_NoTarget, out var visitor);

    var injectCredentialAst = visitor.InjectExpressions.First();
    builder.AddInjectCredential(injectCredentialAst);
    var script = builder.Build();

    var injectCredential = script.InjectCredentials.First();
    injectCredential.Should().BeAssignableTo<IInjectCredentialElement>();
  }

  [Fact]
  public void AddInjectTemplate_AddsInjectTemplateToScript_WhenCalled() {
    var builder = MakeScriptElementBuilder(ScriptTemplates.WithInjectTemplateAttribute_NoTarget, out var visitor);

    var templateAst = visitor.InjectExpressions.First();
    builder.AddInjectTemplate(templateAst);
    var script = builder.Build();

    var injectTemplate = script.InjectTemplates.First();
    injectTemplate.Should().BeAssignableTo<IInjectTemplateElement>();
  }

  [Fact]
  public void AddOnError_AddsOnErrorFunctionToScript_WhenCalled() {
    var builder = MakeScriptElementBuilder(ScriptTemplates.WithOnErrorFunction, out var visitor);

    var onErrorAst = visitor.CallbackFunctions.First();
    builder.AddOnError(onErrorAst);
    var script = builder.Build();

    var onErrorElement = script.OnErrorFunctions.First();
    onErrorElement.Should().BeAssignableTo<IOnErrorElement>();
  }

  [Fact]
  public void AddOnLoad_AddsOnLoadFunctionToScript_WhenCalled() {
    var builder = MakeScriptElementBuilder(ScriptTemplates.WithOnLoadedFunction , out var visitor);

    var onLoadAst = visitor.CallbackFunctions.First();
    builder.AddOnLoad(onLoadAst);
    var script = builder.Build();

    var onLoadElement = script.OnLoadFunctions.First();
    onLoadElement.Should().BeAssignableTo<IOnLoadElement>();
  }
  
  [Fact]
  public void AddBeforeUnload_AddsBeforeUnloadFunctionToScript_WhenCalled() {
    var builder = MakeScriptElementBuilder(ScriptTemplates.WithBeforeUnloadFunction , out var visitor);

    var beforeUnloadAst = visitor.CallbackFunctions.First();
    builder.AddBeforeUnload(beforeUnloadAst);
    var script = builder.Build();

    var beforeUnloadElement = script.BeforeUnloadFunctions.First();
    beforeUnloadElement.Should().BeAssignableTo<IBeforeUnloadElement>();
  }

  private ElementBuilder MakeScriptElementBuilder(string script, out IScriptVisitor visitor) {
    var ast = ParsingUtil.ParseScript(script);
    visitor = new ScriptVisitor();
    visitor.Visit(ast);

    return new ElementBuilder(new ElementFactory());
  }
}