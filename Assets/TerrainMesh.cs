using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMesh : MonoBehaviour
{
    public Terrain terrain;
    public GameObject copyToObject;

    // Start is called before the first frame update
    void Start()
    {
        //to create a custom mesh out of a terrain, get the bounds of the terrain to get length and width of the plan
        var bounds = terrain.terrainData.bounds;
        //Debug.LogError (S"center: (bounds.center) | | size : (bounds.size)-):
        // limagine you already have a plane mesh and you want to sample the height of terrain and plur, that into the mesh
        var mf = copyToObject.GetComponent<MeshFilter> ();
        var m = mf.mesh;
        List<Vector3> newVerts = new List<Vector3>();
        foreach (var vert in m.vertices)

        {
            var wPos = copyToObject.transform.localToWorldMatrix* vert;
            var newVert = vert;
            //var wPosFlip - new Vector3 (WPos.x, WPos.z, WPos.y);
            newVert.y = terrain.SampleHeight(wPos);
            newVerts.Add(newVert);
        }

        m.SetVertices(newVerts.ToArray());

        m.RecalculateNormals();
        m.RecalculateTangents();
        m.RecalculateBounds();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
