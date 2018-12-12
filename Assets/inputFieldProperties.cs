using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inputFieldProperties : MonoBehaviour {
    bool isInputPanelUp = false;
    bool isPlaceholderSet = false;
    string text;
    GameObject selectedPlane;
	// Use this for initialization
	void Start () {
		
	}


	// Update is called once per frame
	void Update () {

            if (gameObject.transform.parent.position.y==panelsMovement.posy_above2 && isInputPanelUp == false)
        {
            isInputPanelUp = true;
            
        }

        if (gameObject.transform.parent.position.y != panelsMovement.posy_above2 && isInputPanelUp == true)
        {
            gameObject.GetComponent<InputField>().text = "";
            isInputPanelUp = false;
            isPlaceholderSet=false;
        }
            

        if(isInputPanelUp && isPlaceholderSet==false)
        {
            if (gameObject.name=="rectangleInputFielda")
            {
                selectedPlane = planeProperties.SearchSelectedPlane();
                text=selectedPlane.transform.GetChild(1).GetChild(selectedPlane.transform.GetChild(1).childCount-1).GetChild(0).Find("Text_a").GetComponent<Text>().text;
                text=text.TrimStart('a','=');
                gameObject.GetComponent<InputField>().placeholder.GetComponent<Text>().text=text;
                isPlaceholderSet = true;
            }
            if (gameObject.name == "rectangleInputFieldb")
            {
                selectedPlane = planeProperties.SearchSelectedPlane();
                text = selectedPlane.transform.GetChild(1).GetChild(selectedPlane.transform.GetChild(1).childCount - 1).GetChild(0).Find("Text_b").GetComponent<Text>().text;
                text = text.TrimStart('b', '=');
                gameObject.GetComponent<InputField>().placeholder.GetComponent<Text>().text = text;
                isPlaceholderSet = true;
            }
            if (gameObject.name == "circleInputFieldxradius")
            {
                selectedPlane = planeProperties.SearchSelectedPlane();
                text = selectedPlane.transform.GetChild(1).GetChild(selectedPlane.transform.GetChild(1).childCount - 1).GetChild(0).Find("Text_radius").GetComponent<Text>().text;
                text = text.TrimStart('r','a','d','i','u','s', '=');
                gameObject.GetComponent<InputField>().placeholder.GetComponent<Text>().text = text;
                isPlaceholderSet = true;
            }
            if (gameObject.name == "polygonInputFieldradius")
            {
                selectedPlane = planeProperties.SearchSelectedPlane();
                text = selectedPlane.transform.GetChild(1).GetChild(selectedPlane.transform.GetChild(1).childCount - 1).GetChild(0).Find("Text_radius").GetComponent<Text>().text;
                text = text.TrimStart('r', 'a', 'd', 'i', 'u', 's', '=');
                gameObject.GetComponent<InputField>().placeholder.GetComponent<Text>().text = text;
                isPlaceholderSet = true;
            }
            if (gameObject.name == "polygonInputFieldSegments")
            {
                selectedPlane = planeProperties.SearchSelectedPlane();
                text = selectedPlane.transform.GetChild(1).GetChild(selectedPlane.transform.GetChild(1).childCount - 1).GetComponent<sketchPolygonProperties>().segments.ToString();
                gameObject.GetComponent<InputField>().placeholder.GetComponent<Text>().text = text;
                isPlaceholderSet = true;
            }
            if (gameObject.name == "extrudeLengthInputField")
            {
                selectedPlane = planeProperties.SearchSelectedPlane();
                text = selectedPlane.transform.GetChild(1).GetChild(selectedPlane.transform.GetChild(1).childCount - 1).gameObject.transform.Find("Extrusion").GetComponent<extrusionProperties>().extrude_length.ToString();
                gameObject.GetComponent<InputField>().placeholder.GetComponent<Text>().text = text;
                isPlaceholderSet = true;
            }

        }
	}
    public void SetDimension_a()
    {
        selectedPlane.transform.GetChild(1).GetChild(selectedPlane.transform.GetChild(1).childCount - 1).GetComponent<sketchRectangleProperties>().a = float.Parse(gameObject.GetComponent<InputField>().textComponent.GetComponent<Text>().text);

    }
    public void SetDimension_b()
    {
        selectedPlane.transform.GetChild(1).GetChild(selectedPlane.transform.GetChild(1).childCount - 1).GetComponent<sketchRectangleProperties>().b = float.Parse(gameObject.GetComponent<InputField>().textComponent.GetComponent<Text>().text);

    }
    public void SetRadius()
    {
        selectedPlane.transform.GetChild(1).GetChild(selectedPlane.transform.GetChild(1).childCount - 1).GetComponent<sketchCircleProperties>().xradius = float.Parse(gameObject.GetComponent<InputField>().textComponent.GetComponent<Text>().text);
        selectedPlane.transform.GetChild(1).GetChild(selectedPlane.transform.GetChild(1).childCount - 1).GetComponent<sketchCircleProperties>().zradius = float.Parse(gameObject.GetComponent<InputField>().textComponent.GetComponent<Text>().text);

    }
    public void SetExtrudeLength()
    {
        float extrude_length= float.Parse(gameObject.GetComponent<InputField>().textComponent.GetComponent<Text>().text);
        extrude.ResizeSimpleExtrude(extrude_length);
    }
    public void SetHoleLength()
    {
        float extrude_length = float.Parse(gameObject.GetComponent<InputField>().textComponent.GetComponent<Text>().text);
        extrude.ResizeCutExtrude(-extrude_length);
    }
    public void SetRadiusPolygon()
    {
        selectedPlane.transform.GetChild(1).GetChild(selectedPlane.transform.GetChild(1).childCount - 1).GetComponent<sketchPolygonProperties>().xradius = float.Parse(gameObject.GetComponent<InputField>().textComponent.GetComponent<Text>().text);
        selectedPlane.transform.GetChild(1).GetChild(selectedPlane.transform.GetChild(1).childCount - 1).GetComponent<sketchPolygonProperties>().zradius = float.Parse(gameObject.GetComponent<InputField>().textComponent.GetComponent<Text>().text);

    }
    public void SetSegmentsPolygon()
    {
        selectedPlane.transform.GetChild(1).GetChild(selectedPlane.transform.GetChild(1).childCount - 1).GetComponent<sketchPolygonProperties>().segments = int.Parse(gameObject.GetComponent<InputField>().textComponent.GetComponent<Text>().text);

    }
}
