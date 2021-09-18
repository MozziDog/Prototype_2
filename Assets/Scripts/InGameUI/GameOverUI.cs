using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.volume = Global.soundVolume;
        Global.OnVolumeChanged += CheckSoundVolume;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CheckSoundVolume()
    {
        audioSource.volume = Global.soundVolume;
    }
}
