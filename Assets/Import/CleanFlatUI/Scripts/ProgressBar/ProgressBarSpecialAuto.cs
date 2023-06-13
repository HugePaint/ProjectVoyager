
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/* set properties in C# example codes
 using UnityEngine;
 using RainbowArt.CleanFlatUI;
public class ProgressBarDemo : MonoBehaviour
{
    public ProgressBarSpecialAuto mProgressBar; // the ProgressBar object.
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
    public class ProgressBarSpecialAuto : MonoBehaviour
    {
        [SerializeField]
        float minValue = 0f;
        [SerializeField]
        float maxValue = 100.0f;
        float currentValue = 0f;
        [SerializeField]
        [Range(0, 1)]
        float loadSpeed = 0.1f;
        [SerializeField]
        bool forward = true;
        [SerializeField]
        bool loop = true;
        [SerializeField]
        bool hasText = true;
        public Text text;  
        public RectTransform foreground;
        public RectTransform foregroundArea;

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
        }

        void InitValue(){
            if(forward){
                currentValue = minValue;
            }else{
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
                    if(loop){
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
                    if(loop){
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
            float maxWidth = foregroundArea.rect.width;
            Vector2 offsetMax = foreground.offsetMax;
            offsetMax.x = -(maxWidth - maxWidth*(currentValue / maxValue));
            foreground.offsetMax = offsetMax; 
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
