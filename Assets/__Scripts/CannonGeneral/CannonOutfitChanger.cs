using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CannonOutfitChanger : MonoBehaviour
{
    public Mesh bulletMesh;
    public Mesh laserMesh;
    public Mesh energyMesh;

    public Material fireMaterial;
    public Material waterMaterial;
    public Material natureMaterial;
    public Material lightMaterial;
    public Material darkMaterial;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshFilter = GetComponentInChildren<MeshFilter>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    public void UpdateOutfit(Global.Misc.CannonAttackType attackType, Global.Misc.ElementType elementType)
    {
        meshFilter.mesh = attackType switch
        {
            Global.Misc.CannonAttackType.Bullet => bulletMesh,
            Global.Misc.CannonAttackType.Laser => laserMesh,
            Global.Misc.CannonAttackType.Energy => energyMesh,
            _ => throw new ArgumentOutOfRangeException(nameof(attackType), attackType, null)
        };

        meshRenderer.material = elementType switch
        {
            Global.Misc.ElementType.Fire => fireMaterial,
            Global.Misc.ElementType.Water => waterMaterial,
            Global.Misc.ElementType.Nature => natureMaterial,
            Global.Misc.ElementType.Light => lightMaterial,
            Global.Misc.ElementType.Dark => darkMaterial,
            _ => throw new ArgumentOutOfRangeException(nameof(elementType), elementType, null)
        };
    }
}
