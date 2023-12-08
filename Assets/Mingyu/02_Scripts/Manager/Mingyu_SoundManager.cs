using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Mingyu_SoundManager : MonoBehaviour
{
    public static float s_volume = 1;
    public AudioSource[] sound_Array;

    // Start is called before the first frame update
    void Start()
    {
        foreach(AudioSource audio in sound_Array)
        {
            audio.GetComponent<AudioSource>().volume = Mingyu_SoundManager.s_volume;
        }
    }

    public void Update()
    {
        Debug.Log(s_volume);
    }

    public void Set_Volume(Slider slider)
    {
        Mingyu_SoundManager.s_volume = slider.value;

        foreach (AudioSource audio in sound_Array)
        {
            audio.GetComponent<AudioSource>().volume = Mingyu_SoundManager.s_volume;
        }
    }
}
