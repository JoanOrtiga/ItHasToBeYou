using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class CSVLoader
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = {'\"'};
    
    public static Dictionary<Localization.Languages, Dictionary<string, string>> LoadCSV(TextAsset data) //filePath
    {
        var list = new Dictionary<Localization.Languages, Dictionary<string, string>>();

        if (data is null)
        {
            return null;
        }
        
        var lines = Regex.Split(data.text, LINE_SPLIT_RE);
        

        if (lines.Length <= 1) return list;

        var header = Regex.Split(lines[0], SPLIT_RE); //key	EN	ESP	CAT

        for (var i = 1; i < lines.Length; i++) //Comença per menu_language	English	Castellano	Català
        {
            var values = Regex.Split(lines[i], SPLIT_RE);

            if (values.Length == 0 || values[0] == "") continue;

            var entry = new Dictionary<string, string>();
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                object finalvalue = value;
                int n;
                float f;
                if (int.TryParse(value, out n))
                {
                    finalvalue = n;
                }
                else if (float.TryParse(value, out f))
                {
                    finalvalue = f;
                }

              //  entry[header[j]] = finalvalue;
            }

           // list.Add(entry);
        }
        
        return list;
    }
}