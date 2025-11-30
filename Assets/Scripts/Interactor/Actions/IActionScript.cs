
public interface IActionScript
{
    IRhythmableScript Rhythmable { get; }

    void Initialize();

    void Execute(object definition);

    void Cancel(object definition); //, ScenarioContext context
}