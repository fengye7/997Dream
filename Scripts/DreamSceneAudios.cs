using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamSceneAudios : MonoBehaviour
{
    private static DreamSceneAudios instance;

    public static DreamSceneAudios Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DreamSceneAudios>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("PlayerAudios");
                    instance = obj.AddComponent<DreamSceneAudios>();
                }
            }
            return instance;
        }
    }

    public AudioClip hitAudio;
    public AudioClip rollAudio;
    public AudioClip injuredAudio;

    public AudioClip bombAudio;
    public AudioClip fetchAudio;
    public AudioClip battleBG;

    public AudioSource audioSource;
    public AudioSource BGAudioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        if (BGAudioSource == null)
        {
            BGAudioSource = gameObject.AddComponent<AudioSource>();
        }
        // …Ë÷√±≥æ∞“Ù¿÷
        if (battleBG != null)
        {
            BGAudioSource.clip = battleBG;
            BGAudioSource.loop = true;
            BGAudioSource.Play();
        }
    }

    public void PlayHitAudio()
    {
        PlayAudio(hitAudio);
    }

    public void PlayRollAudio()
    {
        PlayAudio(rollAudio);
    }

    public void PlayInjuredAudio()
    {
        PlayAudio(injuredAudio);
    }

    public void PlayBombAudio()
    {
        PlayAudio(bombAudio);
    }

    public void PlayFetchAudio()
    {
        PlayAudio(fetchAudio);
    }

    private void PlayAudio(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}

