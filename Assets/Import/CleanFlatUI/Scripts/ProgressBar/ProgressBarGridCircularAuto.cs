using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/* set properties in C# example codes
 using UnityEngine;
 using RainbowArt.CleanFlatUI;
public class ProgressBarDemo : MonoBehaviour
{
    public ProgressBarGridCircularAuto mProgressBar; // the ProgressBar object.
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
    public class ProgressBarGridCircularAuto : MonoBehaviour
    {
        [SerializeField]
        int minValue = 0;
        [SerializeField]
        int maxValue = 10;
        int currentValue = 0;
        [SerializeField]
        [Range(0,1)]
        float loadSpeed = 0.2f;
        [SerializeField]
        bool forward = true;
        [SerializeField]
        bool loop = true;
        [SerializeField]
        bool hasText = true;
        public Text text;  
        public RectTransform background;
        public RectTransform foreground;
        public RectTransform bgTemplate;
        public RectTransform fgTemplate;
        List<RectTransform> bgList = new List<RectTransform>(); 
        List<RectTransform> fgList = new List<RectTransform>();   
        float totalTime = 0f;

        public int CurrentValue
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
        
        public int MaxValue
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
            if(currentValue < 0)
            {
                currentValue = 0;
            }
            if (maxValue < 0)
            {
                maxValue = 10;
            }
            if(currentValue > maxValue)
            {
                currentValue = maxValue;
            }
            UpdateGUI();
        }

        void InitValue()
        {
            if(forward)
            {
                currentValue = minValue;
            }
            else
            {
                currentValue = maxValue;
            }
        }

        void Start()
        {
            InitValue();
            OnValueChanged();
            CreateList(bgList, background, bgTemplate);
            CreateList(fgList, foreground, fgTemplate);
            UpdateGUI();
        } 

        void Update()
        {
            if(forward)
            {          
                totalTime += loadSpeed * (Time.deltaTime * 20);
                if(totalTime >= 1)
                {
                    currentValue++;
                    totalTime = 0f;
                    if (currentValue >= maxValue)
                    {
                        currentValue = maxValue;
                    }
                    UpdateGUI();                    
                    if (loop)
                    {
                        if (currentValue >= maxValue)
                        {
                            currentValue = minValue - 1;
                        }
                    }
                }                     
            }
            else
            {
                totalTime += loadSpeed * (Time.deltaTime * 10);
                if(totalTime >= 1)
                {
                    currentValue--;
                    totalTime = 0f;
                    if (currentValue <= minValue)
                    {
                        currentValue = minValue;
                    }
                    UpdateGUI();
                    if (loop)
                    {
                        if (currentValue <= minValue)
                        {
                            currentValue = maxValue + 1;
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

        void CreateList( List<RectTransform> list, RectTransform rectParent, RectTransform template)
        {
            template.gameObject.SetActive(false);
            float angle = 360f / (float)maxValue;
            for (int i = 0; i < maxValue; i++)
            {
                RectTransform item = CreateItem(rectParent, template, i);
                list.Add(item);
                item.localEulerAngles = new Vector3(0, 0, - angle * i);
            }           
        }

        RectTransform CreateItem(RectTransform rectParent, RectTransform template, int index)
        {
            GameObject obj = GameObject.Instantiate(template.gameObject, rectParent);
            obj.gameObject.SetActive(true);
            obj.gameObject.name = "item"+ (index + 1);
            RectTransform rectTrans = obj.GetComponent<RectTransform>();
            rectTrans.localScale = Vector3.one;
            rectTrans.localEulerAngles = Vector3.zero;
            rectTrans.anchoredPosition3D = Vector3.zero;
            return rectTrans;
        }

        void UpdateForeground()
        {
            for(int i = 0; i < fgList.Count; i++)
            {
                RectTransform rectTrans = fgList[i];
                if(i < currentValue)
                {                    
                    rectTrans.gameObject.SetActive(true);
                }  
                else
                {
                    rectTrans.gameObject.SetActive(false);
                }              
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
                float val = (float)currentValue / (float)maxValue;
                text.text = Mathf.FloorToInt(val * 100) + "%";
            }
        }     
    }
}