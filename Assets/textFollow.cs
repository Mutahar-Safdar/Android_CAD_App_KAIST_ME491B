using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textFollow : MonoBehaviour
{
    Vector3 firstPoint;
    Vector3 secondPoint;
    Camera camera;
    Text text;
    // Use this for initialization
    void Start()
    {
        camera = Camera.main;
        text = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {

        if (gameObject.transform.parent.parent.name.Contains("Rectangle"))
        {
            if (gameObject.name == "Text_a")
            {
                text.text = "a= " + (Mathf.Floor(GetComponentInParent<sketchRectangleProperties>().a * 100) / 100).ToString();
                firstPoint = GetComponentInParent<LineRenderer>().GetPosition(0);
                secondPoint = GetComponentInParent<LineRenderer>().GetPosition(1);
                gameObject.transform.position = camera.WorldToScreenPoint((secondPoint + firstPoint) / 2);
            }
            else if (gameObject.name == "Text_b")
            {
                text.text = "b= " + (Mathf.Floor(GetComponentInParent<sketchRectangleProperties>().b * 100) / 100).ToString();
                firstPoint = GetComponentInParent<LineRenderer>().GetPosition(0);
                secondPoint = GetComponentInParent<LineRenderer>().GetPosition(3);
                gameObject.transform.position = camera.WorldToScreenPoint((secondPoint + firstPoint) / 2);
            }


        }
        else if (gameObject.transform.parent.parent.name.Contains("Circle"))
        {
            if (gameObject.name == "Text_radius")
            {
                text.text = "radius= " + (Mathf.Floor(GetComponentInParent<sketchCircleProperties>().xradius * 100) / 100).ToString();
                firstPoint = new Vector3(GetComponentInParent<sketchCircleProperties>().xcentre, GetComponentInParent<sketchCircleProperties>().zcentre, 0);
                secondPoint = GetComponentInParent<LineRenderer>().GetPosition(8);
                gameObject.transform.position = camera.WorldToScreenPoint((secondPoint + firstPoint) / 2);
            }
        }
        else if (gameObject.transform.parent.parent.name.Contains("Polygon"))
        {
            if (gameObject.name == "Text_radius")
            {
                text.text = "radius= " + (Mathf.Floor(GetComponentInParent<sketchPolygonProperties>().xradius * 100) / 100).ToString();
                firstPoint = new Vector3(GetComponentInParent<sketchPolygonProperties>().xcentre, GetComponentInParent<sketchPolygonProperties>().zcentre, 0);
                gameObject.transform.position = camera.WorldToScreenPoint((firstPoint + firstPoint) / 2);
            }
        }
    }
            
}
