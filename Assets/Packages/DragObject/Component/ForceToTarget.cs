using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ForceToTarget : MonoBehaviour
{
    public float P, I, D;
    Vector3 integral;
    DragTarget dragTarget;
    Vector3 target;
    new Rigidbody rigidbody;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        target = rigidbody.position;
    }

    public void SetDragTarget(Transform target)
    {
        SetDragTarget(target.position);
    }

    public void SetDragTarget(Vector3 position)
    {
        target = position;
    }

    void Update()
    {
        Vector3 move = target - rigidbody.position;
        Vector3 force = Vector3.zero;
        Vector3 delta = rigidbody.velocity;
        force.x = (P * move.x) + (I * integral.x) - (D * delta.x);
        force.y = (P * move.y) + (I * integral.y) - (D * delta.y);
        force.z = (P * move.z) + (I * integral.z) - (D * delta.z);
        integral += move * Time.deltaTime;
        rigidbody.AddForce(force, ForceMode.Force);
    }
}
