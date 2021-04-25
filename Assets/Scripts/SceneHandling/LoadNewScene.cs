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

    public void LoadNextScene()
    {
        StartCoroutine(LoadScene(sceneToLoad));
    }

    private IEnumerator LoadScene(string _sceneToLoad)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_sceneToLoad);
        
        asyncLoad.allowSceneActivation = false;
        
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            loadBar.fillAmount = progress;
            yield return null;

            if (progress >= 0.9f)
            {
                yield return new WaitForSeconds(2);
                asyncLoad.allowSceneActivation = true;

            }
          
        }
        
        
    }
}
