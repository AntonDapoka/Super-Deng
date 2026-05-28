using System;
using System.Collections;
using UnityEngine;

public class StartCountDownPresenterScript : MonoBehaviour
{
    [SerializeField] private StartCountDownViewScript startCountDownView;
    public event Action OnCountDownFinished;
    
    public void StartStartCountDown(float beatInterval, int countingLength)
    {
        StartCoroutine(CountDownRoutine(beatInterval, countingLength));
    }

    private IEnumerator CountDownRoutine(float delay, int countingLength)
    {
        startCountDownView.TurnOn();

        for (int i = countingLength; i > 0; i--)
        {
            startCountDownView.DisplayValue(i.ToString());
            yield return new WaitForSeconds(delay);
        }
        OnCountDownFinished?.Invoke();
        startCountDownView.DisplayValue("GO!");
        yield return new WaitForSeconds(delay);
        startCountDownView.TurnOff();
    }
}
