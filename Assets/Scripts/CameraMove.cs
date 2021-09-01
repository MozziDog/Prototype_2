using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMove : MonoBehaviour
{
    private Vector3 ResetCamera; // original camera position
    private Vector3 Origin; // place where mouse is first pressed
    private Vector3 Diference; // change in position of mouse relative to origin
    private Quaternion originRot;
    [SerializeField] Toggle _viewToggle;
    void Start()
    {
        originRot = Camera.main.transform.rotation;
        ResetCamera = Camera.main.transform.position;
    }

    private void Update()
    {
        if (_viewToggle.isOn)
            Camera.main.transform.rotation = new Quaternion(90, 0, 0, Camera.main.transform.rotation.w);
        else
            Camera.main.transform.rotation = originRot;

    }

    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Origin = MousePos();
        }
        if (Input.GetMouseButton(0))
        {
            Diference = MousePos() - transform.position;
            transform.position = Origin - Diference;
        }
        if (Input.GetMouseButton(1)) // reset camera to original position
        {
            transform.position = ResetCamera;
        }
    }
    // return the position of the mouse in world coordinates (helper method)
    Vector3 MousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
