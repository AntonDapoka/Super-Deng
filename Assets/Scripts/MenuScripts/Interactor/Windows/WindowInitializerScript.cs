using UnityEngine;

public class WindowInitializerScript : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private WindowDragControllerScript[] windowControllers;

    private void Start()
    {
        foreach (var controller in windowControllers)
        { 
            controller.SetCanvas(canvas);
        }
    }
}
