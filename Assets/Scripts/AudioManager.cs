using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{


    static public AudioManager Instance { get; private set; }
    public AudioClip[] Music;
    //public AudioClip Sound;
    private AudioClip currentTrack;
    private AudioSource musicSource;
    private AudioSource soundSource;
    public bool startMusic = false;

    private string curLevel;

    public AudioClip dungeonTheme;
    public AudioClip bossTheme; 
    private float musicVolume = .05f;
    private float soundVolume = .75f;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {

            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(Instance);
    }

    private void Start()
    {
        musicSource = gameObject.transform.Find("Music Source").GetComponent<AudioSource>();

        soundSource = gameObject.transform.Find("SFX Source").GetComponent<AudioSource>();
        playMusic();

    }
    public void Update()
    {
        
        if (startMusic && !musicSource.isPlaying)
        {
            playMusic();
        }
        
        
    }


    
    public void playMusic(bool boss = false)
    {
        musicSource.Stop();
        
        /*if (musicSource == null)
        {
            musicSource = gameObject.transform.Find("Music Source").GetComponent<AudioSource>();

        }


        int rand = Random.Range(0, Music.Length);
        musicSource.clip = Music[rand];
        musicSource.Play();*/


        curLevel = SceneManager.GetActiveScene().name;  

        if (!boss)
        {
            musicSource.clip = dungeonTheme; 
            musicSource.volume = musicVolume;
            musicSource.Play();
        }

        else
        {
            musicSource.clip = bossTheme;
            musicSource.volume = musicVolume;
            musicSource.Play();
        }
        startMusic = true;

    }

    public void changeTrack(/*int track*/)
    {
        musicSource.Stop();
        startMusic=false;
       // currentTrack = Music[track];
        //musicSource.clip = currentTrack;
        playMusic();
    }

    public void playSound(AudioClip sound)
    {
        soundSource.clip = sound;
        soundSource.volume = soundVolume;
        soundSource.PlayOneShot(sound);

    }



    public void stopSound()
    {
        if (soundSource == null)
        {
            soundSource = gameObject.transform.Find("SFX Source").GetComponent<AudioSource>();
        }
        soundSource.Stop();
    }





}
