using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadNewScene : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private Image loadBar;
    
    #if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(LoadScene(sceneToLoad));
        }
    }
    #endif

    private void Start()
    {
        LoadNextScene(sceneToLoad);
    }

    public void LoadNextScene(string _sceneToLoad)
    {
        StartCoroutine(LoadScene(_sceneToLoad));
    }

    private IEnumerator LoadScene(string sceneToLoad)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
        
        asyncLoad.allowSceneActivation = false;
        
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            loadBar.fillAmount = progress;
            yield return null;

            if (progress >= 0.9f)
            {
                //We add extra time in loading screen.
                yield return new WaitForSeconds(2);
                asyncLoad.allowSceneActivation = true;

            }
          
        }
        
        
    }
}
