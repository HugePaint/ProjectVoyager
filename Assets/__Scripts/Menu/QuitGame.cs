using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void DoQuitGame () {
        Global.MainMenu.playerDataManager.SaveToDisk();
        
        Application.Quit();
        Debug.Log("Game is exiting");
    }
}
