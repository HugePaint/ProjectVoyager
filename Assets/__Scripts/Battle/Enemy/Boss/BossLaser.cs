using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaser : MonoBehaviour
{
    public GameObject HitEffect;
    public float HitOffset = 0;
    public bool useLaserRotation = false;

    public float MaxLength;
    private LineRenderer Laser;

    public float MainTextureLength = 1f;
    public float NoiseTextureLength = 1f;
    private Vector4 Length = new Vector4(1,1,1,1);
    private bool LaserSaver = false;
    private bool UpdateSaver = false;

    private ParticleSystem[] Effects;
    private ParticleSystem[] Hit;

    void Start ()
    {
        Laser = GetComponent<LineRenderer>();
        Laser.widthMultiplier = 0.8f;
        Effects = GetComponentsInChildren<ParticleSystem>();
        Hit = HitEffect.GetComponentsInChildren<ParticleSystem>();
    }

    void Update()
    {
        Laser.material.SetTextureScale("_MainTex", new Vector2(Length[0], Length[1]));                    
        Laser.material.SetTextureScale("_Noise", new Vector2(Length[2], Length[3]));
        if (Laser != null && UpdateSaver == false)
        {
            Laser.SetPosition(0, transform.position);
            RaycastHit hit;
            var mask = LayerMask.GetMask("Ground");
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, MaxLength, mask))
            {
                //End laser position if collides with object
                Laser.SetPosition(1, hit.point);

                    HitEffect.transform.position = hit.point + hit.normal * HitOffset;
                if (useLaserRotation)
                    HitEffect.transform.rotation = transform.rotation;
                else
                    HitEffect.transform.LookAt(hit.point + hit.normal);

                foreach (var AllPs in Effects)
                {
                    if (!AllPs.isPlaying) AllPs.Play();
                }
                //Texture tiling
                Length[0] = MainTextureLength * (Vector3.Distance(transform.position, hit.point));
                Length[2] = NoiseTextureLength * (Vector3.Distance(transform.position, hit.point));
            }
            else
            {
                //End laser position if doesn't collide with object
                var EndPos = transform.position + transform.forward * MaxLength;
                Laser.SetPosition(1, EndPos);
                HitEffect.transform.position = EndPos;
                foreach (var AllPs in Hit)
                {
                    if (AllPs.isPlaying) AllPs.Stop();
                }
                //Texture tiling
                Length[0] = MainTextureLength * (Vector3.Distance(transform.position, EndPos));
                Length[2] = NoiseTextureLength * (Vector3.Distance(transform.position, EndPos));
            }
            //Insurance against the appearance of a laser in the center of coordinates!
            if (Laser.enabled == false && LaserSaver == false)
            {
                LaserSaver = true;
                Laser.enabled = true;
            }
        }  
    }

    public void DisablePrepare()
    {
        if (Laser != null)
        {
            Laser.enabled = false;
        }
        UpdateSaver = true;
        //Effects can = null in multiply shooting
        if (Effects != null)
        {
            foreach (var AllPs in Effects)
            {
                if (AllPs.isPlaying) AllPs.Stop();
            }
        }
    }
}
