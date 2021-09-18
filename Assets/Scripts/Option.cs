using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Option : MonoBehaviour
{
    public static float soundVolume = 1f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void ChangeSoundVolume(float value)
    {
        soundVolume = value;
        // GameObject.Find("SoundManager").GetComponent<BGMManager>().SetVolume(value);
    }
}
