using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityTauntInteractorScript : MonoBehaviour, IAbilityScript
{
    public AudioClip soundClipTaunt;
    public AudioSource audioSource;

    [SerializeField] private float duration = 1f;
    [SerializeField] private Vector3 axis = Vector3.up;
    private GameObject playerFace;

    private float elapsed;
    private bool isRotating;
    private Quaternion initialRotation;

    public void Activate(GameObject face)
    {
        if (!isRotating)
        {
            audioSource.clip = soundClipTaunt;
            duration = soundClipTaunt.length;
            if (audioSource != null && soundClipTaunt != null)
            {
                audioSource.Play();
            }

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

        // ÂÐÀÙÀÅÌ ÎÒÍÎÑÈÒÅËÜÍÎ ÑÂÎÅÉ ÎÑÈ
        playerFace.transform.localRotation = initialRotation * Quaternion.AngleAxis(angle, axis.normalized);

        if (t >= 1f)
            isRotating = false;
    }


}
