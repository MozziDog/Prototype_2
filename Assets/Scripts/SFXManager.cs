using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    static AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(Global.soundVolume);
    }

    public static void Play(AudioClip clip)
    {
        audioSource.PlayOneShot(clip, Global.soundVolume);
    }

    public void SetVolume(float value)
    {
        // audioSource.volume = value;
        audioSource.volume = Global.soundVolume;
    }

}
