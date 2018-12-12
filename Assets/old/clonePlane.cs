using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clonePlane : MonoBehaviour {
    GameObject clone;
    Renderer rend;
    Renderer rendClone;
    // by default, only one side of a plane is visible
    // This script creates a clone of the plane and flips its normal
    // Use this for initialization
    void Start () {
        rend = gameObject.GetComponent<Renderer>();

        //By default, planes are visible on only one side. Creating backs to be visible
        if (gameObject.name.Contains("(Clone)"))
        { 
        }
        else
        {
            clone = Instantiate(gameObject);
            gameObject.transform.Rotate(new Vector3(180, 0, 0), Space.Self);
            clone.transform.parent = gameObject.transform;
            rendClone = clone.GetComponent<Renderer>();
        }
        
    }
	
	// Update is called once per frame
	void Update () {
        if (gameObject.name.Contains("(Clone)"))
        {
        }
        else
        {
            if (rendClone.material.color != rend.material.color)
            {
                rendClone.material.color = rend.material.color;
            }
        }
        
    }
}
