using UnityEngine;

public class FaultTargetPointScript : MonoBehaviour
{
    private GameObject target;

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    public GameObject GetTarget()
    {
        return this.target;
    }
}
