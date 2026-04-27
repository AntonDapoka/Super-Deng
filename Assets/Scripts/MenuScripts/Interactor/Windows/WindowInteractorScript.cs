using UnityEngine;

public class WindowInteractorScript : MonoBehaviour
{
    [SerializeField] protected WindowPresenterScript windowPresenter;
    bool isMaximized = true;

    protected void Initialize()//Add some data
    {
        windowPresenter.Initialize();//Add some data
    }

    public void CloseWindow(GameObject window)
    {
        windowPresenter.CloseWindow();
        window.SetActive(false);
    }

    public void MaxMinWindow(GameObject window)
    {
        if (window.TryGetComponent(out WindowComponentsScript components))
        {
            if (isMaximized)
            {
                windowPresenter.MinimizeWindow(components);
                isMaximized = false;
            }
            else
            {
                windowPresenter.MaximizeWindow(components);
                isMaximized = true;
            }
        }
        else
        {
            Debug.LogError("No components on your Window");
        }
    }
}
