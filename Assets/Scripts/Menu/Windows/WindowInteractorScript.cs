using UnityEngine;

public class WindowInteractorScript : MonoBehaviour
{
    [SerializeField] protected WindowPresenterScript windowPresenter;

    protected void Initialize()//Add some data
    {
        windowPresenter.Initialize();//Add some data
    }

    public void CloseWindow(GameObject window)
    {
        if (window.TryGetComponent(out WindowComponentsScript components))
        {
            windowPresenter.CloseWindow(components);
        }
        //window.SetActive(false);
    }

    public void MaxMinWindow(GameObject window)
    {
        if (window.TryGetComponent(out WindowComponentsScript components))
        {
            if (components.GetWindowState().GetIsMaximized())
            {
                windowPresenter.MinimizeWindow(components);
                components.GetWindowState().SetIsMaximized(false);
            }
            else
            {
                windowPresenter.MaximizeWindow(components);
                components.GetWindowState().SetIsMaximized(true);
            }
        }
        else
        {
            Debug.LogError("No components on your Window");
        }
    }
}
