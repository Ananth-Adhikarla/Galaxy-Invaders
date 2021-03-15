using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] AudioClip[] myClips = null;

    [Header("Music Display")]
    [SerializeField] GameObject musicCanvas = null;
    [SerializeField] Text musicText = null;
    float timer = 2f;
    int trackNumber = 0;

    AudioSource myAudioSource;
    float volume = 0.4f;

    Coroutine musicPlayerTracker;

    void Awake()
    {
        SetUpSingleton();
    }

    private void SetUpSingleton()
    {
        if(FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

    }

    private void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
        myAudioSource.volume = volume;
        MusicChanger();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) || myAudioSource.isPlaying == false)
        {
            MusicChanger();
        }

        if (Input.GetKeyDown(KeyCode.P) && volume < 1f)
        {
            myAudioSource.volume += 0.1f;
        }
        if (Input.GetKeyDown(KeyCode.O) && volume > 0f)
        {
            myAudioSource.volume -= 0.1f;
        }
    }


    private void MusicChanger()
    {
        ConditionCheck();

        AudioClip myClip = SelectMusic();
        trackNumber++;
        myAudioSource.clip = myClip;
        myAudioSource.Play();
        musicPlayerTracker = StartCoroutine(ShowMusicText(myClip));
        //myAudioSource.PlayOneShot(myClip,volume);

    }

    private void ConditionCheck()
    {
        if (myAudioSource.isPlaying == true)
        {
            myAudioSource.Stop();
        }
        if (musicPlayerTracker != null)
        {
            StopCoroutine(musicPlayerTracker);
        }
        if (trackNumber + 1 > myClips.Length)
        {
            trackNumber = 0;
        }
    }

    private AudioClip SelectMusic()
    {
        return myClips[trackNumber];
    }

    IEnumerator ShowMusicText(AudioClip myClip)
    {
        musicCanvas.SetActive(true);
        musicText.text = myClip.name;
        yield return new WaitForSeconds(timer);
        musicCanvas.SetActive(false);
    }


}
