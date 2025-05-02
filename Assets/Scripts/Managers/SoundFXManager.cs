using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    private static SoundFXManager instance;
    private const float TIME_OFFSET = 0.4f; // offset to ensure audio plays before being destroyed
    [SerializeField] private AudioSource soundFXObject;


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

    public static void Clear()
    {
        instance = null;
    }

    public static void RegisterCleanup()
    {
        MemoryManager.AddListeners(Clear);
    }


    public static void PlaySoundFXClip(string audioClip, float volume = 0.35f)
    {
        if (instance == null)
        {
            Debug.LogError("SoundFXManager: Instance singleton is null. Returning without playing audio");
            return;
        }

        if (!AudioManager.SoundFXLibrary.ContainsKey(audioClip))
        {
            Debug.LogError("SoundFXManager: Audio clip not found in SoundLibrary. Returning without playing audio");
            return;
        }

        AudioSource audioSource = Instantiate(instance.soundFXObject, Camera.main.transform.position, Quaternion.identity);

        audioSource.clip = AudioManager.SoundFXLibrary[audioClip];

        audioSource.volume = volume;

        audioSource.spatialBlend = 0f;

        audioSource.Play();

        float clipLength = audioSource.clip.length + TIME_OFFSET;


        DontDestroyOnLoad(audioSource.gameObject);
        Destroy(audioSource.gameObject, clipLength);
    }


    /*    public static void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
        {
            if (instance == null)
            {
                Debug.LogError("SoundFXManager: Instance singleton is null. Returning without playing audio");
                return;
            }

            if(AudioManager.SoundLibrary.ContainsKey)

            AudioSource audioSource = Instantiate(instance.soundFXObject, spawnTransform.position, Quaternion.identity);

            audioSource.clip = audioClip;

            audioSource.volume = volume;

            audioSource.Play();

            float clipLength = audioSource.clip.length;

            Destroy(audioSource.gameObject, clipLength); 
        }*/
}
