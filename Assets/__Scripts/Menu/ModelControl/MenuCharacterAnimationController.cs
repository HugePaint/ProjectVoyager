using Animancer;
using DG.Tweening;
using UnityEngine;

public sealed class MenuCharacterAnimationController : MonoBehaviour
{
    [HideInInspector] public AnimancerComponent playerAnimancer;
    [SerializeField] private ClipTransition idleForward;
    [SerializeField] private ClipTransition chipMenuClip;

    //private AnimancerState state;
    
    private void Awake()
    {
        playerAnimancer = GetComponent<AnimancerComponent>();
        EventCenter.GetInstance().AddEventListener(Global.Events.MenuPreGameShow, StartPreGameMenuClip);
        EventCenter.GetInstance().AddEventListener(Global.Events.MenuMainShow, StartIdleClip);
    }
    
    private void OnEnable()
    {
        StartIdleClip();
        //RepeatAnimation(state, true);
    }

    public void StartIdleClip()
    {
        if (!playerAnimancer.IsPlayingClip(idleForward.Clip))
        {
            var state = playerAnimancer.Play(idleForward, 0.5f);
        }
    }
    
    public void StartPreGameMenuClip()
    {
        var state = playerAnimancer.Play(chipMenuClip, 0.5f);
    }
    
    public void RepeatAnimation(AnimancerState state, bool playForward)
    {
        Global.DoTweenWait(5f, () =>
        {
            state.Speed = playForward ? 1f : -1f;
            RepeatAnimation(state, !playForward);
        });
    }
}



