using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattlePlayManager : MonoBehaviour
{
    public ParticleSystem gameEndWave;
    private void Awake()
    {
        EventCenter.GetInstance().AddEventListener(Global.Events.GameOver,GameEnd);
    }

    private void Start()
    {
        if (Global.Battle.battleData.isGainingChips)
        {
            
        }
        else
        {
            if (!Global.Battle.battleData.isTesting)
            {
                EventCenter.GetInstance().EventTrigger(Global.Events.GameStart);
                Global.DoTweenWait(8f, () =>
                {
                    EventCenter.GetInstance().EventTrigger(Global.Events.StartSpawningEnemy);
                    IncreaseChaos();
                });
                Global.DoTweenWait(7f, () =>
                {
                    EventCenter.GetInstance().EventTrigger(Global.Events.LoadInitialUI);
                });
            
                Global.DoTweenWait(1.5f, () =>
                {
                    EventCenter.GetInstance().EventTrigger(Global.Events.EnterBattleAnimation);
                });
                Global.DoTweenWait(Global.Battle.battleData.bossAppearTime, () =>
                {
                    EventCenter.GetInstance().EventTrigger(Global.Events.BossAppear);
                });
            }
            else
            {
                Global.DoTweenWait(0.5f, () =>
                {
                    EventCenter.GetInstance().EventTrigger(Global.Events.StartSpawningEnemy);
                });
                Global.DoTweenWait(1f, () =>
                {
                    EventCenter.GetInstance().EventTrigger(Global.Events.LoadInitialUI);
                });
                Global.DoTweenWait(5f, () =>
                {
                    EventCenter.GetInstance().EventTrigger(Global.Events.BossAppear);
                });
            }
        }
    }

    public void IncreaseChaos()
    {
        Global.DoTweenWait(10f, () =>
        {
            EventCenter.GetInstance().EventTrigger(Global.Events.UpdateChaos, Global.Battle.battleData.chaosIncreasePerTenSeconds);
            IncreaseChaos();
        });
    }

    public void GameEnd()
    {
        if (!Global.Battle.battleTimer.timeEnd)
        {
            EventCenter.GetInstance().EventTrigger(Global.Events.PlayAudioSoundEffect, GameAudios.AudioName.Death);
            Global.Battle.playerBehaviourController.playerCollider.enabled = false;
            Global.Battle.playerGetHitController.playerGetHitCollider.enabled = false;
            Global.Battle.enemyManager.stopSpawningEnemy = true;
            Global.DoTweenWait(0.2f, () =>
            {
                Global.Battle.battleUIManager.gameOverPage.gameObject.SetActive(true);
                Global.Battle.battleUIManager.gameOverPage.Appear();
            });
            Global.DoTweenWait(4f, () =>
            {
                gameEndWave.Play();
                EventCenter.GetInstance().EventTrigger(Global.Events.PlayAudioSoundEffect, GameAudios.AudioName.KillWave);
                Global.Battle.battleUIManager.gameOverPage.gameObject.SetActive(false);
                EventCenter.GetInstance().EventTrigger(Global.Events.KillAllEnemies);
            });
            Global.DoTweenWait(5f, () =>
            {
                Global.Battle.battleUIManager.blackCoverCanvasGroup.gameObject.SetActive(true);
                Global.Battle.battleUIManager.blackCoverCanvasGroup.DOFade(1f, 1f).From(0f).SetEase(Ease.Linear);
            });
            Global.DoTweenWait(6f, () =>
            {
                Global.Battle.battleData.isGainingChips = true;
                DOTween.KillAll();
                EventCenter.GetInstance().Clear();
                Global.Battle.audioManager.StopBgm();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            });
        }
        else
        {
            Global.Battle.enemyManager.stopSpawningEnemy = true;
            gameEndWave.Play();
            EventCenter.GetInstance().EventTrigger(Global.Events.PlayAudioSoundEffect, GameAudios.AudioName.KillWave);
            EventCenter.GetInstance().EventTrigger(Global.Events.KillAllEnemies);
            
            Global.DoTweenWait(2f, () =>
            {
                Global.Battle.battleUIManager.gameWinPage.gameObject.SetActive(true);
                Global.Battle.battleUIManager.gameWinPage.Appear();
            });
            
            Global.DoTweenWait(5f, () =>
            {
                Global.Battle.battleUIManager.blackCoverCanvasGroup.gameObject.SetActive(true);
                Global.Battle.battleUIManager.blackCoverCanvasGroup.DOFade(1f, 1f).From(0f).SetEase(Ease.Linear);
            });
            Global.DoTweenWait(6f, () =>
            {
                Global.Battle.battleData.isGainingChips = true;
                DOTween.KillAll();
                EventCenter.GetInstance().Clear();
                Global.Battle.audioManager.StopBgm();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            });
        }
    }
}
