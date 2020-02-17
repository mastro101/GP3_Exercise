using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

public class Importer : MonoBehaviour
{
    [MenuItem("Localization/Import")]
    public static void Import()
    {
        string path = string.Format("{0}/Localization/Localization_Example.csv", Application.dataPath);
        string[] lines = System.IO.File.ReadAllLines(path);

        Debug.LogFormat("lines: {0}", lines.Length);
        
        string[] firstCols = lines[0].Split(',');
        LanguageData[] languages = new LanguageData[firstCols.Length - 3];
        for (int i = 3; i < firstCols.Length; i++)
        {
            // Prelevo il codice lingua
            string languageCode = firstCols[i];

            // creo il languageData per la lingua, inizializzo e lo salvo
            string languagePath = string.Format("Assets/Localization/Data/LanguageData/{0}.asset", languageCode);
            LanguageData language;
            language = AssetDatabase.LoadAssetAtPath<LanguageData>(languagePath);

            if (language == null)
            {
                language = ScriptableObject.CreateInstance<LanguageData>();
                AssetDatabase.CreateAsset(language, languagePath);
            }

            language.languageCode = languageCode;
            language.values = new string[lines.Length - 1];
            language.keys = new string[language.values.Length];


            // lo salvo in un array per referenziarlo in futuro
            languages[i - 3] = language;
        }

        for (int i = 1; i < lines.Length; i++)
        {
            string[] cols = lines[i].Split(',');

            for (int langIndex = 3; langIndex < cols.Length; langIndex++)
            {
                try
                {
                    languages[langIndex - 3].keys[i-1] = cols[0];
                    languages[langIndex - 3].values[i-1] = cols[langIndex];
                    EditorUtility.SetDirty(languages[langIndex - 3]);
                }

                catch (System.Exception e)
                {
                    Debug.LogErrorFormat("Error importing row {0} col {1}", i, langIndex);
                    Debug.LogException(e);
                }
            }
        }

        AssetDatabase.Refresh();

        CheckConsistency(languages);

        BuildCharacterCollection(languages);
    }

    private static void BuildCharacterCollection(LanguageData[] languages)
    {
        var china = languages.Where(a => a.iso == "zh").FirstOrDefault();

        List<char> characters = new List<char>();
        foreach (var item in china.values)
        {
            foreach (var c in item)
            {
                if (c == 10 || c == 13) continue;
                if (!characters.Contains(c)) characters.Add(c);
            }
        }

        string alls = string.Join("", characters.Select(a => a.ToString()).ToArray());

        System.IO.File.WriteAllText(Application.dataPath + "/Localization/china_usage.txt", alls);
    }

    static void CheckConsistency(LanguageData[] languages)
    {
        var eng = languages.Where(a => a.iso == "en").FirstOrDefault();

        if (eng == null) Debug.LogErrorFormat("English not found");

        for (int i = 0; i < eng.keys.Length; i++)
        {
            string key = eng.keys[i];
            string eng_value = eng.values[i];

            int numberOfBraces_eng_open = eng_value.Count(a => a == '{');
            int numberOfBraces_eng_close = eng_value.Count(a => a == '}');

            if (numberOfBraces_eng_open != numberOfBraces_eng_close)
            {
                Debug.LogErrorFormat("English not found");
            }

            foreach (var language in languages)
            {
                string lang_value = language.values[i];

                int numberOfBraces_lang_open =  lang_value.Count(a => a == '{');
                int numberOfBraces_lang_close = lang_value.Count(a => a == '}');

                if (numberOfBraces_eng_open != numberOfBraces_eng_close)
                {
                    Debug.LogErrorFormat("English not found");
                }

                for (int index = 0; index < numberOfBraces_lang_open; index++)
                {
                    if (lang_value.IndexOf("{" + index.ToString()) == -1)
                        Debug.LogErrorFormat("Format string index {0} not found on key {1} for language {2}", index , key, language.name);
                }
            }
        }
    }
}