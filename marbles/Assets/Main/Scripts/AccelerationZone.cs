using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerationZone : MonoBehaviour
{
    public enum AccelerationType { BURST, CONTINUOUS };

    [SerializeField, Min(0f)]
    float speed = 5f;

    [SerializeField, Min(0f), ShowIf("@accType == AccelerationType.CONTINUOUS")]
    float acceleration = 10f;

    [SerializeField]
    AccelerationType accType = AccelerationType.CONTINUOUS; 


    private void OnTriggerEnter(Collider other)
    {
        if (accType != AccelerationType.BURST)
        {
            return;
        }

        Rigidbody body = other.attachedRigidbody;

        if (body)
        {
            Accelerate(body, false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (accType != AccelerationType.CONTINUOUS)
        {
            return;
        }

        Rigidbody body = other.attachedRigidbody;

        if (body)
        {
            Accelerate(body, true);
        }
    }

    void Accelerate(Rigidbody body, bool includeAcc)
    {
        Vector3 velocity = transform.InverseTransformDirection(body.velocity);
        if (velocity.y >= speed)
        {
            return;
        }

        if (acceleration > 0f && includeAcc)
        {
            velocity.y = Mathf.MoveTowards(velocity.y, speed, acceleration * Time.deltaTime);
        }
        else
        {
            velocity.y = speed;
        }

        body.velocity = transform.TransformDirection(velocity);

        if (body.TryGetComponent(out Player player))
        {
            player.PreventSnapToGround();
        }
    }
}
