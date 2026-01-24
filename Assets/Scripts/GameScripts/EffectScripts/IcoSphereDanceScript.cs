using UnityEngine;

public class IcoSphereDanceScript : MonoBehaviour
{
    [SerializeField] private GameObject[] objects; 
    public float rotationAngle = 15f;  
    public float duration = 0.2f; 
    public bool isTurnOn = false;  
    private bool inProcess = false;
    private int side = -1;
    //[SerializeField] private EnemySpawnSettings enemySpawnSettings;
    /*
    private void Update()
    {
        if (isTurnOn && !inProcess)
        {
            foreach (GameObject obj in objects)
            {
                StartCoroutine(RotateObject(obj, rotationAngle, duration));
            }
        }
    }

    private IEnumerator RotateObject(GameObject obj, float angle, float time)
    {
        if (isTurnOn)
        {
            inProcess = true;
            Quaternion originalRotation = obj.transform.rotation;
            Quaternion targetRotation = originalRotation * Quaternion.Euler(0, side * angle, 0);

            float elapsedTime = 0f;
            while (elapsedTime < time)
            {
                obj.transform.rotation = Quaternion.Slerp(originalRotation, targetRotation, elapsedTime / time);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            obj.transform.rotation = targetRotation;

            elapsedTime = 0f;
            while (elapsedTime < time)
            {
                obj.transform.rotation = Quaternion.Slerp(targetRotation, originalRotation, elapsedTime / time);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            obj.transform.rotation = originalRotation;
            inProcess = false;
            side = -side;
        }
        else
        {
            inProcess = false;
            yield return null;
        }
    }*/
}