using FluentAssertions;
using PSyringe.Language.AstTransformation.CodeGenerationAstExtensions;
using PSyringe.Language.TypeLoader;
using Xunit;
using static PSyringe.Language.Test.AstTransformation.CodeGenerationAstExtensions.Utils.MakeAstUtils;

namespace PSyringe.Language.Test.TypeLoader;

public class ExtensionMethodFinderTest {
  [Fact]
  public void GetExtensionMethodOverloadDerivedAstType_ReturnsCorrectMethodOverload_ForExpressionAst() {
    var methodName = nameof(VariableExpressionAstExtensions.ToStringFromAst);
    var sut = new ExtensionMethodFinder(methodName);
    var expected = typeof(VariableExpressionAstExtensions).GetMethod(methodName);
    var actual = sut.GetExtensionMethodOverloadForConcreteType(Var("Foo"));

    actual.Should().BeSameAs(expected);
  }
}