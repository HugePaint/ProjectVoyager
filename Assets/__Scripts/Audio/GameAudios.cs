using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudios : MonoBehaviour
{
    [Serializable]
    public struct GameAudio
    {
        public AudioClip audioClip;
        public float volume;
    }
    
    public enum AudioName
    {
        BattleBgm,
        BulletShoot,
        BulletHit,
        CannonPrepare,
        LaserDischarge,
        EnergyHit,
        EnergyShoot,

        MenuBgm,
        MenuEntryButtonClick,
        MenuEntryButtonHover,
        MenuEntryButtonFailed,
        MenuCannonHover,
        MenuEnterBattleSE,
        
        BossLaser,
        BossBodyAttack,
        
        KillWave,
        GetHit,
        Death,
        
        ChipGainCharging,
        ChipGainRelease,
        ChipGainClick,
    }
    
    [Serializable]
    public struct AudioNamePair
    {
        public GameAudio gameAudio;
        public AudioName audioName;
    }
    
    public Dictionary<AudioName, GameAudio> audioList;
    public List<AudioNamePair> audioNamePairs;

    private void Awake()
    {
        audioList = new Dictionary<AudioName, GameAudio>();
        foreach (var pair in audioNamePairs)
        {
            audioList.Add(pair.audioName, pair.gameAudio);
        }
    }
}
