using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RainbowArt.CleanFlatUI
{
    [ExecuteAlways]
    public class SliderTransition: MonoBehaviour,IPointerDownHandler
    {
        public Slider slider;
        public Animator animator;
        [SerializeField]
        bool hasText = true;
        public Text text;


        public bool HasText
        {
            get => hasText;
            set
            {
                hasText = value;
                UpdateText();
            }
        }

        void UpdateText()
        {
            if (text != null && text.gameObject.activeSelf != hasText)
            {
                text.gameObject.SetActive(hasText);
            }
            if (hasText && (text != null))
            {
                float useValue = (float)Math.Round((double)slider.value, 1);
                text.text = useValue + "";
            }
        }


        void Start ()
        {
            if(slider == null)
            {
                slider = GetComponent<Slider>();
            }            
            slider.onValueChanged.AddListener(SliderValueChange);
            SliderValueChange(slider.value);
        }

        public void SliderValueChange(float value)
        {
            if (hasText && (text != null))
            {
                float useValue = (float)Math.Round((double)slider.value, 1);
                text.text = useValue +"";
            }       
        } 

        public void OnPointerDown(PointerEventData eventData)
        {
            if(animator!=null)
            {
                animator.Play("Transition",0,0);  
            }                      
        }

        #if UNITY_EDITOR
        protected void OnValidate()
        {
            if(text != null)
            {
                text.gameObject.SetActive(hasText);
            }
            slider = GetComponent<Slider>();
            SliderValueChange(slider.value);     
        }
        #endif
    }
}