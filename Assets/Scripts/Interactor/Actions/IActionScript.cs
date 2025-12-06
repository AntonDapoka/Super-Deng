
public interface IActionScript
{
    void Initialize();

    void SetDefinition(object definition);

    void Execute();

    void Cancel(); //, ScenarioContext context
}