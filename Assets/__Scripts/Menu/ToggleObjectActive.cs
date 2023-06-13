using UnityEngine;

public class ToggleObjectActive : MonoBehaviour
{
    public GameObject objectToToggle;
    public KeyCode firstKey;
    public KeyCode secondKey;

    void Update()
    {
        if (Input.GetKey(firstKey))
        {
            if (Input.GetKeyDown(secondKey))
            {
                objectToToggle.SetActive(!objectToToggle.activeSelf);
            }
        }
    }
}