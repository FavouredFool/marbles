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

        if (SpeedZone)
        {
            player.SpeedSlowZones.Add(true);
        }
        else
        {
            player.SpeedSlowZones.Add(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player == null) return;

        if (SpeedZone)
        {
            player.SpeedSlowZones.Remove(true);
        }
        else
        {
            player.SpeedSlowZones.Remove(false);
        }

        
    }
}
