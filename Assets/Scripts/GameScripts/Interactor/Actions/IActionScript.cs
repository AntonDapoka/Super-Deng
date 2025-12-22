

public interface IActionScript
{
    ActionType Type { get; }

    void Initialize();

    void SetSettings<T>(T settings);

    void Execute();

    void Cancel();

    void TurnOn();

    void TurnOff();
}