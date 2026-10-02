using TMPro;
using UnityEngine;

public class FaceIDViewScript : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;
    
    public void DisplayID(string id)
    {
        if (text != null)
            text.text = id;
    }
}
