using UnityEngine;
using Slider = UnityEngine.UI.Slider;

public class VolumenSlider : MonoBehaviour
{
    public Slider _musicSlider = default;
    public Slider _SfxSlider = default;
    [SerializeField] private AudioSource musicSource, SfxSource = default;
    void Start()
    {
        _musicSlider.value = PlayerPrefs.GetFloat("Music", 0.5f);
        musicSource.volume = _musicSlider.value;
        _SfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        SfxSource.volume = _SfxSlider.value;
    }
    public void ChangeMusicSlider(float value)
    {
        _musicSlider.value = value;
        PlayerPrefs.SetFloat("Music",_musicSlider.value);
        musicSource.volume = _musicSlider.value;
    }

    public void ChangeSfxSlider(float value)
    {
        _SfxSlider.value = value;
        PlayerPrefs.SetFloat("SFXVolume", _SfxSlider.value);
        SfxSource.volume = _SfxSlider.value;
    }
}
