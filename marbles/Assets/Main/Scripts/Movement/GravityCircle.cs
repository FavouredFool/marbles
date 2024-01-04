using Shapes;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityCircle : GravitySource
{
    [SerializeField]
    float gravity = 9.81f;

    [SerializeField]
    float radius = 256f;

    public override Vector3 GetGravity(Vector3 position)
    {
        Vector3 attractPoint = ClosestPointOnCircle(position, radius);

        Vector3 vector = attractPoint - position;
        float distance = vector.magnitude;
        float g = gravity / distance;

        return g * vector;
    }

    public Vector3 ClosestPointOnCircle(Vector3 position, float radius)
    {
        // Circles origin is assumed to be the center
        return new Vector3(position.x, 0, position.z).normalized * radius;
    }
}
