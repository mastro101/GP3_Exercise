using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
            language.values = new Dictionary<string, string>();


            // lo salvo in un array per referenziarlo in futuro
            languages[i - 3] = language;
        }

        for (int i = 1; i < lines.Length; i++)
        {
            string[] cols = lines[i].Split(',');

            for (int langIndex = 3; langIndex < cols.Length; langIndex++)
            {
                languages[langIndex - 3].values.Add(cols[0], cols[langIndex]);
                EditorUtility.SetDirty(languages[langIndex - 3]);
            }
        }

        AssetDatabase.Refresh();
    }
}