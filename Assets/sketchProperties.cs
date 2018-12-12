using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sketchProperties : MonoBehaviour {
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //--------------Self destruction if empty : -------------------

        if (planeProperties.SearchSelectedPlane() == null)
        {
            if (gameObject.name != "sketchPrefab")
            {
                if (gameObject.transform.childCount == 0)
                    GameObject.Destroy(gameObject);
            }
        }
      
        
	}
}
