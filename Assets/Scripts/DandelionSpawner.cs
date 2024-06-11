using UnityEngine;
using System.Collections.Generic;

public class DandelionSpawner : MonoBehaviour
{
    public GameObject dandelionPrefab; // Prefab to be instantiated
    [Range(0, 1)]
    public float spawnPercentage = 0.02f; // Percentage of vertices to spawn dandelions at

    void Start()
    {
        // Get the MeshFilter component on the current GameObject (Lumpy Terrain)
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            // Get the mesh
            Mesh mesh = meshFilter.mesh;
            // Get the vertices of the mesh
            Vector3[] vertices = mesh.vertices;

            List<Vector3> filteredVertices = new List<Vector3>();

            // Loop through each vertex and filter based on criteria
            foreach (Vector3 vertex in vertices)
            {
                // Convert local vertex position to world position
                Vector3 worldPosition = transform.TransformPoint(vertex);

                // Calculate the distance from the center (0, 0, 0)
                float distance = Vector3.Distance(Vector3.zero, worldPosition);

                // Check if the vertex is within the specified distance range from the center
                if (distance >= 85f && distance <= 160f)
                {
                    filteredVertices.Add(worldPosition);
                }
            }

            // Determine the number of vertices to populate
            int spawnCount = Mathf.FloorToInt(filteredVertices.Count * spawnPercentage);

            // Shuffle the filtered vertices list using Fisher-Yates algorithm
            for (int i = filteredVertices.Count - 1; i > 0; i--)
            {
                int randomIndex = Random.Range(0, i + 1);
                Vector3 temp = filteredVertices[i];
                filteredVertices[i] = filteredVertices[randomIndex];
                filteredVertices[randomIndex] = temp;
            }

            // Instantiate the dandelionPrefab at the selected vertices
            for (int i = 0; i < spawnCount; i++)
            {
                Instantiate(dandelionPrefab, filteredVertices[i], Quaternion.identity);
            }
        }
        else
        {
            Debug.LogError("MeshFilter component not found on this GameObject.");
        }
    }
}





