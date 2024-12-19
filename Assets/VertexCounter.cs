using UnityEngine;

public class VertexCounter : MonoBehaviour
{
    void Start()
    {
        // Find all MeshRenderers in the scene
        MeshRenderer[] meshRenderers = FindObjectsOfType<MeshRenderer>();

        Debug.Log("==== Vertex Count Per Object ====");
        foreach (MeshRenderer renderer in meshRenderers)
        {
            MeshFilter filter = renderer.GetComponent<MeshFilter>();
            if (filter != null && filter.sharedMesh != null)
            {
                int vertexCount = filter.sharedMesh.vertexCount;
                Debug.Log(renderer.gameObject.name + " has " + vertexCount + " vertices.");
            }
        }
    }
}
