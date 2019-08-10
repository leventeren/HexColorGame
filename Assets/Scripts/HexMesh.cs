using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace vertigoGames.hexColorGame
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class HexMesh : MonoBehaviour
    {
        private Mesh m_hexMesh;
        private List<Vector3> m_vertices;
        private List<int> m_triangles;
        private MeshCollider m_meshCollider;
        private List<Color> m_colors;

        private void Awake()
        {
            GetComponent<MeshFilter>().mesh = m_hexMesh = new Mesh();
            m_meshCollider = this.gameObject.AddComponent<MeshCollider>();
            m_hexMesh.name = "HexColorGameMesh";
            m_vertices = new List<Vector3>();
            m_triangles = new List<int>();
            m_colors = new List<Color>();
        }

        public void Triangulate(HexCell[] cells)
        {
            m_hexMesh.Clear();
            m_vertices.Clear();
            m_triangles.Clear();
            m_colors.Clear();

            for (int i = 0; i < cells.Length; i++)
            {
                Triangulate(cells[i]);
            }

            m_hexMesh.vertices = m_vertices.ToArray();
            m_hexMesh.triangles = m_triangles.ToArray();
            m_hexMesh.colors = m_colors.ToArray();
            m_hexMesh.RecalculateNormals();
            m_meshCollider.sharedMesh = m_hexMesh;
        }
        
        void Triangulate(HexCell cell)
        {
            Vector3 center = cell.transform.localPosition;
            for (int i = 0; i < 6; i++)
            {
                AddTriangle(
                    center,
                    center + HexMetrics.corners[i],
                    center + HexMetrics.corners[i + 1]
                );
                AddTriangleColor(GameManager.Instance.meshColor);
            }
        }        

        private void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            int vertexIndex = m_vertices.Count;
            m_vertices.Add(v1);
            m_vertices.Add(v2);
            m_vertices.Add(v3);
            m_triangles.Add(vertexIndex);
            m_triangles.Add(vertexIndex + 1);
            m_triangles.Add(vertexIndex + 2);
        }

        private void AddTriangleColor(Color color)
        {
            m_colors.Add(color);
            m_colors.Add(color);
            m_colors.Add(color);
        }

        void AddTriangleColor(Color c1, Color c2, Color c3)
        {
            m_colors.Add(c1);
            m_colors.Add(c2);
            m_colors.Add(c3);
        }

        void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
        {
            int vertexIndex = m_vertices.Count;
            m_vertices.Add(v1);
            m_vertices.Add(v2);
            m_vertices.Add(v3);
            m_vertices.Add(v4);
            m_triangles.Add(vertexIndex);
            m_triangles.Add(vertexIndex + 2);
            m_triangles.Add(vertexIndex + 1);
            m_triangles.Add(vertexIndex + 1);
            m_triangles.Add(vertexIndex + 2);
            m_triangles.Add(vertexIndex + 3);
        }

        void AddQuadColor(Color c1, Color c2)
        {
            m_colors.Add(c1);
            m_colors.Add(c1);
            m_colors.Add(c2);
            m_colors.Add(c2);
        }

        void AddQuadColor(Color c1, Color c2, Color c3, Color c4)
        {
            m_colors.Add(c1);
            m_colors.Add(c2);
            m_colors.Add(c3);
            m_colors.Add(c4);
        }        
    }
}