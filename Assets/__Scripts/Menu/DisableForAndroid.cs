using UnityEngine;

public class DisableForAndroid : MonoBehaviour
{
    void Awake()
    {
        // Initially disable the GameObject
        // gameObject.SetActive(true);

#if UNITY_ANDROID
        // Enable the GameObject only if running on an Android device
        gameObject.SetActive(false);
#endif
    }
}