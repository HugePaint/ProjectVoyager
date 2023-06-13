using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QualitySettingsControl : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown qualityDropdown;
    private const string qualitySettingsPlayerPrefKey = "QualitySettings";

    private void Start()
    {
        LoadPref();
        InitializeDropdown();
    }

    private void InitializeDropdown()
    {
        qualityDropdown.ClearOptions();

        List<TMP_Dropdown.OptionData> qualityOptions = new List<TMP_Dropdown.OptionData>();
        foreach (string qualityLevel in QualitySettings.names)
        {
            qualityOptions.Add(new TMP_Dropdown.OptionData(qualityLevel));
        }

        qualityDropdown.AddOptions(qualityOptions);
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.RefreshShownValue();
        qualityDropdown.onValueChanged.AddListener(delegate { OnQualityChanged(qualityDropdown); });
    }

    public void OnQualityChanged(TMP_Dropdown change)
    {
        QualitySettings.SetQualityLevel(change.value);
        PlayerPrefs.SetInt(qualitySettingsPlayerPrefKey, change.value);
    }

    private void LoadPref()
    {
        int value = PlayerPrefs.GetInt(qualitySettingsPlayerPrefKey, QualitySettings.GetQualityLevel());
        QualitySettings.SetQualityLevel(value);
    }
}