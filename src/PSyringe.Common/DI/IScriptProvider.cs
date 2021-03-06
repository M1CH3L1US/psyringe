namespace PSyringe.Common.DI;

public interface IScriptProvider {
  public bool Optional { init; get; }
  public string? Name { init; get; }
  public string? Scope { get; }
  public Type? Type { init; get; }

  public T GetProvider<T>(IDIContainer container);

  public string GetProviderName();

  public void SetScope(string scope);
}