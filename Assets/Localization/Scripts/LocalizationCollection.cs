using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class LocalizationCollection : ScriptableObject
{
    public LanguageData[] languages;

    public LanguageData fallback; 
}