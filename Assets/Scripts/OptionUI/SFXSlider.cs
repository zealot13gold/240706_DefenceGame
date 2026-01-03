using UnityEngine;
using UnityEngine.UI;

public class SFXSlider : SoundSlider
{
    private void OnEnable()
    {
        SoundManager.instance.sfxVolumeChanged += BringValue;
        slider.value = SoundManager.instance.sfxVolume;             // 활성화 시 SoundManager의 값을 가져옴
    }

    private void OnDisable()
    {
        SoundManager.instance.sfxVolumeChanged-= BringValue;
    }

    public override void ChangeVolume()
    {
        SoundManager.instance.sfxVolume = slider.value;                 // 슬라이더 값 변경 시 SoundManager에 값 전달
    }
}
