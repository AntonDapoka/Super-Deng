using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuTriangleInteractSecretScript : MonoBehaviour
{
    public ParticleSystem particlePrefab;
    public GameObject shadow;
    public float shakeDuration = 0.2f;
    public float shakeAmount = 0.05f;
    public int clickCount = 0;
    public int maxCount = 10;
    public GameObject window;

    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = transform.localPosition;

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickCount++;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider == shadow.GetComponent<Collider>())
                {
                    SpawnParticles(hit.point);
                    StartCoroutine(Shake());
                }
            }
            if (clickCount == maxCount)
            {
                window.SetActive(true);
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