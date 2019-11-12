using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResetSound : MonoBehaviour
{

    private AudioSource audioSource;
    public List<AudioClip> ListSounds;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        int index = Random.Range(0, ListSounds.Count);
        audioSource.clip = ListSounds[index];
        audioSource.Play();
    }
}
