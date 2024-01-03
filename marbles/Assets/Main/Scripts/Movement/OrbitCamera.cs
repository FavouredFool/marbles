using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Camera))]
public class OrbitCamera : MonoBehaviour
{
    [SerializeField]
    Transform focus = default;

    [SerializeField, Range(0f, 100f)]
    float cameraLookAtForwardOffset = 5f;

    [SerializeField, Range(1f, 20f)]
    float distance = 5f;

    [SerializeField, Range(5, 25)]
    float orbitAngle = 12.5f;

    [SerializeField, Range(0, 3000)]
    float upAlignmentSpeed = 5f;

    [SerializeField, Range(0, 10), Min(0f)]
    float focusRadius = 1f;

    [SerializeField, Range(0f, 1f)]
    float focusCentering = 0.5f;

    Vector3 focusPoint;

    Camera regularCamera;
    Quaternion gravityAlignment = Quaternion.identity;
    Quaternion orbitRotation;

    private void Awake()
    {
        regularCamera = GetComponent<Camera>();
        focusPoint = focus.position;
    }


    private void LateUpdate()
    {
        UpdateFocusPoint();
        Vector3 lookAtPoint = focusPoint + focus.forward * cameraLookAtForwardOffset;
        Quaternion goalRotation = Quaternion.LookRotation(lookAtPoint - transform.position, CustomGravity.GetUpAxis(focus.position));

        //Quaternion lookRotation = UpdateTwo(goalRotation);
        //UpdateGravityAlignment();
        //Quaternion lookRotation = gravityAlignment * orbitRotation;
        Quaternion lookRotation = goalRotation;

        Vector3 lookDirection = Quaternion.AngleAxis(orbitAngle, focus.right) * focus.forward;

        Vector3 lookPosition = focusPoint - lookDirection * distance;

        transform.SetPositionAndRotation(lookPosition, lookRotation);
    }

    void UpdateFocusPoint()
    {
        Vector3 targetPoint = focus.position;

        // focuspoint needs to be the same as targetPoint on the focus.forward vector and distance needs to also stay the same
        // Step 1: project focusPoint onto targetpoint plane
        Vector3 focusPointProjected = Vector3.ProjectOnPlane(focusPoint, focus.forward);

        // Re-set Height
        float magnitude = (targetPoint - ClosestPointOnCircle(targetPoint, 512)).magnitude;
        Vector3 pointOnCircle = ClosestPointOnCircle(focusPointProjected, 512);
        focusPoint = pointOnCircle + (focusPointProjected - pointOnCircle).normalized * magnitude;

        if (focusRadius > 0f)
        {
            float distance = Vector3.Distance(targetPoint, focusPoint);
            float t = 1f;
            if (distance > 0.01f && focusCentering > 0f)
            {
                t = Mathf.Pow(1f - focusCentering, Time.unscaledDeltaTime);
            }

            if (distance > focusRadius)
            {
                t = Mathf.Min(t, focusRadius / distance);
            }

            // the lerp puts the focusPoint on exactly the focusRadius outline. Really smarto
            focusPoint = Vector3.Lerp(targetPoint, focusPoint, t);
        }
        else
        {
            focusPoint = targetPoint;
        }
    }

    public Vector3 ClosestPointOnCircle(Vector3 position, float radius)
    {
        // Circles origin is assumed to be the center
        return new Vector3(position.x, 0, position.z).normalized * radius;
    }

    void UpdateGravityAlignment()
    {
        Vector3 fromUp = gravityAlignment * Vector3.up;
        Vector3 toUp = CustomGravity.GetUpAxis(focusPoint);
        float dot = Mathf.Clamp(Vector3.Dot(fromUp, toUp), -1f, -1f);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
        float maxAngle = upAlignmentSpeed * Time.deltaTime;

        Quaternion newAlignment = Quaternion.FromToRotation(fromUp, toUp) * gravityAlignment;
        if (angle <= maxAngle)
        {
            gravityAlignment = newAlignment;
        }
        else
        {
            gravityAlignment = Quaternion.SlerpUnclamped(gravityAlignment, newAlignment, maxAngle / angle);
        }
    }

    static float GetAngle(Vector2 direction)
    {
        float angle = Mathf.Acos(direction.y) * Mathf.Rad2Deg;
        return direction.x < 0f ? 360f - angle : angle;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(focusPoint, 0.5f);
    }
}
