using UnityEngine;
using UnityEngine.UI;

public class UITextLocalizator : MonoBehaviour
{
    private Text text;
    public string textKey;
    
    private void OnEnable()
    {
        Localization.OnLanguageChangeDelegate += OnLanguageChanged;
    }

    private void OnDisable()
    {
        Localization.OnLanguageChangeDelegate -= OnLanguageChanged;
    }

    void Start()
    {
        text = GetComponent<Text>();
        SetText();
    }
    
    private void OnLanguageChanged()
    {
        SetText();
    }
    
    private void SetText()
    {
        text.text = Localization.GetText(textKey);
    }
}
