using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioManager
{
    static bool initialized = false;
    static AudioSource audioSource;
    static Dictionary<AudioClipName, AudioClip> audioClips = new Dictionary<AudioClipName, AudioClip>();

    public static bool Initialized => initialized;

    public static void Initialize(AudioSource source)
    {
        initialized = true;
        audioSource = source;

        audioClips.Add(AudioClipName.Shoot, Resources.Load<AudioClip>(@"Audio/Shoot"));
        audioClips.Add(AudioClipName.Success, Resources.Load<AudioClip>(@"Audio/Success"));
        audioClips.Add(AudioClipName.Explosion, Resources.Load<AudioClip>(@"Audio/Explosion"));
        audioClips.Add(AudioClipName.Invader, Resources.Load<AudioClip>(@"Audio/Invader"));
        audioClips.Add(AudioClipName.InvaderKilled, Resources.Load<AudioClip>(@"Audio/InvaderKilled"));
        audioClips.Add(AudioClipName.Bit8, Resources.Load<AudioClip>(@"Audio/8bit"));
    }

    public static void Play(AudioClipName name)
    {
        audioSource.PlayOneShot(audioClips[name]);
    }
}
