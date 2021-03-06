using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullScreen;
    Resolution[] resolutions;

    [SerializeField] private Camera seeThroughtCamera;
    [SerializeField] private Material seeThroughtMaterial;

    [SerializeField] private Slider masterVolume;
    [SerializeField] private Slider voicesVolume;
    [SerializeField] private Slider SFXVolume;

    public CameraController playerCameraController;
    
    [SerializeField] private PostProcessVolume volume;
    private ColorGrading colorGrading;
    
    void Awake()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            
                options.Add(option);
            
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();


        UpdateRenderTexture(currentResolutionIndex);

        gameObject.SetActive(false);
        
        volume.profile.TryGetSettings(out colorGrading);

    }

    public void ChangeResolution(int option)
    {
        Screen.SetResolution(resolutions[option].width, resolutions[option].height, fullScreen.isOn);

        UpdateRenderTexture(option);
    }

    private void UpdateRenderTexture(int option)
    {
        seeThroughtCamera.targetTexture.Release();
        seeThroughtCamera.targetTexture = new RenderTexture(resolutions[option].width, resolutions[option].height, 24);
        seeThroughtMaterial.mainTexture = seeThroughtCamera.targetTexture;
    }
    
    public void ChangeQualitySettings(int quality)
    {
        QualitySettings.SetQualityLevel(quality);
    }

    public void ChangeFullScreen(bool value)
    {
        Screen.fullScreen = value;
    }

    public void ChangeSensivity(float sensitivity)
    {
        playerCameraController.SetSensitivity((int)sensitivity);
    }

    public void ChangeBrightness(float brightness)
    {
        colorGrading.postExposure.value = brightness;
    }
}