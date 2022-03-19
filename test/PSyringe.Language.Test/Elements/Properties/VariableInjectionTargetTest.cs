using System.Linq;
using System.Management.Automation.Language;
using FluentAssertions;
using PSyringe.Common.Providers;
using PSyringe.Common.Test.Scripts;
using PSyringe.Language.Attributes;
using PSyringe.Language.Elements.Properties;
using PSyringe.Language.Test.Parsing.Utils;
using Xunit;

namespace PSyringe.Language.Test.Elements.Properties;

public class VariableInjectionTargetTest {
  [Fact]
  public void HasDefaultValue_ReturnsTrue_WhenVariableIsPartOfAssignmentExpression() {
    var sut = MakeInjectionTargetFromAttributedExpressionInScript(ScriptTemplates.WithInjectVariableAssigment_NoTarget);

    sut.HasDefaultValue().Should().BeTrue();
  }

  [Fact]
  public void HasDefaultValue_ReturnsFalse_WhenVariableIsNotPartOfAssignmentExpression() {
    var sut = MakeInjectionTargetFromAttributedExpressionInScript(
      ScriptTemplates.WithInjectVariableExpression_NoTarget
      );

    sut.HasDefaultValue().Should().BeFalse();
  }

  [Fact]
  public void GetVariableTypeConstraint_ReturnsTypeConstraint_WhenVariableHasTypeConstraint() {
    var sut = MakeInjectionTargetFromAttributedExpressionInScript(
      ScriptTemplates.WithInjectVariableExpression_ImplicitTarget
    );

    sut.GetVariableTypeConstraint().Should().Be<ILogger>();
  }

  [Fact]
  public void GetVariableTypeConstraint_ReturnsNull_WhenVariableHasNoTypeConstraint() {
    var sut = MakeInjectionTargetFromAttributedExpressionInScript(
      ScriptTemplates.WithInjectVariableExpression_ExplicitTarget
    );

    sut.GetVariableTypeConstraint().Should().BeNull();
  }

  [Fact]
  public void GetVariableTypeConstraint_ReturnsTypeConstraint_WhenVariableHasMultipleExpressions() {
    var sut = MakeInjectionTargetFromAttributedExpressionInScript(
      "[Inject([ILogger])][LogExpression()][ILogger]$Variable"
    );

    sut.GetVariableTypeConstraint().Should().Be<ILogger>();
  }
  
  [Fact]
  public void GetVariableTypeConstraint_ReturnsTypeConstraint_WhenTypeConstraintComesBeforeAttribute() {
    var sut = MakeInjectionTargetFromAttributedExpressionInScript(
      "[ILogger][Inject([ILogger])][LogExpression()]$Variable"
    );

    sut.GetVariableTypeConstraint().Should().Be<ILogger>();
  }
  
  [Fact]
  public void GetInjectAttributeInstance_ReturnsAttributeInstanceWithExplicitTarget_WhenVariableHasExplicitTarget() {
    var sut = MakeInjectionTargetFromAttributedExpressionInScript(
      ScriptTemplates.WithInjectVariableExpression_ExplicitTarget_Named_Provider_Type
    );

    var injectAttribute = sut.GetInjectAttributeInstance<InjectAttribute>();
    
    injectAttribute.TargetType.Should().Be(typeof(ILogger));
  }

  [Fact]
  public void GetAttributedVariableExpression_ReturnsVariableExpression_WhenCalled() {
    var sut = MakeInjectionTargetFromAttributedExpressionInScript(
      ScriptTemplates.WithInjectVariableExpression_ExplicitTarget_Named_Provider
    );
    
    sut.GetAttributedVariableExpression().Should().BeOfType<VariableExpressionAst>();
  }
  
  private VariableInjectionTarget MakeInjectionTargetFromAttributedExpressionInScript(string script) {
    var attributedVariableExpressionAst = ParsingUtil.GetAttributedExpressionAstFromScript(script);
    return new VariableInjectionTarget(attributedVariableExpressionAst!);
  }
}