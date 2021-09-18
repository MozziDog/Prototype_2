using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundVolumeSlider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnSliderChange()
    {
        float value = gameObject.GetComponent<Slider>().value;
        Global.SetSoundVolume(value);
        //Option.ChangeSoundVolume(value);
    }
}
