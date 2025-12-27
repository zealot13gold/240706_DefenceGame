using UnityEngine;
using UnityEngine.UI;

public abstract class SoundSlider : MonoBehaviour
{
    public Slider slider;

    protected void BringValue(float volume)
    {
        slider.value = volume;
    }

    public abstract void ChangeVolume();
}
