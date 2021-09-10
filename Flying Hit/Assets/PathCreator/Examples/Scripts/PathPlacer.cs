using PathCreation;
using UnityEngine;
using System.Collections.Generic;

namespace PathCreation.Examples {

    [ExecuteInEditMode]
    public class PathPlacer : PathSceneTool {

        public GameObject[] prefab;
        public GameObject holder;
        public float spacing = 3;

        const float minSpacing = .1f;

        public float offsetValue;

        [SerializeField]
        bool deleteNow;

        public List<Material> materials;

        void Generate () 
        {
            if (deleteNow)
            {
                DestroyObjects();
            }

            if (pathCreator != null && prefab != null && holder != null) {
                DestroyObjects ();

                VertexPath path = pathCreator.path;

                spacing = Mathf.Max(minSpacing, spacing);
                float dst = 0;

                while (dst < path.length) {
                    Vector3 point = path.GetPointAtDistance (dst);
                    Quaternion rot = path.GetRotationAtDistance (dst);

                    float randomX = Random.Range(-offsetValue, offsetValue);

                    if (randomX == 0f)
                        randomX = -offsetValue;
                    float randomY = Random.Range(-offsetValue, offsetValue);

                    if (randomY == 0f)
                        randomY = -offsetValue;

                    Vector3 positionOffset = new Vector3(randomX, randomY, 0f);

                    int i = Random.Range(0, prefab.Length);
                    int randomMaterial = Random.Range(0, materials.Count);

                    GameObject temp = Instantiate (prefab[i], point + positionOffset, rot,holder.transform);
                    temp.GetComponent<Renderer>().material = materials[randomMaterial];
                    temp.SetActive(true);

                    dst += spacing;
                }
            }
        }

        void DestroyObjects () {
            int numChildren = holder.transform.childCount;
            for (int i = numChildren - 1; i >= 0; i--) {
                DestroyImmediate (holder.transform.GetChild (i).gameObject, false);
            }
        }

        protected override void PathUpdated () {
            if (pathCreator != null) {
                Generate ();
            }
        }
    }
}