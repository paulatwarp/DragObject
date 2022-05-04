using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct Motion
{
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 acceleration;

    public Motion(Vector3 position, Vector3 velocity, Vector3 acceleration)
    {
        this.position = position;
        this.velocity = velocity;
        this.acceleration = acceleration;
    }

    internal void Update(Vector3 position, float deltaTime)
    {
        Vector3 velocity = (position - this.position) / deltaTime;
        acceleration = (velocity - this.velocity) / deltaTime;
        this.velocity = velocity;
        this.position = position;
    }

    public Motion(Motion current, float time)
    {
        position = current.position
                 + current.velocity * time
                 + 0.5f * current.acceleration * time * time;

        velocity = current.velocity + current.acceleration * time;
        acceleration = current.acceleration;
    }

    public Motion Predict(float time)
    {
        return new Motion(this, time);
    }

    Vector3 Square(Vector3 v)
    {
        return Vector3.Scale(v, v);
    }

    float ClosingTime(Motion target, float maximumAcceleration)
    {
        Vector3 squareVelocity = Square(velocity) + 2f * (target.position - position) * maximumAcceleration;
        float time = Mathf.Sqrt(squareVelocity.magnitude) / maximumAcceleration;
        return time;
    }

    public Vector3 Acceleration(Motion target, float deltaTime, float maximumAcceleration)
    {
        float stoppingTime = ClosingTime(target, maximumAcceleration);
        if (stoppingTime > 0f)
        {
            Motion predicted = new Motion(target, stoppingTime);
            Vector3 requiredVelocity = (predicted.position - position) / stoppingTime;
            Vector3 requiredAcceleration = (requiredVelocity - velocity) / stoppingTime;
            return Vector3.ClampMagnitude(requiredAcceleration, maximumAcceleration);
        }
        else
        {
            return target.acceleration;
        }
    }
}


public class MovementMatching
{

}
