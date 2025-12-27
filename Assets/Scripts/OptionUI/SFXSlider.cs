using UnityEngine;
using UnityEngine.UI;

public class SFXSlider : SoundSlider
{
    private void OnEnable()
    {
        SoundManager.instance.sfxVolumeChanged += BringValue;
    }

    public override void ChangeVolume()
    {
        SoundManager.instance.sfxVolume = slider.value;
    }
}
