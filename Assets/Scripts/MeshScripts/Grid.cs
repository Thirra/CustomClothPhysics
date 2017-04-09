using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MeshScripts
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class Grid : MonoBehaviour
    {
        public int xSize;
        public int ySize;

        private Vector3[] vertices;

        private Mesh mesh;

        private void Awake()
        {
            Generate();
        }

        private void Generate()
        {
            GetComponent<MeshFilter>().mesh = mesh = new Mesh();
            mesh.name = "Procedural Grid";

            vertices = new Vector3[(xSize + 1) * (ySize + 1)];
            Vector2[] uv = new Vector2[vertices.Length];
            for (int index = 0, y = 0; y <= ySize; y++)
            {
                for (int x = 0; x <= xSize; x++, index++)
                {
                    vertices[index] = new Vector3(x, y);
                    uv[index] = new Vector2((float)x / xSize, (float)y / ySize);
                }
            }
            mesh.vertices = vertices;
            mesh.uv = uv;

            int[] triangles = new int[xSize * ySize * 6];
            for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)
            {
                for (int x = 0; x < xSize; x++, ti += 6, vi++)
                {
                    triangles[ti] = vi;
                    triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                    triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                    triangles[ti + 5] = vi + xSize + 2;
                }
            }
            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            CreateCollider();
        }

        //Drawing a small black sphere in the scene view for every vertex
        private void OnDrawGizmos()
        {
            if (vertices == null)
            {
                return;
            }

            Gizmos.color = Color.black;
            for (int index = 0; index < vertices.Length; index++)
            {
                Gizmos.DrawSphere(vertices[index], 0.1f);
            }
        }

        private void CreateCollider()
        {
            gameObject.AddComponent<BoxCollider>();
        }
    }
}
