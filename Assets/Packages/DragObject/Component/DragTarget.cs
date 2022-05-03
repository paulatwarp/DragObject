using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class DragTarget : MonoBehaviour
{
    public new Camera camera;
    public Transform ground;
    public UnityEvent<Vector3> updateTarget;
    internal bool grabbed;
    new Collider collider;
    Plane groundPlane;
    Plane offsetPlane;
    Vector3 offset;
    bool mouseOver;
    Vector3 mousePrevious;
    Vector3 mouseVelocity;
    Vector3 mousePreviousVelocity;
    Vector3 mouseAcceleration;

    public void Awake()
    {
        collider = GetComponent<Collider>();
        groundPlane = new Plane(ground.up, ground.position);
        mousePrevious = Input.mousePosition;
        mouseVelocity = Vector3.zero;
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

        if (grabbed && Time.deltaTime > 0f)
        {
            Vector3 mousePosition = Input.mousePosition;
            mouseVelocity = (mousePosition - mousePrevious) / Time.deltaTime;
            mouseAcceleration = mouseVelocity - mousePreviousVelocity;
            Vector3 predictedPosition = mousePosition + mouseVelocity * Time.deltaTime + mouseAcceleration * Time.deltaTime * Time.deltaTime;
            Ray ray = camera.ScreenPointToRay(predictedPosition);
            if (offsetPlane.Raycast(ray, out float distance))
            {
                updateTarget.Invoke(ray.GetPoint(distance) - offset);
            }
            mousePrevious = mousePosition;
            mousePreviousVelocity = mouseVelocity;
        }
    }
}
