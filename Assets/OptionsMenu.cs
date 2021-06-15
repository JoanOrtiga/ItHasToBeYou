using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullScreen;
    Resolution[] resolutions;

    [SerializeField] private RenderTexture renderTexture;

    [SerializeField] private Slider masterVolume;
    [SerializeField] private Slider voicesVolume;
    [SerializeField] private Slider SFXVolume;
    void Awake()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for(int i=0; i <resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if(resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();


      //  renderTexture.width = resolutions[currentResolutionIndex].width;
        //renderTexture.height = resolutions[currentResolutionIndex].height;
        
        gameObject.SetActive(false);
       
    }

    public void ChangeResolution(int option)
    {
        Screen.SetResolution(resolutions[option].width, resolutions[option].height, fullScreen.isOn);
    }

    public void ChangeFullScreen(bool value)
    {
        Screen.fullScreen = value;
    }
}
