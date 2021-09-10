using PathCreation;
using UnityEngine;
using System.Collections.Generic;

namespace PathCreation.Examples
{

    [ExecuteInEditMode]
    public class FloorPlacer : PathSceneTool
    {

        public GameObject prefab;
        public GameObject holder;
        public float spacing = 3;

        const float minSpacing = .1f;

        public float YValue;

        [SerializeField]
        bool deleteNow;

        void Generate()
        {
            if (deleteNow)
            {
                DestroyObjects();
            }

            if (pathCreator != null && prefab != null && holder != null)
            {
                DestroyObjects();

                VertexPath path = pathCreator.path;

                spacing = Mathf.Max(minSpacing, spacing);
                float dst = 0;

                while (dst < path.length)
                {
                    Vector3 point = path.GetPointAtDistance(dst);
                    Quaternion rot = path.GetRotationAtDistance(dst);

                    Instantiate(prefab, new Vector3(point.x,YValue,point.z), Quaternion.identity,holder.transform);

                    dst += spacing;
                }
            }
        }

        void DestroyObjects()
        {
            int numChildren = holder.transform.childCount;
            for (int i = numChildren - 1; i >= 0; i--)
            {
                DestroyImmediate(holder.transform.GetChild(i).gameObject, false);
            }
        }

        protected override void PathUpdated()
        {
            if (pathCreator != null)
            {
                Generate();
            }
        }
    }
}
