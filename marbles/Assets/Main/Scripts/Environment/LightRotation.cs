using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;

public class LightRotation : MonoBehaviour
{
    [SerializeField] Transform relativeObject;
    [SerializeField] float verticalOffset = 15f;

    void LateUpdate()
    {
        transform.rotation = relativeObject.rotation * Quaternion.Euler(180 - verticalOffset, 0, 180);
    }
}
