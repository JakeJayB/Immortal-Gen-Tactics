using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    private static SoundFXManager instance;
    [SerializeField] private AudioSource soundFXObject;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        
    }

    public static void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        if (instance == null)
        {
            Debug.LogError("SoundFXManager: Instance singleton is null. Returning without playing audio");
            return;
        }

        AudioSource audioSource = Instantiate(instance.soundFXObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;

        audioSource.volume = volume;

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength); 
    }
}
