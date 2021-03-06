using System.Management.Automation.Language;

namespace PSyringe.Language.Compiler;

public static class CompilerAstExtensions {
  /// <summary>
  ///   Checks all children of a given AST node using the provided predicate.
  ///   Replaces nodes that match the predicate with the provided replacement.
  ///   Does not replace the node, if the replacement returns null.
  /// </summary>
  /// <param name="ast"></param>
  /// <param name="predicate"></param>
  /// <param name="replacementFunc"></param>
  /// <typeparam name="T"></typeparam>
  /// <typeparam name="TR"></typeparam>
  /// <returns>The updated AST</returns>
  public static TR ReplaceAst<T, TR>(this TR ast, Func<object?, bool> predicate, Func<object, object?> replacementFunc)
    where T : Ast where TR : Ast {
    var visitor = new ReplaceAstVisitor(predicate, replacementFunc);
    return (TR) ast.Visit(visitor);
  }

  internal class ReplaceAstVisitor : ICustomAstVisitor2 {
    private readonly Func<object?, bool> _predicate;
    private readonly Func<object, object?> _replacementFunc;

    public ReplaceAstVisitor(Func<object?, bool> predicate, Func<object, object?> replacementFunc) {
      _predicate = predicate;
      _replacementFunc = replacementFunc;
    }

    public object? VisitAttribute(AttributeAst attributeAst) {
      var positionalArguments = VisitElements(attributeAst.PositionalArguments);
      var namedArguments = VisitElements(attributeAst.NamedArguments);

      return new AttributeAst(attributeAst.Extent, attributeAst.TypeName, positionalArguments, namedArguments);
    }

    public object? VisitCommand(CommandAst commandAst) {
      var commandElements = VisitElements(commandAst.CommandElements);
      var redirections = VisitElements(commandAst.Redirections);

      return new CommandAst(commandAst.Extent, commandElements, commandAst.InvocationOperator, redirections);
    }

    public object? VisitPipeline(PipelineAst pipelineAst) {
      var pipelineElements = VisitElements(pipelineAst.PipelineElements);

      return new PipelineAst(pipelineAst.Extent, pipelineElements);
    }

    public object? VisitTrap(TrapStatementAst trapStatementAst) {
      var body = VisitElement(trapStatementAst.Body);

      return new TrapStatementAst(trapStatementAst.Extent, trapStatementAst.TrapType, body);
    }

    public object? VisitHashtable(HashtableAst hashtableAst) {
      return hashtableAst;
    }

    public object? VisitParameter(ParameterAst parameterAst) {
      var attributes = VisitElements(parameterAst.Attributes);

      // return new ParameterAst(parameterAst.Extent, parameterAst.Name, parameterAst.ParameterType, attributes);
      return null;
    }

    public object? VisitNamedBlock(NamedBlockAst namedBlockAst) {
      var newTraps = VisitElements(namedBlockAst.Traps);
      var newStatements = VisitStatements(namedBlockAst.Statements);
      var statementBlock = new StatementBlockAst(namedBlockAst.Extent, newStatements, newTraps);
      return new NamedBlockAst(namedBlockAst.Extent, namedBlockAst.BlockKind, statementBlock, namedBlockAst.Unnamed);
    }

    public object? VisitParamBlock(ParamBlockAst paramBlockAst) {
      return null;
    }

    public object? VisitFunctionDefinition(FunctionDefinitionAst functionDefinitionAst) {
      return null;
    }

    public object? VisitCatchClause(CatchClauseAst catchClauseAst) {
      return null;
    }

    public object? VisitIfStatement(IfStatementAst ifStmtAst) {
      return null;
    }

    public object? VisitScriptBlock(ScriptBlockAst scriptBlockAst) {
      var newParamBlock = VisitElement(scriptBlockAst.ParamBlock);
      var newBeginBlock = VisitElement(scriptBlockAst.BeginBlock);
      var newProcessBlock = VisitElement(scriptBlockAst.ProcessBlock);
      var newEndBlock = VisitElement(scriptBlockAst.EndBlock);
      var newDynamicParamBlock = VisitElement(scriptBlockAst.DynamicParamBlock);

