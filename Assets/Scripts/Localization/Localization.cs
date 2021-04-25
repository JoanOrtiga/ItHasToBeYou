using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Localization : MonoBehaviour
{
    [SerializeField] private TextAsset localizator;

    public enum Languages
    {
        English,
        Espanol,
        Catala
    }

    public Languages currentLanguage;

    void Awake()
    {
        Dictionary<Languages, Dictionary<string, string>> data = CSVLoader.LoadCSV(localizator);

        print("name " + data[currentLanguage]["key"] + " " +
              "age " + data[currentLanguage]["menu_start"] + " " +
              "speed " + data[currentLanguage]["menu_options"] + " " +
              "desc " + data[currentLanguage]["menu_exit"]);
    }
}