using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SoundFade : MonoBehaviour
{
    public static SoundFade instance {get; private set;}
    public AudioSource musicTrack;

    [Header("Fade")]
    public float fadeInDuration = 0.5f;
    public float fadeOutDuration = 2f;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        musicTrack = AudioManager.instance.musicSource;
    }

    public void FadeIn()
    {
        musicTrack = AudioManager.instance.musicSource;
        StartCoroutine(Fade(true));
    }

    public void FadeOut()
    {
        musicTrack = AudioManager.instance.musicSource;
        StartCoroutine(Fade(false));
    }

    IEnumerator Fade(bool fadeIn)
    {
        float startVolume = Settings.instance.muteMusic ? 0f : AudioManager.instance.music;

        while (musicTrack.volume > 0 && !fadeIn)
        {
            musicTrack.volume -= startVolume * (Time.unscaledDeltaTime / fadeOutDuration);
            yield return null;
        }

        while (musicTrack.volume < startVolume)
        {
            musicTrack.volume += startVolume * (Time.unscaledDeltaTime / fadeInDuration);
            yield return null;
        }

        musicTrack.volume = startVolume;
    }
}