      return new ScriptBlockAst(
        scriptBlockAst.Extent,
        newParamBlock,
        newBeginBlock,
        newProcessBlock,
        newEndBlock,
        newDynamicParamBlock
      );
    }

    public object? VisitArrayLiteral(ArrayLiteralAst arrayLiteralAst) {
      return null;
    }

    public object? VisitForStatement(ForStatementAst forStatementAst) {
      return null;
    }

    public object? VisitTryStatement(TryStatementAst tryStatementAst) {
      return null;
    }

    public object? VisitBreakStatement(BreakStatementAst breakStatementAst) {
      return null;
    }

    public object? VisitDataStatement(DataStatementAst dataStatementAst) {
      return null;
    }

    public object? VisitExitStatement(ExitStatementAst exitStatementAst) {
      return null;
    }

    public object? VisitSubExpression(SubExpressionAst subExpressionAst) {
      return null;
    }

    public object? VisitPipelineChain(PipelineChainAst statementChainAst) {
      return null;
    }

    public object? VisitBlockStatement(BlockStatementAst blockStatementAst) {
      return null;
    }

    public object? VisitErrorStatement(ErrorStatementAst errorStatementAst) {
      return null;
    }

    public object? VisitStatementBlock(StatementBlockAst statementBlockAst) {
      return null;
    }

    public object? VisitThrowStatement(ThrowStatementAst throwStatementAst) {
      return null;
    }

    public object? VisitTypeConstraint(TypeConstraintAst typeConstraintAst) {
      return null;
    }

    public object? VisitTypeExpression(TypeExpressionAst typeExpressionAst) {
      return null;
    }

    public object? DefaultVisit(Ast ast) {
      return null;
    }

    public object? VisitArrayExpression(ArrayExpressionAst arrayExpressionAst) {
      return null;
    }

    public object? VisitAssignmentStatement(AssignmentStatementAst assignmentStatementAst) {
      var newLeft = VisitElement(assignmentStatementAst.Left);
      var newRight = VisitElement(assignmentStatementAst.Right);

      return new AssignmentStatementAst(
        assignmentStatementAst.Extent,
        newLeft,
        assignmentStatementAst.Operator,
        newRight,
        assignmentStatementAst.ErrorPosition
      );
    }

    public object? VisitWhileStatement(WhileStatementAst whileStatementAst) {
      return null;
    }

    public object? VisitFunctionMember(FunctionMemberAst functionMemberAst) {
      return null;
    }

    public object? VisitPropertyMember(PropertyMemberAst propertyMemberAst) {
      return null;
    }

    public object? VisitTypeDefinition(TypeDefinitionAst typeDefinitionAst) {
      return null;
    }

    public object? VisitAttributedExpression(AttributedExpressionAst attributedExpressionAst) {
      var newAttribute = VisitElement(attributedExpressionAst.Attribute)!;
      var newChild = VisitElement(attributedExpressionAst.Child);

      return new AttributedExpressionAst(
        attributedExpressionAst.Extent,
        newAttribute,
        newChild
      );
    }

    public object? VisitUsingStatement(UsingStatementAst usingStatement) {
      return null;
    }

    public object? VisitBinaryExpression(BinaryExpressionAst binaryExpressionAst) {
      return null;
    }

    public object? VisitErrorExpression(ErrorExpressionAst errorExpressionAst) {
      return null;
    }

    public object? VisitFileRedirection(FileRedirectionAst fileRedirectionAst) {
      return null;
    }

    public object? VisitIndexExpression(IndexExpressionAst indexExpressionAst) {
      return null;
    }

    public object? VisitParenExpression(ParenExpressionAst parenExpressionAst) {
      return null;
    }

