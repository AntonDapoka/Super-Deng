using UnityEngine;
using UnityEngine.UI;

public class WindowButtonControllerScript : MonoBehaviour
{
    [SerializeField] protected Button buttonClose;
    [SerializeField] protected Button buttonMaxMin;
    [SerializeField] private WindowInteractorScript windowInteractor;

    protected virtual void Awake()
    {
        buttonClose.onClick.AddListener(OnCloseClicked);
        buttonMaxMin.onClick.AddListener(OnMaxMinClicked);
    }

    protected virtual void OnCloseClicked()
    {
        windowInteractor.CloseWindow(gameObject);
    }

    protected virtual void OnMaxMinClicked()
    {
        windowInteractor.MaxMinWindow(gameObject);
    }
}