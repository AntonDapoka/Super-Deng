using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class SecretPasswordInputScript : MonoBehaviour
{
    [SerializeField] private GameObject holderDescriptionUI;
    [SerializeField] private GameObject holderPasswordUI;
    [SerializeField] private bool isPasswordUnlocked = false;
    public int currentIndex;
    public int amountOfLevels;

    [Header("TheFirtButton")]
    [SerializeField] private Button buttonRight1;
    [SerializeField] private Button buttonLeft1;
    [SerializeField] private GameObject[] items;
    private int currentIndexButton1 = 0;
    [Header("TheSecondButton")]
    [SerializeField] private Button buttonRight2;
    [SerializeField] private Button buttonLeft2;
    [Header("TheThirdButton")]
    [SerializeField] private Button buttonRight3;
    [SerializeField] private Button buttonLeft3;

    private void Start()
    {
        holderDescriptionUI.SetActive(true);
        holderPasswordUI.SetActive(false);

        buttonRight1.onClick.AddListener(NextItem); 
        buttonLeft1.onClick.AddListener(PreviousItem); 
    }

    public void SetAcrivePasswordUI(bool isTurnOn)
    {
        holderDescriptionUI.SetActive(!isTurnOn);
        holderPasswordUI.SetActive(isTurnOn);
        UpdateActiveItem();
    }

    private void NextItem()
    {
        currentIndexButton1 = (currentIndexButton1 + 1) % items.Length;
        UpdateActiveItem();
    }
    private void PreviousItem()
    {
        currentIndexButton1 = (currentIndexButton1 - 1 + items.Length) % items.Length;
        UpdateActiveItem();
    }

    private void UpdateActiveItem()
    {
        for (int i = 0; i < items.Length; i++)
        {
            items[i].SetActive(i == currentIndexButton1);
        }
    }

public void SetCurrentIndex(int index)
    {
        currentIndex = index;
    }

    public void SetAmountOfLevels(int amount) 
    {
        amountOfLevels = amount;
    }


}
