using Shapes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways] public class GravitySource : ImmediateModeShapeDrawer
{

    public virtual Vector3 GetGravity(Vector3 position)
    {
        return Physics.gravity;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        CustomGravity.Register(this);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        CustomGravity.Unregister(this);
    }
}
