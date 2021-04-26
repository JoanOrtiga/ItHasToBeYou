using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Localization : MonoBehaviour
{
    private static Localization Instance;
    
    [SerializeField] private TextAsset localizatorData;

    [SerializeField] private Language currentLanguage = Language.English;
    
    Dictionary<string, LanguageData> languageData;
    
    public delegate void LanguageChangeDelegate();
    public static LanguageChangeDelegate OnLanguageChangeDelegate;
    
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        
        Instance = this; 
        
        //We pass textAsset to CSV Loader, it returns a dictionary of languages.
        languageData = CSVLoader.ReadSheet(localizatorData);
    }
    
    
    /// <summary>
    /// Change Language of game. Updates everyone who is suscribed to delegate.
    /// </summary>
    /// <param name="language"></param>
    
    public static void SetLanguage(Language language)
    {
        Instance.currentLanguage = language;
        OnLanguageChangeDelegate?.Invoke();
    }
    
    /// <summary>
    /// Returns text in selected language given a key.
    /// It will return "wrong key" if key isn't found.
    /// </summary>
    /// <param name="textKey"></param>
    /// <returns></returns>
    public static string GetText(string textKey)
    {
        try
        {
            return Instance.languageData[textKey].GetText(Instance.currentLanguage);
        }
        catch
        {
            Debug.Log("Wrong key" + textKey);
            return "wrong key";
        }
    }
}