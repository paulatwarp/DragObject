using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public ForceToTarget force;

    void Update()
    {
        force.SetDragTarget(transform.position);
        transform.Translate(Vector3.right * Time.deltaTime);
    }
}
