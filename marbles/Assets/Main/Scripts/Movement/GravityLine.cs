using Shapes;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityLine : GravitySource
{
    [SerializeField]
    float gravity = 9.81f;

    public override Vector3 GetGravity(Vector3 position)
    {
        Vector3 attractPoint = ClosestPointOnLine(position, Vector3.forward);

        Vector3 vector = attractPoint - position;
        float distance = vector.magnitude;
        float g = gravity / distance;

        return g * vector;
    }

    public Vector3 ClosestPointOnLine(Vector3 position, Vector3 lineDirection)
    {
        return Vector3.Dot(position, lineDirection) * lineDirection;
    }
}
