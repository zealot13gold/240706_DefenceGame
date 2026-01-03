using UnityEngine;
using UnityEngine.UI;

public class BGMSlider : SoundSlider
{
    private void OnEnable()
    {
        //.instance.bgmVolumeChanged += BringValue;
        slider.value = SoundManager.instance.bgmVolume;             // 활성화 시 SoundManager의 값을 가져옴
    }

    private void OnDisable()
    {
        //SoundManager.instance.bgmVolumeChanged -= BringValue;
    }

    public override void ChangeVolume()
    {
        SoundManager.instance.bgmVolume = slider.value;                 // 슬라이더 값 변경 시 SoundManager에 값 전달
    }
}
