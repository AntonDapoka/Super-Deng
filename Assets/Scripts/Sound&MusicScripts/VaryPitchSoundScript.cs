using UnityEngine;

public class VaryPitchSoundScript : MonoBehaviour
{
    [SerializeField] private float minPitch = 0.95f;
    [SerializeField] private float maxPitch = 1.05f;

    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        audioSource.playOnAwake = false;
    }

    public void PlayVaryPitchSound(AudioClip clip)
    {
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.PlayOneShot(clip);
    }
}