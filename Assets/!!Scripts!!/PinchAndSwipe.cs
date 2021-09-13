using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchAndSwipe : MonoBehaviour
{
    //cam move
    //bool bDragging;
    Vector3 oldPos;
    Vector3 panOrigin;
    public float panSpeed = 2f;

    //pinch
    public float perspectiveZoomSpeed = 0.5f;
    public float orthoZoomSpeed = 0.5f;
    Camera cam;

    private void Start()
    {
        cam = Camera.main.GetComponent<Camera>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //bDragging = true;
            oldPos = Camera.main.transform.position;
            panOrigin = Camera.main.ScreenToViewportPoint(Input.mousePosition);                    //Get the ScreenVector the mouse clicked
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition) - panOrigin;
            //Debug.Log(pos);
            Camera.main.transform.position = new Vector3(Mathf.Clamp(oldPos.x + -pos.x * panSpeed, -10f + cam.fieldOfView / 8f, 10f - cam.fieldOfView / 8f), oldPos.y, Mathf.Clamp(oldPos.z + -pos.y * panSpeed, -8f + cam.fieldOfView / 10f, 8f - cam.fieldOfView / 10f));
        }

        if (Input.GetMouseButtonUp(0))
        {
            //bDragging = false;
        }
        
        if (Input.touchCount == 2)
        {
            PinchToZoom();
        }

    }

    void PinchToZoom()
    {
        Touch _touch1 = Input.GetTouch(0);
        Touch _touch2 = Input.GetTouch(1);

        Vector2 _touch1PrevPos = _touch1.position - _touch1.deltaPosition;
        Vector2 _touch2PrevPos = _touch2.position - _touch2.deltaPosition;

        float prevTouchDeltaMag = (_touch1PrevPos - _touch2PrevPos).magnitude;
        float touchDeltaMag = (_touch1.position - _touch2.position).magnitude;

        float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

        if (cam.orthographic)
        {
            cam.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;
            cam.orthographicSize = Mathf.Max(cam.orthographicSize, 0.1f);
        }
        else
        {
            cam.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;
            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, 40f, 80f);
        }
    }
}
