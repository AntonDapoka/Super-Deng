using UnityEngine;

public class MenuButtonControllerScript : MonoBehaviour
{
    protected IButtonInputInteractorScript interactor;

    public virtual void Initialize(IButtonInputInteractorScript interactor)
    {
        this.interactor = interactor;
    }

    public virtual void OnButtonClicked()
    {
        interactor.HandleInput(new ButtonInputDataScript
        {
            IsClick = true
        });
    }

    public virtual void OnPointerDown()
    {
        interactor.HandleInput(new ButtonInputDataScript
        {
            IsPointerDown = true
        });
    }

    public virtual void OnPointerUp()
    {
        interactor.HandleInput(new ButtonInputDataScript
        {
            IsPointerUp = true
        });
    }
    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            interactor.HandleInput(new ButtonInputDataScript
            {
                IsKeyboard = true,
                Key = KeyCode.Space
            });
        }
    }*/
}