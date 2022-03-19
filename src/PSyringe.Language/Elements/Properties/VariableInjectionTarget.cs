using System.Management.Automation.Language;
using PSyringe.Common.Language.Attributes;
using PSyringe.Common.Language.Parsing.Elements.Properties;
using PSyringe.Language.Extensions;
using PSyringe.Language.TypeLoader;

namespace PSyringe.Language.Elements.Properties;

public class VariableInjectionTarget : IInjectionTarget {
  public readonly AttributeAst Attribute;
  private readonly AttributedExpressionAst _ast;

  public VariableInjectionTarget(AttributedExpressionAst ast) {
    _ast = ast;
    Attribute = _ast.Attribute as AttributeAst;
  }

  public bool HasDefaultValue() {
    // We assume that the variable has a default value;
    // if it's part of an assigment expression.
    // [Inject()]$var = <value>;
    var isPartOfAssignment = GetVariableAssignmentStatement() is not null;
    return isPartOfAssignment;
  }
  
  private AssignmentStatementAst? GetVariableAssignmentStatement() {
    return _ast.GetAstInTreeAssignableToType<AssignmentStatementAst>();
  }

  public T GetInjectAttributeInstance<T>() where T : Attribute, IInjectionTargetAttribute {
    return AttributeTypeLoader.CreateAttributeInstanceFromAst<T>(Attribute);
  }
  
  public VariableExpressionAst? GetAttributedVariableExpression() {
    return _ast.GetAstInTreeAssignableToType<VariableExpressionAst>();
  }

  public Type? GetVariableTypeConstraint() {
    var typeConstraintExpressionAst = GetTypeConstraintAst();
    return typeConstraintExpressionAst?.GetAttributeType();
  }
  
  private TypeConstraintAst? GetTypeConstraintAst() {
    var convertExpressionAst = _ast.GetAstInTreeAssignableToType<ConvertExpressionAst>();
    return convertExpressionAst?.Type;
  }
}