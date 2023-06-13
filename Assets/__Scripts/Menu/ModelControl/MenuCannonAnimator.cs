using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class MenuCannonAnimator : MonoBehaviour
{
    public GameObject cannonParent;
    [Range(0, 1)]
    public float defaultSmoothness;

    private List<MeshRenderer> cannonMeshes;
    private List<Tweener> allCannonHighlight;

    // Start is called before the first frame update
    void Awake()
    {
        cannonMeshes = cannonParent.GetComponentsInChildren<MeshRenderer>().ToList();
        allCannonHighlight = new List<Tweener>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HightlightAllCannons()
    {
        allCannonHighlight = new List<Tweener>();
        for (int i = 0; i < cannonMeshes.Count; i++)
        {
            HighlightForward(i,-1);
        }
    }
    
    public void StopHighlight()
    {
        for (int i = 0; i < allCannonHighlight.Count; i++)
        {
            // TODO: wei shenme sha bu diao??
            allCannonHighlight[i].OnUpdate(null);
            //allCannonHighlight[i].ForceInit();
            allCannonHighlight[i].Kill();
        }
        DOTween.Kill(this);
    }

    void HighlightForward(int index, int loops)
    {
        float smoothness = 0f;
        allCannonHighlight.Add(DOTween.To(() => smoothness, x => smoothness = x, 1f, 1.5f).OnUpdate(() =>
        {
            int i = index;
            cannonMeshes[i].material.SetFloat(Shader.PropertyToID("_Metallic"), smoothness);
        }).SetLoops(loops, LoopType.Yoyo));
    }
    
}
