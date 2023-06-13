using Animancer;
using DG.Tweening;
using UnityEngine;

public class VoidCenter : MonoBehaviour
{
    public Transform expEndLocation;
    public float expValue;
    public float expSpeed;
    public float maxPowerUpExp;
    public AnimationClip rotationAnimationClip;

    private AnimancerComponent animancerComponent;

    private AnimancerState animationState;


    private void Awake()
    {
        Global.Battle.voidCenter = this;
        EventCenter.GetInstance().AddEventListener(Global.Events.StartSpawningEnemy, GenerateChaosBall);
        expValue = 0;

        animancerComponent = GetComponent<AnimancerComponent>();
        animationState = animancerComponent.Play(rotationAnimationClip);
    }

    private void Start()
    {
        maxPowerUpExp = Global.Battle.battleData.pickUpConfig.voidCenterExpPowerUpValue;
        Global.Battle.voidCenterProgressBar.maxValue = maxPowerUpExp;
    }

    public void CollectExp(ExpBall expBall, float expValueInput)
    {
        expValue += expValueInput;
        var distance = Vector3.Distance(expBall.transform.position, expEndLocation.position);
        expBall.transform.DOMove(expEndLocation.position, distance / expSpeed).SetEase(Ease.InSine).SetDelay(0.3f).OnComplete(() =>
        {
            Global.Battle.battlePrefabManager.expBallObjectPool.Release(expBall);
            CheckGeneratePowerUp(expValueInput);
            Global.Battle.battleEnergyDisplay.UpdateCount(expValueInput);
        });
    }

    public void GenerateChaosBall()
    {
        Global.DoTweenWait(30f, () =>
        {
            Global.Battle.voidCenterPickUpManager.GeneratePickUp(VoidCenterPickUpManager.PickUpTypes.ChaosBall);
            GenerateChaosBall();
        });
    }

    public void CheckGeneratePowerUp(float expIncrease)
    {
        Global.Battle.voidCenterProgressBar.UpdateValue(expIncrease);
        Global.Battle.voidCenterProgressBar.UpdateBar();
    }

    public void UpdateAnimationSpeed(float newSpeed)
    {
        if (animationState != null) animationState.Speed = newSpeed;
    }

    public void KillWave()
    {
        
    }
}