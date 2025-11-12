using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractorScript : MonoBehaviour
{
    public GameObject player;
    //Добавить часть про то, какие кнопки отвечают за что

    public void HandleInput(KeyCode keyCode)
    {
        Debug.Log("Key: " + keyCode.ToString());
    }
}
