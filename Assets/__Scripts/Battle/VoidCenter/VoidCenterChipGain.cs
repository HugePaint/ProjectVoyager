using System.Collections.Generic;
using DG.Tweening;
using MoreMountains.Tools;
using UnityEngine;

public class VoidCenterChipGain : MonoBehaviour
{
    public ParticleSystem[] particleSystems;
    public GameObject outerRing;
    public ChipShredManager chipShredManager;

    public float normalSize;
    public float weakSize;
    public float strongSize;

    private VoidCenter voidCenter;

    private void Awake()
    {
        voidCenter = GetComponent<VoidCenter>();
    }

    private void Start()
    {
        ModifyParticleMainSize(normalSize);
        
        if (Global.Battle.battleData.isGainingChips)
        {
            GetChips();
            outerRing.SetActive(false);
            Global.DoTweenWait(2f,()=>
            {
                AnimationPartOne();
            });
        }
        else
        {
            outerRing.SetActive(true);
            chipShredManager.gameObject.SetActive(false);
        }
    }

    public void GetChips()
    {
        var score = Global.Battle.energyGainWhenGainingChips;
        Debug.Log("Score:" + score);
        Global.Battle.chipsGettingInTheEnd = new List<BattleDataManager.ChipGetting>();

        switch (score)
        {
            case >= 2000f:
                Global.Battle.chipsGettingInTheEnd.Add(new BattleDataManager.ChipGetting
                {
                    chipInfo = BattleRewardManager.CreateChip(Global.Misc.Rarity.Legendary),
                    rarity = Global.Misc.Rarity.Legendary
                });
                Global.Battle.chipsGettingInTheEnd.Add(new BattleDataManager.ChipGetting
                {
                    chipInfo = BattleRewardManager.CreateChip(Global.Misc.Rarity.Epic),
                    rarity = Global.Misc.Rarity.Epic
                });
                Global.Battle.chipsGettingInTheEnd.Add(new BattleDataManager.ChipGetting
                {
                    chipInfo = BattleRewardManager.CreateChip(Global.Misc.Rarity.Legendary),
                    rarity = Global.Misc.Rarity.Legendary
                });
                Global.Battle.chipsGettingInTheEnd.Add(new BattleDataManager.ChipGetting
                {
                    chipInfo = BattleRewardManager.CreateChip(Global.Misc.Rarity.Legendary),
                    rarity = Global.Misc.Rarity.Legendary
                });
                Global.Battle.chipsGettingInTheEnd.Add(new BattleDataManager.ChipGetting
                {
                    chipInfo = BattleRewardManager.CreateChip(Global.Misc.Rarity.Epic),
                    rarity = Global.Misc.Rarity.Epic
                });
                Global.Battle.chipsGettingInTheEnd.Add(new BattleDataManager.ChipGetting
                {
                    chipInfo = BattleRewardManager.CreateChip(Global.Misc.Rarity.Epic),
                    rarity = Global.Misc.Rarity.Epic
                });
                break;
            case >= 1000f:
                Global.Battle.chipsGettingInTheEnd.Add(new BattleDataManager.ChipGetting
                {
                    chipInfo = BattleRewardManager.CreateChip(Global.Misc.Rarity.Legendary),
                    rarity = Global.Misc.Rarity.Legendary
                });
                Global.Battle.chipsGettingInTheEnd.Add(new BattleDataManager.ChipGetting
                {
                    chipInfo = BattleRewardManager.CreateChip(Global.Misc.Rarity.Epic),
                    rarity = Global.Misc.Rarity.Epic
                });
                Global.Battle.chipsGettingInTheEnd.Add(new BattleDataManager.ChipGetting
                {
                    chipInfo = BattleRewardManager.CreateChip(Global.Misc.Rarity.Rare),
                    rarity = Global.Misc.Rarity.Common
                });
                Global.Battle.chipsGettingInTheEnd.Add(new BattleDataManager.ChipGetting
                {
                    chipInfo = BattleRewardManager.CreateChip(Global.Misc.Rarity.Legendary),
                    rarity = Global.Misc.Rarity.Legendary
                });
                Global.Battle.chipsGettingInTheEnd.Add(new BattleDataManager.ChipGetting
                {
                    chipInfo = BattleRewardManager.CreateChip(Global.Misc.Rarity.Epic),
                    rarity = Global.Misc.Rarity.Epic
                });
                Global.Battle.chipsGettingInTheEnd.Add(new BattleDataManager.ChipGetting
                {
                    chipInfo = BattleRewardManager.CreateChip(Global.Misc.Rarity.Rare),
                    rarity = Global.Misc.Rarity.Common
                });
                break;
            case >= 500f:
                Global.Battle.chipsGettingInTheEnd.Add(new BattleDataManager.ChipGetting
                {
                    chipInfo = BattleRewardManager.CreateChip(Global.Misc.Rarity.Legendary),
                    rarity = Global.Misc.Rarity.Legendary
                });
                Global.Battle.chipsGettingInTheEnd.Add(new BattleDataManager.ChipGetting
                {
                    chipInfo = BattleRewardManager.CreateChip(Global.Misc.Rarity.Epic),
                    rarity = Global.Misc.Rarity.Epic
                });
                Global.Battle.chipsGettingInTheEnd.Add(new BattleDataManager.ChipGetting
                {
                    chipInfo = BattleRewardManager.CreateChip(Global.Misc.Rarity.Rare),
                    rarity = Global.Misc.Rarity.Epic
                });
                Global.Battle.chipsGettingInTheEnd.Add(new BattleDataManager.ChipGetting
                {
                    chipInfo = BattleRewardManager.CreateChip(Global.Misc.Rarity.Uncommon),
                    rarity = Global.Misc.Rarity.Common
                });
                Global.Battle.chipsGettingInTheEnd.Add(new BattleDataManager.ChipGetting
                {
                    chipInfo = BattleRewardManager.CreateChip(Global.Misc.Rarity.Uncommon),
                    rarity = Global.Misc.Rarity.Common
                });
                Global.Battle.chipsGettingInTheEnd.Add(new BattleDataManager.ChipGetting
                {
                    chipInfo = BattleRewardManager.CreateChip(Global.Misc.Rarity.Uncommon),
                    rarity = Global.Misc.Rarity.Common
                });
                break;
            case >= 400f:
                Global.Battle.chipsGettingInTheEnd.Add(new BattleDataManager.ChipGetting
                {
                    chipInfo = BattleRewardManager.CreateChip(Global.Misc.Rarity.Epic),
                    rarity = Global.Misc.Rarity.Epic
                });
                Global.Battle.chipsGettingInTheEnd.Add(new BattleDataManager.ChipGetting
                {
                    chipInfo = BattleRewardManager.CreateChip(Global.Misc.Rarity.Epic),
                    rarity = Global.Misc.Rarity.Epic
                });
                Global.Battle.chipsGettingInTheEnd.Add(new BattleDataManager.ChipGetting
                {
                    chipInfo = BattleRewardManager.CreateChip(Global.Misc.Rarity.Uncommon),
                    rarity = Global.Misc.Rarity.Common
                });
                Global.Battle.chipsGettingInTheEnd.Add(new BattleDataManager.ChipGetting
                {
                    chipInfo = BattleRewardManager.CreateChip(Global.Misc.Rarity.Uncommon),
                    rarity = Global.Misc.Rarity.Common
                });
                Global.Battle.chipsGettingInTheEnd.Add(new BattleDataManager.ChipGetting
                {
                    chipInfo = BattleRewardManager.CreateChip(Global.Misc.Rarity.Uncommon),
                    rarity = Global.Misc.Rarity.Common
                });
                break;
            case >= 300f:
                Global.Battle.chipsGettingInTheEnd.Add(new BattleDataManager.ChipGetting
                {
                    chipInfo = BattleRewardManager.CreateChip(Global.Misc.Rarity.Epic),
                    rarity = Global.Misc.Rarity.Epic
                });
                Global.Battle.chipsGettingInTheEnd.Add(new BattleDataManager.ChipGetting
                {
                    chipInfo = BattleRewardManager.CreateChip(Global.Misc.Rarity.Epic),
                    rarity = Global.Misc.Rarity.Epic
                });
                Global.Battle.chipsGettingInTheEnd.Add(new BattleDataManager.ChipGetting
                {
                    chipInfo = BattleRewardManager.CreateChip(Global.Misc.Rarity.Uncommon),
                    rarity = Global.Misc.Rarity.Common
                });
                Global.Battle.chipsGettingInTheEnd.Add(new BattleDataManager.ChipGetting
                {
                    chipInfo = BattleRewardManager.CreateChip(Global.Misc.Rarity.Common),
                    rarity = Global.Misc.Rarity.Common
                });
                break;
            case >= 200f:
                Global.Battle.chipsGettingInTheEnd.Add(new BattleDataManager.ChipGetting
                {
                    chipInfo = BattleRewardManager.CreateChip(Global.Misc.Rarity.Epic),
                    rarity = Global.Misc.Rarity.Epic
                });
                Global.Battle.chipsGettingInTheEnd.Add(new BattleDataManager.ChipGetting
                {
                    chipInfo = BattleRewardManager.CreateChip(Global.Misc.Rarity.Uncommon),
                    rarity = Global.Misc.Rarity.Common
                });
                Global.Battle.chipsGettingInTheEnd.Add(new BattleDataManager.ChipGetting
                {
                    chipInfo = BattleRewardManager.CreateChip(Global.Misc.Rarity.Common),
                    rarity = Global.Misc.Rarity.Common
                });
                break;
            case >= 100f:
                Global.Battle.chipsGettingInTheEnd.Add(new BattleDataManager.ChipGetting
                {
                    chipInfo = BattleRewardManager.CreateChip(Global.Misc.Rarity.Uncommon),
                    rarity = Global.Misc.Rarity.Common
                });
                Global.Battle.chipsGettingInTheEnd.Add(new BattleDataManager.ChipGetting
                {
                    chipInfo = BattleRewardManager.CreateChip(Global.Misc.Rarity.Uncommon),
                    rarity = Global.Misc.Rarity.Common
                });
                break;
            default:
                Global.Battle.chipsGettingInTheEnd.Add(new BattleDataManager.ChipGetting
                {
                    chipInfo = BattleRewardManager.CreateChip(Global.Misc.Rarity.Uncommon),
                    rarity = Global.Misc.Rarity.Common
                });
                break;
        }
        Global.Battle.chipsGettingInTheEnd.MMShuffle();


        var chipGetting = new List<ChipData.ChipInfo>();
        foreach (var chip in Global.Battle.chipsGettingInTheEnd)
        {
            chipGetting.Add(chip.chipInfo);
        }
        BattleRewardManager.AddToInventory(chipGetting);

        var finalChipCount = Global.Battle.chipsGettingInTheEnd.Count;
        Global.Battle.chipGainUIManager.AdjustChipUINumber(finalChipCount);
    }

