using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MoreMountains.Feedbacks;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class CannonLocalMotionManager : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private int id;
    private List<CannonStartPosition> cannonStartPositions;
    private MMF_Player mmfPlayer;
    private AudioSource audioSource;
    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        mmfPlayer = GetComponent<MMF_Player>();
        audioSource = GetComponent<AudioSource>();
        EventCenter.GetInstance().AddEventListener(Global.Events.EnterBattleAnimation, PlayStartGameAnimation);
        cannonStartPositions =
            new List<CannonStartPosition>(Camera.main.GetComponentsInChildren<CannonStartPosition>());
        EventCenter.GetInstance().AddEventListener(Global.Events.GameStart, StartRealGameAction);
    }

    public void StartRealGameAction()
    {
        meshRenderer.enabled = false;
    }

    public void DoGunKick(float duration)
    {
        transform.DOPunchPosition(new Vector3(0f,0f,-0.5f), duration);
    }

    public void PlayStartGameAnimation()
    {
        meshRenderer.material.DisableKeyword("_EMISSION");
        meshRenderer.material.EnableKeyword("_EMISSION");
        meshRenderer.material.SetColor("_EmissionColor", Color.white * 5f);
        id = GetComponentInParent<CannonBattle>().cannonId;
        transform.position = cannonStartPositions[id].transform.position;
        var randomWait = Random.Range(0.3f, 0.5f);
        Global.DoTweenWait(id*randomWait + 1f, () =>
        {
            mmfPlayer.PlayFeedbacks();
            audioSource.Play();
            meshRenderer.enabled = true;
            transform.LookAt(Global.Battle.playerBehaviourController.transform);
            var angle = transform.rotation.eulerAngles;
            transform.DOLocalMove(new Vector3(0f, 0f, 0f), 3f);
            transform.DOLocalRotate(new Vector3(0f, 0f, 0f), 2f).SetDelay(1f);
        });
    }
}