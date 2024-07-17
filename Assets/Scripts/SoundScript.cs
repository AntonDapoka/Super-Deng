using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScript : MonoBehaviour
{
    public AudioClip soundClip;  // ���������� ���� ��� ��������� � ����������
    private AudioSource audioSource;

    private void Start()
    {
        // ��������� ��������� AudioSource � �������, ���� ��� ��� ���
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = soundClip;
    }

    public void TurnOnSound()
    {
        // ������������� ���� �� ����� �� ������
        if (audioSource != null && soundClip != null)
        {
            audioSource.Play();
        }
    }
}
