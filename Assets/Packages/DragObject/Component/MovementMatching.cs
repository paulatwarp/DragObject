using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct Motion
{
    public float acceleration;
    public float velocity;
    public float position;

    public float AccelerateToPosition(Motion target, float maxAcceleration, float time)
    {
        float Apos = target.position + target.velocity * time + target.acceleration * time * time;
        float Avel = target.velocity + target.acceleration * time;

        float dPos = Apos - position;
        float dVel = Avel - velocity;
        float dAcc = target.acceleration - acceleration;
        acceleration = dAcc + dVel / time + dPos / time / time;

        // Avel' = Avel + Aacc.t
        // Apos' = Apos + Avel.t + 1/2.Aacc.t.t
        // Bpos + Bvel.t + 1/2.Bacc.t.t = Apos'
        // 

        return acceleration;
    }

    public float MatchAcceleration(Motion motion, float maxAcceleration, float maxVelocity)
    {
        return 0f;
    }
}


public class MovementMatching
{

}
