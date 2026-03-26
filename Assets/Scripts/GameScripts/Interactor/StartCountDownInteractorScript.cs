using UnityEngine;

public class StartCountDownInteractorScript : MonoBehaviour
{
    [SerializeField] private int countingLength = 3;
    [SerializeField] private RhythmManager rhythmManager;
    [SerializeField] private StartCountDownPresenterScript startCountDownPresenter;
    [SerializeField] private ActionInteractorScript actionInteractor;
    [SerializeField] private PlayerMovementInteractorScript playerMovementInteractor;

    private void OnEnable()
    {
        startCountDownPresenter.OnCountDownFinished += HandleCountDownFinished;
    }

    private void OnDisable()
    {
        startCountDownPresenter.OnCountDownFinished -= HandleCountDownFinished;
    }

    public void StartStartCountDown()
    {
        startCountDownPresenter.StartStartCountDown(
            rhythmManager.beatInterval,
            countingLength
        );
    }

    private void HandleCountDownFinished()
    {
        actionInteractor.TurnOn();
        playerMovementInteractor.TurnOn();
    }
}