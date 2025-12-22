using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class LightShutDownScript : MonoBehaviour
{
    private GameObject[] faces;
    [SerializeField] private LoseScript LS;
    [SerializeField] private FaceArrayScript FAS;
    [SerializeField] private GameObject[] lights;
    [SerializeField] private PlayerScript player;
    [SerializeField] private AudioClip[] soundsLight;
    [SerializeField] private AudioClip soundFace;
    [SerializeField] private Material material;
    [SerializeField] private float delay = 1f; 
    [Space]
    public PostProcessVolume postProcessVolume;
    private Vignette vignette;
    public float fadeDuration = 2f;
    public float targetIntensity = 1f;

    private AudioSource audioSource;

    private void Start()
    {
        faces = FAS.GetAllFaces();
        audioSource = gameObject.AddComponent<AudioSource>();
        if (postProcessVolume.profile.TryGetSettings(out vignette))
        {
            vignette.intensity.value = 0.4f;
            vignette.smoothness.value = 0.4f;
        }

    }

    public void StartFullShutDown()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        StartCoroutine(ShutDownLight());
        StartCoroutine(FadeInVignette());
    }

    public void StartLocalShutDown()
    {
        StartCoroutine(ShutDownLight());
    }

    private IEnumerator ShutDownLight()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            if (lights[i] != null)
            {
                lights[i].SetActive(false);
                audioSource.Stop();
                audioSource.clip = soundsLight[Random.Range(0, soundsLight.Length)];
                audioSource.Play();
                yield return new WaitForSeconds(delay);
            }
        }
        if (LS != null) LS.ShowImage();
        StartCoroutine(ShutDownIcosahedron());
        
    }

    private IEnumerator ShutDownIcosahedron()
    {
        for (int i = 0; i < faces.Length; i++)
        {
            FaceDanceScript FDS = faces[i].GetComponent<FaceDanceScript>();
            FDS.isTurnOn = false;
            //FDS.StopScaling();
        }

        List<int> indices = new List<int>();
        for (int i = 0; i < faces.Length; i++)
            indices.Add(i);
        Shuffle(indices);
        audioSource.Stop();
        audioSource.clip = soundFace;
        audioSource.Play();

        foreach (int i in indices)
        {
            if (faces[i] != null)
            {
                //faces[i].GetComponent<FaceScript>().rend.material = material;
                yield return new WaitForSeconds(delay/10);
            }
        }
        audioSource.Stop();
    }

    public void Shuffle(List<int> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }

    private IEnumerator FadeInVignette()
    {
        float elapsedTime = 0f;
        float initialIntensity = vignette.intensity.value;

        while (elapsedTime < fadeDuration)
        {
            vignette.intensity.value = Mathf.Lerp(initialIntensity, targetIntensity, elapsedTime / fadeDuration);
            vignette.smoothness.value = Mathf.Lerp(initialIntensity, targetIntensity, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        vignette.intensity.value = targetIntensity;
        vignette.smoothness.value = targetIntensity;
    }
}