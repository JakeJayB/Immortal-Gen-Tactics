using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public class Sound
    {
        public string soundName;
        public AudioClip clip;
    }

    public List<Sound> soundFXList;
    public List<Sound> BGAudioList;
    public static Dictionary<string, AudioClip> SoundFXLibrary;
    public static Dictionary<string, AudioClip> BGAudioLibrary;

    private void Awake()
    {
        if (SoundFXLibrary == null)
        {
            SoundFXLibrary = new Dictionary<string, AudioClip>();
            BGAudioLibrary = new Dictionary<string, AudioClip>();

            foreach(var sound in soundFXList)
                SoundFXLibrary[sound.soundName] = sound.clip;

            foreach (var sound in BGAudioList)
                BGAudioLibrary[sound.soundName] = sound.clip;

            soundFXList = null;
            BGAudioList = null;
        }
        else
        {
            Destroy(this);
        }

    }

    public static void Clear()
    {
        SoundFXLibrary = null;
        BGAudioLibrary = null;
    }

    public static void RegisterCleanup()
    {
        MemoryManager.AddListeners(Clear);
    }

}
