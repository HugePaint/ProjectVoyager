using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpBall : MonoBehaviour
{
    public void Collect(float expValue)
    {
        Global.Battle.voidCenter.CollectExp(this, expValue);
    }
}
