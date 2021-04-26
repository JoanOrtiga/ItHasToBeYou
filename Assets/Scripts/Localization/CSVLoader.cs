using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System;

public class CSVLoader
{
    private static char endLine = '\n';
    private static char separator = ',';
    
    public static Dictionary<string, LanguageData> ReadSheet(TextAsset data)
    {
        Dictionary<string, LanguageData> languageDatas = new Dictionary<string, LanguageData>();
        
        string[] lines = data.text.Split(new char[]{endLine});
        for (int i = 1; i < lines.Length; i++)
        {
            if(lines.Length > 1)
                AddNewDataEntry(lines[i], ref languageDatas);
        }

        return languageDatas;
    }

    private static void AddNewDataEntry(string s, ref Dictionary<string, LanguageData> languageDatas)
    {
        string[] t = s.Split(new char[] { separator });
        var languageData = new LanguageData(t); 
        
        languageDatas.Add(t[0], languageData);
    }
    
}



