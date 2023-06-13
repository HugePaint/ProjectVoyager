using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* set properties in C# example codes
 using UnityEngine;
 using RainbowArt.CleanFlatUI;
 public class ProgressBarDemo : MonoBehaviour
{  
    public ProgressBarSpecialPattern mProgressBar; // the ProgressBar object.
    void Start()
    {
        mProgressBar.HasText = false; //set whether show the text value
        mProgressBar.MaxValue = 100; // set the maximum value of the Progress Bar
        mProgressBar.CurrentValue = 50; //set the current value of the Progress Bar
    }        
}
 */




namespace RainbowArt.CleanFlatUI
{
    public class ProgressBarSpecialPattern : MonoBehaviour
    {
        [SerializeField]
        float currentValue = 0f;
        [SerializeField]
        float maxValue = 100.0f;
        [SerializeField]
        bool hasText = true;
        public Text text;  
        public RectTransform foreground;
        public RectTransform foregroundArea;

        public RawImage patternImage;
        public RectTransform patternRect;        
        public bool patternPlay = true;
        public float patternSpeed = 0.5f;
        public bool patternForward = true;  
        public float patternScale = 5;     

        public float CurrentValue
        {
            get => currentValue;
            set
            {
                if(currentValue == value)
                {
                    return;
                }
                currentValue = value;
                OnValueChanged();
            }
        }
        public float MaxValue
        {
            get => maxValue;
            set
            {
                if(maxValue == value)
                {
                    return;
                }
                maxValue = value;
                OnValueChanged();
            }
        }

        public bool HasText
        {
            get => hasText;
            set
            {
                if (hasText == value)
                {
                    return;
                }
                hasText = value;
                UpdateText();
            }
        }

        void OnValueChanged()
        {
            if(maxValue < 0)
            {
                maxValue = 100.0f;
            }
            if(currentValue < 0)
            {
                currentValue = 0f;
            }
            currentValue = Mathf.Clamp(currentValue, 0, maxValue);
            Rect r = patternImage.uvRect;
            r.width = (currentValue / maxValue)*patternScale;
            patternImage.uvRect = r;
        }

        void Start()
        {
            UpdateGUI();
        }

        void Update()
        {
            UpdateGUI();
        }
       
        void UpdateGUI()
        {            
            UpdateForegroundAndPattern();  
            UpdateText();
        }

        void UpdateForegroundAndPattern()
        {
            float maxWidth = foregroundArea.rect.width;
            Vector2 offsetMax1 = foreground.offsetMax;
            offsetMax1.x = -(maxWidth - maxWidth*(currentValue / maxValue));
            foreground.offsetMax = offsetMax1;     
            if(patternPlay)
            {
                Rect r = patternImage.uvRect;
                if(patternForward)
                {
                    r.x -= Time.deltaTime * patternSpeed;
                }
                else
                {
                    r.x += Time.deltaTime * patternSpeed;
                }
                r.width = (currentValue / maxValue)*patternScale;
                patternImage.uvRect = r;
            }
            else
            {
                Rect r = patternImage.uvRect;
                r.width = (currentValue / maxValue)*patternScale;
                patternImage.uvRect = r;
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
                text.text = Mathf.Floor(currentValue) +"/"+ Mathf.Floor(maxValue);
            }
        }

        #if UNITY_EDITOR
        protected void OnValidate()
        {
            OnValueChanged();
            UpdateGUI();        
        }
        #endif
    }
}