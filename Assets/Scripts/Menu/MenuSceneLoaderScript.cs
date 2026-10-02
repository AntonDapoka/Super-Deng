using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSceneLoaderScript : MonoBehaviour
{
    public void LoadSceneByIndex(int indexScene)
    {
        SceneManager.LoadScene(indexScene);
    }
}
