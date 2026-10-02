using System.Collections;
using UnityEngine;

public class MenuTriangleInteractSecretScript : MonoBehaviour
{
    [SerializeField] private ParticleSystem particlePrefab;
    [SerializeField] private GameObject shadow;
    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private float shakeAmount = 0.05f;
    [SerializeField] private int clickCount = 0;
    [SerializeField] private int activationCount = 0;

    [SerializeField] private int maxCount = 10;
    [SerializeField] private GameObject window;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider == shadow.GetComponent<Collider>())
                {
                    clickCount++;
                    SpawnParticles(hit.point);
                    StartCoroutine(Shake());
                }
            }
            if (clickCount == maxCount && activationCount==0)
            {
                window.SetActive(true);
                activationCount = 1;
                clickCount = 0;
            }
        }
    }

    private void SpawnParticles(Vector3 position)
    {
        if (particlePrefab != null)
        {
            ParticleSystem particles = Instantiate(particlePrefab, position, Quaternion.identity);
            particles.Play();
            Destroy(particles.gameObject, particles.main.duration + particles.main.startLifetime.constantMax);
        }
    }

    private IEnumerator Shake()
    {
        float elapsed = 0f;
        Vector3 startPos = transform.localPosition;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeAmount;
            float y = Random.Range(-1f, 1f) * shakeAmount;

            transform.localPosition = startPos + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = startPos;
    }
}