using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLibrary : MonoBehaviour
{

    [Header("sounds")]
    public List<AudioClip> sounds = new List<AudioClip>();
    public List<string> sound_names = new List<string>();
    private Dictionary<string, AudioClip> lib = new Dictionary<string, AudioClip>();

    // Clip spawns with file name - autofill sound_name list instead.
    void Start()
    {
        int index = 0;
        foreach (AudioClip clip in sounds) {
            lib.Add(sound_names[index], clip);
            index++;
        }
    }


    public AudioClip returnAudio(string name) {
        return lib[name];
    }

}
