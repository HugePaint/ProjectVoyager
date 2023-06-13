using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MagicaCloth;
using MoreMountains.Feedbacks;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerModelLocalController : MonoBehaviour
{
    public MagicaPhysicsManager magicaPhysicsManager;
    private MMF_Player mmfPlayer;
    private AudioSource audioSource;
    private void Awake()
    {
        EventCenter.GetInstance().AddEventListener(Global.Events.EnterBattleAnimation,PlayEnterBattleAnimation);
        EventCenter.GetInstance().AddEventListener(Global.Events.GameStart, StartRealGameAction);
        mmfPlayer = GetComponent<MMF_Player>();
        audioSource = GetComponent<AudioSource>();
    }

    public void StartRealGameAction()
    {
        //Hide player
        transform.position = new Vector3(0f, -10f, 0f);
        if (magicaPhysicsManager) magicaPhysicsManager.enabled = false;
    }

    public void PlayEnterBattleAnimation()
    {
        audioSource.Play();
        Global.DoTweenWait(0.1f, () =>
        {
            mmfPlayer.PlayFeedbacks();
            if (magicaPhysicsManager) magicaPhysicsManager.enabled = true;
            transform.position = Camera.main.GetComponentInChildren<CharacterStartPosition>().transform.position;
            transform.LookAt(Global.Battle.playerBehaviourController.transform);
            transform.DOLocalMove(new Vector3(0f, 0f, 0f),3f);
            transform.DOLocalRotate(new Vector3(0f, 0f, 0f), 1f).SetDelay(2f);
        });
    }
}
