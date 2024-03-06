using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject BlackCurtain;
    [SerializeField] private GameObject EnterGameCamera;

    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject chipMenuPanel;
    [SerializeField] private GameObject preGameMenuPanel;
    [SerializeField] private GameObject cannonOverviewMenuPanel;
    [SerializeField] private GameObject upgradeMenuPanel;
    [SerializeField] private GameObject optionsMenuPanel;
    [SerializeField] private GameObject creditsMenuPanel;
    [SerializeField] private GameObject languageMenuPanel;
    [SerializeField] private GameObject splashPanel;
    
    public float curtainFadeOutTime = 3f;
    public const string FirstStartupKey = "IsFirstStartUp";
    
    // Start is called before the first frame update
    void Start()
    {
        Global.MainMenu.menuController = this;
        mainMenuPanel.SetActive(true);
        chipMenuPanel.SetActive(true);
        preGameMenuPanel.SetActive(true);
        cannonOverviewMenuPanel.SetActive(true);
        upgradeMenuPanel.SetActive(true);
        optionsMenuPanel.SetActive(true);
        creditsMenuPanel.SetActive(true);
        languageMenuPanel.SetActive(false);
        BlackCurtain.SetActive(true);
        splashPanel.SetActive(false);
        
        if (PlayerPrefs.HasKey(FirstStartupKey))
            ShowSplash();
        else
            languageMenuPanel.SetActive(true);
    }

    public void ShowSplash()
    {
        if (Global.BetweenMenuAndBattle.playerInitialCannons == null)
        {
            splashPanel.SetActive(true);
            var splashAnimator = splashPanel.GetComponent<SplashAnimator>();
            splashAnimator.Animate();
        }
        else
        {
            splashPanel.SetActive(false);
            ShowMainMenu();
        }
    }
    
    public void ShowMainMenu()
    {
        RawImage ri = BlackCurtain.GetComponent<RawImage>();
        Tweener fadeOutCurtain = ri.DOColor(Color.clear, curtainFadeOutTime).From(Color.black).SetEase(Ease.InCubic);
        fadeOutCurtain.OnComplete(() =>
        {
            EventCenter.GetInstance().EventTrigger(Global.Events.MenuMainFirstShow);
            DOTween.To(() => 0, x => _ = x, 1f, 1f)
                .OnComplete(() =>
                {
                    BlackCurtain.SetActive(false);
                    Global.DoTweenWait(0.55f, () =>
                        // EventCenter.GetInstance().EventTrigger(Global.Events.PlayAudioBattleBgm,GameAudios.AudioName.MenuBgm));
                        Global.MainMenu.menuBgmManager.MainMenuInitialize());
                });
        });
        
    }

    public void EnterGame()
    {
        EnterGameCamera.SetActive(true);
        
        var Vcam = EnterGameCamera.GetComponent<CinemachineVirtualCamera>();
        float endFOV = 180f;
        Tweener changeFOV = DOTween.To(() => Vcam.m_Lens.FieldOfView, x => Vcam.m_Lens.FieldOfView = x, endFOV, 2f)
            .SetEase(Ease.OutQuint);
        
        Vector3 targetRotate = EnterGameCamera.transform.rotation.eulerAngles;
        targetRotate.x += 180;
        Tweener moveCamera = EnterGameCamera.transform.DORotate(targetRotate, 5f).SetEase(Ease.InQuad).SetDelay(0.5f);
     
        EventCenter.GetInstance().EventTrigger(Global.Events.MenuPreGameHide);
        // EventCenter.GetInstance().EventTrigger(Global.Events.FadeOutBgm, GameAudios.AudioName.MenuBgm);
        Global.MainMenu.menuBgmManager.StopAll(true, 0.5f);
        EventCenter.GetInstance().EventTrigger(Global.Events.PlayAudioSoundEffect, GameAudios.AudioName.MenuEnterBattleSE);
        
        BlackCurtain.SetActive(true);
        RawImage ri = BlackCurtain.GetComponent<RawImage>();
        ri.color = Color.clear;
        Tweener fadeInCurtain = ri.DOColor(Color.black, 3f).SetDelay(1f).OnComplete(() =>
        {
            EventCenter.GetInstance().EventTrigger(Global.Events.MenuGameStart);
            EventCenter.GetInstance().Clear();
        });
        
    }

    public void CannonOverviewToCannonChipSetting(int cannonIndex)
    {
        Global.MainMenu.cannonDisplayController.UpdateSlot(cannonIndex);
        Global.MainMenu.inventoryDisplayController.UpdateAll();
        
        EventCenter.GetInstance().EventTrigger<int>(Global.Events.MenuCannonChipSettingShow, cannonIndex);
        EventCenter.GetInstance().EventTrigger(Global.Events.MenuCannonOverviewHide);
    }
    
    public void CannonChipSettingToCannonOverview()
    {
        EventCenter.GetInstance().EventTrigger(Global.Events.MenuCannonChipSettingHide);
        EventCenter.GetInstance().EventTrigger(Global.Events.MenuCannonOverviewShow);
    }
    
    public void MainToPreGame()
    {
        // Global.MainMenu.menuBgmManager.PlayBGM(MenuBgmManager.MenuLevel.PreGame);
        EventCenter.GetInstance().EventTrigger(Global.Events.MenuMainHide); 
        EventCenter.GetInstance().EventTrigger(Global.Events.MenuPreGameShow); 
    }

    public void PreGameToMain()
    {
        // Global.MainMenu.menuBgmManager.PlayBGM(MenuBgmManager.MenuLevel.Main);
        EventCenter.GetInstance().EventTrigger(Global.Events.MenuPreGameHide);
        EventCenter.GetInstance().EventTrigger(Global.Events.MenuMainShow);
    }
    
    public void MainToOptions()
    {
        // Global.MainMenu.menuBgmManager.PlayBGM(MenuBgmManager.MenuLevel.Options);
        EventCenter.GetInstance().EventTrigger(Global.Events.MenuMainHide); 
        EventCenter.GetInstance().EventTrigger(Global.Events.MenuOptionsShow); 
    }

    public void OptionsToMain()
    {
        // Global.MainMenu.menuBgmManager.PlayBGM(MenuBgmManager.MenuLevel.Main);
        EventCenter.GetInstance().EventTrigger(Global.Events.MenuMainShow); 
        EventCenter.GetInstance().EventTrigger(Global.Events.MenuOptionsHide); 
    }
    
    public void MainToCredits()
    {
        if (Random.Range(0, 4) == 0)
            Global.MainMenu.menuBgmManager.PlayBGM(MenuBgmManager.MenuLevel.Special);
        else
            Global.MainMenu.menuBgmManager.PlayBGM(MenuBgmManager.MenuLevel.Credits);
        
        EventCenter.GetInstance().EventTrigger(Global.Events.MenuMainHide); 
        EventCenter.GetInstance().EventTrigger(Global.Events.MenuCreditsShow); 
    }

    public void CreditsToMain()
    {
        Global.MainMenu.menuBgmManager.PlayBGM(MenuBgmManager.MenuLevel.Main);
        EventCenter.GetInstance().EventTrigger(Global.Events.MenuCreditsHide);
        // Global.DoTweenWait(0.5f, () =>
        // {
        //     EventCenter.GetInstance().EventTrigger(Global.Events.MenuMainShow);
        // });
        EventCenter.GetInstance().EventTrigger(Global.Events.MenuMainShow);
    }
    
    public void PreGameToCannonOverview()
    {
        // Global.MainMenu.menuBgmManager.PlayBGM(MenuBgmManager.MenuLevel.CannonOverview);
        Global.MainMenu.cannonDisplayController.UpdateAll();
        EventCenter.GetInstance().EventTrigger(Global.Events.MenuPreGameHide);
        EventCenter.GetInstance().EventTrigger(Global.Events.MenuCannonOverviewShow);
    }
    
    public void CannonOverviewToPreGame()
    {
        // Global.MainMenu.menuBgmManager.PlayBGM(MenuBgmManager.MenuLevel.PreGame);
        // Global.MainMenu.menuBgmManager.PlayBGM(MenuBgmManager.MenuLevel.Normal);
        EventCenter.GetInstance().EventTrigger(Global.Events.MenuCannonOverviewHide);
        EventCenter.GetInstance().EventTrigger(Global.Events.MenuPreGameShow);
    }
    
    public void PreGameToUpgrade()
    {
        EventCenter.GetInstance().EventTrigger(Global.Events.MenuPreGameHide);
        EventCenter.GetInstance().EventTrigger(Global.Events.MenuUpgradeShow);
        Global.DoTweenWait(0f, () =>
        {
            Global.MainMenu.upgradeInventoryDisplayController.InitializeUpgrade();
        });
    }
    
    public void UpgradeToPreGame()
    {
        EventCenter.GetInstance().EventTrigger(Global.Events.MenuUpgradeHide);
        EventCenter.GetInstance().EventTrigger(Global.Events.MenuPreGameShow);
    }

    public void GetTestChips()
    {
        Global.MainMenu.menuBgmManager.PlayBGM(MenuBgmManager.MenuLevel.Special);
        
        Global.MainMenu.playerDataManager.Test();
        
        Debug.Log($"GetTestChips: Chip Test Set Added.");
        
        Global.MainMenu.cannonDisplayController.UpdateAll();
        Global.MainMenu.inventoryDisplayController.UpdateAll();
    }

    public void GetRandomChip(int rarity)
    {
        var newChip = BattleRewardManager.CreateChip((Global.Misc.Rarity)rarity);
        Global.MainMenu.playerDataManager.AddItem(newChip);
        
        Debug.Log($"GetRandomChip: {rarity} Chip Added. {newChip.modificationEffectIDs.Count} Mod Effects in total.");

        Global.MainMenu.cannonDisplayController.UpdateAll();
        Global.MainMenu.inventoryDisplayController.UpdateAll();
    }

    public void DeleteSaveAndReload()
    {
        System.IO.File.Delete(Global.Misc.savePath);
        PlayerPrefs.DeleteAll();
        DOTween.KillAll();
        EventCenter.GetInstance().Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OpenSaveFolder()
    {
        GUIUtility.systemCopyBuffer = Application.persistentDataPath;
        System.Diagnostics.Process.Start("explorer.exe", "/select,"+ Application.persistentDataPath);
    }

    public void Reload() {
        DOTween.KillAll();
        EventCenter.GetInstance().Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}