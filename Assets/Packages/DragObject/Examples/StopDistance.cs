using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopDistance : MonoBehaviour
{
    public float velocity;
    public float maxAccel;
    public Transform target;
    new Rigidbody rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForce(Vector3.right * velocity, ForceMode.VelocityChange);
    }

    float MinimumStopDistance(float velocity, float maximumAcceleration)
    {
        return 0.5f * (velocity * velocity) / maximumAcceleration;
    }

    

    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        float minimumStopDistance = MinimumStopDistance(rigidbody.velocity.x, maxAccel);
        if (distance < minimumStopDistance)
        {
            rigidbody.AddForce(Vector3.left * maxAccel, ForceMode.Acceleration);
        }
    }
}
