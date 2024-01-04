using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    [SerializeField] bool SpeedZone = true;

    private void OnTriggerEnter(Collider other)
    {
        
        Player player = other.gameObject.GetComponent<Player>();
        if (player == null) return;

        Debug.Log("entered");

        if (SpeedZone)
        {
            player.IsInSpeedZone = true;
        }
        else
        {
            player.IsInSlowZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player == null) return;

        Debug.Log("exited");

        player.IsInSpeedZone = false;
        player.IsInSlowZone = false;
    }
}
