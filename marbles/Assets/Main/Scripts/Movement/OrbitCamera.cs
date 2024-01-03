using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
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
    float focusCentering = 0.999f;

    [SerializeField, Range(0f, 1000f)]
    float rotationDelta = 0.5f;

    [SerializeField]
    AnimationCurve camRotationCurve;

    Vector3 focusPoint;

    Vector3 convertedPosition = Vector3.zero;

    private void Awake()
    {
        focusPoint = focus.position;
    }


    private void LateUpdate()
    {
        // position
        UpdateFocusPoint();
        Vector3 lookDirection = Quaternion.AngleAxis(orbitAngle, focus.right) * focus.forward;
        Vector3 lookPosition = focusPoint - lookDirection * distance;

        // rotation
        float upAngle = Vector3.Angle(focus.up, transform.up);
        float maxAngleAbs = 90;

        float t = Mathf.Clamp01(Remap(upAngle, 0, maxAngleAbs, 0, 1));
        // At 1 -> push back with exactly as much angle as you get by rotating to the side
        // MAGIC NUMBER
        float maxHorizontalSpeed = 52;

        t = camRotationCurve.Evaluate(t);
        float horizontalPushback = maxHorizontalSpeed * t;

        Vector3 lookAtPoint = focusPoint + focus.forward * cameraLookAtForwardOffset;
        Quaternion goalRotation = Quaternion.LookRotation(lookAtPoint - focusPoint, CustomGravity.GetUpAxis(focusPoint));
        Quaternion partRotation = Quaternion.RotateTowards(transform.rotation, goalRotation, horizontalPushback * Time.deltaTime);
        Quaternion lookRotation = partRotation;

        transform.SetPositionAndRotation(lookPosition, lookRotation);
    }

    void UpdateFocusPoint()
    {
        if (focusRadius > 0f)
        {
            Vector3 focusPointClosestPoint = ClosestPointOnCircle(focusPoint, 512);
            Vector3 focusPointDirectionToZero = -focusPointClosestPoint.normalized;
            float focusPointAngle = Vector3.SignedAngle(Vector3.up, (focusPoint - focusPointClosestPoint).normalized, Vector3.Cross(Vector3.up, focusPointDirectionToZero.normalized));

            Vector3 targetPointClosestPoint = ClosestPointOnCircle(focus.position, 512);
            Vector3 targetPointDirectionToZero = - targetPointClosestPoint.normalized;
            float targetPointMagnitude = (focus.position - targetPointClosestPoint).magnitude;

            Quaternion angleRotation = Quaternion.AngleAxis(focusPointAngle, Vector3.Cross(Vector3.up, targetPointDirectionToZero.normalized));
            Vector3 convertedDirection = RotatePointAroundPivot(Vector3.up, Vector3.zero, angleRotation).normalized;
            convertedPosition = targetPointClosestPoint + convertedDirection * targetPointMagnitude;

            focusPoint = convertedPosition;

            float distance = Vector3.Distance(focus.position, focusPoint);
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
            //Debug.Log(t);
            focusPoint = Vector3.Lerp(focus.position, focusPoint, t);
        }
        else
        {
            focusPoint = focus.position;
        }
    }

    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation)
    {
        return rotation * (point - pivot) + pivot;
    }

    public Vector3 ClosestPointOnCircle(Vector3 position, float radius)
    {
        // Circles origin is assumed to be the center
        return new Vector3(position.x, 0, position.z).normalized * radius;
    }

    float Remap(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(focusPoint, 0.4f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(convertedPosition, 0.5f);
    }
}
