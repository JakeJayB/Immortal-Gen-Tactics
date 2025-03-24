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

    public List<Sound> soundList;
    public static Dictionary<string, AudioClip> SoundLibrary;

    private void Awake()
    {
        if (SoundLibrary == null)
        {
            SoundLibrary = new Dictionary<string, AudioClip>();
            foreach(var sound in soundList)
                SoundLibrary[sound.soundName] = sound.clip;
            soundList.Clear();
        }
        else
        {
            Destroy(this);
        }

    }

}
