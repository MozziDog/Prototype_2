using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] BGM;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;
        Global.OnVolumeChanged += OnVolumeChanged;
        audioSource.loop = true;
    }

    public void OnVolumeChanged()
    {
        audioSource.volume = Global.soundVolume;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Change BGM when entered in-Game
        if (scene.name == "GameScene" || scene.name == "SampleScene_TH")
        {
            switch (Global._chapter)
            {
                case 1:
                    audioSource.clip = BGM[1];
                    audioSource.Play();
                    break;
                case 2:
                    audioSource.clip = BGM[2];
                    audioSource.Play();
                    break;
            }

        }
        else
        {
            if (audioSource.clip == BGM[0])
            {
                // DO NOTHING;
            }
            else
            {
                audioSource.clip = BGM[0];
                audioSource.Play();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void play(AudioClip clip)
    {
        audioSource.PlayOneShot(clip, Global.soundVolume);
    }

    public void SetVolume(float value)
    {
        //audioSource.volume = value;
        audioSource.volume = Global.soundVolume;
    }
}
