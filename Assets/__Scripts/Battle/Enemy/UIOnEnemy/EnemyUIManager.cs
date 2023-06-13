using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUIManager : MonoBehaviour
{
    public ElementSymbolOnEnemy elementSymbol;

    private void Update()
    {
        transform.LookAt(Camera.main.transform);
    }
}
