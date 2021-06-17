using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class EndGame : MonoBehaviour , IInteractable
{
    private PlayerController playerController;
    private bool active = false;

    [SerializeField] private Camera telescopeCamera;

    [SerializeField] private GameObject telescopeCanvas;
    [SerializeField] private Transform lastMoon;

    private Crosshair crosshair;

    [SerializeField] private GameObject[] canvasToDeActivate;

    [SerializeField] private Light endingLight;
    [SerializeField] private float speedOfLight;

    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private PostProcessVolume volume;
    private Bloom bloom;
    
    [SerializeField] private float speedOfBloom;
    [SerializeField] private float speedOfDiffusion;
    
    private bool waiting = false;

    [SerializeField] private GameObject canvasCredits;
    [SerializeField] private Transform canvasPanel;
    [SerializeField] private float creditsSpeed;


    private bool audioOnce;
    public TextBox textBoxFinal;
    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        crosshair = FindObjectOfType<Crosshair>();
        volume.profile.TryGetSettings(out bloom);
    }

    public void Interact()
    {
        if(!this.enabled)
            return;
        
        if(active)
            return;

        if (audioOnce == false)
        {
            audioOnce = true;
            textBoxFinal.StartTextPuzzle();
        }
        //FALTA REPRODUIR AUDIO FINAL
        
        active = true;
        playerController.DisableController(true, true, true, true);

        telescopeCamera.gameObject.SetActive(true);
       
        telescopeCamera.enabled = true;
        telescopeCamera.transform.LookAt(lastMoon);
        playerController.mainCamera.enabled = false;
        telescopeCanvas.SetActive(true);

        bloom.enabled.value = true;
        
        canvasCredits.SetActive(true);
        
        foreach (var useless in canvasToDeActivate)
        {
            useless.SetActive(false);
        }

        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        waiting = true;
    }
    private void Update()
    {
        if (active && waiting)
        {

            endingLight.intensity += Time.deltaTime * speedOfLight;
            
            bloom.intensity.value += Time.deltaTime * speedOfBloom ;      
            bloom.diffusion.value += Time.deltaTime * speedOfDiffusion;
            
            if (endingLight.intensity >= 30)
            {
                canvasGroup.alpha -= Time.deltaTime;
            }

            if (endingLight.intensity >= 31)
            {
                canvasPanel.transform.position += new Vector3(0, creditsSpeed * Time.deltaTime, 0);
            }
        }
    }

    private void LateUpdate()
    {
        if(active)
            crosshair.ChangeCrosshairState(false,false);
            
    }
}
