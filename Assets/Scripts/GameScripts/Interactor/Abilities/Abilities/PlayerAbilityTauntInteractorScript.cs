using UnityEngine;

public class PlayerAbilityTauntInteractorScript : PlayerAbilityScript
{
    public AudioClip soundClipTaunt;
    public AudioSource audioSource;

    [SerializeField] private float duration = 1f;
    [SerializeField] private Vector3 axis = Vector3.up;
    private GameObject playerFace;

    private float elapsed;
    private bool isRotating;
    private Quaternion initialRotation;

    public override void Activate(GameObject face)
    {
        if (!isRotating)
        {
            audioSource.clip = soundClipTaunt;
            duration = soundClipTaunt.length;

            if (audioSource != null && soundClipTaunt != null)
                audioSource.Play();

            playerFace = face;
            elapsed = 0f;
            isRotating = true;
            initialRotation = playerFace.transform.localRotation;
        }
    }

    private void Update()
    {
        if (!isRotating || duration <= 0f)
            return;

        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / duration);
        float angle = 360f * t;

        playerFace.transform.localRotation = initialRotation * Quaternion.AngleAxis(angle, axis.normalized);

        if (t >= 1f)
            isRotating = false;
    }


}
