using UnityEngine;
using TMPro;

public class StartCountDownViewScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countDownText;

    public void TurnOn()
    {
        countDownText.text = "";
        countDownText.gameObject.SetActive(true);
    }

    public void TurnOff()
    {
        countDownText.gameObject.SetActive(false);
    }

    public void DisplayValue(string value)
    {
        countDownText.text = value;
    }
}
