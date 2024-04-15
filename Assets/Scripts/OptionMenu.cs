using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using System;

public class OptionMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Dropdown resolutionDropdown;
    public Slider volumeSlider;
    public Toggle fullscreenToggle;
    float currentVolume;
    Resolution[] resolutions;

    private void Start()
    {
        volumeSlider.value = currentVolume;
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
        currentVolume = volume;
    }
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, 
            resolution.height, Screen.fullScreen);
    }
    public void SaveSettings()
    {
        PlayerPrefs.SetInt("FullscreenPreference", 
            Convert.ToInt32(Screen.fullScreen));
        PlayerPrefs.SetFloat("VolumePreference", 
            currentVolume); 
        OnBackButton();
    }
    
    
    public void OnBackButton()
    {
        SceneManager.LoadScene(0);
    }
}
