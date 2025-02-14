using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAndRotateScript : MonoBehaviour
{
    private Transform target; 
    [SerializeField] private Vector3 centerPoint;
    [SerializeField] private float impulseForce = 10f;
    [SerializeField] private float torqueStrength = 10f;
    [SerializeField] private float moveDuration = 2f;
    [SerializeField] private float delay;

    private void Start()
    {
        Destination targetGameobject = gameObject.GetComponentInChildren<Destination>();
        if (targetGameobject != null)
        {
            target = targetGameobject.transform;
            Invoke(nameof(ApplyImpulse), delay);
        }

    }

    private void ApplyImpulse()
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        Vector3 direction = (rb.transform.position - centerPoint).normalized;
        rb.AddForce(direction * impulseForce, ForceMode.Impulse);
        Vector3 randomTorque = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized * torqueStrength;

        rb.AddTorque(randomTorque, ForceMode.Impulse);

        StartCoroutine(MoveToTargetAfterImpulse(rb, target.position, moveDuration));
    }

    private IEnumerator MoveToTargetAfterImpulse(Rigidbody rb, Vector3 targetPosition, float moveDuration)
    {
        yield return new WaitForSeconds(0.5f);

        Vector3 startPosition = rb.transform.position;
        Quaternion startRotation = rb.transform.rotation;

        float elapsedTime = 0f;
        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveDuration;
            rb.MovePosition(Vector3.Lerp(startPosition, targetPosition, t));

            yield return null; 
        }
        rb.MovePosition(targetPosition);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}