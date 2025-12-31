using UnityEngine;
using UnityEngine.UI;

public class BGMSlider : SoundSlider
{
    private void OnEnable()
    {
        SoundManager.instance.bgmVolumeChanged += BringValue;
        slider.value = SoundManager.instance.bgmVolume;
    }

    private void OnDisable()
    {
        SoundManager.instance.bgmVolumeChanged -= BringValue;
    }

    public override void ChangeVolume()
    {
        SoundManager.instance.bgmVolume = slider.value;
    }
}
