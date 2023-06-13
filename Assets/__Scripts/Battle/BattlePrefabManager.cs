using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BattlePrefabManager : MonoBehaviour
{
    #region WeaponHoldParent

    public Transform bulletHolderParent;
    public Transform expBallHolderParent;
    public Transform laserHolderParent;
    public Transform energyHolderParent;

    #endregion

    #region Misc
    [SerializeField] private ExpBall expBallPrefab;
    public ObjectPool<ExpBall> expBallObjectPool;

    public Sprite fireElementSprite;
    public Sprite waterElementSprite;
    public Sprite natureElementSprite;

    public Sprite laserSprite;
    public Sprite bulletSprite;
    public Sprite energySprite;

    #endregion

    #region Bullet

    [SerializeField] private Bullet bulletFire;
    [SerializeField] private Bullet bulletWater;
    [SerializeField] private Bullet bulletNature;
    [SerializeField] private Bullet bulletLight;
    [SerializeField] private Bullet bulletDark;

    [SerializeField] private GameObject bulletFireHit;
    [SerializeField] private GameObject bulletWaterHit;
    [SerializeField] private GameObject bulletNatureHit;
    [SerializeField] private GameObject bulletLightHit;
    [SerializeField] private GameObject bulletDarkHit;

    public ObjectPool<Bullet> bulletFirePool;
    private int bulletFireCapacity;
    private int bulletFireMaxSize;
    public ObjectPool<GameObject> bulletFireHitPool;
    private int bulletFireHitCapacity;
    private int bulletFireHitMaxSize;

    public ObjectPool<Bullet> bulletWaterPool;
    private int bulletWaterCapacity;
    private int bulletWaterMaxSize;
    public ObjectPool<GameObject> bulletWaterHitPool;
    private int bulletWaterHitCapacity;
    private int bulletWaterHitMaxSize;

    public ObjectPool<Bullet> bulletNaturePool;
    private int bulletNatureCapacity;
    private int bulletNatureMaxSize;
    public ObjectPool<GameObject> bulletNatureHitPool;
    private int bulletNatureHitCapacity;
    private int bulletNatureHitMaxSize;

    public ObjectPool<Bullet> bulletLightPool;
    private int bulletLightCapacity;
    private int bulletLightMaxSize;
    public ObjectPool<GameObject> bulletLightHitPool;
    private int bulletLightHitCapacity;
    private int bulletLightHitMaxSize;

    public ObjectPool<Bullet> bulletDarkPool;
    private int bulletDarkCapacity;
    private int bulletDarkMaxSize;
    public ObjectPool<GameObject> bulletDarkHitPool;
    private int bulletDarkHitCapacity;
    private int bulletDarkHitMaxSize;

    #endregion

    #region Laser

    [SerializeField] private Laser laserFire;
    [SerializeField] private Laser laserWater;
    [SerializeField] private Laser laserNature;
    [SerializeField] private Laser laserDark;
    [SerializeField] private Laser laserLight;

    [SerializeField] private LaserHitEffect laserFireHit;
    [SerializeField] private LaserHitEffect laserWaterHit;
    [SerializeField] private LaserHitEffect laserNatureHit;
    [SerializeField] private LaserHitEffect laserDarkHit;
    [SerializeField] private LaserHitEffect laserLightHit;
    
    public ObjectPool<Laser> laserFirePool;
    private int laserFireCapacity;
    private int laserFireMaxSize;
    public ObjectPool<LaserHitEffect> laserFireHitPool;
    private int laserFireHitCapacity;
    private int laserFireHitMaxSize;
    
    public ObjectPool<Laser> laserWaterPool;
    private int laserWaterCapacity;
    private int laserWaterMaxSize;
    public ObjectPool<LaserHitEffect> laserWaterHitPool;
    private int laserWaterHitCapacity;
    private int laserWaterHitMaxSize;
    
    public ObjectPool<Laser> laserNaturePool;
    private int laserNatureCapacity;
    private int laserNatureMaxSize;
    public ObjectPool<LaserHitEffect> laserNatureHitPool;
    private int laserNatureHitCapacity;
    private int laserNatureHitMaxSize;
    
    public ObjectPool<Laser> laserLightPool;
    private int laserLightCapacity;
    private int laserLightMaxSize;
    public ObjectPool<LaserHitEffect> laserLightHitPool;
    private int laserLightHitCapacity;
    private int laserLightHitMaxSize;
    
    public ObjectPool<Laser> laserDarkPool;
    private int laserDarkCapacity;
    private int laserDarkMaxSize;
    public ObjectPool<LaserHitEffect> laserDarkHitPool;
    private int laserDarkHitCapacity;
    private int laserDarkHitMaxSize;

    #endregion

    #region Energy

    [SerializeField] private Energy energyFire;
    [SerializeField] private Energy energyWater;
    [SerializeField] private Energy energyNature;
    [SerializeField] private Energy energyDark;
    [SerializeField] private Energy energyLight;

    [SerializeField] private EnergyHit energyFireHit;
    [SerializeField] private EnergyHit energyWaterHit;
    [SerializeField] private EnergyHit energyNatureHit;
    [SerializeField] private EnergyHit energyDarkHit;
    [SerializeField] private EnergyHit energyLightHit;
    
    public ObjectPool<Energy> energyFirePool;
    private int energyFireCapacity;
    private int energyFireMaxSize;
    public ObjectPool<EnergyHit> energyFireHitPool;
    private int energyFireHitCapacity;
    private int energyFireHitMaxSize;
    
    public ObjectPool<Energy> energyWaterPool;
    private int energyWaterCapacity;
    private int energyWaterMaxSize;
    public ObjectPool<EnergyHit> energyWaterHitPool;
    private int energyWaterHitCapacity;
    private int energyWaterHitMaxSize;
    
    public ObjectPool<Energy> energyNaturePool;
    private int energyNatureCapacity;
    private int energyNatureMaxSize;
    public ObjectPool<EnergyHit> energyNatureHitPool;
    private int energyNatureHitCapacity;
    private int energyNatureHitMaxSize;
    
    public ObjectPool<Energy> energyLightPool;
    private int energyLightCapacity;
    private int energyLightMaxSize;
    public ObjectPool<EnergyHit> energyLightHitPool;
    private int energyLightHitCapacity;
    private int energyLightHitMaxSize;
    
    public ObjectPool<Energy> energyDarkPool;
    private int energyDarkCapacity;
    private int energyDarkMaxSize;
    public ObjectPool<EnergyHit> energyDarkHitPool;
    private int energyDarkHitCapacity;
    private int energyDarkHitMaxSize;
    

    #endregion

    private void Awake()
    {
        Global.Battle.battlePrefabManager = this;
    }

    private void Start()
    {
        
    }

    public void BuildPool()
    {
        CalculateCapacity();
        BuildBulletPool();
        BuildLaserPool();
        BuildEnergyPool();
        BuildMiscPool();
    }

    public void CalculateCapacity()
    {
        bulletNatureCapacity += 20;
        bulletNatureMaxSize += 100;
        bulletNatureHitCapacity += 20;
        bulletNatureHitMaxSize += 100;
        
        foreach (var cannon in Global.Battle.cannonBattleManager.cannonBattles)
        {
            switch (cannon.attackType)
            {
                case Global.Misc.CannonAttackType.Bullet:
                    switch (cannon.elementType)
                    {
                        case Global.Misc.ElementType.Fire:
                            bulletFireCapacity += 20;
                            bulletFireMaxSize += 100;
                            bulletFireHitCapacity += 20;
                            bulletFireHitMaxSize += 100;
                            break;
                        case Global.Misc.ElementType.Water:
                            bulletWaterCapacity += 20;
                            bulletWaterMaxSize += 100;
                            bulletWaterHitCapacity += 20;
                            bulletWaterHitMaxSize += 100;
                            break;
                        case Global.Misc.ElementType.Nature:
                            bulletNatureCapacity += 20;
                            bulletNatureMaxSize += 100;
                            bulletNatureHitCapacity += 20;
                            bulletNatureHitMaxSize += 100;
                            break;
                        case Global.Misc.ElementType.Light:
                            bulletLightCapacity += 20;
                            bulletLightMaxSize += 100;
                            bulletLightHitCapacity += 20;
                            bulletLightHitMaxSize += 100;
                            break;
                        case Global.Misc.ElementType.Dark:
                            bulletDarkCapacity += 20;
                            bulletDarkMaxSize += 100;
                            bulletDarkHitCapacity += 20;
                            bulletDarkHitMaxSize += 100;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    break;
                case Global.Misc.CannonAttackType.Laser:
                    switch (cannon.elementType)
                    {
                        case Global.Misc.ElementType.Fire:
                            laserFireCapacity += 10;
                            laserFireMaxSize += 50;
                            laserFireHitCapacity += 30;
                            laserFireHitMaxSize += 150;
                            break;
                        case Global.Misc.ElementType.Water:
                            laserWaterCapacity += 10;
                            laserWaterMaxSize += 50;
                            laserWaterHitCapacity += 30;
                            laserWaterHitMaxSize += 150;
                            break;
                        case Global.Misc.ElementType.Nature:
                            laserNatureCapacity += 10;
                            laserNatureMaxSize += 50;
                            laserNatureHitCapacity += 30;
                            laserNatureHitMaxSize += 150;
                            break;
                        case Global.Misc.ElementType.Light:
                            laserLightCapacity += 10;
                            laserLightMaxSize += 50;
                            laserLightHitCapacity += 30;
                            laserLightHitMaxSize += 150;
                            break;
                        case Global.Misc.ElementType.Dark:
                            laserDarkCapacity += 10;
                            laserDarkMaxSize += 50;
                            laserDarkHitCapacity += 30;
                            laserDarkHitMaxSize += 150;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    break;
                case Global.Misc.CannonAttackType.Energy:
                    switch (cannon.elementType)
                    {
                        case Global.Misc.ElementType.Fire:
                            energyFireCapacity += 10;
                            energyFireMaxSize += 50;
                            energyFireHitCapacity += 20;
                            energyFireHitMaxSize += 100;
                            break;
                        case Global.Misc.ElementType.Water:
                            energyWaterCapacity += 10;
                            energyWaterMaxSize += 50;
                            energyWaterHitCapacity += 20;
                            energyWaterHitMaxSize += 100;
                            break;
                        case Global.Misc.ElementType.Nature:
                            energyNatureCapacity += 10;
                            energyNatureMaxSize += 50;
                            energyNatureHitCapacity += 20;
                            energyNatureHitMaxSize += 100;
                            break;
                        case Global.Misc.ElementType.Light:
                            energyLightCapacity += 10;
                            energyLightMaxSize += 50;
                            energyLightHitCapacity += 20;
                            energyLightHitMaxSize += 100;
                            break;
                        case Global.Misc.ElementType.Dark:
                            energyDarkCapacity += 10;
                            energyDarkMaxSize += 50;
                            energyDarkHitCapacity += 20;
                            energyDarkHitMaxSize += 100;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void BuildBulletPool()
    {
        if (bulletFireCapacity > 0)
        {
            bulletFirePool = new ObjectPool<Bullet>(() => { return Instantiate(bulletFire, bulletHolderParent); },
                bullet => { bullet.gameObject.SetActive(true); }, bullet => { bullet.gameObject.SetActive(false); },
                bullet => { Destroy(bullet.gameObject); }, false, bulletFireCapacity, bulletFireMaxSize);
        }

        if (bulletFireHitCapacity > 0)
        {
            bulletFireHitPool = new ObjectPool<GameObject>(() => { return Instantiate(bulletFireHit, bulletHolderParent); },
                hit => { hit.SetActive(true); }, hit => { hit.SetActive(false); },
                hit => { Destroy(hit); }, false, bulletFireHitCapacity, bulletFireHitMaxSize);
        }


        if (bulletWaterCapacity > 0)
        {
            bulletWaterPool = new ObjectPool<Bullet>(() => { return Instantiate(bulletWater, bulletHolderParent); },
                bullet => { bullet.gameObject.SetActive(true); }, bullet => { bullet.gameObject.SetActive(false); },
                bullet => { Destroy(bullet.gameObject); }, false, bulletWaterCapacity, bulletWaterMaxSize);
        }

        if (bulletWaterHitCapacity > 0)
        {
            bulletWaterHitPool = new ObjectPool<GameObject>(() => { return Instantiate(bulletWaterHit, bulletHolderParent); },
                hit => { hit.SetActive(true); }, hit => { hit.SetActive(false); },
                hit => { Destroy(hit); }, false, bulletWaterHitCapacity, bulletWaterHitMaxSize);
        }


        if (bulletNatureCapacity > 0)
        {
            bulletNaturePool = new ObjectPool<Bullet>(() => { return Instantiate(bulletNature, bulletHolderParent); },
                bullet => { bullet.gameObject.SetActive(true); }, bullet => { bullet.gameObject.SetActive(false); },
                bullet => { Destroy(bullet.gameObject); }, false, bulletNatureCapacity, bulletNatureMaxSize);
        }

        if (bulletNatureHitCapacity > 0)
        {
            bulletNatureHitPool = new ObjectPool<GameObject>(() => { return Instantiate(bulletNatureHit, bulletHolderParent); },
                hit => { hit.SetActive(true); }, hit => { hit.SetActive(false); },
                hit => { Destroy(hit); }, false, bulletNatureHitCapacity, bulletNatureHitMaxSize);
        }


        if (bulletLightCapacity > 0)
        {
            bulletLightPool = new ObjectPool<Bullet>(() => { return Instantiate(bulletLight, bulletHolderParent); },
                bullet => { bullet.gameObject.SetActive(true); }, bullet => { bullet.gameObject.SetActive(false); },
                bullet => { Destroy(bullet.gameObject); }, false, bulletLightCapacity, bulletLightMaxSize);
        }

        if (bulletLightHitCapacity > 0)
        {
            bulletLightHitPool = new ObjectPool<GameObject>(() => { return Instantiate(bulletLightHit, bulletHolderParent); },
                hit => { hit.SetActive(true); }, hit => { hit.SetActive(false); },
                hit => { Destroy(hit); }, false, bulletLightHitCapacity, bulletLightHitMaxSize);
        }


        if (bulletDarkCapacity > 0)
        {
            bulletDarkPool = new ObjectPool<Bullet>(() => { return Instantiate(bulletDark, bulletHolderParent); },
                bullet => { bullet.gameObject.SetActive(true); }, bullet => { bullet.gameObject.SetActive(false); },
                bullet => { Destroy(bullet.gameObject); }, false, bulletDarkCapacity, bulletDarkMaxSize);
        }

        if (bulletDarkHitCapacity > 0)
        {
            bulletDarkHitPool = new ObjectPool<GameObject>(() => { return Instantiate(bulletDarkHit, bulletHolderParent); },
                hit => { hit.SetActive(true); }, hit => { hit.SetActive(false); },
                hit => { Destroy(hit); }, false, bulletDarkHitCapacity, bulletDarkHitMaxSize);
        }
    }

    private void BuildLaserPool()
    {
        if (laserFireCapacity > 0)
        {
            laserFirePool = new ObjectPool<Laser>(() => { return Instantiate(laserFire); },
                laser => { laser.gameObject.SetActive(true); }, laser => { laser.gameObject.SetActive(false); },
                laser => { Destroy(laser.gameObject); }, false, laserFireCapacity, laserFireMaxSize);
        }

        if (laserFireHitCapacity > 0)
        {
            laserFireHitPool = new ObjectPool<LaserHitEffect>(() => { return Instantiate(laserFireHit, laserHolderParent); },
                hit => { hit.gameObject.SetActive(true); }, hit => { hit.gameObject.SetActive(false); },
                hit => { Destroy(hit.gameObject); }, false, laserFireHitCapacity, laserFireHitMaxSize);
        }
        
        
        
        if (laserWaterCapacity > 0)
        {
            laserWaterPool = new ObjectPool<Laser>(() => { return Instantiate(laserWater); },
                laser => { laser.gameObject.SetActive(true); }, laser => { laser.gameObject.SetActive(false); },
                laser => { Destroy(laser.gameObject); }, false, laserWaterCapacity, laserWaterMaxSize);
        }

        if (laserWaterHitCapacity > 0)
        {
            laserWaterHitPool = new ObjectPool<LaserHitEffect>(() => { return Instantiate(laserWaterHit, laserHolderParent); },
                hit => { hit.gameObject.SetActive(true); }, hit => { hit.gameObject.SetActive(false); },
                hit => { Destroy(hit.gameObject); }, false, laserWaterHitCapacity, laserWaterHitMaxSize);
        }
        
        
        
        if (laserNatureCapacity > 0)
        {
            laserNaturePool = new ObjectPool<Laser>(() => { return Instantiate(laserNature); },
                laser => { laser.gameObject.SetActive(true); }, laser => { laser.gameObject.SetActive(false); },
                laser => { Destroy(laser.gameObject); }, false, laserNatureCapacity, laserNatureMaxSize);
        }

        if (laserNatureHitCapacity > 0)
        {
            laserNatureHitPool = new ObjectPool<LaserHitEffect>(() => { return Instantiate(laserNatureHit, laserHolderParent); },
                hit => { hit.gameObject.SetActive(true); }, hit => { hit.gameObject.SetActive(false); },
                hit => { Destroy(hit.gameObject); }, false, laserNatureHitCapacity, laserNatureHitMaxSize);
        }
        
        
        
        if (laserDarkCapacity > 0)
        {
            laserDarkPool = new ObjectPool<Laser>(() => { return Instantiate(laserDark); },
                laser => { laser.gameObject.SetActive(true); }, laser => { laser.gameObject.SetActive(false); },
                laser => { Destroy(laser.gameObject); }, false, laserDarkCapacity, laserDarkMaxSize);
        }

        if (laserDarkHitCapacity > 0)
        {
            laserDarkHitPool = new ObjectPool<LaserHitEffect>(() => { return Instantiate(laserDarkHit, laserHolderParent); },
                hit => { hit.gameObject.SetActive(true); }, hit => { hit.gameObject.SetActive(false); },
                hit => { Destroy(hit.gameObject); }, false, laserDarkHitCapacity, laserDarkHitMaxSize);
        }
        
        
        
        if (laserLightCapacity > 0)
        {
            laserLightPool = new ObjectPool<Laser>(() => { return Instantiate(laserLight); },
                laser => { laser.gameObject.SetActive(true); }, laser => { laser.gameObject.SetActive(false); },
                laser => { Destroy(laser.gameObject); }, false, laserLightCapacity, laserLightMaxSize);
        }

        if (laserLightHitCapacity > 0)
        {
            laserLightHitPool = new ObjectPool<LaserHitEffect>(() => { return Instantiate(laserLightHit, laserHolderParent); },
                hit => { hit.gameObject.SetActive(true); }, hit => { hit.gameObject.SetActive(false); },
                hit => { Destroy(hit.gameObject); }, false, laserLightHitCapacity, laserLightHitMaxSize);
        }
    }

    private void BuildEnergyPool()
    {
        if (energyFireCapacity > 0)
        {
            energyFirePool = new ObjectPool<Energy>(() => { return Instantiate(energyFire, energyHolderParent); },
                energy => { energy.gameObject.SetActive(true); }, energy => { energy.gameObject.SetActive(false); },
                energy => { Destroy(energy.gameObject); }, false, energyFireCapacity, energyFireMaxSize);
        }
        
        if (energyFireHitCapacity > 0)
        {
            energyFireHitPool = new ObjectPool<EnergyHit>(() => { return Instantiate(energyFireHit, energyHolderParent); },
                hit => { hit.gameObject.SetActive(true); }, hit => { hit.gameObject.SetActive(false); },
                hit => { Destroy(hit.gameObject); }, false, energyFireHitCapacity, energyFireHitMaxSize);
        }
        
        
        if (energyWaterCapacity > 0)
        {
            energyWaterPool = new ObjectPool<Energy>(() => { return Instantiate(energyWater, energyHolderParent); },
                energy => { energy.gameObject.SetActive(true); }, energy => { energy.gameObject.SetActive(false); },
                energy => { Destroy(energy.gameObject); }, false, energyWaterCapacity, energyWaterMaxSize);
        }
        
        if (energyWaterHitCapacity > 0)
        {
            energyWaterHitPool = new ObjectPool<EnergyHit>(() => { return Instantiate(energyWaterHit, energyHolderParent); },
                hit => { hit.gameObject.SetActive(true); }, hit => { hit.gameObject.SetActive(false); },
                hit => { Destroy(hit.gameObject); }, false, energyWaterHitCapacity, energyWaterHitMaxSize);
        }
        
        
        if (energyNatureCapacity > 0)
        {
            energyNaturePool = new ObjectPool<Energy>(() => { return Instantiate(energyNature, energyHolderParent); },
                energy => { energy.gameObject.SetActive(true); }, energy => { energy.gameObject.SetActive(false); },
                energy => { Destroy(energy.gameObject); }, false, energyNatureCapacity, energyNatureMaxSize);
        }
        
        if (energyNatureHitCapacity > 0)
        {
            energyNatureHitPool = new ObjectPool<EnergyHit>(() => { return Instantiate(energyNatureHit, energyHolderParent); },
                hit => { hit.gameObject.SetActive(true); }, hit => { hit.gameObject.SetActive(false); },
                hit => { Destroy(hit.gameObject); }, false, energyNatureHitCapacity, energyNatureHitMaxSize);
        }
        
        
        if (energyLightCapacity > 0)
        {
            energyLightPool = new ObjectPool<Energy>(() => { return Instantiate(energyLight, energyHolderParent); },
                energy => { energy.gameObject.SetActive(true); }, energy => { energy.gameObject.SetActive(false); },
                energy => { Destroy(energy.gameObject); }, false, energyLightCapacity, energyLightMaxSize);
        }
        
        if (energyLightHitCapacity > 0)
        {
            energyLightHitPool = new ObjectPool<EnergyHit>(() => { return Instantiate(energyLightHit, energyHolderParent); },
                hit => { hit.gameObject.SetActive(true); }, hit => { hit.gameObject.SetActive(false); },
                hit => { Destroy(hit.gameObject); }, false, energyLightHitCapacity, energyLightHitMaxSize);
        }
        
        
        if (energyDarkCapacity > 0)
        {
            energyDarkPool = new ObjectPool<Energy>(() => { return Instantiate(energyDark, energyHolderParent); },
                energy => { energy.gameObject.SetActive(true); }, energy => { energy.gameObject.SetActive(false); },
                energy => { Destroy(energy.gameObject); }, false, energyDarkCapacity, energyDarkMaxSize);
        }
        
        if (energyDarkHitCapacity > 0)
        {
            energyDarkHitPool = new ObjectPool<EnergyHit>(() => { return Instantiate(energyDarkHit, energyHolderParent); },
                hit => { hit.gameObject.SetActive(true); }, hit => { hit.gameObject.SetActive(false); },
                hit => { Destroy(hit.gameObject); }, false, energyDarkHitCapacity, energyDarkHitMaxSize);
        }
    }

    private void BuildMiscPool()
    {
        expBallObjectPool = new ObjectPool<ExpBall>(() => { return Instantiate(expBallPrefab, expBallHolderParent); },
            expBall => { expBall.gameObject.SetActive(true); }, expBall => { expBall.gameObject.SetActive(false); },
            expBall => { Destroy(expBall.gameObject); }, false, 100, 500);
    }
}