using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class BGAudioManager : MonoBehaviour
{
    private static BGAudioManager instance;
    [SerializeField] private AudioSource BGAudioObject;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        System.Random rand = new System.Random();

        string audioClip = AudioManager.BGAudioLibrary.ElementAt(rand.Next(0, AudioManager.BGAudioLibrary.Count)).Key;
        PlayBGSoundClip(audioClip, 1f);

    }


    public static void PlayBGSoundClip(string audioClip, float volume)
    {
        if (instance == null)
        {
            Debug.LogError("BackgroundMusicManager: Instance singleton is null. Returning without playing audio");
            return;
        }

        if (!AudioManager.BGAudioLibrary.ContainsKey(audioClip))
        {
            Debug.LogError("BackgroundMusicManager: Audio clip not found in backgroundMusicLibrary. Returning without playing audio");
            return;
        }

        AudioSource audioSource = instance.BGAudioObject;

        audioSource.clip = AudioManager.BGAudioLibrary[audioClip];

        audioSource.volume = volume;

        audioSource.spatialBlend = 0f;

        audioSource.Play();
    }
}
