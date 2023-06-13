using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/* set properties in C# example codes
 using UnityEngine;
 using RainbowArt.CleanFlatUI;
 public class ProgressBarDemo : MonoBehaviour
{  
    public ProgressBarCircularMove mProgressBar; // the ProgressBar object.
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
    public class ProgressBarCircularMove : MonoBehaviour
    {
        [SerializeField]
        public float currentValue = 0f;
        [SerializeField]
        public float maxValue = 100.0f;
        [SerializeField]
        public bool hasText = true;
        public Text text;  
        
        public RectTransform foregroundArea;
        public enum  Origin
        {
            Bottom,
            Top,
            Left,
            Right,           
        };

        public enum PatternOriginVertical
        {
            Bottom,
            Top,
        };
        public enum PatternOriginHorizontal
        {
            Left,
            Right,
        };

        public RawImage patternImage;   
        public RectTransform patternRect;

        [SerializeField]
        Origin origin;
        [SerializeField]
        bool patternPlay = true;
        [SerializeField]
        float patternSpeed = 1.5f;
        [SerializeField]
        int patternOrigin;

        public Origin CurOrigin
        {
            get => origin;
            set
            {
                origin = value;
            }
        }

        public bool PatternPlay
        {
            get => patternPlay;
            set
            {
                patternPlay = value;
            }
        }

        public float PatternSpeed
        {
            get => patternSpeed;
            set
            {
                patternSpeed = value;
            }
        }

        public int PatternOrigin
        {
            get => patternOrigin;
            set
            {
                patternOrigin = value;
            }
        }


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
        }
        void Start(){
            UpdateGUI();
        }
        
        void Update()
        {
            if (Application.isPlaying)
            {
                UpdateGUI();
            }
        }         
             
        void UpdateGUI()
        {            
            UpdateForeground();  
            UpdatePattern();
            UpdateText();
        }
        
        void UpdatePattern()
        {   
            if(patternPlay && Application.isPlaying)
            {
                switch (origin)
                {
                    case Origin.Bottom:
                    case Origin.Top:
                    {
                        Rect r = patternImage.uvRect;
                        if(patternOrigin == (int)PatternOriginHorizontal.Left)
                        {
                            r.x -= Time.deltaTime * patternSpeed;
                        }
                        else
                        {
                            r.x += Time.deltaTime * patternSpeed;
                        }
                        patternImage.uvRect = r;
                        break;
                    }
                    case Origin.Left:
                    case Origin.Right:
                    {
                        Rect r = patternImage.uvRect;
                        if(patternOrigin == (int)PatternOriginVertical.Bottom)
                        {
                            r.y -= Time.deltaTime * patternSpeed;
                        }
                        else
                        {
                            r.y += Time.deltaTime * patternSpeed;
                        }
                        patternImage.uvRect = r;
                        break;
                    }
                }                
            }
        }

        void  UpdateForeground()
        {
            if(currentValue == 0)
            {
                foregroundArea.gameObject.SetActive(false);
            }
            else
            {
                foregroundArea.gameObject.SetActive(true);
                ResetForegroundOrigon();
                switch (origin)
                {
                    case Origin.Bottom:
                    {
                        UpdateForegroundFromBottom();
                        break;
                    }
                    case Origin.Top:
                    {
                        UpdateForegroundFromTop();
                        break;
                    }
                    case Origin.Left:
                    {

                        UpdateForegroundFromLeft();
                        break;
                    }
                    case Origin.Right:
                    {
                        UpdateForegroundFromRight();
                        break;
                    }
                }
            }            
        }

        void ResetForegroundOrigon()
        {
            patternRect.offsetMax = Vector2.zero;
            patternRect.offsetMin = Vector2.zero;
        }

        void UpdateForegroundFromBottom()
        {
            float maxHeight = foregroundArea.rect.height;
            Vector2 offsetMax = patternRect.offsetMax;
            offsetMax.y = -(maxHeight - maxHeight*(currentValue / maxValue));
            Vector2 offsetMin = patternRect.offsetMin;
            offsetMin.y = -(maxHeight - maxHeight*(currentValue / maxValue));
            patternRect.offsetMax = offsetMax;
            patternRect.offsetMin = offsetMin;
        }

         void UpdateForegroundFromTop()
        {
            float maxHeight = foregroundArea.rect.height;
            Vector2 offsetMax = patternRect.offsetMax;
            offsetMax.y = maxHeight - maxHeight*(currentValue / maxValue);
            Vector2 offsetMin = patternRect.offsetMin;
            offsetMin.y = maxHeight - maxHeight*(currentValue / maxValue);
            patternRect.offsetMax = offsetMax;
            patternRect.offsetMin = offsetMin;
        }

        void UpdateForegroundFromLeft()
        {
            float maxWidth = foregroundArea.rect.width;
            Vector2 offsetMax = patternRect.offsetMax;
            offsetMax.x = -(maxWidth - maxWidth*(currentValue / maxValue));
            Vector2 offsetMin = patternRect.offsetMin;
            offsetMin.x = -(maxWidth - maxWidth*(currentValue / maxValue));
            patternRect.offsetMax = offsetMax;
            patternRect.offsetMin = offsetMin;
        }

        void UpdateForegroundFromRight()
        {
            float maxWidth = foregroundArea.rect.width;
            Vector2 offsetMax = patternRect.offsetMax;
            offsetMax.x = maxWidth - maxWidth*(currentValue / maxValue);
            Vector2 offsetMin = patternRect.offsetMin;
            offsetMin.x = maxWidth - maxWidth*(currentValue / maxValue);
            patternRect.offsetMax = offsetMax;
            patternRect.offsetMin = offsetMin;
        }

        void UpdateText()
        {
            if (text != null && text.gameObject.activeSelf != hasText)
            {
                text.gameObject.SetActive(hasText);
            }
            if (hasText && (text != null))
            {
                text.text = (int)((currentValue/maxValue)*100) + "%";
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

