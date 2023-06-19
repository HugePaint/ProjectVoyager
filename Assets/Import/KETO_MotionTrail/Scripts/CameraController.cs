using UnityEngine;

namespace KETO
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float distance = 7f;
        [SerializeField] private float height = 1f;
        [SerializeField] private float angle = 15f;
        private void LateUpdate()
        {
            Quaternion localRotation = Quaternion.Euler(angle, 0f, 0f);
            transform.rotation = localRotation;

            Vector3 negDistance = new Vector3(0f, height, -distance);
            Vector3 position = localRotation * negDistance + target.position;

            transform.position = position;
        }
    }
}