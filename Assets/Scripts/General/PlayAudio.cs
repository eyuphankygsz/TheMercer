using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    private AudioSource _sr;
    private void Awake()
    {
        _sr = GetComponent<AudioSource>();
    }
    public void PlayAudios()
    {
        _sr.Play();
    }
}
