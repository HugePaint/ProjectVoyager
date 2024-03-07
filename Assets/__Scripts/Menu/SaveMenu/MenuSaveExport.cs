using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuSaveExport : MonoBehaviour
{
    public TMP_InputField saveInputField;

    public void PutSaveStringInField() {
        // Set the text of your TextMeshPro text field
        saveInputField.text = Global.MainMenu.playerDataManager.CreateSaveString();
    }
    
    public void CopyText()
    {
        string textToCopy = saveInputField.text; // Get the text from your text field
        GUIUtility.systemCopyBuffer = textToCopy; // Copy text to clipboard
    }
}
