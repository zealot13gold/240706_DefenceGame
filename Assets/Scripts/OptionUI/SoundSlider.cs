using System;
using UnityEngine;
using UnityEngine.UI;

// 역할: SoundManager에 값 변화 이벤트를 전달

public abstract class SoundSlider : MonoBehaviour
{
    public Slider slider;
    public event Action<float> audioSources;

    protected void BringValue(float volume)
    {
        slider.value = volume;
    }

    public abstract void ChangeVolume();
}
