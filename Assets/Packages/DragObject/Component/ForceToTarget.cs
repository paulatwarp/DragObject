using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ForceToTarget : MonoBehaviour
{
    public float maximumAcceleration = 1f;
    new Rigidbody rigidbody;
    Vector3 targetPosition;
    Motion current;
    Motion target;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        current = new Motion(rigidbody.position, rigidbody.velocity, Vector3.zero);
    }

    public void SetDragTarget(Transform target)
    {
        SetDragTarget(target.position);
    }

    public void SetDragTarget(Vector3 position)
    {
        targetPosition = position;
    }

    void FixedUpdate()
    {
        current.Update(rigidbody.position, Time.deltaTime);
    }

    void Update()
    {
        if (Time.deltaTime > 0f)
        {
            target.Update(targetPosition, Time.deltaTime);
            Vector3 acceleration = current.Acceleration(target, Time.deltaTime, maximumAcceleration);
            Debug.Log(acceleration);
            rigidbody.AddForce(acceleration, ForceMode.Acceleration);
        }
    }
}
