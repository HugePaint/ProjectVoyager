using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using static Global.Misc;

public class ChipSpriteController : MonoBehaviour
{
    [Serializable] public class AttackTypeIcons
    {
        public GameObject bulletIcon;
        public GameObject laserIcon;
        public GameObject energyIcon;
    };

    [Serializable]
    public class HSVOffset
    {
        [Range(-100, 100)]
        public float H;
        [Range(-100, 100)]
        public float S;
        [Range(-100, 100)]
        public float V;
    }
    
    public GameObject chipBase;
    [Header("----------------")]
    public GameObject chipComponent1;
    public HSVOffset component1ColorOffset;
    [Header("----------------")]
    public GameObject chipComponent2;
    public HSVOffset component2ColorOffset;
    [Header("----------------")]
    public GameObject chipOutline;
    public AttackTypeIcons attackTypeIcons;
    [Header("----------------")]
    public GameObject rarityIndicator;
    public HSVOffset rarityIndicatorColorOffset;
    public Image boarder;
    private bool isboarderNotNull;

    // Start is called before the first frame update
    void Start()
    {
        isboarderNotNull = boarder != null;
        //SetDefault(ElementType.Fire);
    }

    public void SetDefault(ElementType elementType)
    {
        List<Rarity> testRarities = new List<Rarity>();
        testRarities.Add(Rarity.Common);
        testRarities.Add(Rarity.Uncommon);
        testRarities.Add(Rarity.Rare);
        testRarities.Add(Rarity.Epic);
        testRarities.Add(Rarity.Legendary);
        
        UpdateSprite(elementType, CannonAttackType.Bullet, testRarities);
    }

    public void UpdateSprite(ElementType elementType, CannonAttackType cannonAttackType, List<Rarity> rarities)
    {
        ColorData colorData = Global.Misc.colorData;
        
        Color elementColor;
        switch (elementType)
        {
            case ElementType.Fire: 
                elementColor = colorData.Fire; 
                break;
            case ElementType.Water: elementColor = colorData.Water; break;
            case ElementType.Nature: elementColor = colorData.Nature; break;
            case ElementType.Light: elementColor = colorData.Light; break;
            case ElementType.Dark: elementColor = colorData.Dark; break;
            case ElementType.None: elementColor = colorData.Common; break;
            default:
                throw new ArgumentOutOfRangeException(nameof(elementType), elementType, null);
        }

        List<Color> rarityColors = new List<Color>();
        foreach (Rarity rarity in rarities)
        {
            switch (rarity)
            {
                case Rarity.Common: rarityColors.Add(colorData.Common); break;
                case Rarity.Uncommon: rarityColors.Add(colorData.Uncommon); break;
                case Rarity.Rare: rarityColors.Add(colorData.Rare); break;
                case Rarity.Epic: rarityColors.Add(colorData.Epic); break;
                case Rarity.Legendary: rarityColors.Add(colorData.Legendary); break;
                case Rarity.None: rarityColors.Add(colorData.None); break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        UpdateBaseAndFrame(elementColor);
        if (elementType != ElementType.None)
            UpdateAttackTypeIcon(cannonAttackType);
        else ClearAttackTypeIcon();
        UpdateRarityIndicators(rarities);
        // if (isboarderNotNull) UpdateBoarderColor(rarities.Count);
        if (boarder != null) UpdateBoarderColor(rarities.Count);
    }

    private void UpdateBoarderColor(int totalModEffect)
    {
        totalModEffect = totalModEffect - 2;
        var chipRarity = (Rarity)totalModEffect;
        Color boarderColor = Color.clear;
        switch (chipRarity)
        {
            case Rarity.None: boarderColor = colorData.None; break;
            case Rarity.Common: boarderColor = colorData.Common; break;
            case Rarity.Uncommon: boarderColor = colorData.Uncommon; break;
            case Rarity.Rare: boarderColor = colorData.Rare; break;
            case Rarity.Epic:
                boarderColor = colorData.Epic; break;
            case Rarity.Legendary: boarderColor = colorData.Legendary; break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        boarderColor.a = 0.3f;
        boarder.color = boarderColor;
    }

    void UpdateBaseAndFrame(Color elementColor)
    {
        Image baseImage = chipBase.GetComponent<Image>();
        baseImage.color = elementColor;

        Image component1Image = chipComponent1.GetComponent<Image>();
        component1Image.color = HSVaddup(elementColor, component1ColorOffset);

        Image component2Image = chipComponent2.GetComponent<Image>();
        component2Image.color = HSVaddup(elementColor, component2ColorOffset);
    }

    Color HSVaddup(Color color, HSVOffset offset)
    {
        float h, s, v;
        Color.RGBToHSV(color, out h, out s, out v);
        return Color.HSVToRGB(h + offset.H/100f, s + offset.S/100f, v + offset.V/100f);
    }

    void UpdateAttackTypeIcon(CannonAttackType attackType)
    {
        attackTypeIcons.bulletIcon.SetActive(false);
        attackTypeIcons.laserIcon.SetActive(false);
        attackTypeIcons.energyIcon.SetActive(false);

        switch (attackType)
        {
            case CannonAttackType.Bullet:
                attackTypeIcons.bulletIcon.SetActive(true);
                break;
            case CannonAttackType.Laser:
                attackTypeIcons.laserIcon.SetActive(true);
                break;
            case CannonAttackType.Energy:
                attackTypeIcons.energyIcon.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(attackType), attackType, null);
        }
    }
    
    void ClearAttackTypeIcon()
    {
        attackTypeIcons.bulletIcon.SetActive(false);
        attackTypeIcons.laserIcon.SetActive(false);
        attackTypeIcons.energyIcon.SetActive(false);
    }
    
    void UpdateRarityIndicators(List<Rarity> rarities)
    {
        if (rarities.Count > 6)
            throw new ArgumentOutOfRangeException(nameof(rarities), rarities, null);

        // Fill each bar with None color
        for (int i = 0; i < 6; i++)
        {
            Image currentBar = rarityIndicator.transform.GetChild(i).GetComponent<Image>();
            currentBar.color = colorData.None;
        }
    
        // Fill bars with rarities
        for (int i = 0; i < rarities.Count; i++)
        {
            Image currentBar = rarityIndicator.transform.GetChild(i).GetComponent<Image>();
            switch (rarities[i])
            {
                case Rarity.Common:
                    currentBar.color = colorData.Common;
                    break;
                case Rarity.Uncommon:
                    currentBar.color = colorData.Uncommon;
                    break;
                case Rarity.Rare:
                    currentBar.color = colorData.Rare;
                    break;
                case Rarity.Epic:
                    currentBar.color = colorData.Epic;
                    break;
                case Rarity.Legendary:
                    currentBar.color = colorData.Legendary;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            currentBar.color = HSVaddup(currentBar.color, rarityIndicatorColorOffset);
        }
    }
}
