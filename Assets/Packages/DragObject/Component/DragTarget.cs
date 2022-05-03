using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DragTarget : MonoBehaviour
{
    public new Camera camera;
    public Transform ground;
    internal Vector3 position;
    internal bool grabbed;
    new Collider collider;
    Plane groundPlane;
    Plane offsetPlane;
    Vector3 offset;
    bool mouseOver;

    public void Awake()
    {
        collider = GetComponent<Collider>();
        groundPlane = new Plane(ground.up, ground.position);
    }

    public void OnMouseOver()
    {
    }

    public void OnMouseEnter()
    {
        mouseOver = true;
    }

    public void OnMouseExit()
    {
        mouseOver = false;
    }

    public void Update()
    {
        if (mouseOver && Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (collider.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                grabbed = true;
                offset = hit.point - transform.position;
                offsetPlane = new Plane(groundPlane.normal, hit.point);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            grabbed = false;
        }

        if (grabbed)
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (offsetPlane.Raycast(ray, out float distance))
            {
                position = ray.GetPoint(distance) - offset;
            }
        }
    }
}
