using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swapPanels : MonoBehaviour {

    // Use this for initialization
    public GameObject panelDown;
    public GameObject panelUp;
    static int totalSteps = 25;
    string panelDown_panelUp;
    bool ready = false;
    int step = 0;
    Vector3 posUp;
    Vector3 posDown;

    public void Swap(string panelDown_panelUp)
    {
        step = 0;
        enabled = true;
        char[] delimiters = { ' ', ',','_'};
        string[] panels = panelDown_panelUp.Split(delimiters);
        if (panels[0] == "null")
            panelDown = null;
        else
            panelDown = GameObject.Find(panels[0]);

        if (panels[1] == "null")
            panelUp = null;
        else
            panelUp = GameObject.Find(panels[1]);
        if (panelDown!=null || panelUp!=null)
        {
            ready = true;
        }

    }
	// Update is called once per frame
	public void Update () {
        if (ready)
        {
            
            if (step<totalSteps)
            {
                if (panelDown!=null)
                    panelDown.transform.position += new Vector3(0, -1.80f, 0);
                if (panelUp!=null)
                    panelUp.transform.position += new Vector3(0, 1f, 0);
                step++;
            }
            else
                enabled = false;
        }


    }
}
