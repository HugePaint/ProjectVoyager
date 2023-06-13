using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

public class TextVariablesGroupBuilder : MonoBehaviour
{
    [Tooltip("This asset will be overwritten once Build() is called.")]
    public VariablesGroupAsset textVariableGroupAsset;
    private List<ModificationEffectScriptableObject> scriptableObjectList;
    private int maxLevels = 11;
    private int maxModifiers = 6;

    public void Build()
    {
        var newTextVariableGroupAsset = ScriptableObject.CreateInstance<VariablesGroupAsset>();
        scriptableObjectList = ModificationEffectManager.GetAll();

        foreach (var modEffect in scriptableObjectList)
        {
            
        }

        var savePath = AssetDatabase.GetAssetPath(textVariableGroupAsset);
        AssetDatabase.CreateAsset(newTextVariableGroupAsset, savePath);
    }

    VariablesGroupAsset createModEffectVariablesGroupAsset(ModificationEffectScriptableObject modEffect)
    {
        VariablesGroupAsset asset = ScriptableObject.CreateInstance<VariablesGroupAsset>();
        // for (int i = 0; i < maxLevels)
        
        AssetDatabase.CreateAsset(asset,
            $"Assets/Settings/Localization/ModificationEffectVariableGroups/{modEffect.id}.asset");
        return asset;
    }


}