    public void AnimationPartOne()
    {
        chipShredManager.gameObject.SetActive(true);
        var animationSpeed = 1f;
        DOTween.To(() => animationSpeed, x => animationSpeed = x, 3f, 2f).SetEase(Ease.Linear).OnUpdate(() =>
        {
            voidCenter.UpdateAnimationSpeed(animationSpeed);
        });
        
        var size = normalSize;
        DOTween.To(() => size, x => size = x, strongSize, 2f).SetEase(Ease.Linear).OnUpdate(() =>
        {
            ModifyParticleMainSize(size);
        });
        
        transform.DOMoveY(transform.position.y + 2f, 3f);
        EventCenter.GetInstance().EventTrigger(Global.Events.PlayAudioSoundEffect, GameAudios.AudioName.ChipGainCharging);
        Global.Battle.battleEnergyDisplay.Release();

        Global.DoTweenWait(3.5f, () =>
        {
            var animationSpeedStop = 3f;
            DOTween.To(() => animationSpeedStop, x => animationSpeedStop = x, 0.3f, 3f).SetEase(Ease.Linear).OnUpdate(() =>
            {
                voidCenter.UpdateAnimationSpeed(animationSpeedStop);
            }).OnComplete(() =>
            {
                Global.DoTweenWait(0.5f, () =>
                {
                    AnimationPartTwo();
                });
                Global.DoTweenWait(1f, () =>
                {
                    var animationSpeedStop = 0.3f;
                    DOTween.To(() => animationSpeedStop, x => animationSpeedStop = x, 1f, 1f).SetEase(Ease.Linear).OnUpdate(() =>
                    {
                        voidCenter.UpdateAnimationSpeed(animationSpeedStop);
                    });
                    var sizeNew = strongSize;
                    DOTween.To(() => sizeNew, x => sizeNew = x, weakSize, 1f).SetEase(Ease.Linear).OnUpdate(() =>
                    {
                        ModifyParticleMainSize(sizeNew);
                    });
                });
            });
        });
    }

    public void AnimationPartTwo()
    {
        chipShredManager.StartPartTwoAnimation();
        Global.DoTweenWait(6f, () =>
        {
            AnimationPartThree();
        });
    }

    public void AnimationPartThree()
    {
        Global.Battle.chipGainUIManager.StageThreeAnimation();
    }
    
    

    public void ModifyParticleMainSize(float newSize)
    {
        foreach (var ps in particleSystems)
        {
            var mainModule = ps.main;
            mainModule.startSize = newSize;
        }
    }
}
