using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public GameAudios gameAudios;
    public AudioSource bgmAudioSource;

    public AudioSource soundEffectObject;
    public ObjectPool<AudioSource> soundEffectObjectPool;

    private GameAudios.AudioName currentBgmName;

    private void Awake()
    {
        Global.Battle.audioManager = this;
        gameAudios = GetComponent<GameAudios>();

        //temp volume
        // Global.Misc.bgmVolume = 1f;
        // Global.Misc.soundEffectVolume = 1f;
        // Now initialized in VolumeControl

        EventCenter.GetInstance().AddEventListener<GameAudios.AudioName>(Global.Events.PlayAudioBattleBgm, PlayBgm);
        EventCenter.GetInstance().AddEventListener<GameAudios.AudioName>(Global.Events.PlayAudioSoundEffect, PlayOnce);
        EventCenter.GetInstance().AddEventListener<GameAudios.AudioName>(Global.Events.PlayMenuSoundEffect, PlayMenuSE);
        EventCenter.GetInstance().AddEventListener<GameAudios.AudioName>(Global.Events.FadeOutBgm, FadeOutBgm);
        
        soundEffectObjectPool = new ObjectPool<AudioSource>(() => { return Instantiate(soundEffectObject, parent:transform); },
            audioSource => { audioSource.gameObject.SetActive(true); }, audioSource => { audioSource.gameObject.SetActive(false); },
            audioSource => { Destroy(audioSource.gameObject); }, false, 100, 300);
    }
    
    

    public void PlayOnce(GameAudios.AudioName audioName)
    {
        var audioSource = soundEffectObjectPool.Get();
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        var gameAudio = gameAudios.audioList[audioName];
        audioSource.clip = gameAudio.audioClip;
        var volumeMultiplier = Random.Range(0.9f, 1.1f);
        audioSource.volume = gameAudio.volume * Global.Misc.soundEffectVolume * volumeMultiplier;
        audioSource.loop = false;
        audioSource.playOnAwake = false;
        audioSource.Play();
        Global.DoTweenWait(audioSource.clip.length, () =>
        {
            soundEffectObjectPool.Release(audioSource);
        });
    }

    public void PlayBgm(GameAudios.AudioName audioName)
    {
        if (bgmAudioSource) bgmAudioSource.Stop();
        currentBgmName = audioName;
        var gameAudio = gameAudios.audioList[audioName];
        bgmAudioSource.clip = gameAudio.audioClip;
        bgmAudioSource.volume = gameAudio.volume * Global.Misc.bgmVolume;
        bgmAudioSource.loop = true;
        bgmAudioSource.playOnAwake = false;
        bgmAudioSource.Play();
    }

    public void StopBgm()
    {
        if (bgmAudioSource) bgmAudioSource.Stop();
    }

    public void PlayMenuSE(GameAudios.AudioName audioName)
    {
        // TODO: REFACTOR THIS!!!!
        var audioSource = soundEffectObjectPool.Get();
        audioSource.pitch = Random.Range(0.8f, 1f);
        var gameAudio = gameAudios.audioList[audioName];
        audioSource.clip = gameAudio.audioClip;
        var volumeMultiplier = Random.Range(0.9f, 1.1f);
        audioSource.volume = gameAudio.volume * Global.Misc.soundEffectVolume * volumeMultiplier;
        audioSource.loop = false;
        audioSource.playOnAwake = false;
        audioSource.Play();
        Global.DoTweenWait(audioSource.clip.length, () =>
        {
            soundEffectObjectPool.Release(audioSource);
        });
    }

    public void FadeOutBgm(GameAudios.AudioName audioName)
    {
        DOTween.To(() => bgmAudioSource.volume, x => bgmAudioSource.volume = x, 0f, 0.5f)
            .OnComplete(() =>
            {
                bgmAudioSource.Stop();
            });
    }

    public void UpdateBgmVolume()
    {
        var gameAudio = gameAudios.audioList[currentBgmName];
        bgmAudioSource.volume = gameAudio.volume * Global.Misc.bgmVolume;
    }
}