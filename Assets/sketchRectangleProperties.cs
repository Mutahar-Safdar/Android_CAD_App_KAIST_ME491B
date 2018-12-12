using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class sketchRectangleProperties : MonoBehaviour
{
    // Use this for initialization
    bool isSketchDone;
    private Vector3 mouseDownPoint;
    private Vector3 mouseCurrentPoint;
    public float b; //dimensions of the rectangle axb
    public float a;
    private bool onCorrectPlane = false;
    GameObject selectedPlane;

    Vector3 sketchOffset;
    RaycastHit hit;
    private Camera cam;
    private Vector3[] points = new Vector3[4];

    public panelsMovement pM;
    Camera camera;

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    void Start()
    {
        camera = Camera.main;
        if (gameObject.name != "sketchRectanglePrefab")
        {
            gameObject.GetComponent<LineRenderer>().enabled = true;
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
        cam = Camera.main;
        isSketchDone = false;
        points[0] = new Vector3(0, 0, 0);
        points[1] = new Vector3(0, 0, 0);
        points[2] = new Vector3(0, 0, 0);
        points[3] = new Vector3(0, 0, 0);
        gameObject.GetComponent<LineRenderer>().SetPositions(points);

    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.name != "sketchRectanglePrefab" && isSketchDone == false)
        {


            //----------------------------------------------------------------------------------------------------
            // #if UNITY_STANDALONE || UNITY_WEBPLAYER

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                selectedPlane = planeProperties.SearchSelectedPlane();

                mouseCurrentPoint = selectedPlane.transform.InverseTransformPoint(hit.point);
                if (hit.collider.gameObject == gameObject.transform.parent.parent.gameObject && (!IsPointerOverUIObject())) //checks if our mouse is on the plane of the sketch and is not on the interface
                {
                    onCorrectPlane = true;
                }
                else
                    onCorrectPlane = false;
            }
            if (onCorrectPlane)
            {
                sketchOffset = selectedPlane.transform.up*0.5f; //if no offset, sketch is merged with sketching surface and is not visible
                if (Input.GetMouseButtonDown(0))
                {
                    mouseDownPoint = mouseCurrentPoint;
                    points[0] = selectedPlane.transform.TransformPoint(mouseDownPoint) + sketchOffset;
                    points[1] = selectedPlane.transform.TransformPoint(mouseDownPoint ) + sketchOffset;
                    points[2] = selectedPlane.transform.TransformPoint(mouseDownPoint ) + sketchOffset;
                    points[3] = selectedPlane.transform.TransformPoint(mouseDownPoint) + sketchOffset;
                    //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                }
                if (Input.GetMouseButton(0))
                {
                    points[1] = selectedPlane.transform.TransformPoint(new Vector3(mouseCurrentPoint.x, mouseDownPoint.y, mouseDownPoint.z)) + sketchOffset;
                    points[2] = selectedPlane.transform.TransformPoint(new Vector3(mouseCurrentPoint.x, mouseDownPoint.y, mouseCurrentPoint.z)) + sketchOffset;
                    points[3] = selectedPlane.transform.TransformPoint(new Vector3(mouseDownPoint.x, mouseDownPoint.y, mouseCurrentPoint.z)) + sketchOffset;
                    b = selectedPlane.transform.InverseTransformPoint(points[3]).z - mouseDownPoint.z;
                    a = selectedPlane.transform.InverseTransformPoint(points[1]).x - mouseDownPoint.x;
                }


                if (Input.GetMouseButtonUp(0))
                {
                        isSketchDone = true;
                        pM.GoAbove2(GameObject.Find("rectangleDimensionsInputPanel"));
                }


            }
                gameObject.GetComponent<LineRenderer>().SetPositions(points);

            }
            if (isSketchDone)
            {
                if (camera.GetComponent<cameraMovement>().cameraOnSketch == false)
                {
                    sketchOffset = Vector3.zero;
                    points[0] = selectedPlane.transform.TransformPoint(mouseDownPoint) + sketchOffset;
                }

                if (points[3] != selectedPlane.transform.TransformPoint(new Vector3(mouseDownPoint.x, mouseDownPoint.y, mouseDownPoint.z + b)) + sketchOffset || points[1] != selectedPlane.transform.TransformPoint(new Vector3(mouseDownPoint.x + a, mouseDownPoint.y, mouseDownPoint.z)) + sketchOffset && !Input.GetMouseButton(0)) //update the lengths if dimensions are edited via input
                {
                    points[3] = selectedPlane.transform.TransformPoint(new Vector3(mouseDownPoint.x, mouseDownPoint.y, mouseDownPoint.z + b)) + sketchOffset;
                    points[2] = selectedPlane.transform.TransformPoint(new Vector3(mouseDownPoint.x + a, mouseDownPoint.y, mouseDownPoint.z + b)) + sketchOffset;
                    points[1] = selectedPlane.transform.TransformPoint(new Vector3(mouseDownPoint.x + a, mouseDownPoint.y, mouseDownPoint.z)) + sketchOffset;
                    gameObject.GetComponent<LineRenderer>().SetPositions(points);
                }
            
                
            }


        }
    }
