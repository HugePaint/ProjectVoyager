using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpTimer : MonoBehaviour {
    public float timeout = 1f;
    void OnEnable() {
        Global.DoTweenWait(timeout, () => {
            gameObject.SetActive(false);
        });
    }
}
