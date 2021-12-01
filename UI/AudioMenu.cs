using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioMenu : MonoBehaviour
{
    public delegate void SomethingChanged(AudioMenu audioMenu);
    public static event SomethingChanged OnSomeChange;

    public Slider MasterVolSlider;
    public Slider MusicVolSlider;

    public void OnEnable()
    {
        PositionSliders();
    }

    void PositionSliders()
    {
        float master = Sound.Guy.masterVol;
        float music = Sound.Guy.mixer.channels[Sound.Guy.music];
        MasterVolSlider.value = master;
        MusicVolSlider.value = music;
    }

    public void SomeChange() => OnSomeChange?.Invoke(this);
}
