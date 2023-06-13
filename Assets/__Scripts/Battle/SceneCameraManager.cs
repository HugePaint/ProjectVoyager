using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCameraManager : MonoBehaviour
{
    public GameObject battleCamera;
    public GameObject chipGainCamera;

    private void Start()
    {
        if (Global.Battle.battleData.isGainingChips)
        {
            SwitchToChipCamera();
        }
        else
        {
            SwitchToBattleCamera();
        }
    }

    public void SwitchToBattleCamera()
    {
        chipGainCamera.SetActive(false);
        battleCamera.SetActive(true);
    }

    public void SwitchToChipCamera()
    {
        chipGainCamera.SetActive(true);
        battleCamera.SetActive(false);
    }
}
