using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;

public class LightRotation : MonoBehaviour
{
    [SerializeField] float verticalOffset = 15f;

    public void UpdateLight(Quaternion rotation)
    {
        transform.rotation = rotation * Quaternion.Euler(180 - verticalOffset, 0, 180);
    }
}
