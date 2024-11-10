using UnityEngine;
using System.Collections.Generic;

public class TubeMapGenerator : MonoBehaviour
{
    public float pathLength = 10f; // Total length of the path
    public int pathPoints = 100; // Number of points on the path
    public float radius = 1.0f; // Radius of the tube
    public int segments = 16; // Number of segments (sides) per path point
    private Mesh mesh;

    private List<Vector3> path;

    void Start()
    {
        // Generate path using Perlin Noise
        path = GeneratePath();

        if (path.Count < 2)
        {
            Debug.LogError("Path does not contain enough points.");
            return;
        }

        GenerateTube();
    }

    List<Vector3> GeneratePath()
    {
        List<Vector3> generatedPath = new List<Vector3>();

        // Define noise parameters
        float noiseScale = 0.3f; // Adjust the scale of the noise
        float yOffset = 0f; // Control the vertical movement offset

        for (int i = 0; i < pathPoints; i++)
        {
            // Calculate X position evenly spaced along the path length
            float x = i * (pathLength / pathPoints);

            // Apply Perlin noise to Y and Z for more natural variation
            float y = Mathf.PerlinNoise(i * noiseScale, 0) * 2f; // Control vertical movement
            float z = Mathf.PerlinNoise(i * noiseScale, 100f) * 2f; // Control horizontal variation

            generatedPath.Add(new Vector3(x, y + yOffset, z));
        }

        return generatedPath;
    }

    void GenerateTube()
    {
        if (path.Count == 0)
        {
            Debug.LogError("Path has no points. Cannot generate tube.");
            return;
        }

        mesh = new Mesh();
        int vertexCount = path.Count * segments; // Total number of vertices (points * segments)
        int triangleCount =
            (path.Count - 1) * segments * 6; // Triangles count, 2 triangles per segment, 3 vertices per triangle

        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[triangleCount];

        // Generate vertices for the tube
        for (int i = 0; i < path.Count; i++)
        {
            for (int j = 0; j < segments; j++)
            {
                float angle = (float)j / segments * Mathf.PI * 2;
                float x = Mathf.Cos(angle) * radius;
                float y = Mathf.Sin(angle) * radius;

                Vector3 circlePoint = new Vector3(x, y, 0);
                Vector3 pathPoint = path[i];

                int vertexIndex = i * segments + j;
                vertices[vertexIndex] = pathPoint + circlePoint;
            }
        }

        // Generate triangles
        int triangleIndex = 0;
        for (int i = 0; i < path.Count - 1; i++)
        {
            for (int j = 0; j < segments; j++)
            {
                // Current and next segment indices
                int current = i * segments + j;
                int next = (i + 1) * segments + j;
                int nextJ = (j + 1) % segments; // Wrap around to the first segment

                // Create two triangles for each segment
                triangles[triangleIndex] = current;
                triangles[triangleIndex + 1] = next;
                triangles[triangleIndex + 2] = next + nextJ;

                triangles[triangleIndex + 3] = current;
                triangles[triangleIndex + 4] = next + nextJ;
                triangles[triangleIndex + 5] = current + nextJ;

                triangleIndex += 6; // Two triangles per segment
            }
        }

        // Assign the mesh data
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals(); // Optional: Recalculate normals for proper lighting
        
        // Apply the mesh to a MeshFilter component
        GetComponent<MeshFilter>().mesh = mesh;
        OnDrawGizmos();
    }
    void OnDrawGizmos()
    {
        if (path == null) return;
        foreach (var point in path)
        {
            Gizmos.DrawSphere(point, 0.1f);
        }
    }

}