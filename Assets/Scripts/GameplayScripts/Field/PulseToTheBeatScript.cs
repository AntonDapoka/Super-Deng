using UnityEngine;

public class PulseToTheBeatScript : MonoBehaviour
{ 
    public bool isTurnOn = false;
    [SerializeField] private float pulseSize = 1.5f;
    [SerializeField] private float returnSpeed = 10f;
    private Vector3 startSize;

    private void Start()
    {
        startSize = transform.localScale;
    }
    
    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, startSize, Time.deltaTime * returnSpeed);
    }

    public void Pulse()
    {
        if (isTurnOn) transform.localScale = startSize * pulseSize;
    }
}
