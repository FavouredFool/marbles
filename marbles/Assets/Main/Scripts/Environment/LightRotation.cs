using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;

public class LightRotation : MonoBehaviour
{
    [SerializeField] Transform relativeObject;
    [SerializeField] float verticalOffset = 15f;
    [SerializeField] float maxSmoothAngle = 0.5f;
    [SerializeField] float smoothingExponent = 5;

    void LateUpdate()
    {
        Quaternion rotation = transform.rotation;
        Quaternion goalRotation = relativeObject.rotation * Quaternion.Euler(180 - verticalOffset, 0, 180);
        //
        //// the closer they are, the weaker the turn
        //
        //float angle = Mathf.Clamp(Quaternion.Angle(rotation, goalRotation), 0, maxSmoothAngle);
        //if (angle != 0)
        //{
        //    Debug.Log("hi");d
        //}
        //float baseT = Remap(angle, 0, maxSmoothAngle, 0, 1);
        //float smoothedT = EaseInQuint(baseT);
        //float smoothAngle = Remap(smoothedT, 0, 1, 0, maxSmoothAngle);
        //
        //Debug.Log($"BaseT: {baseT}, smoothedT: {smoothedT}, smoothedAngle: {smoothAngle}");



        //transform.rotation = Quaternion.RotateTowards(rotation, goalRotation, smoothAngle);

        transform.rotation = goalRotation;
    }

    float Remap(float value, float from1, float from2, float to1, float to2)
    {
        //https://forum.unity.com/threads/re-map-a-number-from-one-range-to-another.119437/
        return to1 + (value - from1) * (to2 - to1) / (from2 - from1);
    }

    float EaseInQuint(float t)
    {
        return Mathf.Pow(t, smoothingExponent);
    }
}
