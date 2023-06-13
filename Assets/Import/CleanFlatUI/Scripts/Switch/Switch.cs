using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
namespace RainbowArt.CleanFlatUI
{
    public class Switch : MonoBehaviour,IPointerDownHandler 
    {
        [SerializeField]
        bool isOn = false;        
        public bool IsOn
        {
            get => isOn;
            set
            {
                if(isOn == value)
                {
                    return;
                }
                isOn = value;
                UpdateGUI(false);
            }
        }   
        public Animator animator;  
        public UnityEvent switchOn;
        public UnityEvent switchOff;         

        void Start () 
        {
           UpdateGUI(true);
        }   

        public void OnPointerDown(PointerEventData eventData)
        {
            isOn = !isOn;  
            UpdateGUI(false);      
        }  

        void UpdateGUI(bool isInit)
        {
            if(isInit)
            {
                if(isOn)
                {
                    animator.Play("On Init",0,0); 
                }
                else
                {
                    animator.Play("Off Init",0,0); 
                } 
            }
            else
            {
                if(isOn)
                {
                    animator.Play("On",0,0); 
                    switchOn.Invoke();
                }
                else
                {
                    animator.Play("Off",0,0); 
                    switchOff.Invoke();
                } 
            }            
        }  
    }
}