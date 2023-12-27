using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class OrbitCamera : MonoBehaviour
{
    [SerializeField]
    Transform focus = default;

    [SerializeField, Range(0f, 100f)]
    float cameraLookAtForwardOffset = 5f;

    [SerializeField, Range(1f, 20f)]
    float distance = 5f;

    [SerializeField, Range(1f, 360f)]
    float rotationSpeed = 90f;

    [SerializeField, Range(5, 25)]
    float orbitAngle = 12.5f;

    [SerializeField, Range(0, 3000)]
    float upAlignmentSpeed = 5f;

    Vector3 focusPoint;

    Camera regularCamera;
    Quaternion gravityAlignment = Quaternion.identity;
    Quaternion orbitRotation;

    private void Awake()
    {
        regularCamera = GetComponent<Camera>();
        transform.localRotation = orbitRotation = Quaternion.Euler(orbitAngle, 0, 0);
    }


    private void LateUpdate()
    {
        focusPoint = focus.position + Vector3.forward * cameraLookAtForwardOffset;
        orbitRotation = Quaternion.Euler(orbitAngle, 0, 0);

        UpdateGravityAlignment();

        //Vector3 toUp = CustomGravity.GetUpAxis(focusPoint);
        //Vector3 fromUp = gravityAlignment * Vector3.up;
        //
        //gravityAlignment = Quaternion.FromToRotation(fromUp, toUp) * gravityAlignment;

        Quaternion lookRotation = gravityAlignment * orbitRotation;

        Vector3 lookDirection = lookRotation * Vector3.forward;
        Vector3 lookPosition = focusPoint - lookDirection * distance;

        transform.SetPositionAndRotation(lookPosition, lookRotation);
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
}
