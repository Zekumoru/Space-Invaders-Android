using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudioSource : MonoBehaviour
{
    void Awake()
    {
        if (AudioManager.Initialized)
        {
            Destroy(gameObject);
            return;
        }

        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        AudioManager.Initialize(audioSource);
        DontDestroyOnLoad(gameObject);
    }
}
