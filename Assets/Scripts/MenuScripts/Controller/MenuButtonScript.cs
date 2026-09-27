using UnityEngine;
using UnityEngine.UI;

public class MenuButtonScript : MonoBehaviour
{
    [SerializeField] private Button button;
    private Command command;

    public void Initialize(Command command)
    {
        this.command = command;
    }

    private void Start()
    {
        button.onClick.AddListener(Execute);
    }

    private void Execute()
    {
        command.Execute();
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(Execute);
    }
}