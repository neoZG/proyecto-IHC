using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    // Sliders for music and voice
    public Slider musicSlider;
    public Slider voiceSlider;

    // AudioSources for background music and voice
    public AudioSource musicSource;
    public AudioSource voiceSource;

    private void Start()
    {
        // Initialize sliders with current audio volumes
        if (musicSource != null)
        {
            musicSlider.value = musicSource.volume;
            musicSlider.onValueChanged.AddListener(AdjustMusicVolume);
        }
        else
        {
            Debug.LogWarning("Music AudioSource not assigned!");
        }

        if (voiceSource != null)
        {
            voiceSlider.value = voiceSource.volume;
            voiceSlider.onValueChanged.AddListener(AdjustVoiceVolume);
        }
        else
        {
            Debug.LogWarning("Voice AudioSource not assigned!");
        }
    }

    private void AdjustMusicVolume(float value)
    {
        if (musicSource != null)
        {
            musicSource.volume = value;
        }
    }

    private void AdjustVoiceVolume(float value)
    {
        if (voiceSource != null)
        {
            voiceSource.volume = value;
        }
    }
}
