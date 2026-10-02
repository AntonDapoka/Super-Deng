using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class StartLevelAnimationScript : MonoBehaviour
{
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject[] canvases;
    [SerializeField] private GameObject[] levelObjects;
    [SerializeField] private GameObject prefabDoor;
    [SerializeField] private Transform targetPoint;
    [SerializeField] private Transform enviromentHolder;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private Vector3 doorPosition;
    [SerializeField] private float rotationDuration = 1.5f;
    [SerializeField] private float imageResizeDuration = 0.8f;

    public Task StartTransitionAsync()
    {
        background.SetActive(false);
        foreach (var levelObject in levelObjects) levelObject.SetActive(false);
        foreach (var canvas in canvases) canvas.SetActive(false);
        return PerformEffectsAsync();
    }

    private async Task PerformEffectsAsync()
    {
        Task cameraRotation = this.RunAsync(RotateCameraToTarget());
        await this.RunAsync(WaitSeconds(0.6f));
        _ = this.RunAsync(ExpandDoor());
        await this.RunAsync(WaitSeconds(0.5f));
        _ = this.RunAsync(ExpandDoor());
        await this.RunAsync(WaitSeconds(0.5f));
        Task lastDoor = this.RunAsync(ExpandDoor());

        await cameraRotation;
        await lastDoor;
    }

    private static IEnumerator WaitSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    private IEnumerator RotateCameraToTarget()
    {
        float elapsedTime = 0;

        Vector3 startPosition = enviromentHolder.position;
        Vector3 startSize = enviromentHolder.localScale;
        Vector3 targetSize = startSize * 30;

        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / rotationDuration;
            enviromentHolder.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / rotationDuration);
            enviromentHolder.localScale = Vector3.Lerp(startSize, targetSize, elapsedTime / rotationDuration);
            yield return null;
        }

        enviromentHolder.position = targetPosition;
        enviromentHolder.localScale = targetSize;
    }

    private IEnumerator ExpandDoor()
    { 
        GameObject door = Instantiate(prefabDoor);
        door.transform.position = doorPosition;

        float elapsedTime = 0;
        Vector3 startScale = Vector3.zero;
        Vector3 targetScale = new Vector3(20f,20f,20f);

        while (elapsedTime < imageResizeDuration)
        {
            elapsedTime += Time.deltaTime;
            door.transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / imageResizeDuration);
            yield return null;
        }

        door.transform.localScale = targetScale;
    }
}
