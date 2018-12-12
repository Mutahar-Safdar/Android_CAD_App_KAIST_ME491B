using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class sketchPolygonProperties : MonoBehaviour
{
    private bool isSketchDone;
    private Vector3 mouseDownPoint;
    private Vector3 mouseCurrentPoint;
    private Camera cam;
    RaycastHit hit;
    bool onCorrectPlane = false;

    public int segments;
    public float xradius;
    public float zradius;
    public float xcentre;
    public float zcentre;
    LineRenderer line;

    Vector3 sketchOffset;


    GameObject mainInterface;
    GameObject selectedPlane;

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
        isSketchDone = false;
        if (gameObject.name != "sketchPolygonPrefab")
        {
            gameObject.GetComponent<LineRenderer>().enabled = true;
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
        mainInterface = GameObject.Find("mainInterface");
        line = gameObject.GetComponent<LineRenderer>();
        segments = 5;
        gameObject.GetComponent<LineRenderer>().positionCount = segments;
    }

    void CreatePoints()
    {
        float x;
        float z;
        gameObject.GetComponent<LineRenderer>().positionCount = segments;
        float angle = 0;

        for (int i = 0; i < (segments); i++)
        {
            xcentre = mouseDownPoint.x;
            zcentre = mouseDownPoint.z;
            x = xcentre + Mathf.Sin(Mathf.Deg2Rad * angle) * xradius;
            z = zcentre + Mathf.Cos(Mathf.Deg2Rad * angle) * zradius;

            line.SetPosition(i, selectedPlane.transform.TransformPoint(new Vector3(x, mouseDownPoint.y, z)) + sketchOffset);

            angle += (360f / segments);
        }
    }
    void Update()
    {
        if (gameObject.name != "sketchPolygonPrefab" && isSketchDone == false)
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //Debug.Log(mainInterface.GetComponent<GraphicRaycaster>().Raycast(data, rayResults))

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                selectedPlane = planeProperties.SearchSelectedPlane();
                mouseCurrentPoint = selectedPlane.transform.InverseTransformPoint(hit.point);

                if (hit.collider.gameObject == gameObject.transform.parent.parent.gameObject && (!IsPointerOverUIObject())) //checks if our mouse is on the plane of the sketch and is not on the interface
                    onCorrectPlane = true;
                else
                    onCorrectPlane = false;
            }
            if (onCorrectPlane)
            {
                sketchOffset = selectedPlane.transform.up * 0.5f; //if no offset, sketch is merged with sketching surface and is not visible

                if (Input.GetMouseButtonDown(0))
                {
                    mouseDownPoint = mouseCurrentPoint;

                }
                if (Input.GetMouseButton(0))
                {
                    xradius = Mathf.Sqrt(Mathf.Pow((mouseCurrentPoint.x - mouseDownPoint.x), 2) + Mathf.Pow((mouseCurrentPoint.z - mouseDownPoint.z), 2));
                    zradius = xradius;
                    CreatePoints();
                }
                if (Input.GetMouseButtonUp(0))
                {
                    isSketchDone = true;
                    pM.GoAbove2(GameObject.Find("polygonDimensionsInputPanel"));
                }
            }
        }
        if (isSketchDone)
        {
            if (camera.GetComponent<cameraMovement>().cameraOnSketch == false)
            {
                sketchOffset = Vector3.zero;
            }
            if ((line.GetPosition(0) != new Vector3(xcentre, mouseDownPoint.y, xcentre + zcentre) + sketchOffset || xradius != zradius || xradius == zradius || segments != gameObject.GetComponent<LineRenderer>().positionCount) && !Input.GetMouseButton(0))
            {
                CreatePoints();
            }
        }
    }
}