    public object? VisitReturnStatement(ReturnStatementAst returnStatementAst) {
      return null;
    }

    public object? VisitSwitchStatement(SwitchStatementAst switchStatementAst) {
      return null;
    }

    public object? VisitUnaryExpression(UnaryExpressionAst unaryExpressionAst) {
      return null;
    }

    public object? VisitUsingExpression(UsingExpressionAst usingExpressionAst) {
      return null;
    }

    public object? VisitCommandParameter(CommandParameterAst commandParameterAst) {
      return null;
    }

    public object? VisitMemberExpression(MemberExpressionAst memberExpressionAst) {
      return null;
    }

    public object? VisitCommandExpression(CommandExpressionAst commandExpressionAst) {
      return null;
    }

    public object? VisitContinueStatement(ContinueStatementAst continueStatementAst) {
      return null;
    }

    public object? VisitConvertExpression(ConvertExpressionAst convertExpressionAst) {
      return null;
    }

    public object? VisitConfigurationDefinition(ConfigurationDefinitionAst configurationDefinitionAst) {
      return null;
    }

    public object? VisitTernaryExpression(TernaryExpressionAst ternaryExpressionAst) {
      return null;
    }

    public object? VisitConstantExpression(ConstantExpressionAst constantExpressionAst) {
      return null;
    }

    public object? VisitMergingRedirection(MergingRedirectionAst mergingRedirectionAst) {
      return null;
    }

    public object? VisitVariableExpression(VariableExpressionAst variableExpressionAst) {
      return null;
    }

    public object? VisitDoUntilStatement(DoUntilStatementAst doUntilStatementAst) {
      return null;
    }

    public object? VisitDoWhileStatement(DoWhileStatementAst doWhileStatementAst) {
      return null;
    }

    public object? VisitDynamicKeywordStatement(DynamicKeywordStatementAst dynamicKeywordAst) {
      return null;
    }

    public object? VisitForEachStatement(ForEachStatementAst forEachStatementAst) {
      return null;
    }

    public object? VisitScriptBlockExpression(ScriptBlockExpressionAst scriptBlockExpressionAst) {
      return null;
    }

    public object? VisitInvokeMemberExpression(InvokeMemberExpressionAst invokeMemberExpressionAst) {
      return null;
    }

    public object? VisitNamedAttributeArgument(NamedAttributeArgumentAst namedAttributeArgumentAst) {
      return null;
    }

    public object? VisitStringConstantExpression(StringConstantExpressionAst stringConstantExpressionAst) {
      return null;
    }

    public object? VisitExpandableStringExpression(ExpandableStringExpressionAst expandableStringExpressionAst) {
      return null;
    }

    public object? VisitBaseCtorInvokeMemberExpression(
      BaseCtorInvokeMemberExpressionAst baseCtorInvokeMemberExpressionAst) {
      return null;
    }

    private IEnumerable<StatementAst> VisitStatements(IEnumerable<StatementAst> statements) {
      var newStatements = new List<StatementAst>();

      foreach (var statement in statements) {
        newStatements.Add(VisitElement(statement)!);
      }

      return newStatements;
    }

    private T? VisitElement<T>(T? element) where T : Ast {
      if (ReplaceAstIfMatches(element, out var newElement)) {
        return newElement ?? element!.Copy() as T;
      }

      return element?.Visit(this) as T ?? (element?.Copy() as T)!;
    }

    private IEnumerable<T?>? VisitElements<T>(IEnumerable<T>? elements) where T : Ast {
      List<T?> newElements = new();

      if (elements is null) {
        return null;
      }

      foreach (var element in elements) {
        newElements.Add(VisitElement(element));
      }

      return newElements;
    }

    private bool ReplaceAstIfMatches<T>(T? ast, out T? replacementAst) where T : class {
      replacementAst = null;

      if (_predicate.Invoke(ast)) {
        replacementAst = (T?) _replacementFunc.Invoke(ast);
        return true;
      }

      return false;
    }
  }
}