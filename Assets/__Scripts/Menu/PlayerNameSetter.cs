using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNameSetter : MonoBehaviour
{
    public TMP_Text tmpText; 
    // Start is called before the first frame update
    void Start()
    {
        tmpText.text = Environment.UserName;
    }
}
