using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LocaleControl : MonoBehaviour
{
    public List<Toggle> toggles;
    public List<Locale> locales;

    public const string localePrefKey = "Locale";
    
    private void Awake()
    {
        // LoadSettings();
    }

    private void Start()
    {
        LoadSettings();
    }
    
    private void LoadSettings()
    {
        string currentLocale = PlayerPrefs.GetString(localePrefKey);
        LocaleIdentifier localeCode = new LocaleIdentifier(currentLocale);//can be "en" "de" "ja" etc.
        foreach (var aLocale in locales)
        {
            LocaleIdentifier anIdentifier = aLocale.Identifier;
            if(anIdentifier == localeCode)
            {
                LocalizationSettings.SelectedLocale = aLocale;
            }
        }
        
        for (int i = 0; i < locales.Count; i++)
        {
            toggles[i].SetIsOnWithoutNotify(
                (locales[i].Identifier == LocalizationSettings.SelectedLocale.Identifier));
        }
    }

    public void MatchSettingsOnDisplay()
    {
        for (int i = 0; i < locales.Count; i++)
        {
            toggles[i].SetIsOnWithoutNotify(
                (locales[i].Identifier == LocalizationSettings.SelectedLocale.Identifier));
        }
    }
    
    // public static void SetLocale(string languageIdentifier)
    // {
    //     LocalizationSettings.SelectedLocale.Identifier = languageIdentifier;
    //     PlayerPrefs.SetString(localePrefKey, languageIdentifier);
    // }
}
