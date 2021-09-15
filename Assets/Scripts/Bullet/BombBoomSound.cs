using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBoomSound : MonoBehaviour
{
    
    private AudioSource musicPlayer;
    [SerializeField]
     private AudioClip boomSound;
    // Start is called before the first frame update
    void Start()
    {
        musicPlayer = GetComponent<AudioSource>();
        musicPlayer.clip = boomSound;
        musicPlayer.time = 0;
        musicPlayer.Play();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
