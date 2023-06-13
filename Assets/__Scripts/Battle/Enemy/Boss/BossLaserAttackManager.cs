using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaserAttackManager : MonoBehaviour
{
    private BossLaser laserInstance;
    public BossLaser bossLaserPrefab;
    public GameObject firePoint;

    private bool shooting;
    public Transform lookAtTransform;

    public void Shoot(Transform hitPoint)
    {
        lookAtTransform = hitPoint;
        firePoint.transform.LookAt(lookAtTransform);
        if(laserInstance) Destroy(laserInstance.gameObject);
        laserInstance = Instantiate(bossLaserPrefab, firePoint.transform.position, firePoint.transform.rotation);
        laserInstance.transform.parent = firePoint.transform;
        shooting = true;
    }

    private void Update()
    {
        if (!shooting) return;
        firePoint.transform.LookAt(lookAtTransform);
    }

    public void LaserEnd()
    {
        if (laserInstance) laserInstance.DisablePrepare();
        shooting = false;
        Destroy(laserInstance.gameObject,1);
    }
}
