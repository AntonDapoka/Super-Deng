using UnityEngine;

public interface IActionScript
{
    void Initialize();

    void SetSettings<T>(T settings) where T : Component;

    void Execute();

    void Cancel(); //, ScenarioContext context
}