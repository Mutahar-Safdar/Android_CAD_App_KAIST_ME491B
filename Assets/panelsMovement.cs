using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class panelsMovement : MonoBehaviour {
    GameObject[] panelList;
    GameObject panel;
    GameObject panelAbove;
    List<GameObject> panelUnder;
    Vector2 currentPosition;

    GameObject sketchButton;

    GameObject sketchPanel;
    public float posy_init = 90f;
    public float posy_hiden;
    public float posy_under =-25f;
    public float posy_above =153f;
    public static float posy_above2 = 311f;

    bool startHiding=false;
    public static bool startReset = false;
    bool startGoAbove = false;
    bool startGoUnder = false;
    bool startGoAbove2 = false;

    List<float> panels_posy;
    List<GameObject> panelsInMovement;
    // Use this for initialization


    void Start () {

        posy_hiden = -posy_init;
        panelList = GameObject.FindGameObjectsWithTag("panel");
        panels_posy= new List<float>();
        sketchButton = GameObject.Find("sketchButton");
        sketchPanel = GameObject.Find("sketchPanel");
        panelUnder= new List<GameObject>();
        panelsInMovement = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        //-------------------------------------------------------------------------------------------------------ResetPositions
        if (startHiding)
        {
            
            startHiding = true;
            panels_posy.Clear();
            for (int i = 0; i < panelList.Length; i++) //animation
            {
                currentPosition = panelList[i].GetComponent<RectTransform>().anchoredPosition;
                panelList[i].GetComponent<RectTransform>().anchoredPosition = Vector2.MoveTowards(currentPosition, new Vector2(0, posy_hiden), Mathf.Abs((currentPosition.y - posy_hiden)) / 16);
                
                panels_posy.Add(panelList[i].GetComponent<RectTransform>().anchoredPosition.y);
            }
            if (panels_posy.TrueForAll(y => Mathf.Abs((y - posy_hiden)) < 1f))
            {
                for (int i = 0; i < panelList.Length; i++) //animation
                {
                    panelList[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, posy_hiden);
                }
                startHiding = false;
                panelsInMovement.Clear();
            }

        }

        //-------------------------------------------------------------------------------------------------------HideAll
        if (startReset)
        {
            startReset = true;

            panels_posy.Clear();
            for (int i = 0; i < panelList.Length; i++)
            {
                currentPosition = panelList[i].GetComponent<RectTransform>().anchoredPosition;
                panelList[i].GetComponent<RectTransform>().anchoredPosition = Vector2.MoveTowards(currentPosition, new Vector2(0, posy_init), Mathf.Abs((currentPosition.y - posy_init)) / 16);
                panels_posy.Add(panelList[i].GetComponent<RectTransform>().anchoredPosition.y);
            }
            if (panels_posy.TrueForAll(y => Mathf.Abs((y - posy_init)) < 1f))
            {
                for (int i = 0; i < panelList.Length; i++) //animation
                {
                    panelList[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, posy_init);
                }
                startReset = false;
                panelsInMovement.Clear();
            }
        }
        //-------------------------------------------------------------------------------------------------------GoAbove
        if (startGoAbove)
        {
            startGoAbove = true;
            

            currentPosition = panelAbove.GetComponent<RectTransform>().anchoredPosition;
            panelAbove.GetComponent<RectTransform>().anchoredPosition = Vector2.MoveTowards(currentPosition, new Vector2(0, posy_above), Mathf.Abs((currentPosition.y - posy_above)) / 8);
            if (Mathf.Abs(currentPosition.y-posy_above)< 1f)
            {
                panelAbove.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, posy_above);
                startGoAbove = false;
                panelsInMovement.Remove(panelAbove);
            }

        }
        //-------------------------------------------------------------------------------------------------------GoAbove
        if (startGoAbove2)
        {
            startGoAbove2 = true;


            currentPosition = panelAbove.GetComponent<RectTransform>().anchoredPosition;
            panelAbove.GetComponent<RectTransform>().anchoredPosition = Vector2.MoveTowards(currentPosition, new Vector2(0, posy_above2), Mathf.Abs((currentPosition.y - posy_above2)) / 8);
            if (Mathf.Abs(currentPosition.y - posy_above2) < 1f)
            {
                panelAbove.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, posy_above2);
                startGoAbove2 = false;
                panelsInMovement.Remove(panelAbove);
            }

        }
        //-------------------------------------------------------------------------------------------------------GoUnder
        if (startGoUnder)
        {
            startGoUnder = true;

            for (int i = 0; i < panelUnder.Count; i++) //animation
            {
                currentPosition = panelUnder[i].GetComponent<RectTransform>().anchoredPosition;
                panelUnder[i].GetComponent<RectTransform>().anchoredPosition = Vector2.MoveTowards(currentPosition, new Vector2(0, posy_under), Mathf.Abs((currentPosition.y - posy_under)) / 8);
                if (Mathf.Abs(currentPosition.y - posy_under) < 1f)
                {
                    panelUnder[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, posy_under);
                    startGoUnder = false;
                    panelsInMovement.Remove(panelUnder[i]);
                    panelUnder.Clear();
                }
            }

        }
        //CLOSE SKETCH PANEL IF SKETCH IS DISABLED
        if(sketchPanel.transform.position.y==posy_above)
        {
            if (sketchButton.GetComponent<Button>().interactable == false)
                startReset = true;
        }
       
            
    }

    //-------------------------------------------------------------------------------------------------------
    public void ResetPositions()
    {
        if (panelsInMovement.Count == 0)
        {
            panelsInMovement.AddRange(panelList);
            startReset = true;
        }
            
    }
    //-------------------------------------------------------------------------------------------------------
    public void HideAll()
    {
        if(panelsInMovement.Count==0)
        {
            panelsInMovement.AddRange(panelList);
            startHiding = true;
        }       
    }
    //-------------------------------------------------------------------------------------------------------
    public void GoAbove(GameObject Panel)
    {
        panelAbove = Panel;
        if (panelsInMovement.Contains(Panel)==false)
        {
            startGoAbove = true;
        }
            
    }
    //-------------------------------------------------------------------------------------------------------
    public void GoAbove2(GameObject Panel)
    {

        panelAbove = Panel;

        if (panelsInMovement.Contains(Panel) == false)
        {
            startGoAbove2 = true;
        }

    }
    //-------------------------------------------------------------------------------------------------------
    public void GoUnder(GameObject Panel)
    {
        if (panelsInMovement.Contains(Panel) == false)
        {
            panelUnder.Add(Panel); //list of the panel to move under
            startGoUnder = true;
        }
            
    }
    
    

}

