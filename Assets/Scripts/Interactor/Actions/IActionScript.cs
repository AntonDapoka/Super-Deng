using UnityEngine;

public interface IActionScript
{
    void Initialize();

    void SetSettings<T>(T settings);

    void Execute();

    void Cancel(); //, ScenarioContext context
}