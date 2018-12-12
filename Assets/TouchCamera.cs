using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchCamera : MonoBehaviour
{
     float perspectiveZoomSpeed = 0.05f;        // The rate of change of the field of view in perspective mode.
     float orthoZoomSpeed = 0.05f;        // The rate of change of the orthographic size in orthographic mode.
    Camera camera;
     float panningSpeed = 0.1f;
    const float minPinchDistance=3;
    const float stillThreshold=2f;
     float rotationSpeed =0.5f;
    int isRotating = 0;
    int isPanning = 0;
    int isZooming = 0;
    void Update()
    {
        // If there are two touches on the device...
        if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            camera = gameObject.GetComponent<Camera>();
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            if ((touchZero.deltaPosition.magnitude < stillThreshold || touchOne.deltaPosition.magnitude < stillThreshold) && isZooming < 0 && isPanning < 0) // IF ONE OF THE FINGERS IS STILL //ROTATING
            {
                
                    Debug.Log("rotating");
                if (touchZero.deltaPosition.magnitude < stillThreshold && touchOne.deltaPosition.magnitude > stillThreshold) //if finger 0 is still
                {
                        camera.transform.RotateAround(Vector3.zero, Vector3.up, rotationSpeed * touchOne.deltaPosition.magnitude * Mathf.Sign(touchOne.deltaPosition.x));

                }
                else if (touchOne.deltaPosition.magnitude < stillThreshold && touchZero.deltaPosition.magnitude > stillThreshold) //if finger 1 is still
                {
                        camera.transform.RotateAround(Vector3.zero, Vector3.up, rotationSpeed * touchZero.deltaPosition.magnitude * Mathf.Sign(touchZero.deltaPosition.x));

                }
                else
                {
                    isRotating = 0;
                }
                
            }
            else
            {
                if (Mathf.Abs(deltaMagnitudeDiff) > minPinchDistance ) //PINCH ZOOM
                {
                    if (Mathf.Sign(touchZero.deltaPosition.x * touchOne.deltaPosition.x) == -1 || Mathf.Sign(touchZero.deltaPosition.y * touchOne.deltaPosition.y) == -1) //fingers go in opposite direction
                    {
                        isZooming = 3;
                        Debug.Log("zooming");
                        // If the camera is orthographic...
                        if (camera.orthographic)
                        {
                            // ... change the orthographic size based on the change in distance between the touches.
                            camera.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;

                            // Make sure the orthographic size never drops below zero.
                            camera.orthographicSize = Mathf.Max(camera.orthographicSize, 0.1f);
                        }
                        else
                        {
                            // Otherwise change the field of view based on the change in distance between the touches.
                            camera.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;

                            // Clamp the field of view to make sure it's between 0 and 180.
                            camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, 0.1f, 179.9f);
                        }
                    }
                      
                }

                else
                {
                    if (touchZero.deltaPosition.magnitude > stillThreshold && touchOne.deltaPosition.magnitude > stillThreshold)
                    {
                        isPanning = 3;
                        Debug.Log("panning");
                        camera.transform.position -= camera.transform.TransformVector(touchZero.deltaPosition.x, touchZero.deltaPosition.y, 0) * panningSpeed;
                    }
                        

                    
                }
            }
           
        }
        isZooming-=1;
        isPanning -= 1;
        isRotating -= 1;
    }
}

