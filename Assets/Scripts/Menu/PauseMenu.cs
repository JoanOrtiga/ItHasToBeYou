using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused { get; private set; }

    [SerializeField] private AudioMixer mixer;

    public GameObject pauseMenu;

    [SerializeField] private Slider[] soundSliders;

    private void Start()
    {
        if(soundSliders == null)
            return;
        
        if (soundSliders.Length > 0)
        {
            soundSliders[0].value = PlayerPrefs.GetFloat("MasterVolume");
            soundSliders[1].value = PlayerPrefs.GetFloat("MusicVolume");
            soundSliders[2].value = PlayerPrefs.GetFloat("SFXVolume");
            soundSliders[3].value = PlayerPrefs.GetFloat("VoicesVolume");
        }
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && IsPaused)
        {
            UnPause();
        }

        if (Input.GetKeyDown(KeyCode.T) && IsPaused is false)
        {
            Pause();
        }
    }

    private void Pause()
    {
        Cursor.lockState = CursorLockMode.Confined;
        IsPaused = true;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    private void UnPause()
    {
        Cursor.lockState = CursorLockMode.Locked;
        IsPaused = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void continueGame()
    {
        UnPause();
    }

    public void goToMainMenu()
    {
        UnPause();
    }

    public void MasterSlider(float value)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("MasterVolume", value);
    }
    public void SFXSlider(float value)
    {
        mixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }
    public void VoicesSlider(float value)
    {
        mixer.SetFloat("VoicesVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("VoicesVolume", value);
    }
    public void MusicSlider(float value)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }
}