using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class cameraMovement : MonoBehaviour
{
    RaycastHit hit;

    bool sketchCameraStart=false;
    bool sketchCameraGoBack = false;
    bool isThereASelectedPlane = false;
    public  bool cameraOnSketch = false;
    bool currentlyPanning = false;



    new Camera camera;
    Vector3 startPosition;
    Quaternion startRotation;
    Vector3 currentPosition;
    Quaternion currentRotation;
    Vector3 sketchPosition;
    Quaternion sketchRotation;
    GameObject[] planesList;
    GameObject selectedPlane;
    // Use this for initialization
    void Start()
    {
        camera = Camera.main;
    }
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            gameObject.transform.Translate(new Vector3(0, 0, 1), Space.Self);
            
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            gameObject.transform.Translate(new Vector3(0, 0, -1), Space.Self);
        }
        //if(Input.touchCount>0)
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out hit, Mathf.Infinity) && !IsPointerOverUIObject()) //checks if our mouse is not on an object nor on the interface
        {
            //Zoom
            

            if (Input.GetMouseButtonDown(0) || currentlyPanning == true)
            {
                currentlyPanning = false;
                //Pan
                if (Input.GetMouseButton(0))
                {
                    currentlyPanning = true;
                    if (Input.GetAxis("Mouse X") < 0)
                    {
                        //Code for action on mouse moving left
                        gameObject.transform.Translate(new Vector3(-1, 0, 0), Space.Self);
                    }
                    if (Input.GetAxis("Mouse X") > 0)
                    {
                        //Code for action on mouse moving right
                        gameObject.transform.Translate(new Vector3(1, 0, 0), Space.Self);
                    }
                    if (Input.GetAxis("Mouse Y") < 0)
                    {
                        //Code for action on mouse moving left
                        gameObject.transform.Translate(new Vector3(0, -1, 0), Space.Self);
                    }
                    if (Input.GetAxis("Mouse Y") > 0)
                    {
                        //Code for action on mouse moving right
                        gameObject.transform.Translate(new Vector3(0, 1, 0), Space.Self);
                    }
                }
            }
        }
        // CAMERA GOING ON SKETCH POSITION
       if(sketchCameraStart)
        {
            //selectedPlane = planeProperties.SearchSelectedPlane();
            if(selectedPlane.name.Contains("planeOfNormal"))
            {
                selectedPlane.GetComponent<MeshRenderer>().enabled = true;
                selectedPlane.GetComponent<MeshCollider>().enabled = true;
            }

            currentPosition = camera.GetComponent<Transform>().position; //current camera position
            currentRotation = camera.GetComponent<Transform>().rotation;
            camera.GetComponent<Transform>().position = Vector3.MoveTowards(currentPosition, sketchPosition, Vector3.Distance(currentPosition, sketchPosition)/16);
            camera.GetComponent<Transform>().rotation = Quaternion.RotateTowards(currentRotation, sketchRotation, Quaternion.Angle(currentRotation, sketchRotation)/16);
            //selectedPlane.transform.localScale += new Vector3((100-selectedPlane.transform.localScale.x)/3000, (100 - selectedPlane.transform.localScale.x) / 3000, (100 - selectedPlane.transform.localScale.x) / 3000);
            if ((currentPosition - sketchPosition).magnitude <0.2f && Quaternion.Angle(currentRotation, sketchRotation)<0.2f)
            {
                camera.GetComponent<Transform>().position = sketchPosition;
                camera.GetComponent<Transform>().rotation = sketchRotation;
                cameraOnSketch = true;
                sketchCameraStart = false;
                //selectedPlane.transform.localScale = new Vector3(100, 100, 100);

                selectedPlane.transform.position = selectedPlane.transform.position + selectedPlane.transform.up * 0.05f;

            }



        }
        // CAMERA GOING BACK TO PRECEDENT POSITION
        if (Input.GetMouseButtonDown(0) && cameraOnSketch && !IsPointerOverUIObject())
        {
            RaycastHit hit;
            if ((Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit) && hit.collider.gameObject != planeProperties.SearchSelectedPlane()) || !Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
            {

                //sketchCamera();
                //if (selectedPlane == null)
                sketchCameraGoBack = true;
            }

        }
        if (sketchCameraGoBack)
        {
            if (selectedPlane.name.Contains("planeOfNormal"))
            {
                selectedPlane.GetComponent<MeshRenderer>().enabled = false;
                selectedPlane.GetComponent<MeshCollider>().enabled = false;
            }

            //selectedPlane.transform.localScale = new Vector3(1, 1, 1);

            currentPosition = camera.GetComponent<Transform>().position; //current camera position
            currentRotation = camera.GetComponent<Transform>().rotation;
            camera.GetComponent<Transform>().position = Vector3.MoveTowards(currentPosition, startPosition, 0.2f);
            camera.GetComponent<Transform>().rotation = Quaternion.RotateTowards(currentRotation, startRotation, 1f);      
            if (currentPosition == startPosition && currentRotation == startRotation)
            {
                cameraOnSketch = false;
                sketchCameraGoBack = false;

                selectedPlane.transform.position = selectedPlane.transform.position- selectedPlane.transform.up * 0.05f;
            }

        }

    }
    public void sketchCamera()
    {
        selectedPlane = planeProperties.SearchSelectedPlane();
        if (selectedPlane!=null && cameraOnSketch==false)
        {
            startPosition = camera.GetComponent<Transform>().position; //current camera position
            startRotation = camera.GetComponent<Transform>().rotation;
            sketchPosition = selectedPlane.transform.position;
            sketchRotation = selectedPlane.transform.rotation;

            //Steps to rotate the camera to be facing the plane
            camera.transform.SetPositionAndRotation(sketchPosition, sketchRotation);
            camera.transform.forward = -selectedPlane.transform.up;
            camera.transform.Translate(new Vector3(0, 0, -10), Space.Self);
            sketchPosition = camera.transform.position;
            sketchRotation = camera.transform.rotation;
            camera.transform.SetPositionAndRotation(startPosition, startRotation); //putting the camera back the start, ready to start the animation

            sketchCameraStart = true;
        }
        
    }
    public void SketchCameraGoBack()
    {
        sketchCameraGoBack = true;
    }
}
