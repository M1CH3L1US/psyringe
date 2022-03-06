using System.Management.Automation.Language;
using PSyringe.Common.Language.Parsing.Elements;

namespace PSyringe.Language.Elements;

public class InjectionSiteParameterElement : IInjectionSiteParameter {
  public InjectionSiteParameterElement(ParameterAst ast) {
    Ast = ast;
  }

  public Ast Ast { get; }
}