using UnityEngine;
using UnityEngine.UI;

public class AudioOptions : MonoBehaviour
{
    public GameObject audioOptionsPanel;
    public GameObject[] hideOnOpen;

    public Slider musicSlider;
    public Slider sfxSlider;

    public AudioSource musicSource;
    public AudioSource sfxPreviewSource;
    public AudioClip sfxPreviewClip;

    void Start()
    {
        audioOptionsPanel.SetActive(false);

        if (AudioManager.Instance != null)
        {
            musicSlider.value = AudioManager.Instance.MusicVolume;
            sfxSlider.value = AudioManager.Instance.SfxVolume;
        }

        musicSlider.onValueChanged.AddListener(OnMusicChanged);
        sfxSlider.onValueChanged.AddListener(OnSfxChanged);
    }

    public void OpenOptions()
    {
        audioOptionsPanel.SetActive(true);
        foreach (var obj in hideOnOpen)
            obj.SetActive(false);
    }

    public void CloseOptions()
    {
        audioOptionsPanel.SetActive(false);
        foreach (var obj in hideOnOpen)
            obj.SetActive(true);
    }

    void OnMusicChanged(float value)
    {
        AudioManager.Instance?.SetMusicVolume(value);
        if (musicSource != null)
            musicSource.volume = value * 0.6f;
    }

    void OnSfxChanged(float value)
    {
        AudioManager.Instance?.SetSfxVolume(value);
        if (sfxPreviewSource != null && sfxPreviewClip != null)
            sfxPreviewSource.PlayOneShot(sfxPreviewClip, value);
    }
}
