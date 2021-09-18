using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundVolumeSlider : MonoBehaviour
{
    Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider = gameObject.GetComponent<Slider>();
        slider.value = Global.soundVolume;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnSliderChange()
    {
        float value = slider.value;
        Global.SetSoundVolume(value);
        //Option.ChangeSoundVolume(value);
    }
}
