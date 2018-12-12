using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class faceDistinction : MonoBehaviour
{
    Color highlightColor;
    Mesh lastMesh;
    Vector3 lastHitNormal;
    Camera camera;
    GameObject newPlane;
    GameObject planePrefab;
    //GameObject selectedPlane;
    void Start()
    {
        camera = Camera.main;
        highlightColor = Color.yellow;
        planePrefab = GameObject.Find("planePrefab");
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if(hit.collider.gameObject.tag!="plane")
                {
                    HighlightSelectedFace(hit);
                    if (hit.collider.transform.Find("planeOfNormal" + hit.normal.ToString())==null)
                        {
                        newPlane = Instantiate(planePrefab, hit.point, Quaternion.LookRotation(Vector3.Cross(hit.normal, Vector3.up), hit.normal), hit.collider.transform);
                        newPlane.name = "planeOfNormal" + hit.normal.ToString();
                        newPlane.GetComponent<planeProperties>().selected = true;
                    }
                    else
                    {
                        //Debug.Log(hit.collider.transform.Find("planeOfNormal" + hit.normal.ToString()).gameObject.GetComponent<planeProperties>().selected);
                        hit.collider.transform.Find("planeOfNormal" + hit.normal.ToString()).gameObject.GetComponent<planeProperties>().selected = true;
                        //Debug.Log(hit.collider.transform.Find("planeOfNormal" + hit.normal.ToString()).gameObject.GetComponent<planeProperties>().selected);
                    }
                    


                }
                else
                    RemoveHighlights();

            }
            else
            {
                RemoveHighlights();
            }
        }
        
    }


    void HighlightSelectedFace(RaycastHit hit)
    {
        if (camera.GetComponent<cameraMovement>().cameraOnSketch == false)
        {
            if (lastHitNormal != Vector3.zero)
            {
                RemoveHighlights();
            }
            MeshCollider meshCollider = hit.collider as MeshCollider;
            if (meshCollider == null || meshCollider.sharedMesh == null)
            {
                return;
            }

            //Debug.Log(meshCollider.transform.gameObject.name);

            Mesh mesh = meshCollider.sharedMesh;
            var vertices = mesh.vertices;
            var normals = mesh.normals;
            var triangles = mesh.triangles;
            var colors = new Color[vertices.Length];
            //-

            //-
            for (var i = 0; i < vertices.Length; i++)
            {
                Vector3 norm = hit.collider.gameObject.transform.TransformDirection(mesh.normals[i]);
                if (normals[i] == hit.normal) //normals[i] always 0,1,0 cause local space
                {
                    colors[i] = highlightColor;
                }
                else
                {
                    colors[i] = Color.white;
                }

            }

            mesh.colors = colors;
            lastMesh = mesh;
            lastHitNormal = hit.normal;
        }
            

    }


    void RemoveHighlights()
    {
        if (camera.GetComponent<cameraMovement>().cameraOnSketch == false)
        {
            if (lastMesh)
            {
                Mesh mesh = lastMesh;
                var vertices = mesh.vertices;
                var normals = mesh.normals;
                var triangles = mesh.triangles;
                var originalColors = mesh.colors;
                var colors = new Color[vertices.Length];

                for (var i = 0; i < vertices.Length; i++)
                {
                    colors[i] = Color.white;
                }

                mesh.colors = colors;
                lastMesh = null;
                lastHitNormal = Vector3.zero;
            }
        }
    }
}