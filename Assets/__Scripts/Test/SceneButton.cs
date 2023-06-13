using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneButton : MonoBehaviour
{
    public Button button;

    private void Awake()
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(Reload);
    }

    public void Reload()
    {
        Global.Battle.chipGainUIManager.blackCoverCanvasGroup.gameObject.SetActive(true);
        Global.Battle.chipGainUIManager.clickCover.SetActive(true);
        Global.Battle.chipGainUIManager.blackCoverCanvasGroup.transform.SetAsLastSibling();
        Global.Battle.chipGainUIManager.blackCoverCanvasGroup.DOFade(1f, 1f).From(0f).SetEase(Ease.Linear);
        Global.Battle.battleBgmManager.BGMChipGainFadeOut();
        Global.DoTweenWait(2f, () =>
        {
            EventCenter.GetInstance().Clear();
            DOTween.KillAll();
            Global.Battle.audioManager.StopBgm();
            Global.Battle.battleData.isGainingChips = false;
            SceneManager.LoadScene("MainMenu");
        });
    }
}
