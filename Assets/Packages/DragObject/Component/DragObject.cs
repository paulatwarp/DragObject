using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class DragObject : MonoBehaviour
{
    public new Camera camera;
    public Transform ground;
    public float force = 1f;
    public float maxSpeed = 1f;
    new Rigidbody rigidbody;
    new Collider collider;
    Plane groundPlane;
    Plane offsetPlane;
    bool grabbed;
    int colliding;
    Vector3 offset;

    public void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        groundPlane = new Plane(ground.up, ground.position);
    }

    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (collider.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                grabbed = true;
                rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                offset = hit.point - transform.position;
                offsetPlane = new Plane(groundPlane.normal, hit.point);
            }
        }
    }

    public void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            grabbed = false;
            rigidbody.constraints = RigidbodyConstraints.None;
        }

        if (grabbed)
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (colliding == 0 && offsetPlane.Raycast(ray, out float distance))
            {
                Vector3 target = ray.GetPoint(distance) - offset;
                Vector3 move = target - transform.position;
                if (rigidbody.SweepTest(move.normalized, out RaycastHit hit, move.magnitude))
                {
                    target = transform.position + move.normalized * hit.distance;
                }
                transform.position = target;
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != ground.gameObject)
        {
            colliding++;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject != ground.gameObject)
        {
            colliding--;
        }
    }
}
