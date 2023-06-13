using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace RainbowArt.CleanFlatUI
{
    public class Selector : MonoBehaviour 
    {
        public Button buttonPrevious;
        public Button buttonNext;
        public Image imageNew;
        public Image imageCurrent;
        public Text textNew;
        public Text textCurrent;
        [SerializeField]
        public bool loop = false;
        [SerializeField]
        public bool hasIndicator = false;
        public Text indicator;
        public RectTransform indicatorRect;
        public Animator animator;
        bool changed = true;
        [SerializeField]
        public int startIndex = 0;
        [Serializable]
        public class OptionItem
        {
            public string optionText = "option"; 
            public Sprite optionImage;                       
        }
        public OptionItem[] options;
        int newIndex = 0;
        int currentIndex = 0;                
        public int CurrentIndex
        {
            get => currentIndex;
            set
            {
                currentIndex = value;
            }
        }

        public int StartIndex
        {
            get => startIndex;
            set
            {
                startIndex = value;
            }
        }

        public bool HasIndicator
        {
            get => hasIndicator;
            set
            {
                hasIndicator = value;
                if (indicator != null && indicator.gameObject.activeSelf != hasIndicator)
                {
                    indicator.gameObject.SetActive(hasIndicator);
                }
            }
        }


        void Start()
        {
            if(buttonPrevious != null)
            {
                buttonPrevious.onClick.AddListener(OnButtonClickPrevious); 
            } 
            if(buttonNext != null)
            {
                buttonNext.onClick.AddListener(OnButtonClickNext); 
            }    
            InitOptions();             
        }
        public void OnButtonClickPrevious()
        {
            UpdateOptions(false);
            animator.enabled = false; 
            if(changed)
            {
                animator.enabled = true;
                animator.Play("Previous",0,0); 
            }                    
        }
        
        public void OnButtonClickNext()
        {
            UpdateOptions(true);
            animator.enabled = false;           
            if(changed)
            {
                animator.enabled = true;
                animator.Play("Next",0,0);    
            }                 
        }

        void InitOptions()
        {
            currentIndex = startIndex;
            newIndex = startIndex;
            textCurrent.text = options[currentIndex].optionText;
            if(imageCurrent != null)
            {
                imageCurrent.sprite = options[currentIndex].optionImage;
            }                
            textNew.text = options[newIndex].optionText; 
            if(imageNew != null)
            {
                imageNew.sprite = options[newIndex].optionImage;
            }    
            if(hasIndicator &&(indicator != null))
            {
                indicator.text = (currentIndex+1) +" / "+ options.Length;
            }      

        }

        void UpdateOptions(bool bNext)
        {
            changed = true;
            if( bNext )
            {
                if(newIndex == options.Length -1)
                {
                    if(loop)
                    {
                        currentIndex = newIndex;
                        newIndex = 0;
                    }
                    else
                    {
                        changed = false;
                    }                    
                }
                else
                {
                    if(currentIndex != newIndex)
                    {
                        currentIndex = newIndex;
                    }                    
                    newIndex = currentIndex + 1;                    
                }             
            }
            else
            {
                if(newIndex == 0)
                {
                    if(loop)
                    {
                        currentIndex = newIndex;
                        newIndex = options.Length -1;
                    }
                    else
                    {
                        changed = false;
                    }                    
                }
                else
                {
                    if(currentIndex != newIndex)
                    {
                        currentIndex = newIndex;
                    }                    
                    newIndex = currentIndex - 1;                    
                }                 
            } 
            if(changed)
            {                
                textCurrent.text = options[currentIndex].optionText;
                if(imageCurrent != null)
                {
                    imageCurrent.sprite = options[currentIndex].optionImage;
                }                
                textNew.text = options[newIndex].optionText; 
                if(imageNew != null)
                {
                    imageNew.sprite = options[newIndex].optionImage;
                }    
                if(hasIndicator &&(indicator != null))
                {
                    indicator.text = (newIndex+1) +" / "+ options.Length;
                }            
            }          
        }
        #if UNITY_EDITOR
        protected void OnValidate()
        {
            if(indicatorRect != null)
            {
                indicatorRect.gameObject.SetActive(hasIndicator);
            } 
        }
        #endif
    }
}