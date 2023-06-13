using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LocaleSetter : MonoBehaviour
{
    public Toggle attachedToggle;

    public void SetLocale(string languageIdentifier)
    {
        if (attachedToggle.isOn == false) return;
        LocaleIdentifier localeCode = new LocaleIdentifier(languageIdentifier);//can be "en" "de" "ja" etc.
        foreach (var aLocale in LocalizationSettings.AvailableLocales.Locales)
        {
            LocaleIdentifier anIdentifier = aLocale.Identifier;
            if(anIdentifier == localeCode)
            {
                LocalizationSettings.SelectedLocale = aLocale;
            }
        }
        PlayerPrefs.SetString(LocaleControl.localePrefKey, languageIdentifier);
    }
}
