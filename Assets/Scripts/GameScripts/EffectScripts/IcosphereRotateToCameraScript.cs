using UnityEngine;

public class IcosphereRotateToCameraScript : MonoBehaviour
{
    [SerializeField] private Transform transformSphere;
    [SerializeField] private Transform transformTarget;
    [SerializeField] private Transform transformChildObject;
    [SerializeField] private float rotationSpeed = 5f;

    public void Initialize(Transform transformTarget)
    {
       transformChildObject = transformTarget;
    }

    public void SetUpField(GameObject sphere)
    {
         transformSphere = sphere.transform;
    }

    private void Update()
    {
        if (transformTarget == null || transformChildObject == null) return;

        Vector3 directionToTarget = transformTarget.position - transformChildObject.position;
        if (directionToTarget == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.FromToRotation(-transformChildObject.up, directionToTarget) * transformSphere.rotation;

        transformSphere.rotation = Quaternion.Slerp(transformSphere.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}

