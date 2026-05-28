using UnityEngine;

public class LevelTimeManagementScript : MonoBehaviour
{
    [SerializeField] private LevelTimePresenterScript timePresenter;
    private bool isTurnOn = false;
    private float timeElapsed;
    private float timeTotal;

    public void InitializeTime(float timeStart, float timeTotal)
    {
        timeElapsed = timeStart;
        this.timeTotal = timeTotal;
        isTurnOn = true;
        timePresenter.InitializeTime(timeElapsed, timeTotal);
    }

    private void Update()
    {
        if (isTurnOn && timeElapsed <= timeTotal)
        {
            timeElapsed += Time.deltaTime;
            timePresenter.UpdateTime(timeElapsed);
        }
    }

    public void TurnOn()
    {
        isTurnOn = true;
    }

    public void TurnOff()
    {
        isTurnOn = false;
    }

    public float GetCurrentTime()
    {
        return timeElapsed;
    }

    public void ResetTime()
    {
        timeElapsed = 0f;
        isTurnOn = true;
        timePresenter.InitializeTime(timeElapsed, timeTotal);
    }
}
