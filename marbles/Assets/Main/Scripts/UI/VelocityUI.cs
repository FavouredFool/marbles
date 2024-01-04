using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VelocityUI : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] TMP_Text textfield;
    public void LateUpdate()
    {
        textfield.text = "" + Vector3.Dot(rb.transform.forward, rb.velocity);
    }
}
