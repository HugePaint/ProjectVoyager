using DG.Tweening;
using UnityEngine;

public class BattleBgmManager : MonoBehaviour
{
    public AudioSource layer0;
    public AudioSource layer1;
    public AudioSource layer2;
    
    public AudioSource chipGainAudioSource;

    public float layer0TargetVolume;
    public float layer1TargetVolume;
    public float layer2TargetVolume;
    public float chipGainBGMVolume;
    public float globalBGMTargetVolume;

    private float layer0CurrentVolume;
    private float layer1CurrentVolume;
    private float layer2CurrentVolume;
    private float globalBGMCurrentVolume;

    private int level;

    private Tween layer0Tween;
    private Tween layer1Tween;
    private Tween layer2Tween;

    private void Awake()
    {
        EventCenter.GetInstance().AddEventListener(Global.Events.UnlockCannon, UnlockBGM);
        EventCenter.GetInstance().AddEventListener(Global.Events.GameOver, StopPlay);
        Global.Battle.battleBgmManager = this;

        level = 0;
        globalBGMCurrentVolume = 1f;
        layer0CurrentVolume = layer0TargetVolume;
        layer1CurrentVolume = 0f;
        layer2CurrentVolume = 0f;
        UpdateVolume();
    }

    public void UpdateVolume(bool fade = false)
    {
        if (!fade)
        {
            layer0.volume = layer0CurrentVolume * globalBGMCurrentVolume * Global.Misc.bgmVolume;
            layer1.volume = layer1CurrentVolume * globalBGMCurrentVolume * Global.Misc.bgmVolume;
            layer2.volume = layer2CurrentVolume * globalBGMCurrentVolume * Global.Misc.bgmVolume;
        }
        else
        {
            layer0Tween?.Kill();
            var targetLayer0Volume = layer0CurrentVolume * globalBGMCurrentVolume * Global.Misc.bgmVolume;
            var layer0Volume = layer0.volume;
            layer0Tween = DOTween.To(() => layer0Volume, x => layer0Volume = x, targetLayer0Volume, 2f).SetEase(Ease.Linear).OnUpdate(() =>
            {
                layer0.volume = layer0Volume;
            });

            layer1Tween?.Kill();
            var targetLayer1Volume = layer1CurrentVolume * globalBGMCurrentVolume * Global.Misc.bgmVolume;
            var layer1Volume = layer1.volume;
            layer1Tween = DOTween.To(() => layer1Volume, x => layer1Volume = x, targetLayer1Volume, 2f).SetEase(Ease.Linear).OnUpdate(() =>
            {
                layer1.volume = layer1Volume;
            });

            layer2Tween?.Kill();
            var targetLayer2Volume = layer2CurrentVolume * globalBGMCurrentVolume * Global.Misc.bgmVolume;
            var layer2Volume = layer2.volume;
            layer2Tween = DOTween.To(() => layer2Volume, x => layer2Volume = x, targetLayer2Volume, 2f).SetEase(Ease.Linear).OnUpdate(() =>
            {
                layer2.volume = layer2Volume;
            });
        }
    }

    public void StartPlayBGM()
    {
        layer0.Play();
        layer1.Play();
        layer2.Play();

        var currentLayer0Volume = layer0.volume;
        var volume = 0f;
        DOTween.To(() => volume, x => volume = x, currentLayer0Volume, 2f).SetEase(Ease.Linear).OnUpdate(() =>
        {
            layer0.volume = volume;
        });
    }
    
    public void StartPlayBGMChipGain()
    {
        chipGainAudioSource.volume = chipGainBGMVolume * Global.Misc.bgmVolume;
        chipGainAudioSource.Play();
        var currentVolume = chipGainAudioSource.volume;
        var volume = 0f;
        DOTween.To(() => volume, x => volume = x, currentVolume, 2f).SetEase(Ease.Linear).OnUpdate(() =>
        {
            chipGainAudioSource.volume = volume;
        });
    }

    public void BGMChipGainFadeOut()
    {
        var volume = chipGainAudioSource.volume;
        DOTween.To(() => volume, x => volume = x, 0f, 2f).SetEase(Ease.Linear).OnUpdate(() =>
        {
            chipGainAudioSource.volume = volume;
        });
    }

    public void StopPlay()
    {
        layer0.Stop();
        layer1.Stop();
        layer2.Stop();
    }

    public void UnlockBGM()
    {
        level += 1;
        if (level == 2)
        {
            layer1CurrentVolume = layer1TargetVolume;
            UpdateVolume(true);
        }

        if (level == 4)
        {
            layer2CurrentVolume = layer2TargetVolume;
            UpdateVolume(true);
        }
    }

    public void GlobalVolumeDown(float duration)
    {
        var volume = globalBGMCurrentVolume;
        DOTween.To(() => volume, x => volume = x, globalBGMTargetVolume, duration).SetEase(Ease.Linear).SetUpdate(isIndependentUpdate: true).OnUpdate(() =>
        {
            globalBGMCurrentVolume = volume;
            UpdateVolume();
        });
    }

    public void GlobalVolumeBack(float duration)
    {
        var volume = globalBGMCurrentVolume;
        DOTween.To(() => volume, x => volume = x, 1f, duration).SetEase(Ease.Linear).SetUpdate(isIndependentUpdate: true).OnUpdate(() =>
        {
            globalBGMCurrentVolume = volume;
            UpdateVolume();
        });
    }
}