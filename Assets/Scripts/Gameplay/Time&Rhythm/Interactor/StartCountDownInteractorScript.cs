using UnityEngine;

public class StartCountDownInteractorScript : MonoBehaviour
{
    [SerializeField] private int countingLength = 3;
    [SerializeField] private StartCountDownPresenterScript startCountDownPresenter;
    [SerializeField] private ActionInteractorScript actionInteractor;
    [SerializeField] private PlayerMovementInteractorScript playerMovementInteractor;
    [SerializeField] private CameraBehaivorInteractorScript cameraBehaivorInteractor;

    private void OnEnable()
    {
        startCountDownPresenter.OnCountDownFinished += HandleCountDownFinished;
    }

    private void OnDisable()
    {
        startCountDownPresenter.OnCountDownFinished -= HandleCountDownFinished;
    }

    public void StartStartCountDown(float beatInterval)
    {
        startCountDownPresenter.StartStartCountDown(beatInterval, countingLength);
    }

    private void HandleCountDownFinished()
    {
        actionInteractor.TurnOn();
        playerMovementInteractor.TurnOn();
        cameraBehaivorInteractor.TurnOn();
    }
}