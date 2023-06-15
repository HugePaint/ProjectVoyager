using UnityEngine;
using System.Collections;

namespace KETO
{
    public class CharacterTrail : MonoBehaviour
    {
        [SerializeField] private float duration = 3f;
        [SerializeField] private float interval = 0.05f;
        [SerializeField] private float destroyDelay = 3f;
        [SerializeField] private Transform spawnTransform;
        [SerializeField] private Material trailMaterial;
        [SerializeField] private GameObject particles;


        private SkinnedMeshRenderer[] skinnedMeshRenderers;
        private GameObject trailParent;

        private bool isSpawn = false;

        private void Start()
        {
            if (particles != null)
                particles.SetActive(false);
            trailParent = new GameObject("Trail");
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !isSpawn)
            {
                isSpawn = true;
                if (particles != null)
                    particles.SetActive(isSpawn);
                StartCoroutine(SpawnTrail(duration));
            }
        }

        private IEnumerator SpawnTrail(float elapsedTime)
        {
            skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

            while (elapsedTime > 0)
            {
                elapsedTime -= interval;

                for (int i = 0; i < skinnedMeshRenderers.Length; i++)
                {
                    Mesh mesh = new Mesh();
                    skinnedMeshRenderers[i].BakeMesh(mesh);

                    GameObject trail = new GameObject();
                    trail.transform.parent = trailParent.transform;
                    trail.transform.position = spawnTransform.position;
                    trail.transform.rotation = spawnTransform.rotation;

                    if (trail.GetComponent<MeshRenderer>() == null)
                    {
                        trail.AddComponent<MeshRenderer>();
                    }

                    if (trail.GetComponent<MeshFilter>() == null)
                    {
                        trail.AddComponent<MeshFilter>();
                    }

                    MeshFilter meshFilter = trail.GetComponent<MeshFilter>();
                    MeshRenderer meshRenderer = trail.GetComponent<MeshRenderer>();

                    meshFilter.mesh = mesh;
                    meshRenderer.material = trailMaterial;

                    StartCoroutine(FadeOut(meshRenderer));
                    Destroy(trail, destroyDelay);
                }

                yield return new WaitForSeconds(interval);
            }
            isSpawn = false;
            if (particles != null)
                particles.SetActive(isSpawn);
        }

        private IEnumerator FadeOut(MeshRenderer meshRenderer)
        {
            Material mat = meshRenderer.material;

            float valueToAnimate = 1f;

            while (valueToAnimate > 0)
            {
                valueToAnimate -= interval;
                mat.SetFloat("_Alpha", valueToAnimate);
                mat.SetFloat("_LerpAmount", 1 - valueToAnimate);
                yield return new WaitForSeconds(interval);
            }
        }
    }
}