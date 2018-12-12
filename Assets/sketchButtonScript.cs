using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class sketchButtonScript : MonoBehaviour {
    GameObject[] planesList;
    List<GameObject> selectedPlanes;
    GameObject selectedPlane;
    UnityAction CreateSketch;
    bool listenerAlreadyAdded;

    // Use this for initialization
    void Start () {
        gameObject.GetComponent<Button>().interactable = false;
        listenerAlreadyAdded = false;
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
    void Update () {
        if (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject())
        {
            selectedPlane = planeProperties.SearchSelectedPlane();

            if (selectedPlane!=null)
            {
                gameObject.GetComponent<Button>().interactable = true;

                if (listenerAlreadyAdded==true)
                {
                    gameObject.GetComponent<Button>().onClick.RemoveListener(sketchFunctions.CreateSketch);
                    listenerAlreadyAdded = false;
                }
                   
               gameObject.GetComponent<Button>().onClick.AddListener(sketchFunctions.CreateSketch);
               listenerAlreadyAdded = true;
                
            }
            else
                gameObject.GetComponent<Button>().interactable = false;

        }
	}
}
