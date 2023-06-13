using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePlayer : MonoBehaviour
{
    private void Awake()
    {
        Global.Battle.fakePlayer = this;
    }

    public void SetLookAt(Vector3 dir)
    {
        transform.LookAt(new Vector3(dir.x, -10f, dir.z));
    }

    public float GetLookAtDirectionY()
    {
        return transform.rotation.eulerAngles.y;
    }
}