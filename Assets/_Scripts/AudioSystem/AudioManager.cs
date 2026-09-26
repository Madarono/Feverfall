using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    [Header("Volume Controls")]
    [Range(0f, 1f)] public float master = 1f;
    [Range(0f, 1f)] public float music = 1f;
    public AudioSource source;

    [Header("Play GameObject")]
    public GameObject playPrefab;
    public AudioSource musicSource;

    [Header("Villagers")]
    public AudioClip villagerAssign;
    public AudioClip villagerRevoke;
    public AudioClip villagerCome;
    public AudioClip villagerLeave;
    public AudioClip villagerDie;
    public AudioClip villagerSelect;
    public AudioClip villagerDeselect;

    [Header("UI")]
    public AudioClip[] buttonClicks;
    public AudioClip slider;
    public AudioClip moneyIncrease;
    public AudioClip moneyDecrease;
    public AudioClip popupText;
    public AudioClip textFieldSelect;

    [Header("Laboratory")]
    public AudioClip cureFailure;
    public AudioClip cureSuccess;
    public AudioClip vaccineBuy;
    public AudioClip stats;

    [Header("Building")]
    public AudioClip roadPut;
    public AudioClip roadShovel;
    public AudioClip shovel;
    public AudioClip pickup;
    public AudioClip place;
    public AudioClip building;
    public AudioClip select;

    [Header("Building - Market")]
    public AudioClip sellMarket;

    [Header("Misc.")]
    public AudioClip envellope;
    public AudioClip loseSong;
    public AudioClip hoverButton;

    [Header("Camera")]
    public AudioClip zoomIn;
    public AudioClip zoomOut;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindAndAssignMusicSource();
        UpdateVolume();
    }

    void Start()
    {
        FindAndAssignMusicSource();
    }

    public void FindAndAssignMusicSource()
    {
        GameObject musicObj = GameObject.Find("MusicTrack");
        if (musicObj != null)
        {
            if (musicObj.TryGetComponent(out AudioSource audioSrc))
            {
                musicSource = audioSrc;
            }
        }
        else
        {
            AudioSource[] sources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
            foreach (AudioSource src in sources)
            {
                if (src != source)
                {
                    musicSource = src;
                    break;
                }
            }
        }
    }

    public void Play(AudioClip clip, float amplification = 1f, bool isMusic = false)
    {
        if((isMusic && Settings.instance.muteMusic) || (!isMusic && Settings.instance.muteSfx)) return;

        float baseVolume = isMusic ? music : master;
        float finalVolume = Mathf.Clamp01(baseVolume * amplification);

        if (source != null)
        {
            source.PlayOneShot(clip, finalVolume);
        }
    }

    public GameObject PlayGameObject(AudioClip clip, float amplification = 1f, bool isMusic = false)
    {
        if((isMusic && Settings.instance.muteMusic) || (!isMusic && Settings.instance.muteSfx)) return null;

        float baseVolume = isMusic ? music : master;
        float finalVolume = Mathf.Clamp01(baseVolume * amplification);

        if (playPrefab == null) return null;

        GameObject go = Instantiate(playPrefab, Vector3.zero, Quaternion.identity);
        if (go.TryGetComponent(out AudioItem goScript))
        {
            goScript.clip = clip;
            goScript.volume = finalVolume;
            goScript.Play();
        }

        return go;
    }

    public void UpdateMusic(AudioClip clip, bool loop)
    {
        if (musicSource == null)
        {
            FindAndAssignMusicSource();
        }

        if (musicSource != null)
        {
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.Play();
        }
    }
    
    public void UpdateVolume()
    {
        if (Settings.instance == null) return;

        master = Mathf.Clamp01(Settings.instance.sfxValue);
        music = Mathf.Clamp01(Settings.instance.musicValue);

        if (musicSource == null)
        {
            FindAndAssignMusicSource();
        }

        if (musicSource != null)
        {
            musicSource.volume = Settings.instance.muteMusic ? 0f : music;
        }
    }
}