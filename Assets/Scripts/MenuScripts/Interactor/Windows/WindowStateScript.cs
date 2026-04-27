using UnityEngine;

public class WindowStateScript : MonoBehaviour
{
    private bool isMaximized = true;

    public bool GetIsMaximized() => isMaximized;
    public void SetIsMaximized(bool newIsMaximized) => isMaximized = newIsMaximized;
    
}
