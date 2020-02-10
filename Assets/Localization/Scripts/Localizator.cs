using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class Localizator
{
    static LocalizationCollection collections;
    static LanguageData currentLanguage;

    static Localizator()
    {
        collections = Resources.Load<LocalizationCollection>("LocalizationCollection");
        SetLanguage();
    }

    private static void SetLanguage()
    {
        string customLanguage = PlayerPrefs.GetString("LANGUAGE", null);

        if (!string.IsNullOrEmpty(customLanguage))
        {
            // ....
            return;
        }

        CultureInfo ci = CultureInfo.InstalledUICulture;

        currentLanguage = null;
        foreach (var lang in collections.languages)
        {
            if (lang.iso == ci.TwoLetterISOLanguageName) currentLanguage = lang;
        }

        if (currentLanguage == null) currentLanguage = collections.fallback;
    }
}