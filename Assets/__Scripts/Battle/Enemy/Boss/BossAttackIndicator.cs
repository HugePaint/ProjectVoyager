using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BossAttackIndicator : MonoBehaviour
{
    public Transform startPosition;
    public Transform endPosition;
    public Image image;
    public CanvasGroup imageCanvasGroup;
    public bool following;
    void Update()
    {
        if (!following) return;
        transform.LookAt(Global.Battle.playerBehaviourController.transform.position + new Vector3(0f,0.1f,0f));
    }

    public void Appear()
    {
        following = true;
        imageCanvasGroup.DOFade(1, 1f).From(0);
        image.material.DisableKeyword("GLOW_ON");
    }

    public void Disappear()
    {
        following = false;
        image.material.EnableKeyword("GLOW_ON");
        var glow = 1f;
        DOTween.To(() => glow, x => glow = x, 20f, 0.3f).OnUpdate(() =>
        {
            image.material.SetFloat("_Glow", glow);
        });
    }
}
