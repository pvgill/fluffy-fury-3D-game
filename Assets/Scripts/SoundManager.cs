using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    [Header("--------------------------------  Audio Source  ------------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("------------ Default Music Clip ------------")]
    public AudioClip backgroundMusic;

    [Header("------------ Scene Music Clips (playing specific music per scene------------")]
    public AudioClip MainMenuScene;
    public AudioClip TutorialLevelScene;
    public AudioClip Level1Scene;
    public AudioClip DogLevel1Scene;
    public AudioClip HumanLevelScene;
    public AudioClip DogLevel2Scene;
    public AudioClip DogHumanLevel;
    public AudioClip MonkeyLevelScene;
    public AudioClip BossLevelScene;
    public AudioClip VictoryLevelScene;

    [Header("--------------------------------  Audio Clip  ------------")]
    //public AudioClip backgroundMusic;
    public AudioClip jump;
    public AudioClip collectFood;
    public AudioClip pickupProjectile;
    public AudioClip shootProjectile;
    public AudioClip noProjectile;
    public AudioClip doorsUnlocked;
    public AudioClip doorOpen;
    public AudioClip dropSap;
    public AudioClip loseLife;

    public static SoundManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("Duplicate SoundManager detected. Destroying extra instance.");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            Debug.Log("SoundManager initialized and DontDestroyOnLoad is active.");
        }
    }

    /*
    void Start()
    {
        musicSource.clip = backgroundMusic;
        musicSource.Play();
    } 
    */

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " + scene.name);

        switch (scene.name)
        {
            case "MainMenuScene":
                ChangeMusic(MainMenuScene);
                break;
            case "TutorialLevelScene":
                ChangeMusic(TutorialLevelScene);
                break;
            case "Level1Scene":
                ChangeMusic(Level1Scene);
                break;
            case "DogLevel1Scene":
                ChangeMusic(DogLevel1Scene);
                break;
            case "HumanLevelScene":
                ChangeMusic(HumanLevelScene);
                break;
            case "DogLevel2Scene":
                ChangeMusic(DogLevel2Scene);
                break;
            case "DogHumanLevel":
                ChangeMusic(DogHumanLevel);
                break;
            case "MonkeyLevelScene":
                ChangeMusic(MonkeyLevelScene);
                break;
            case "BossLevelScene":
                ChangeMusic(BossLevelScene);
                break;
            case "VictoryLevelScene":
                ChangeMusic(VictoryLevelScene);
                break;
            default:
                ChangeMusic(backgroundMusic); // fallback music in case scene music is not assigned 
                break;
        }
    }

    public void ChangeMusic(AudioClip newMusic)
    {
        //if (newMusic == null || musicSource.clip == newMusic) return;

        if (newMusic == null)
        {
            Debug.LogWarning("ChangeMusic called with null clip");
            return;
        }

        if (musicSource.clip == newMusic)
        {
            Debug.LogWarning("Same music already playing: " + newMusic.name);
            return;
        }

        Debug.Log("Changing music to: " + newMusic.name);
        musicSource.Stop();
        musicSource.clip = newMusic;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("PlaySFX called with null clip");
            return;
        }

        Debug.Log("Playing SFX: " + clip.name);
        SFXSource.PlayOneShot(clip);
    }


}
