using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VelocityUI : MonoBehaviour
{
    [SerializeField] Rigidbody rigidbody;
    [SerializeField] TMP_Text textfield;
    public void LateUpdate()
    {
        textfield.text = "" + Vector3.Dot(rigidbody.transform.forward, rigidbody.velocity);
    }
}
