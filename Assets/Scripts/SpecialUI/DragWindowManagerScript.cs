using UnityEngine;

public class DragWindowManagerScript : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private DragWindowScript[] windows;

    private void Start()
    {
        foreach (var window in windows)
        { 
            window.SetCanvas(canvas);
        }
    }
}
