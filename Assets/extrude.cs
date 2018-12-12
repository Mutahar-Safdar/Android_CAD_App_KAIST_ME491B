using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Parabox.CSG;

public class extrude : MonoBehaviour
{
    public GameObject selectedPlane;
    public static GameObject sketchToExtude;
    LineRenderer sketchLineRenderer;
    public Material mtl;
    // Use this for initialization
    public static GameObject extruded_object;
    public static GameObject composite;
    public static void ResizeSimpleExtrude(float extrude_length)
    {
        
        GameObject sketchPrefab = GameObject.Find("sketchPrefab");
        sketchPrefab.GetComponent<extrude>().DestroySimpleExtrude();
        sketchPrefab.GetComponent<extrude>().SimpleExtrude(extrude_length);
        
    }
    public void SimpleExtrude(float extrude_length)
    {
        selectedPlane = planeProperties.SearchSelectedPlane();
        sketchToExtude = selectedPlane.transform.GetChild(1).GetChild(selectedPlane.transform.GetChild(1).childCount - 1).gameObject;
        sketchToExtude.transform.GetChild(0).gameObject.GetComponent<Canvas>().enabled = false;
        sketchLineRenderer = sketchToExtude.GetComponent<LineRenderer>();
        Vector2[] pos = new Vector2[sketchLineRenderer.positionCount];

        for (int j = 0; j < pos.Length; j++)
        {
            pos[j] = new Vector2(selectedPlane.transform.InverseTransformPoint(sketchLineRenderer.GetPosition(j)).x, selectedPlane.transform.InverseTransformPoint(sketchLineRenderer.GetPosition(j)).z);
            //Debug.Log(pos[j]);
        }


        // Use triangulator to get indices for creating triangles
        Triangulator tr = new Triangulator(pos);
        int[] indices = tr.Triangulate();

        // Create the Vector3 vertices
        Vector3[] vertices = new Vector3[pos.Length];

        for (int j = 0; j < vertices.Length; j++)
        {
            vertices[j] = new Vector3(pos[j].x, pos[j].y, 0); //XY Plane
                                                              //  vertices[j] = new Vector3(pos[j].x, 0, pos[j].y);  //XZ Plane
                                                              //  vertices[j] = new Vector3(0, pos[j].x, pos[j].y); //YZ Plane
        }

        // Create the mesh
        Mesh msh = new Mesh();
        msh.vertices = vertices;
        msh.triangles = indices;
        msh.RecalculateNormals();
        msh.RecalculateBounds();

        float[] length = new float[vertices.Length];
        float net_length = 0;
        for (int j = 0; j < vertices.Length; j++)
        {
            if (j < vertices.Length - 1)
            {
                length[j] = (vertices[j] - vertices[j + 1]).magnitude;
                net_length += length[j];
            }
            else
            {
                length[j] = (vertices[j] - vertices[0]).magnitude;
                net_length += length[j];
            }
        }

        Vector3[] tvertices = msh.vertices;
        Vector2[] uvs = new Vector2[tvertices.Length];
        float cummulated_length = 0;
        for (int j = 0; j < uvs.Length; j++)
        {
            //uvs[j] = new Vector2 (vertices[j].x, vertices[j].z);

            uvs[j] = new Vector2(cummulated_length / net_length, 0);
            cummulated_length += length[j];

        }

        msh.uv = uvs;

        Matrix4x4[] finalSections = new Matrix4x4[2];
        if (extrude_length >= 0)
            finalSections[0] = Matrix4x4.TRS(selectedPlane.transform.position, Quaternion.LookRotation(-selectedPlane.transform.up, selectedPlane.transform.forward), Vector3.one) ;
        else
            finalSections[0] = Matrix4x4.TRS(selectedPlane.transform.position + selectedPlane.transform.up * extrude_length, Quaternion.LookRotation(-selectedPlane.transform.up, selectedPlane.transform.forward), Vector3.one);

        Debug.Log(Quaternion.LookRotation(selectedPlane.transform.up, selectedPlane.transform.forward));
        //  finalSections[1] = Matrix4x4.TRS(new Vector3(extrude_length,0,0), Quaternion.identity, Vector3.one);
        //  finalSections[1] = Matrix4x4.TRS(new Vector3(0, extrude_length, 0), Quaternion.identity, Vector3.one);
        if(extrude_length>=0)
            finalSections[1] = Matrix4x4.TRS(selectedPlane.transform.position + selectedPlane.transform.up * extrude_length, Quaternion.LookRotation(-selectedPlane.transform.up, selectedPlane.transform.forward), Vector3.one);
        else
            finalSections[1] = Matrix4x4.TRS(selectedPlane.transform.position, Quaternion.LookRotation(-selectedPlane.transform.up, selectedPlane.transform.forward), Vector3.one);


        MeshExtrusion.Edge[] precomputedEdges = MeshExtrusion.BuildManifoldEdges(msh);
        MeshExtrusion.ExtrudeMesh(msh, msh, finalSections, precomputedEdges, true);


        // Set up game object with mesh;
        extruded_object = new GameObject();
        // extruded_object.name = "extrude1";
        MeshFilter filter = extruded_object.AddComponent<MeshFilter>();
        filter.mesh = msh;
        MeshRenderer mr = extruded_object.AddComponent<MeshRenderer>();
        mr.material = mtl;
        mr.material.shader = Shader.Find("Custom/VertexColor");

        MeshCollider mc = extruded_object.AddComponent<MeshCollider>();
        //mc.sharedMesh = null;
        mc.sharedMesh = msh;
        extruded_object.transform.SetParent(sketchToExtude.transform);
        extruded_object.name = "Extrusion";
        extruded_object.AddComponent<extrusionProperties>();
        extruded_object.GetComponent<extrusionProperties>().extrude_length=extrude_length;
    }

