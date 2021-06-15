using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    private Transform player;
    public static bool IsPaused { get; private set; }

    [SerializeField] private AudioMixer mixer;

    public GameObject pauseMenu;
    public GameObject optionsMenu;

    [SerializeField] private Slider[] soundSliders;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;

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
        if (Input.GetKeyDown(KeyCode.T) || Input.GetKeyDown(KeyCode.Escape) && IsPaused)
        {
          
            UnPause();
        }
        else if (Input.GetKeyDown(KeyCode.T) || Input.GetKeyDown(KeyCode.Escape) && IsPaused == false)
        {
          
            Pause();
        }
    }

    private void Pause()
    {
        Cursor.visible = true;
        FMODUnity.RuntimeManager.PlayOneShot("event:/MENU/Select/Tracks", player.position);
        Cursor.lockState = CursorLockMode.Confined;
        IsPaused = true;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    private void UnPause()
    {
        Cursor.visible = false;
        FMODUnity.RuntimeManager.PlayOneShot("event:/MENU/Return/Tracks", player.position);
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

    public void ShowOptionsMenu()
    {
        optionsMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void HideOptionsMenu()
    {
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(true);
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