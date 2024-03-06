using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuSaveImport : MonoBehaviour
{
    public TMP_InputField saveInputField;
    public GameObject PopUpSuccess;
    public GameObject PopUpFailed;
    public GameObject curtain;

    public void ImportSaveString() {
        int status = Global.MainMenu.playerDataManager.LoadSaveFromString(saveInputField.text);
        if (status == 1)
        {
            PopUpSuccess.SetActive(true);
            curtain.SetActive(true);
            Global.DoTweenWait(1.5f, () => {
                Global.MainMenu.menuController.Reload();
            });
        }

        if (status == 0)
        {
            PopUpFailed.SetActive(true);
        }
    }
}
