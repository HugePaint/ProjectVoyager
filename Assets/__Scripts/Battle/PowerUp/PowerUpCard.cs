using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PowerUpCard : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public GameObject unlockNewCannonCard;
    public GameObject upgradeCannonCard;
    public GameObject newEffectCard;
    public Vector3 displayPosition;
    
    //unlock cannon
    public LockedChip lockedChip;
    public List<ChipUIModificationEffect> chipUIModificationEffects;
    public GameObject chipUIModificationEffectsParentObject;
    public float modificationEffectShowX;
    public float modificationEffectHideX;
    public float lockedChipShowX;
    public float lockedChipHideX;
    public Tween chipPointerTween;
    public List<Tween> chipUIModificationEffectsMoveTween;
    public List<Tween> chipUIModificationEffectsFadeTween;

    public CanvasGroup unlockHintTextGroup;
    public Tween unlockHintTextGroupTween;
    public CanvasGroup unlockImageGroup;
    public Tween unlockImageGroupTween;
    public CanvasGroup keyImageCanvasGroup;
    public Tween keyTween;


    //upgradeCannon
    public LockedChip lockedChipUpgrade;
    public List<ChipUIModificationEffect> chipUIModificationEffectsUpgrade;
    public GameObject chipUIModificationEffectsParentObjectUpgrade;
    public float modificationEffectShowXUpgrade;
    public float modificationEffectHideXUpgrade;
    public float lockedChipShowXUpgrade;
    public float lockedChipHideXUpgrade;
    public Tween chipPointerTweenUpgrade;
    public List<Tween> chipUIModificationEffectsMoveTweenUpgrade;
    public List<Tween> chipUIModificationEffectsFadeTweenUpgrade;
    
    public CanvasGroup upgradeHintTextGroup;
    public Tween upgradeHintTextGroupTween;
    public CanvasGroup upgradeImageGroup;
    public Tween upgradeImageGroupTween;
    public TMP_Text upgradeStartLevel;
    public TMP_Text upgradeEndLevel;
    
    //new effect
    public ChipUIModificationEffect newEffect;
    public LocalizeStringEvent effectDescription;
    public ModificationEffectScriptableObject modificationApplying;
    public Global.Misc.Rarity rarityApplying;


    public Button chooseButton;
    public Image cardFrameMaterial;
    private float currentGlow;
    private Tween glowTween;

    public bool chosen;

    private int unlockingId;
    private int upgradingId;
    private PowerUpType powerUpType;
    
    private enum PowerUpType
    {
        UnlockCannon,
        NewEffect,
        UpgradeCannon
    }

    private void Awake()
    {
        cardFrameMaterial.material = Instantiate(cardFrameMaterial.material);
        currentGlow = 0f;
        cardFrameMaterial.material.SetFloat("_Glow",currentGlow);
    }

    public void ResetHint()
    {
        upgradeHintTextGroup.alpha = 0f;
        upgradeImageGroup.alpha = 0f;
        unlockHintTextGroup.alpha = 0f;
        unlockImageGroup.alpha = 0f;
    }

    public void ActiveUnlockNewCannonCard(int chipId)
    {
        ResetHint();
        lockedChip.cover.gameObject.SetActive(true);
        keyImageCanvasGroup.alpha = 1f;
        chipUIModificationEffectsMoveTween = new List<Tween>();
        chipUIModificationEffectsFadeTween = new List<Tween>();
        powerUpType = PowerUpType.UnlockCannon;
        chipUIModificationEffectsParentObject.gameObject.SetActive(true);
        foreach (var effect in chipUIModificationEffects)
        {
            effect.canvasGroup.alpha = 0f;
        }
        unlockingId = chipId;
        var chipUnlocking = Global.Battle.chipInCannonsInBattle[0];
        foreach (var chip in Global.Battle.chipInCannonsInBattle)
        {
            if (chip.inBattleID == chipId) chipUnlocking = chip;
        }
        var chipInfoUnlocking = chipUnlocking.info;
        lockedChip.chipSpriteController.UpdateSprite(chipInfoUnlocking.elementType, chipInfoUnlocking.attackType, chipInfoUnlocking.modificationEffectRarities);
        for (var i = 0; i < chipInfoUnlocking.modificationEffectIDs.Count; i++)
        {
            chipUIModificationEffects[i].gameObject.SetActive(true);
            chipUIModificationEffects[i].UpdateInfo(chipInfoUnlocking.modificationEffectIDs[i], chipInfoUnlocking.modificationEffectRarities[i]);
        }
        
        for (var i = chipInfoUnlocking.modificationEffectIDs.Count; i < chipUIModificationEffects.Count; i++)
        {
            chipUIModificationEffects[i].gameObject.SetActive(false);
            chipUIModificationEffects[i].UpdateInfo(-1, Global.Misc.Rarity.None);
        }
        
        unlockNewCannonCard.SetActive(true);
        upgradeCannonCard.SetActive(false);
        newEffectCard.SetActive(false);
        chooseButton.onClick.RemoveAllListeners();
        chooseButton.onClick.AddListener(ChooseUnlockNewCannon);
    }

    public void ActiveUpgradeCannonCard(int chipId, int startLevel)
    {
        ResetHint();
        lockedChipUpgrade.cover.gameObject.SetActive(false);
        upgradeStartLevel.text = "Lv. " + startLevel;
        upgradeEndLevel.text = (startLevel == 5) ? "Lv. MAX" : "Lv. " + (startLevel + 1);
        chipUIModificationEffectsMoveTweenUpgrade = new List<Tween>();
        chipUIModificationEffectsFadeTweenUpgrade = new List<Tween>();
        powerUpType = PowerUpType.UpgradeCannon;
        chipUIModificationEffectsParentObjectUpgrade.gameObject.SetActive(true);
        foreach (var effect in chipUIModificationEffectsUpgrade)
        {
            effect.canvasGroup.alpha = 0f;
        }
        upgradingId = chipId;
        
        var chipUpgrading = Global.Battle.chipInCannonsInBattle[0];
        foreach (var chip in Global.Battle.chipInCannonsInBattle)
        {
            if (chip.inBattleID == chipId) chipUpgrading = chip;
        }
        var chipInfoUpgrading = chipUpgrading.info;
        
        lockedChipUpgrade.chipSpriteController.UpdateSprite(chipInfoUpgrading.elementType, chipInfoUpgrading.attackType, chipInfoUpgrading.modificationEffectRarities);
        for (var i = 0; i < chipInfoUpgrading.modificationEffectIDs.Count; i++)
        {
            chipUIModificationEffectsUpgrade[i].gameObject.SetActive(true);
            chipUIModificationEffectsUpgrade[i].UpdateInfo(chipInfoUpgrading.modificationEffectIDs[i], chipInfoUpgrading.modificationEffectRarities[i]);
        }
        
        for (var i = chipInfoUpgrading.modificationEffectIDs.Count; i < chipUIModificationEffectsUpgrade.Count; i++)
        {
            chipUIModificationEffectsUpgrade[i].gameObject.SetActive(false);
            chipUIModificationEffectsUpgrade[i].UpdateInfo(-1, Global.Misc.Rarity.None);
        }
        
        unlockNewCannonCard.SetActive(false);
        upgradeCannonCard.SetActive(true);
        newEffectCard.SetActive(false);
        chooseButton.onClick.RemoveAllListeners();
        chooseButton.onClick.AddListener(ChooseUpgradeCannon);
    }

    public void ActiveNewEffectCard()
    {
        powerUpType = PowerUpType.NewEffect;
        var possibleEffectList = new List<ModificationEffectScriptableObject>();
        foreach (var cannon in Global.Battle.cannonBattleManager.cannonBattles)
        {
            possibleEffectList.Add(ModificationEffectManager.GetRandom(cannon.elementType));
        }
        var randomNewEffect = Global.GetRandomFromList(possibleEffectList);
        modificationApplying = randomNewEffect;
        var rarity = Global.GetARandomRarity(0);
        rarityApplying = rarity;
        newEffect.UpdateInfo(randomNewEffect.id, rarity);
        TextManager.SetModificationEffectDescriptionToLocalizeEvent(effectDescription,randomNewEffect.id, rarity);

        unlockNewCannonCard.SetActive(false);
        upgradeCannonCard.SetActive(false);
        newEffectCard.SetActive(true);
        chooseButton.onClick.RemoveAllListeners();
        chooseButton.onClick.AddListener(ChooseNewEffect);
    }
    
    
    public void ChooseUnlockNewCannon()
    {
        chooseButton.onClick.RemoveAllListeners();
        chosen = true;
        Global.Battle.battleUIManager.powerUpUIManager.CloseAll();
        Global.Battle.unlockChipArea.StationIsTaken();
        if (Global.Battle.unlockChipArea) Global.Battle.unlockChipArea.Appear(unlockingId);
    }

    public void ChooseUpgradeCannon()
    {
        chooseButton.onClick.RemoveAllListeners();
        chosen = true;
        Global.Battle.battleUIManager.powerUpUIManager.CloseAll();
        Global.Battle.powerUpManager.CannonPowerUp(upgradingId);
    }

    public void ChooseNewEffect()
    {
        chooseButton.onClick.RemoveAllListeners();
        chosen = true;
        Global.Battle.battleUIManager.powerUpUIManager.CloseAll();
        Global.Battle.powerUpManager.ApplyNewEffect(modificationApplying,rarityApplying);
    }

    public void Appear(Transform startPosition)
    {
        var randomDelay = Random.Range(0f, 0.5f);
        transform.DOLocalMove(displayPosition, 0.75f).From(startPosition.localPosition).SetEase(Ease.OutBack).SetDelay(randomDelay).SetUpdate(isIndependentUpdate:true);
        transform.DOScale(1, 0.75f).From(0).SetEase(Ease.OutBack).SetDelay(randomDelay).SetUpdate(isIndependentUpdate:true);
        chosen = false;
    }

    public void Close(Transform endPosition)
    {
        if (!chosen)
        {
            var randomDelay = Random.Range(0f, 0.3f);
            transform.DOLocalMove(endPosition.localPosition, 0.5f).From(displayPosition).SetEase(Ease.InBack).SetDelay(randomDelay).SetUpdate(isIndependentUpdate:true);
            transform.DOScale(0, 0.5f).From(1).SetEase(Ease.InBack).SetDelay(randomDelay).SetUpdate(isIndependentUpdate:true);
            chooseButton.onClick.RemoveAllListeners();
        }
        else
        {
            transform.DOScale(1.2f, 0.5f).From(1).SetEase(Ease.OutBack).OnComplete(() =>
            {
                transform.DOScale(0f, 0.5f).SetDelay(0.5f).SetUpdate(isIndependentUpdate:true);
            }).SetUpdate(isIndependentUpdate:true);
        }
    }

    public void UnlockCannonPointerEnter()
    {
        //chip
        var chipXDifference = Mathf.Abs(lockedChip.transform.localPosition.x - lockedChipShowX);
        var chipAnimationTimeMultiplier = chipXDifference / Mathf.Abs(lockedChipHideX - lockedChipShowX);
        chipPointerTween?.Kill();
        chipPointerTween = lockedChip.transform.DOLocalMoveX(lockedChipShowX, 0.5f * chipAnimationTimeMultiplier).SetUpdate(isIndependentUpdate:true);

        for (var i = 0; i < chipUIModificationEffects.Count; i++)
        {
            var alphaDifference = 1f - chipUIModificationEffects[i].canvasGroup.alpha;
            if (chipUIModificationEffectsFadeTween.Count <= i)
            {
                chipUIModificationEffectsFadeTween.Add(chipUIModificationEffects[i].canvasGroup.DOFade(1f, alphaDifference * 0.5f).SetDelay(0.1f * i).SetUpdate(isIndependentUpdate:true));
            }
            else
            {
                chipUIModificationEffectsFadeTween[i]?.Kill();
                chipUIModificationEffectsFadeTween[i] = chipUIModificationEffects[i].canvasGroup.DOFade(1f, alphaDifference * 0.5f).SetDelay(0.1f * i).SetUpdate(isIndependentUpdate:true);
            }

            var effectXDifference = Mathf.Abs(chipUIModificationEffects[i].transform.localPosition.x - modificationEffectShowX);
            var effectAnimationTimeMultiplier = effectXDifference / Mathf.Abs(modificationEffectHideX - modificationEffectShowX);
            if (chipUIModificationEffectsMoveTween.Count <= i)
            {
                chipUIModificationEffectsMoveTween.Add(chipUIModificationEffects[i].transform.DOLocalMoveX(modificationEffectShowX, effectAnimationTimeMultiplier * 0.5f).SetDelay(0.05f * i).SetUpdate(isIndependentUpdate:true) );
            }
            else
            {
                chipUIModificationEffectsMoveTween[i]?.Kill();
                chipUIModificationEffectsMoveTween[i] = chipUIModificationEffects[i].transform.DOLocalMoveX(modificationEffectShowX, effectAnimationTimeMultiplier * 0.5f).SetDelay(0.05f * i).SetUpdate(isIndependentUpdate:true);   
            }
        }


        //textHint
        var textHintAlphaDifference = 1f - unlockHintTextGroup.alpha;
        unlockHintTextGroupTween?.Kill();
        unlockHintTextGroupTween = unlockHintTextGroup.DOFade(1f, textHintAlphaDifference * 0.5f).SetUpdate(isIndependentUpdate: true);
        
        
        //imageHint
        var imageHintAlphaDifference = 1f - unlockImageGroup.alpha;
        unlockImageGroupTween?.Kill();
        unlockImageGroupTween = unlockImageGroup.DOFade(1f, imageHintAlphaDifference * 0.5f).SetUpdate(isIndependentUpdate: true);
        
        //keyImage
        var keyAlphaDifference = keyImageCanvasGroup.alpha;
        keyTween?.Kill();
        keyTween = keyImageCanvasGroup.DOFade(0f, keyAlphaDifference * 0.5f).SetUpdate(isIndependentUpdate: true);
    }
    
    public void UnlockCannonPointerExit()
    {
        //chip
        var chipXDifference = Mathf.Abs(lockedChip.transform.localPosition.x - lockedChipHideX);
        var chipAnimationTimeMultiplier = chipXDifference / Mathf.Abs(lockedChipHideX - lockedChipShowX);
        chipPointerTween?.Kill();
        chipPointerTween = lockedChip.transform.DOLocalMoveX(lockedChipHideX, 0.5f * chipAnimationTimeMultiplier).SetUpdate(isIndependentUpdate:true);
        
        for (var i = 0; i < chipUIModificationEffects.Count; i++)
        {
            var alphaDifference = chipUIModificationEffects[i].canvasGroup.alpha;
            chipUIModificationEffectsFadeTween[i]?.Kill();
            chipUIModificationEffectsFadeTween[i] = chipUIModificationEffects[i].canvasGroup.DOFade(0f, alphaDifference * 0.5f).SetUpdate(isIndependentUpdate:true);

            var effectXDifference = Mathf.Abs(chipUIModificationEffects[i].transform.localPosition.x - modificationEffectHideX);
            var effectAnimationTimeMultiplier = effectXDifference / Mathf.Abs(modificationEffectHideX - modificationEffectShowX);
            chipUIModificationEffectsMoveTween[i]?.Kill();
            chipUIModificationEffectsMoveTween[i] = chipUIModificationEffects[i].transform.DOLocalMoveX(modificationEffectHideX, effectAnimationTimeMultiplier * 0.5f).SetUpdate(isIndependentUpdate:true);
        }
        
        //textHint
        var textHintAlphaDifference = unlockHintTextGroup.alpha;
        unlockHintTextGroupTween?.Kill();
        unlockHintTextGroupTween = unlockHintTextGroup.DOFade(0f, textHintAlphaDifference * 0.5f).SetUpdate(isIndependentUpdate: true);
        
        //imageHint
        var imageHintAlphaDifference = unlockImageGroup.alpha;
        unlockImageGroupTween?.Kill();
        unlockImageGroupTween = unlockImageGroup.DOFade(0f, imageHintAlphaDifference * 0.5f).SetUpdate(isIndependentUpdate: true);

        //keyImage
        var keyAlphaDifference = 1f - keyImageCanvasGroup.alpha;
        keyTween?.Kill();
        keyTween = keyImageCanvasGroup.DOFade(1f, keyAlphaDifference * 0.5f).SetUpdate(isIndependentUpdate: true);
    }
    
    public void UpgradeCannonPointerEnter()
    {
        var chipXDifference = Mathf.Abs(lockedChipUpgrade.transform.localPosition.x - lockedChipShowXUpgrade);
        var chipAnimationTimeMultiplier = chipXDifference / Mathf.Abs(lockedChipHideXUpgrade - lockedChipShowXUpgrade);
        chipPointerTweenUpgrade?.Kill();
        chipPointerTweenUpgrade = lockedChipUpgrade.transform.DOLocalMoveX(lockedChipShowXUpgrade, 0.5f * chipAnimationTimeMultiplier).SetUpdate(isIndependentUpdate:true);

        for (var i = 0; i < chipUIModificationEffectsUpgrade.Count; i++)
        {
            var alphaDifference = 1f - chipUIModificationEffectsUpgrade[i].canvasGroup.alpha;
            if (chipUIModificationEffectsFadeTweenUpgrade.Count <= i)
            {
                chipUIModificationEffectsFadeTweenUpgrade.Add(chipUIModificationEffectsUpgrade[i].canvasGroup.DOFade(1f, alphaDifference * 0.5f).SetDelay(0.1f * i).SetUpdate(isIndependentUpdate:true));
            }
            else
            {
                chipUIModificationEffectsFadeTweenUpgrade[i]?.Kill();
                chipUIModificationEffectsFadeTweenUpgrade[i] = chipUIModificationEffectsUpgrade[i].canvasGroup.DOFade(1f, alphaDifference * 0.5f).SetDelay(0.1f * i).SetUpdate(isIndependentUpdate:true);
            }

            var effectXDifference = Mathf.Abs(chipUIModificationEffectsUpgrade[i].transform.localPosition.x - modificationEffectShowXUpgrade);
            var effectAnimationTimeMultiplier = effectXDifference / Mathf.Abs(modificationEffectHideXUpgrade - modificationEffectShowXUpgrade);
            if (chipUIModificationEffectsMoveTweenUpgrade.Count <= i)
            {
                chipUIModificationEffectsMoveTweenUpgrade.Add(chipUIModificationEffectsUpgrade[i].transform.DOLocalMoveX(modificationEffectShowXUpgrade, effectAnimationTimeMultiplier * 0.5f).SetDelay(0.05f * i).SetUpdate(isIndependentUpdate:true) );
            }
            else
            {
                chipUIModificationEffectsMoveTweenUpgrade[i]?.Kill();
                chipUIModificationEffectsMoveTweenUpgrade[i] = chipUIModificationEffectsUpgrade[i].transform.DOLocalMoveX(modificationEffectShowXUpgrade, effectAnimationTimeMultiplier * 0.5f).SetDelay(0.05f * i).SetUpdate(isIndependentUpdate:true);   
            }
        }


        //textHint
        var textHintAlphaDifference = 1f - upgradeHintTextGroup.alpha;
        upgradeHintTextGroupTween?.Kill();
        upgradeHintTextGroupTween = upgradeHintTextGroup.DOFade(1f, textHintAlphaDifference * 0.5f).SetUpdate(isIndependentUpdate: true);
        
        //imageHint
        var imageHintAlphaDifference = 1f - upgradeImageGroup.alpha;
        upgradeImageGroupTween?.Kill();
        upgradeImageGroupTween = upgradeImageGroup.DOFade(1f, imageHintAlphaDifference * 0.5f).SetUpdate(isIndependentUpdate: true);
        
    }
    
    public void UpgradeCannonPointerExit()
    {
        var chipXDifference = Mathf.Abs(lockedChipUpgrade.transform.localPosition.x - lockedChipHideXUpgrade);
        var chipAnimationTimeMultiplier = chipXDifference / Mathf.Abs(lockedChipHideXUpgrade - lockedChipShowXUpgrade);
        chipPointerTweenUpgrade?.Kill();
        chipPointerTweenUpgrade = lockedChipUpgrade.transform.DOLocalMoveX(lockedChipHideXUpgrade, 0.5f * chipAnimationTimeMultiplier).SetUpdate(isIndependentUpdate:true);
        
        for (var i = 0; i < chipUIModificationEffectsUpgrade.Count; i++)
        {
            var alphaDifference = chipUIModificationEffectsUpgrade[i].canvasGroup.alpha;
            chipUIModificationEffectsFadeTweenUpgrade[i]?.Kill();
            chipUIModificationEffectsFadeTweenUpgrade[i] = chipUIModificationEffectsUpgrade[i].canvasGroup.DOFade(0f, alphaDifference * 0.5f).SetUpdate(isIndependentUpdate:true);

            var effectXDifference = Mathf.Abs(chipUIModificationEffectsUpgrade[i].transform.localPosition.x - modificationEffectHideXUpgrade);
            var effectAnimationTimeMultiplier = effectXDifference / Mathf.Abs(modificationEffectHideXUpgrade - modificationEffectShowXUpgrade);
            chipUIModificationEffectsMoveTweenUpgrade[i]?.Kill();
            chipUIModificationEffectsMoveTweenUpgrade[i] = chipUIModificationEffectsUpgrade[i].transform.DOLocalMoveX(modificationEffectHideXUpgrade, effectAnimationTimeMultiplier * 0.5f).SetUpdate(isIndependentUpdate:true);
        }
        
        //textHint
        var textHintAlphaDifference = upgradeHintTextGroup.alpha;
        upgradeHintTextGroupTween?.Kill();
        upgradeHintTextGroupTween = upgradeHintTextGroup.DOFade(0f, textHintAlphaDifference * 0.5f).SetUpdate(isIndependentUpdate: true);
        
        //imageHint
        var imageHintAlphaDifference = upgradeImageGroup.alpha;
        upgradeImageGroupTween?.Kill();
        upgradeImageGroupTween = upgradeImageGroup.DOFade(0f, imageHintAlphaDifference * 0.5f).SetUpdate(isIndependentUpdate: true);
    }

    public void GeneralPointerEnter()
    {
        var glowDifference = 15f - currentGlow;
        var timeMultiplier = glowDifference / 15f;
        glowTween?.Kill();
        glowTween = DOTween.To(() => currentGlow, x => currentGlow = x, 15f, 0.5f * timeMultiplier).SetUpdate(isIndependentUpdate: true).OnUpdate(() =>
        {
            cardFrameMaterial.material.SetFloat("_Glow", currentGlow);
        });
    }
    
    public void GeneralPointerLeave()
    {
        var glowDifference = currentGlow;
        var timeMultiplier = glowDifference / 15f;
        glowTween?.Kill();
        glowTween = DOTween.To(() => currentGlow, x => currentGlow = x, 0f, 0.5f * timeMultiplier).SetUpdate(isIndependentUpdate: true).OnUpdate(() =>
        {
            cardFrameMaterial.material.SetFloat("_Glow", currentGlow);
        });
    }

    private void CardSelect() {
        GeneralPointerEnter();
        switch (powerUpType)
        {
            case PowerUpType.UnlockCannon:
                UnlockCannonPointerEnter();
                break;
            case PowerUpType.NewEffect:
                break;
            case PowerUpType.UpgradeCannon:
                UpgradeCannonPointerEnter();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void CardDeselect() {
        GeneralPointerLeave();
        switch (powerUpType)
        {
            case PowerUpType.UnlockCannon:
                UnlockCannonPointerExit();
                break;
            case PowerUpType.NewEffect:
                break;
            case PowerUpType.UpgradeCannon:
                UpgradeCannonPointerExit();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        CardSelect();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       CardDeselect();
    }

    public void OnSelect(BaseEventData eventData) {
        CardSelect();
    }

    public void OnDeselect(BaseEventData eventData) {
        CardDeselect();
    }
}
