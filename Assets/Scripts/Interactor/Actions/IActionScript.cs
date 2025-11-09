
public interface IActionScript
{
    IRhythmableScript Rhythmable { get; }

    void Initialize();

    void Execute();

    void Cancel();
}