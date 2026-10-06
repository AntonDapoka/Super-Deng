using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class MenuSceneLoaderScript : MonoBehaviour
    {
        public void LoadSceneByIndex(int indexScene)
        {
            SceneManager.LoadScene(indexScene);
        }
    }
}