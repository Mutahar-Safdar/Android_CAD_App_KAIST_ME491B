using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class AccelerometerCamera : MonoBehaviour
{
    bool enabled_ = false;
    public GameObject target;
    public Vector3 centerOffset;
    public float sensitivity = 1000;
    public float horizontalRange = 60;
    public float verticalRange = 30;
    public int filterWindowSize = 5;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Queue<Vector3> filter;

    // Use this for initialization
    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        filter = new Queue<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enabled_ == true)
        {
            transform.position = initialPosition;
            transform.rotation = initialRotation;

            filter.Enqueue(Input.acceleration);
            if (filter.Count > filterWindowSize)
                filter.Dequeue();

            float totalX = 0, totalY = 0;
            foreach (Vector3 acc in filter)
            {
                totalX += acc.x;
                totalY += acc.y;
            }
            float filteredX = totalX / filter.Count;
            float filteredY = totalY / filter.Count;

            float xc = -filteredX * horizontalRange;
            float yc = (0.5f + filteredY) * 2 * verticalRange;

            xc = Clamp(xc, -horizontalRange, horizontalRange);
            yc = Clamp(yc, -verticalRange, verticalRange);

            transform.RotateAround(target.transform.position + centerOffset, Vector3.up, xc);
            transform.RotateAround(target.transform.position + centerOffset, Vector3.right, yc);

        }

    }

    public T Clamp<T>(T val, T min, T max) where T : IComparable<T>
    {
        if (val.CompareTo(min) < 0) return min;
        else if (val.CompareTo(max) > 0) return max;
        else return val;
    }

    public void OnDisable()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }
    public void Toggle()
    {
        if (enabled_== true )
        {
            transform.position = initialPosition;
            transform.rotation = initialRotation;
            enabled_ = false;
        }
        else
        {
            enabled_ = true;
            initialPosition = transform.position;
            initialRotation = transform.rotation;
            filter = new Queue<Vector3>();
        }
    }


        /*
        void OnGUI()
        {
            GUI.skin.label.fontSize = GUI.skin.box.fontSize = GUI.skin.button.fontSize = 20;
            GUI.Box(new Rect(5, 5, 200, 40), String.Format("{0:0.000}", Input.acceleration.x));
            GUI.Box(new Rect(5, 50, 200, 40), String.Format("{0:0.000}", Input.acceleration.y));
            GUI.Box(new Rect(5, 95, 200, 40), String.Format("{0:0.000}", Input.acceleration.z));

            float xc = -Input.acceleration.x * horizontalRange;
            float yc = (0.5f + Input.acceleration.y) * 2 * verticalRange;

            xc = Clamp(xc, -horizontalRange, horizontalRange);
            yc = Clamp(yc, -verticalRange, verticalRange);

            GUI.Box(new Rect(5, 150, 200, 40), String.Format("XC:{0:0.000}", xc));
            GUI.Box(new Rect(5, 195, 200, 40), String.Format("YC:{0:0.000}", yc));
        }
        */
    }