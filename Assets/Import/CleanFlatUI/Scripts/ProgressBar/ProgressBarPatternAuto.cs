using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



/* set properties in C# example codes
 using UnityEngine;
 using RainbowArt.CleanFlatUI;
public class ProgressBarDemo : MonoBehaviour
{
    public ProgressBarPatternAuto mProgressBar; // the ProgressBar object.
    void Start()
    {
        mProgressBar.HasText = false; //set whether show the text value
        mProgressBar.MinValue = 25; // set the minimum value of the Progress Bar
        mProgressBar.MaxValue = 100; // set the maximum value of the Progress Bar
        mProgressBar.Loop = true; // set whether auto loop 
        mProgressBar.Forward = true; // set the current progress value auto changing direction
        mProgressBar.LoadSpeed = 0.2f; //set how fast the current progress value auto changing
    }
}
*/


namespace RainbowArt.CleanFlatUI
{
    public class ProgressBarPatternAuto : MonoBehaviour
    {
        [SerializeField]
        float minValue = 0f;
        [SerializeField]
        float maxValue = 100.0f;
        float currentValue = 0f;
        [SerializeField]
        [Range(0,1)]
        float loadSpeed = 0.1f;
        [SerializeField]
        bool forward = true;
        [SerializeField]
        bool loop = true;
        [SerializeField]
        bool hasText = true;
        public Text text;  
        public Image foreground;

        public RawImage patternImage;
        public RectTransform patternRect;
        [SerializeField]
        bool patternPlay = true;
        [SerializeField]
        float patternSpeed = 0.5f;
        [SerializeField]
        bool patternForward = true;
        [SerializeField]
        float patternScale = 5;


        public bool PatternPlay
        {
            get => patternPlay;
            set
            {
                patternPlay = value;
            }
        }

        public bool PatternForward
        {
            get => patternForward;
            set
            {
                patternForward = value;
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

        public float PatternScale
        {
            get => patternScale;
            set
            {
                patternScale = value;
            }
        }

        public float MinValue
        {
            get => minValue;
            set
            {
                if(minValue == value)
                {
                    return;
                }
                minValue = value;
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

        public float LoadSpeed
        {
            get => loadSpeed;
            set { loadSpeed = value; }
        }

        public bool Loop
        {
            get => loop;
            set
            {
                loop = value;
            }
        }

        public bool Forward
        {
            get => forward;
            set
            {
                forward = value;
            }
        }


        void OnValueChanged()
        {
            if(maxValue < 0)
            {
                maxValue = 100.0f;
            }
            if(minValue < 0)
            {
                minValue = 0f;
            }

            currentValue = Mathf.Clamp(minValue, 0, maxValue);
            Rect r = patternImage.uvRect;
            r.width = (currentValue / maxValue)*patternScale;
            patternImage.uvRect = r;
        } 

        void InitValue(){
            if(forward){
                currentValue = minValue;
            }
            else
            {
                currentValue = maxValue;
            }
        }

        void Start(){
            InitValue();
            UpdateGUI();
        }        

        void Update(){
            if (Application.isPlaying)
            {
                if(forward){
                    if (currentValue < maxValue)
                    {
                        currentValue += loadSpeed * (Time.deltaTime * 100);
                        if (currentValue >= maxValue)
                        {
                            currentValue = maxValue;
                        }
                        UpdateGUI();                        
                    }
                    if(loop)
                    {
                        if (currentValue >= maxValue)
                        {
                            currentValue = minValue;
                        }
                    }
                }else{
                    if (currentValue > minValue)
                    {
                        currentValue -= loadSpeed * (Time.deltaTime * 100);
                        if (currentValue <= minValue)
                        {
                            currentValue = minValue;
                        }
                        UpdateGUI();
                    }
                    if(loop)
                    {
                        if (currentValue <= minValue)
                        {
                            currentValue = maxValue;
                        }
                    }
                }                
            }        
        }

        void UpdateGUI()
        {
            UpdateForeground();                                  
            UpdateText();
        }

        void UpdateForeground()
        {
            foreground.fillAmount = currentValue / maxValue;
            float maxWidth = foreground.GetComponent<RectTransform>().rect.width;
            Vector2 offsetMax = patternRect.offsetMax;
            offsetMax.x = -(maxWidth - maxWidth*(currentValue / maxValue));
            patternRect.offsetMax = offsetMax;            
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