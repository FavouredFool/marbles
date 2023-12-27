using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RaceGoalController : MonoBehaviour
{
    [SerializeField] UnityEvent goalEvent = default;

    public void OnTriggerEnter(Collider other)
    {
        goalEvent.Invoke();
    }
}
