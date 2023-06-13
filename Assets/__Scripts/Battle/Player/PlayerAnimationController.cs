using Animancer;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [HideInInspector] public AnimancerComponent playerAnimancerComponent;
    public AnimationClip idleAnimationClip;
    public AnimationClip moveFrontAnimationClip;
    public AnimationClip moveFrontLeftAnimationClip;
    public AnimationClip moveFrontRightAnimationClip;
    public AnimationClip moveBackAnimationClip;
    public AnimationClip moveBackLeftAnimationClip;
    public AnimationClip moveBackRightAnimationClip;
    public AnimationClip dieClip;

    private void Awake()
    {
        playerAnimancerComponent = GetComponent<AnimancerComponent>();
        EventCenter.GetInstance().AddEventListener(Global.Events.EnterBattleAnimation, () =>
        {
            PlayAnimationFront(0);
            Global.DoTweenWait(2f, () => { PlayAnimationIdle(0.8f); });
        });
    }

    public void PlayAnimationIdle(float fadeDuration = 0.2f)
    {
        playerAnimancerComponent.Play(idleAnimationClip, fadeDuration);
    }

    public void PlayAnimationFront(float fadeDuration = 0.2f)
    {
        playerAnimancerComponent.Play(moveFrontAnimationClip, fadeDuration);
    }

    public void PlayAnimationFrontLeft(float fadeDuration = 0.2f)
    {
        playerAnimancerComponent.Play(moveFrontLeftAnimationClip, fadeDuration);
    }

    public void PlayAnimationFrontRight(float fadeDuration = 0.2f)
    {
        playerAnimancerComponent.Play(moveFrontRightAnimationClip, fadeDuration);
    }

    public void PlayAnimationBack(float fadeDuration = 0.2f)
    {
        playerAnimancerComponent.Play(moveBackAnimationClip, fadeDuration);
    }

    public void PlayAnimationBackLeft(float fadeDuration = 0.2f)
    {
        playerAnimancerComponent.Play(moveBackLeftAnimationClip, fadeDuration);
    }

    public void PlayAnimationBackRight(float fadeDuration = 0.2f)
    {
        playerAnimancerComponent.Play(moveBackRightAnimationClip, fadeDuration);
    }

    public void PlayAnimationDie(float fadeDuration = 0.2f)
    {
        playerAnimancerComponent.Play(dieClip, fadeDuration);
    }
}