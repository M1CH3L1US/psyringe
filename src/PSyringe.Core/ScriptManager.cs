using System.Management.Automation;
using System.Management.Automation.Runspaces;
using PSyringe.Common.Runtime;

namespace PSyringe.Core;

/// <summary>
///   Load runner dependencies at the top of the script
///   Parse Script
///   - Find all Inject variable nodes
///   - Find the script entrypoint function
///   | Find all parameters to inject script arguments into
///   | Find all parameters to inject DI providers into
///   Validate all required providers exist [Take optional into account]
///   Replace injection targets in the script AST
///   Load the PowerShell process
///   - Add internal dependencies
///   - Load global injection target variables
///   - Load script templates
///   - Load the new Script AST
///   - Add Command that calls the entrypoint method with injection paramters
///   Invoke the script
/// </summary>
/*Run scopes

// Is only included when RunScope is POST
[RunScope(Type = "POST")]{ 
}

// Flag is set
[RunScope(Flag = "Some Flag")]{ 
}
*/
public class ScriptManager {
  public ScriptManager(
    IScriptLoader loader,
    IScriptParser parser,
    IScriptRepository repository,
    IScriptEnvironment globalEnvironment
  ) {
  }

  public IReadOnlyCollection<T> GetScripts<T>() {
    return default;
  }

  public T GetInvokableScript<T>() {
    return default;
  }

  public void DisableScript<T>(T script) {
  }

  private void InitializeRunspace(Runspace rs) {
    IScriptInvocationContext context = null;

    var state = rs.SessionStateProxy;
    state.SetVariable("PS_INVOCATION_CONTEXT", context);
  }

  private void InitializePowerShell(PowerShell ps) {
    ps.AddScript("$ErrorActionPreference = 'Stop'");
    ps.AddScript(
      "function Get-TimeElapsedSinceInvocation { return $PS_INVOCATION_CONTEXT.GetTimeElapsedSinceInvocation(); }");
    ps.AddScript(
      "Trap { foreach($OnErrorFunction in $PS_ON_ERROR_FUNCTIONS) { $OnErrorFunction.Invoke(@{ Error = $Error }) } }");
  }

  public interface IScriptRepository {
    IReadOnlyCollection<T> GetScripts<T>();
    void DisableScript<T>(T script);
  }

  public interface IScriptParser {
  }

  public interface IScriptLoader {
    public void Load(string script, IScriptEnvironment environment);
    public void LoadAll(IList<string> scripts);
  }
}

public interface IScriptEnvironment {
  public IDictionary<string, object> Constants { get; }
}