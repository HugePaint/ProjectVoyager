using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimatorListener : MonoBehaviour
{
    public MenuAnimator menuAnimator;
    public Global.Events eventToShow;
    public Global.Events eventToHide;

    // Start is called before the first frame update
    void Awake()
    {
        EventCenter.GetInstance().AddEventListener(eventToHide, menuAnimator.Hide);
        EventCenter.GetInstance().AddEventListener(eventToShow, menuAnimator.Show);
    }

}
