using System.Collections.Generic;
using UnityEngine;

struct ContactPlane
{
    public ContactPlane(ContactPoint contact)
    {
        collider = contact.otherCollider;
        plane = new Plane(contact.normal, contact.point);
    }

    public bool Match(ContactPoint contact)
    {
        return
            contact.otherCollider == collider &&
            Vector3.Angle(contact.normal, plane.normal) < Mathf.Epsilon;
    }

    public Collider collider;
    public Plane plane;
}

[RequireComponent(typeof(Rigidbody))]
public class TeleportToTarget : MonoBehaviour
{
    new Rigidbody rigidbody;
    List<ContactPlane> contacts = new List<ContactPlane>();
    Vector3 target;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void SetDragTarget(Vector3 position)
    {
        target = position;
    }

    void FixedUpdate()
    {
        if (Time.deltaTime > 0f)
        {
            Vector3 move = target - rigidbody.position;
            if (move.sqrMagnitude > 0f)
            {
                foreach (ContactPlane contact in contacts)
                {
                    float dot = Vector3.Dot(move.normalized, contact.plane.normal);
                    if (dot < 0.0f)
                    {
                        Vector3 origin = contact.plane.normal * -contact.plane.distance;
                        Vector3 slide = contact.plane.ClosestPointOnPlane(origin + move);
                        move = slide - origin;
                    }
                    if (move.sqrMagnitude < Mathf.Epsilon)
                    {
                        break;
                    }
                }
                if (move.sqrMagnitude > 0f)
                {
                    if (rigidbody.SweepTest(move.normalized, out RaycastHit hit, move.magnitude))
                    {
                        move = move.normalized * hit.distance;
                    }
                }
            }
            rigidbody.isKinematic = true;
            rigidbody.position += move;
            rigidbody.isKinematic = false;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (!contacts.Exists((plane) => plane.Match(contact)))
            {
                contacts.Add(new ContactPlane(contact));
            }
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        contacts.RemoveAll((contact) => contact.collider == collision.collider);
    }
}
