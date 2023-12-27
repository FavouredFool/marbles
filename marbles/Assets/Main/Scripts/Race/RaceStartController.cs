using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RaceStartController : MonoBehaviour
{
    [SerializeField] UnityEvent startEvent = default;

    public void OnTriggerEnter(Collider other)
    {
        startEvent.Invoke();
    }
}
