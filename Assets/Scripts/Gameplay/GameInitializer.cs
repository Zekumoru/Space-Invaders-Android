using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    void Awake()
    {
        EventManager.Initialize();
        ScreenUtils.Initialize();
    }
}
