using UnityEngine;

public class AutoAddCollider : MonoBehaviour
{
    public bool useMeshBounds = true;
    
    void Start()
    {
        AddAppropriateCollider();
    }

    void AddAppropriateCollider()
    {
        // Si ya tiene collider, no hacer nada
        if (GetComponent<Collider>() != null) return;

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null) return;

        // Decidir tipo de collider basado en la forma
        Mesh mesh = meshFilter.mesh;
        
        if (IsBoxShaped(mesh))
        {
            BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
            if (useMeshBounds)
            {
                boxCollider.center = mesh.bounds.center;
                boxCollider.size = mesh.bounds.size;
            }
        }
        else if (IsSphereShaped(mesh))
        {
            SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
            if (useMeshBounds)
            {
                sphereCollider.center = mesh.bounds.center;
                sphereCollider.radius = mesh.bounds.extents.magnitude;
            }
        }
        else
        {
            // Por defecto usar MeshCollider para formas complejas
            MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
            meshCollider.convex = true; // Para física dinámica
        }
    }

    bool IsBoxShaped(Mesh mesh)
    {
        // Lógica simple para detectar si es similar a un cubo
        Vector3 size = mesh.bounds.size;
        float aspectRatio = Mathf.Max(size.x, size.y, size.z) / Mathf.Min(size.x, size.y, size.z);
        return aspectRatio < 3f; // Si no es muy alargado
    }

    bool IsSphereShaped(Mesh mesh)
    {
        // Lógica simple para detectar si es similar a una esfera
        Vector3 size = mesh.bounds.size;
        return Mathf.Abs(size.x - size.y) < 0.1f && Mathf.Abs(size.y - size.z) < 0.1f;
    }
}