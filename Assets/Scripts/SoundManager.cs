using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void play(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void SetVolume(float value)
    {
        audioSource.volume = value;
    }
}
