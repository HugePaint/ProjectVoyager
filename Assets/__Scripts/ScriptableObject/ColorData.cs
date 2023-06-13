using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorData", menuName = "ScriptableObjects/ColorData")]
public class ColorData : ScriptableObject
{
    [Header("Element Color")]
    public Color Fire;
    public Color Water;
    public Color Nature;
    public Color Light;
    public Color Dark;

    [Header(("Rarity Color"))] 
    public Color None;
    public Color Common;
    public Color Uncommon;
    public Color Rare;
    public Color Epic;
    public Color Legendary;
    
}
