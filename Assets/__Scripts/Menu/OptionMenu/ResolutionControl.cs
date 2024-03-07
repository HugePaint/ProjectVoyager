using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionControl : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;

    private Resolution selectedResolution;
    private List<Resolution> filteredResolutions;

    private float currentRefreshRate;
    private int currentResolutionIndex = 0;
    
    private const string resolutionWidthPlayerPrefKey = "ResolutionWidth";
    private const string resolutionHeightPlayerPrefKey = "ResolutionHeight";
    private const string resolutionRefreshRatePlayerPrefKey = "RefreshRate";
    private const string fullScreenPlayerPrefKey = "FullScreen";

    // Start is called before the first frame update
    void Awake()
    {
        List<Resolution> resolutions = new List<Resolution>(Screen.resolutions);
        resolutions.Reverse();
        filteredResolutions = new List<Resolution>();
        
        resolutionDropdown.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRate;

        Debug.Log("RefreshRate: " + currentRefreshRate);
        
        // for (int i = resolutions.Length - 1; i >= 0; i--)
        for (int i = 0; i < resolutions.Count; i++)
        {
            Debug.Log("Added Resolution: " + resolutions[i]);
            if (Mathf.Approximately(resolutions[i].refreshRate, currentRefreshRate))
            {
                filteredResolutions.Add(resolutions[i]);
            }
        }
#if (UNITY_EDITOR) || (UNITY_STANDALONE)
        LoadSettings();
#endif
        
        List<string> options = new List<string>();
        for (int i = 0; i < filteredResolutions.Count; i++)
        // for (int i = filteredResolutions.Count - 1; i >= 0; i--)
        {
            string resolutionOption =
                $"{filteredResolutions[i].width}x{filteredResolutions[i].height} {filteredResolutions[i].refreshRate} Hz";
            options.Add(resolutionOption);
            if (Mathf.Approximately(filteredResolutions[i].width, selectedResolution.width) 
                && Mathf.Approximately(filteredResolutions[i].height, selectedResolution.height))
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.SetValueWithoutNotify(currentResolutionIndex);
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
#if (UNITY_EDITOR) || (UNITY_STANDALONE)
        selectedResolution = filteredResolutions[resolutionIndex];
        currentResolutionIndex = resolutionIndex;
        Debug.Log("Set Resolution: " + selectedResolution);
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, fullscreenToggle.isOn);
        
        PlayerPrefs.SetInt(resolutionWidthPlayerPrefKey, selectedResolution.width);
        PlayerPrefs.SetInt(resolutionHeightPlayerPrefKey, selectedResolution.height);
        PlayerPrefs.SetInt(resolutionRefreshRatePlayerPrefKey, selectedResolution.refreshRate);
#endif
    }

    public void SetFullscreen()
    {
        Debug.Log("Fullscreen: " + fullscreenToggle.isOn);
        Screen.fullScreen = fullscreenToggle.isOn;
        
        PlayerPrefs.SetInt(fullScreenPlayerPrefKey, fullscreenToggle.isOn ? 1 : 0);
    }
    
    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey(fullScreenPlayerPrefKey))
        {
            selectedResolution = new Resolution
            {
                width = PlayerPrefs.GetInt(resolutionWidthPlayerPrefKey, Screen.currentResolution.width),
                height = PlayerPrefs.GetInt(resolutionHeightPlayerPrefKey, Screen.currentResolution.height),
                refreshRate = PlayerPrefs.GetInt(resolutionRefreshRatePlayerPrefKey, Screen.currentResolution.refreshRate)
            };

            fullscreenToggle.isOn = PlayerPrefs.GetInt(fullScreenPlayerPrefKey, Screen.fullScreen ? 1 : 0) > 0;
        }
        else
        {
            selectedResolution = filteredResolutions[0];
            fullscreenToggle.isOn = true;
        }

        Screen.SetResolution(
            selectedResolution.width,
            selectedResolution.height,
            fullscreenToggle.isOn
        );
        
        PlayerPrefs.SetInt(resolutionWidthPlayerPrefKey, selectedResolution.width);
        PlayerPrefs.SetInt(resolutionHeightPlayerPrefKey, selectedResolution.height);
        PlayerPrefs.SetInt(resolutionRefreshRatePlayerPrefKey, selectedResolution.refreshRate);
        PlayerPrefs.SetInt(fullScreenPlayerPrefKey, fullscreenToggle.isOn ? 1 : 0);
    }
}
