using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class MenuBgmManager : MonoBehaviour
{
    public enum Layer
    {
        Piano,
        Drums,
        Plucks,
        SinBass,
        SynthBass,
        Arp,
        Damage,
        CarAlarm,
        Vox,
        GlassyFm,
        ChipTune,
        Major,
        Normal
    }

    public enum MenuLevel
    {
        Main,
        PreGame,
        Upgrade,
        CannonOverview,
        CannonChipSetting,
        Options,
        Credits,
        Special,
        Normal
    }
    
    [Serializable]
    public class AudioLayer
    {
        public Layer identifier;
        public AudioSource audioSource;
        [Range(0, 1f)]
        public float targetVolume = 1f;
        [HideInInspector]
        public float currentVolume = 0;
        [HideInInspector]
        public Tween layerTween;
    }

    [Serializable]
    public class LayersInMenuLevel
    {
        public MenuLevel menuLevel;
        public List<Layer> layers;
    }

    public class LayerPlayer
    {
        public MenuLevel level;
        public List<AudioLayer> playingLayers;
        public List<AudioLayer> silenceLayers;

        public LayerPlayer()
        {
            playingLayers = new List<AudioLayer>();
            silenceLayers = new List<AudioLayer>();
        }

        public void SetVolume()
        {
            foreach (var audioLayer in playingLayers)
            {
                audioLayer.currentVolume = audioLayer.targetVolume;
            }
            foreach (var audioLayer in silenceLayers)
            {
                audioLayer.currentVolume = 0f;
            }
        }
    }
    
    public List<AudioLayer> audioLayers;
    public List<LayersInMenuLevel> layersInMenuLevels;
    public float fadeDuration;
    
    private Dictionary<MenuLevel, LayerPlayer> menuLevelPlayers;

    private void Awake()
    {
        // EventCenter.GetInstance().AddEventListener(Global.Events.UnlockCannon, UnlockBGM);
        // EventCenter.GetInstance().AddEventListener(Global.Events.GameOver, StopPlay);
        Global.MainMenu.menuBgmManager = this;
        
        var layerEnumToAudioLayer = new Dictionary<Layer, AudioLayer>();
        foreach (var item in audioLayers)
        {
            layerEnumToAudioLayer.Add(item.identifier, item);
        }

        menuLevelPlayers = new Dictionary<MenuLevel, LayerPlayer>();
        foreach (var level in layersInMenuLevels)
        {
            var layerPlayer = new LayerPlayer();
            foreach (var layer in level.layers)
            {
                layerPlayer.playingLayers.Add(layerEnumToAudioLayer[layer]);
            }

            foreach (var audioLayer in audioLayers)
            {
                if (layerPlayer.playingLayers.Contains(audioLayer)) continue;
                layerPlayer.silenceLayers.Add(audioLayer);
            }

            menuLevelPlayers.Add(level.menuLevel, layerPlayer);
        }
        
        UpdateVolume();
    }

    public void UpdateVolume(bool fade = false)
    {
        if (!fade)
        {
            foreach (var layer in audioLayers)
            {
                layer.audioSource.volume = layer.currentVolume * Global.Misc.bgmVolume;
            }
        }
        else
        {
            foreach (var layer in audioLayers)
            {
                layer.layerTween?.Kill();
                var targetLayer0Volume = layer.currentVolume * Global.Misc.bgmVolume;
                var layer0Volume = layer.audioSource.volume;
                layer.layerTween = DOTween.To(() => layer0Volume, x => layer0Volume = x, targetLayer0Volume, 0.5f).SetEase(Ease.Linear).OnUpdate(() =>
                {
                    layer.audioSource.volume = layer0Volume;
                });
            }
        }
    }

    public void MainMenuInitialize()
    {
        // menuLevelPlayers[MenuLevel.Main].SetVolume();
        menuLevelPlayers[MenuLevel.Normal].SetVolume();
        UpdateVolume();
        foreach (var audioLayer in audioLayers)
        {
            audioLayer.audioSource.Play();
        }
    }
    
    public void PlayBGM(MenuLevel menuLevel)
    {
        menuLevelPlayers[menuLevel].SetVolume();
        UpdateVolume(true);
    }
    
    // public void StartPlayBGMChipGain()
    // {
    //     chipGainAudioSource.volume = chipGainBGMVolume * Global.Misc.bgmVolume;
    //     chipGainAudioSource.Play();
    //     var currentVolume = chipGainAudioSource.volume;
    //     var volume = 0f;
    //     DOTween.To(() => volume, x => volume = x, currentVolume, 2f).SetEase(Ease.Linear).OnUpdate(() =>
    //     {
    //         chipGainAudioSource.volume = volume;
    //     });
    // }
    //
    // public void BGMChipGainFadeOut()
    // {
    //     var volume = chipGainAudioSource.volume;
    //     DOTween.To(() => volume, x => volume = x, 0f, 2f).SetEase(Ease.Linear).OnUpdate(() =>
    //     {
    //         chipGainAudioSource.volume = volume;
    //     });
    // }

    public void ResumeAll(bool fade = false)
    {
        foreach (var audioLayer in audioLayers)
        {
            if (fade == true)
            {
                audioLayer.audioSource.Play();
                var currentVolume = audioLayer.audioSource.volume;
                var volume = 0f;
                DOTween.To(() => volume, x => volume = x, currentVolume, fadeDuration).SetEase(Ease.Linear).OnUpdate(() =>
                {
                    audioLayer.audioSource.volume = volume;
                });
            }
            else
                audioLayer.audioSource.Play();
        }
    }
    
    public void StopAll(bool fade = false, float duration = -1f)
    {
        foreach (var audioLayer in audioLayers)
        {
            if (fade == true)
            {
                if (duration < 0) duration = fadeDuration;
                var volume = audioLayer.audioSource.volume;
                DOTween.To(() => volume, x => volume = x, 0f, duration).SetEase(Ease.Linear).OnUpdate(() =>
                {
                    audioLayer.audioSource.volume = volume;
                }).OnComplete(() =>
                {
                    audioLayer.audioSource.Stop();
                });
            }
            else
                audioLayer.audioSource.Stop();
        }
    }
    
    public void PauseAll(bool fade = false)
    {
        foreach (var audioLayer in audioLayers)
        {
            if (fade == true)
            {
                var volume = audioLayer.audioSource.volume;
                DOTween.To(() => volume, x => volume = x, 0f, fadeDuration).SetEase(Ease.Linear).OnUpdate(() =>
                {
                    audioLayer.audioSource.volume = volume;
                }).OnComplete(() =>
                {
                    audioLayer.audioSource.Pause();
                });
            }
            else
                audioLayer.audioSource.Pause();
        }
    }
}