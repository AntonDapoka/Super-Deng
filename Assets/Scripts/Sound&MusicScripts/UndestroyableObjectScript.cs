using UnityEngine;
using UnityEngine.SceneManagement;

public class UndestroyableObjectScript : MonoBehaviour
{
    private void Awake()
    {
        UndestroyableObjectScript[] objects = FindObjectsByType<UndestroyableObjectScript>(FindObjectsSortMode.InstanceID);

        if (objects.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        if (SceneManager.GetActiveScene().buildIndex != 2)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 1)
        {
            Destroy(gameObject);
        }
    }
}
