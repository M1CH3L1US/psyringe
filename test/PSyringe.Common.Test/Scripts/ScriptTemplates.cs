namespace PSyringe.Common.Test.Scripts;

public class ScriptTemplates {
  public const string StartupFunctionName = "Startup";
  public const string InjectionSiteFunctionName = "InjectionSite";
  public const string CallbackFunctionName = "Callback";
  public const string InjectVariableName = "Variable";
  public const string TemplateName = "Template";
  public const string ProviderName = "Provider";
  public const string DatabaseProviderName = "DatabaseProvider";
  public const string DatabaseConnectionString = "ConnectionString";

  public const string EmptyScript = "";

  public const string InvalidScript = "InvalidScript | Foo";

  public const string WithStartupFunction = @$"
function {StartupFunctionName} {{
  [Startup()]
  param() 
}}";

  public const string WithInjectVariableExpression_NoTarget = @$"
[Inject()]${InjectVariableName};";

  public const string WithInjectVariableExpression_ExplicitTarget = @$"
[Inject()]${InjectVariableName};";

  public const string WithInjectVariableExpression_ImplicitTarget = @$"
[Inject()][ILogger]${InjectVariableName};";

  public const string WithInjectVariableAssigment_NoTarget = @$"
[Inject()]${InjectVariableName} = 'value'";

  public const string WithInjectVariableExpression_ExplicitTarget_Named_Provider = @$"
[Inject(Target = 'LoggerProvider')]${InjectVariableName} = 'value'";

  public const string WithInjectVariableExpression_ExplicitTarget_Named_Provider_Type = @$"
[Inject(Target = [ILogger])]${InjectVariableName} = 'value'";

  public const string WithInjectVariableExpression_ExplicitTarget_Named_Provider_Optional = @$"
[Inject(Target = 'LoggerProvider', Optional = $true)]${InjectVariableName} = 'value'";


  public const string WithInjectVariableExpression_ExplicitTarget_Positional_Provider = @$"
[Inject('LoggerProvider')]${InjectVariableName} = 'value'";

  public const string WithInjectVariableExpression_ExplicitTarget_Positional_Provider_Optional = @$"
[Inject('LoggerProvider', $true)]${InjectVariableName} = 'value'";

  public const string WithInjectVariableExpression_ExplicitTarget_Mixed_Provider_Optional = @$"
[Inject(Target = 'LoggerProvider', $true)]${InjectVariableName} = 'value'";

  public const string WithInjectVariableAssigment_ExplicitTarget_Type = @$"
[Inject(Target = [ILogger])]${InjectVariableName} = 'value'";

  public const string WithInjectVariableAssigment_ImplicitTarget = @$"
[Inject()][ILogger]${InjectVariableName} = 'value'";

  public const string WithInjectCredentialVariable_NoTarget = @$"
[InjectSecret()]${InjectVariableName};";

  public const string WithInjectCredentialVariable_NamedParameter = @$"
[InjectSecret(Target = 'Credential')]${InjectVariableName};";

  public const string WithInjectCredentialVariable_IndexParameter = @$"
[InjectSecret('Credential')]${InjectVariableName};";

  public const string WithInjectDatabaseVariable_NoTarget = @$"
[InjectDatabase()]${InjectVariableName};";

  public const string WithInjectDatabaseVariable_ConnectionString = @$"
[InjectDatabase(ConnectionString = '{DatabaseConnectionString}')]${InjectVariableName};";

  public const string WithInjectDatabaseVariable_IndexProviderToken = @$"
[InjectDatabase('{DatabaseProviderName}')]${InjectVariableName};";

  public const string WithInjectDatabaseVariable_ProviderToken = @$"
[InjectDatabase(Target = '{DatabaseProviderName}')]${InjectVariableName};";

  public const string WithInjectConstantVariable_NoTarget = @$"
[InjectConstant()]${InjectVariableName};";

  public const string WithInjectParameterFunction_NamedParameter = @$"
function {InjectionSiteFunctionName} {{
  [InjectionSite()]
  param(
    [InjectParameter(Target = 'Name')]$Parameter 
  ) 
}}";

  public const string WithInjectParameterFunction_IndexParameter = @$"
function {InjectionSiteFunctionName} {{
  [InjectionSite()]
  param(
    [InjectParameter('Name')]$Parameter 
  ) 
}}";

  public const string WithInjectParameterFunction_NoTarget = @$"
function {InjectionSiteFunctionName} {{
  [InjectionSite()]
  param(
    [InjectParameter()]$Parameter 
  ) 
}}";

  public const string WithInjectParameterFunction_NoInjectionSite = @$"
function {InjectionSiteFunctionName} {{
  param(
    [InjectParameter()]$Parameter 
  ) 
}}";

  public const string WithInjectionSiteFunction = @$"
function {InjectionSiteFunctionName} {{
  [InjectionSite()]
  param()
}}";

  public const string WithInjectionSiteFunction_NamedScope = @$"
function {InjectionSiteFunctionName} {{
  [InjectionSite(Scope = 'Scope')]
  param()
}}";

  public const string WithInjectionSiteFunction_IndexedScope = @$"
function {InjectionSiteFunctionName} {{
  [InjectionSite('Scope')]
  param()
}}";

  public const string WithInjectParameter_NoTarget = @$"
function {InjectionSiteFunctionName} {{
  [InjectionSite('Scope')]
  param(
    [Inject()]$Parameter 
  )
}}";

  public const string WithInjectParameter_ImplicitTarget = @$"
function {InjectionSiteFunctionName} {{
  [InjectionSite('Scope')]
  param(
    [Inject()][ILogger]$Parameter 
  )
}}";

  public const string WithInjectParameter_ExplicitTarget = @$"
function {InjectionSiteFunctionName} {{
  [InjectionSite('Scope')]
  param(
    [Inject(Target = 'LoggerProvider')]$Parameter 
  )
}}";

  public const string WithInjectParameter_DefaultValue = @$"
function {InjectionSiteFunctionName} {{
  [InjectionSite('Scope')]
  param(
    [Inject()]$Parameter = 'value'
  )
}}";

  public const string WithInjectTemplateAttribute_NamedTarget = @$"
[InjectTemplate('{TemplateName}')]{{}}
";

  public const string WithInjectTemplateAttribute_NoTarget = @"
[InjectTemplate()]{}
";

  public const string WithProvideExpressionAttribute_ExplicitTarget = $@"
  function {TemplateName} {{
    [Provide(Target = '{ProviderName}')]
    param()
  }};
";

  public const string WithProvideExpressionAttribute_ImplicitTarget = $@"
  function {TemplateName} {{
    [Provide()]
    param()
  }};
";

  public const string WithProvideExpressionAttribute_NoTarget = @"
  [Provide()]{
  };
";

  public const string WithOnErrorFunction = @$"
function {CallbackFunctionName} {{
  [OnError()]
  param()
}}";

  public const string WithOnLoadedFunction = @$"
function {CallbackFunctionName} {{
  [OnLoaded()]
  param()
}}";

  public const string WithBeforeUnloadFunction = @$"
function {CallbackFunctionName} {{
  [BeforeUnload()]
  param()
}}";

  public const string WithAllFeatures = @$"
[Inject()][ILogger]${InjectVariableName};
[Inject([ILogger])]${InjectVariableName};
[InjectCredential()]${InjectVariableName};
[InjectTemplate()]{{}}

function {InjectionSiteFunctionName} {{
  [InjectionSite()]
  param(
    [InjectParameter()]$Parameter 
  )
}}
";
}