using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public List<Sound> backgroundMusicList;
    public static Dictionary<string, AudioClip> SoundFXLibrary;
    public static Dictionary<string, AudioClip> backgroundMusicLibrary;

    private void Awake()
    {
        if (SoundFXLibrary == null)
        {
            SoundFXLibrary = new Dictionary<string, AudioClip>();
            foreach(var sound in soundFXList)
                SoundFXLibrary[sound.soundName] = sound.clip;
            soundFXList = null;
        }
        else
        {
            Destroy(this);
        }

    }

}
