using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class planeProperties : MonoBehaviour {
    GameObject clone;

    Color baseColor;
    Color selectedColor=Color.yellow;
    public bool selected;
    GameObject selectedPlane;

    RaycastHit hit;

    GameObject _3DButton;
    // Use this for initialization
    void Start()
    {
        
        gameObject.tag = "plane";
        _3DButton = GameObject.Find("3DButton");
        if (!gameObject.name.Contains("(Clone)") && !gameObject.name.Contains("planePrefab")) //if it is not already a clone, create a clone for visualisation from both sides of the plane
        {
            Debug.Log("oui");
            clone = Instantiate(gameObject);
            clone.transform.parent = gameObject.transform;
            clone.transform.Rotate(new Vector3(180, 0, 0), Space.Self);

            
        }

        selectedColor.a = 0.5f;
        baseColor = gameObject.GetComponent<Renderer>().material.color;
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
        
        // Test to see if the plane is selected
        if (Input.GetMouseButtonDown(0))
        {
            if (!IsPointerOverUIObject()) //checks if mouse isn't on the interface
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    //Debug.Log(hit.collider.gameObject.name);

                    //Debug.Log(hit.collider.gameObject.transform.IsChildOf(gameObject.transform));
                    if (hit.collider.gameObject == gameObject) //checks if (our mouse is on the plane)
                    {
                        selected = true;
                    }
                    else
                    {
                        if(gameObject.name.Contains("planeOfNormal"))
                        {
                            selectedPlane = SearchSelectedPlane();
                            if (selectedPlane != null)
                            {
                                if (hit.normal != selectedPlane.transform.up)
                                {
                                    selected = false;
                                }

                            }

                        }
                        else
                        {
                            selected = false;
                        }

                    }
                        
                }
                else
                {
                    selected = false;
                }
            }
            

            if (selected)
            {
                gameObject.GetComponent<Renderer>().material.color = selectedColor;
            }
            else
            {
                gameObject.GetComponent<Renderer>().material.color = baseColor;
            }
            if (planeProperties.SearchSelectedPlane() == null || planeProperties.SearchSelectedPlane().transform.Find("Sketch") == null)
            {
                _3DButton.GetComponent<Button>().interactable = false;
                panelsMovement.startReset = true;
            }
            else
                _3DButton.GetComponent<Button>().interactable = true;
        }
    }
    public void createPlane()
    {

    }
    public static GameObject SearchSelectedPlane() //Returns the selected plane
    {
        
        GameObject selectedPlane;
        GameObject[] planesList;
        
        selectedPlane = null;
        planesList = GameObject.FindGameObjectsWithTag("plane"); 
        for (int i = 0; i<planesList.Length; i++)
        {
            if (planesList[i].GetComponent<planeProperties>().selected == true)
            {               
                selectedPlane = planesList[i];
                return selectedPlane;
            }      
        }
        return null;
    }
}
