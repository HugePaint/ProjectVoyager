using UnityEngine;

public class EnableForAndroid : MonoBehaviour
{
    void Awake()
    {
        // Initially disable the GameObject
        gameObject.SetActive(false);

#if UNITY_ANDROID
        // Enable the GameObject only if running on an Android device
        gameObject.SetActive(true);
#endif
    }
}