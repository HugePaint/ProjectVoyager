using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    public int id;
    public CannonBattle cannonAssigned;
    public Image weaponImage;
    public CanvasGroup weaponCanvasGroup;
    public Image coolDownIndicatorImage;
    public CanvasGroup coolDonIndicatorCanvasGroup;
    public Image fireIndicatorImage;
    public CanvasGroup fireIndicatorCanvasGroup;
    public ParticleSystem powerUpParticleSystem;
    public UnlockChipMoving unlockChipMoving;
    public Transform readyPosition;
    public Transform unreadyPosition;
    public TMP_Text levelText;
    public GameObject levelTextGameObject;
    public GameObject maxTextGameObject;
    public CanvasGroup levelTextCanvasGroup;

    public Sprite bulletSprite;
    public Sprite laserSprite;
    public Sprite energySprite;

    private void Awake()
    {
        coolDonIndicatorCanvasGroup.DOFade(0f, 0f);
        fireIndicatorCanvasGroup.DOFade(0f, 0f);
        weaponCanvasGroup.DOFade(0f, 0f);

        EventCenter.GetInstance().AddEventListener(Global.Events.LoadInitialUI, Appear);
        AssignEventListener();
        levelTextGameObject.SetActive(false);
        maxTextGameObject.SetActive(false);
    }

    private void Start()
    {
        cannonAssigned = Global.Battle.cannonBattleManager.cannonBattles[id];
        UpdateWeaponImage();
    }

    public void UnlockChipUIAnimation(Transform startPosition)
    {
        unlockChipMoving.gameObject.SetActive(true);
        unlockChipMoving.FlyToChip(startPosition, weaponImage.transform, id);
    }

    public void AssignEventListener()
    {
        switch (id)
        {
            case 0:
                EventCenter.GetInstance().AddEventListener(Global.Events.CannonZeroFire, Fire);
                break;
            case 1:
                EventCenter.GetInstance().AddEventListener(Global.Events.CannonOneFire, Fire);
                break;
            case 2:
                EventCenter.GetInstance().AddEventListener(Global.Events.CannonTwoFire, Fire);
                break;
            case 3:
                EventCenter.GetInstance().AddEventListener(Global.Events.CannonThreeFire, Fire);
                break;
            case 4:
                EventCenter.GetInstance().AddEventListener(Global.Events.CannonFourFire, Fire);
                break;
            case 5:
                EventCenter.GetInstance().AddEventListener(Global.Events.CannonFiveFire, Fire);
                break;
        }
    }

    private void UpdateWeaponImage()
    {
        weaponImage.sprite = cannonAssigned.attackType switch
        {
            Global.Misc.CannonAttackType.Bullet => bulletSprite,
            Global.Misc.CannonAttackType.Laser => laserSprite,
            Global.Misc.CannonAttackType.Energy => energySprite,
            _ => throw new ArgumentOutOfRangeException()
        };
    }


    public void Appear()
    {
        UpdateWeaponImage();
        weaponCanvasGroup.DOFade(1f, 0.5f);
        weaponImage.transform.DOMoveX(weaponImage.transform.position.x, 0.5f)
            .From(weaponImage.transform.position.x - 2f).OnComplete(() =>
            {
                UpdateLevelText();
            });
    }

    public void Fire()
    {
        StartCoolDown();
        coolDonIndicatorCanvasGroup.DOFade(0f, 0f);
        fireIndicatorCanvasGroup.DOFade(1f, 0.05f).From(0.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            fireIndicatorCanvasGroup.DOFade(0.5f, 0.05f).From(1).SetEase(Ease.Linear).OnComplete(() =>
            {
                fireIndicatorCanvasGroup.DOFade(1f, 0.05f).From(0.5f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    fireIndicatorCanvasGroup.DOFade(0f, 0.1f).From(1f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        coolDonIndicatorCanvasGroup.DOFade(1f, 0.2f);
                        weaponImage.color = new Color(0.3f, 0.3f, 0.3f, 1f);
                    });
                });
            });
        });
    }

    public void StartCoolDown()
    {
        var fillValue = 0f;
        DOTween.To(() => fillValue, x => fillValue = x, 1f, cannonAssigned.attackCoolDown).SetEase(Ease.Linear)
            .OnUpdate(
                () => { coolDownIndicatorImage.fillAmount = fillValue; }).OnComplete(() =>
            {
                weaponImage.color = Color.white;
                coolDonIndicatorCanvasGroup.DOFade(0f, 0.2f);
            });
    }

    public void PlayerPowerUpParticleSystem()
    {
        powerUpParticleSystem.Play();
        UpdateLevelText();
    }

    public void UnReady()
    {
        levelTextCanvasGroup.DOFade(0f, 0f);
        weaponImage.transform.localPosition = unreadyPosition.localPosition;
        weaponImage.color = Color.black;
    }

    public void BecomeReady()
    {
        UpdateLevelText();
        levelTextCanvasGroup.DOFade(1f, 0.5f);
        weaponImage.transform.DOLocalMove(readyPosition.localPosition, 0.5f).SetEase(Ease.OutBack);
        weaponImage.DOColor(Color.white, 0.5f).OnComplete(() =>
        {
            Global.Battle.cannonBattleManager.UnlockCannon(id);
            Global.Battle.unlockChipArea.chipUnlocked = true;
        });
    }

    public void UpdateLevelText()
    {
        foreach (var chip in Global.Battle.chipInCannonsInBattle)
        {
            if (chip.inBattleID == id)
            {
                var level = chip.upgradeLevel;
                if (level == 6)
                {
                    levelTextGameObject.SetActive(false);
                    maxTextGameObject.SetActive(true);
                }
                else
                {
                    levelTextGameObject.SetActive(true);
                    maxTextGameObject.SetActive(false);
                    levelText.text = chip.upgradeLevel.ToString();
                }
            }
        }
    }
}