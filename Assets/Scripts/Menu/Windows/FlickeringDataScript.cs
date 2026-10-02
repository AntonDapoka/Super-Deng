using UnityEngine;

public struct FlickeringDataScript
{
    public Component СomponentRenderer { get; } 
    public float Duration { get; } 
    public Color InitialColor { get; } 
    public Color TargetColor { get; } 
    public bool IsTurningOn { get; } 
    public bool IsBlinking { get; } 

    public FlickeringDataScript(Component component , float duration, Color initialColor, Color targetColor, bool isTurningOn, bool isBlinking)
    {
        СomponentRenderer = component;
        Duration = duration;
        InitialColor = initialColor;
        TargetColor = targetColor;
        IsTurningOn = isTurningOn;
        IsBlinking = isBlinking;
    }
}
