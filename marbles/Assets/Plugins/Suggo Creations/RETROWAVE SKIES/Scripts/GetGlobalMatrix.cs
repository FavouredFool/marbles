using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]

public class GetGlobalMatrix : MonoBehaviour
{

    void LateUpdate()
    {
        Shader.SetGlobalMatrix("_LightMatrix", Matrix4x4.Rotate(transform.rotation));
    }
}
