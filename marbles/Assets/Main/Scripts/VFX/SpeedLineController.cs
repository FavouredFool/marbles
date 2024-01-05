using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

public class SpeedLineController : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] VisualEffect vfx;

    Rigidbody rb;

    public void Awake()
    {
        rb = player.GetComponent<Rigidbody>();
        vfx.gameObject.SetActive(true);
    }

    public void LateUpdate()
    {
        float speed = Vector3.Dot(rb.transform.forward, rb.velocity);

        if (speed < 27)
        {
            vfx.SetFloat("SpawnRate", 0);
        }
        else
        {
            float t = Remap(speed, 27, 45, 0, 1);

            float speedMin = Remap(t, 0, 1, 2f, 3.5f);
            Vector2 vfxSpeed = new Vector2(speedMin, speedMin * 1.5f);
            vfx.SetVector2("Speed", vfxSpeed);

            float spawnRate = Remap(t, 0, 1, 25, 100);
            vfx.SetFloat("SpawnRate", spawnRate);
        }
        
    }
    float Remap(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

}