    //CUT EXTRUDE-----------------------------------------------------------------------------------------------------------------------------------------------------

    public static void ResizeCutExtrude(float extrude_length)
    {
        Destroy(extruded_object);
        Destroy(composite);
        GameObject sketchPrefab = GameObject.Find("sketchPrefab");
        sketchPrefab.GetComponent<extrude>().DestroySimpleExtrude();
        sketchPrefab.GetComponent<extrude>().CutExtrude(extrude_length);

    }


    public void CutExtrude(float extrude_length)
    {
        SimpleExtrude(-extrude_length);
        extruded_object.GetComponent<MeshRenderer>().enabled = true;
        SearchSelectedGeometry().GetComponent<MeshRenderer>().enabled = true;
        Mesh m = CSG.Subtract(SearchSelectedGeometry(),extruded_object); //objecthat is cut,object that cuts
        //Mesh m = CSG.Subtract(GameObject.Find("Cube"), GameObject.Find("Sphere"));
        //Debug.Log(SearchSelectedGeometry().transform.parent.name);
        //Debug.Log(extruded_object.transform.parent.name);
        composite = new GameObject();
        MeshCollider mc = composite.AddComponent<MeshCollider>();
        mc.sharedMesh = m;
        composite.AddComponent<MeshFilter>().sharedMesh = m;
        composite.AddComponent<MeshRenderer>().sharedMaterial = mtl;
        composite.GetComponent<MeshRenderer>().material.shader = Shader.Find("Custom/VertexColor");
        composite.name = "Extrusion";
        composite.transform.SetParent(sketchToExtude.transform);

        extruded_object.GetComponent<MeshRenderer>().enabled = false;
        SearchSelectedGeometry().GetComponent<MeshRenderer>().enabled = false;
        //Destroy(extruded_object);
        //Destroy(SearchSelectedGeometry());
        //GenerateBarycentric(composite);
    }


    public void DestroySimpleExtrude()
    {
        selectedPlane = planeProperties.SearchSelectedPlane();
        sketchToExtude = selectedPlane.transform.GetChild(1).GetChild(selectedPlane.transform.GetChild(1).childCount - 1).gameObject;
        Destroy(sketchToExtude.transform.Find("Extrusion").gameObject);
    }
    static public GameObject SearchSelectedGeometry()
    {
        GameObject selectedGeometry;
        GameObject selectedPlane = planeProperties.SearchSelectedPlane();
        if (selectedPlane.transform.parent.gameObject.tag != "plane")
        {
            selectedGeometry = selectedPlane.transform.parent.gameObject;
            return selectedGeometry;
        }
        return null;
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}




