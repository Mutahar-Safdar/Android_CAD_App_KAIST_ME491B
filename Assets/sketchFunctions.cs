using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sketchFunctions : MonoBehaviour {
    GameObject sketch;
    GameObject sketchRectanglePrefab;
    GameObject sketchRectangle;
    GameObject sketchCirclePrefab;

    //----------------------Called in the sketchButtonScript : ----------------------------------------------------------------------------
    public static void CreateSketch()
    {
        GameObject plane = planeProperties.SearchSelectedPlane();
        GameObject sketch;
        GameObject sketchPrefab;
        sketchPrefab = GameObject.Find("sketchPrefab");
        


        if (plane.transform.Find("Sketch") ==null) //checks if plane doesn't already have a sketch
        {
            sketch = GameObject.Instantiate(sketchPrefab, plane.transform.position, plane.transform.rotation, plane.transform);
            sketch.name = "Sketch";
        }
        
    }
    //----------------------------------------------------------------------------------------------------------------------------------

    public void CreateSketchRectangle()
    {
        GameObject plane = planeProperties.SearchSelectedPlane();
        Transform sketchTransform;
        GameObject sketchRectangle;
        GameObject sketchRectanglePrefab;
        sketchRectanglePrefab = GameObject.Find("sketchRectanglePrefab");
        sketchTransform = plane.transform.Find("Sketch");

        sketchRectangle = GameObject.Instantiate(sketchRectanglePrefab, sketchTransform.position, sketchTransform.rotation, sketchTransform);
        sketchRectangle.name = "SketchRectangle";
    }

    public void CreateSketchCircle()
    {
        GameObject plane = planeProperties.SearchSelectedPlane();
        Transform sketchTransform;
        GameObject sketchCircle;
        GameObject sketchCirclePrefab;
        sketchCirclePrefab = GameObject.Find("sketchCirclePrefab");
        sketchTransform = plane.transform.Find("Sketch");

        sketchCircle = GameObject.Instantiate(sketchCirclePrefab, sketchTransform.position, sketchTransform.rotation, sketchTransform);
        sketchCircle.name = "SketchCircle";
    }
    public void CreateSketchPolygon()
    {
        GameObject plane = planeProperties.SearchSelectedPlane();
        Transform sketchTransform;
        GameObject sketchPolygon;
        GameObject sketchPolygonPrefab;
        sketchPolygonPrefab = GameObject.Find("sketchPolygonPrefab");
        sketchTransform = plane.transform.Find("Sketch");

        sketchPolygon = GameObject.Instantiate(sketchPolygonPrefab, sketchTransform.position, sketchTransform.rotation, sketchTransform);
        sketchPolygon.name = "SketchPolygon";
    }
    public void DeleteSketch()
    {
        GameObject selectedPlane = planeProperties.SearchSelectedPlane();
        Destroy(selectedPlane.transform.Find("Sketch").gameObject);
    }

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}


}
